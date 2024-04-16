using DashVector.Enums;
using DashVector.Models;
using Microsoft.SemanticKernel.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace DashVector.SemanticKernel
{
    /// <summary>
    /// An IMemoryStore implementation that uses DashVector as the underlying storage.
    /// </summary>
    /// <param name="client"></param>
    public class DashVectorMemoryStore : IMemoryStore
    {
        static readonly Dictionary<string, FieldType> FieldsSchema = new()
        {
            [nameof(MemoryRecordMetadata.IsReference)] = FieldType.BOOL,
            [nameof(MemoryRecordMetadata.ExternalSourceName)] = FieldType.STRING,
            [nameof(MemoryRecordMetadata.Id)] = FieldType.STRING,
            [nameof(MemoryRecordMetadata.Description)] = FieldType.STRING,
            [nameof(MemoryRecordMetadata.Text)] = FieldType.STRING,
            [nameof(MemoryRecordMetadata.AdditionalMetadata)] = FieldType.STRING,
        };

        static MemoryRecordMetadata ToMetaData(Dictionary<string, FieldValue> fields)
        {
            return new MemoryRecordMetadata(
                fields[nameof(MemoryRecordMetadata.IsReference)].GetValue<bool>(),
                fields[nameof(MemoryRecordMetadata.Id)].GetValue<string>(),
                fields[nameof(MemoryRecordMetadata.Text)].GetValue<string>(),
                fields[nameof(MemoryRecordMetadata.Description)].GetValue<string>(),
                fields[nameof(MemoryRecordMetadata.ExternalSourceName)].GetValue<string>(),
                fields[nameof(MemoryRecordMetadata.AdditionalMetadata)].GetValue<string>()
                );
        }
        static Dictionary<string, FieldValue> ToFieldValues(MemoryRecordMetadata metadata)
        {
            return new Dictionary<string, FieldValue>()
            {
                [nameof(MemoryRecordMetadata.IsReference)] = metadata.IsReference,
                [nameof(MemoryRecordMetadata.ExternalSourceName)] = metadata.ExternalSourceName,
                [nameof(MemoryRecordMetadata.Id)] = metadata.Id,
                [nameof(MemoryRecordMetadata.Description)] = metadata.Description,
                [nameof(MemoryRecordMetadata.Text)] = metadata.Text,
                [nameof(MemoryRecordMetadata.AdditionalMetadata)] = metadata.AdditionalMetadata,
            };
        }



        private readonly DashVectorClient client;
        private readonly DashVectorCollectionOptions options;

        public DashVectorMemoryStore(DashVectorClient client, DashVectorCollectionOptions options)
        {
            this.client = client;
            this.options = options;
        }

        /// <inheritdoc/>
        public async Task CreateCollectionAsync(string collectionName, CancellationToken cancellationToken = default)
        {
            await client.CreateCollectionAsync(new Models.Requests.CreateCollectionRequest()
            {
                Name = collectionName,
                DataType = options.DataType,
                Dimension = options.Dimension,
                Metric = options.Metric,
                ExtraParams = options.ExtraParams,
                FieldsSchema = FieldsSchema,
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task DeleteCollectionAsync(string collectionName, CancellationToken cancellationToken = default)
        {
            await client.DeleteCollectionAsync(collectionName, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> DoesCollectionExistAsync(string collectionName, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await client.DescribeCollectionAsync(collectionName, cancellationToken).ConfigureAwait(false);

                return result.OutPut?.Status == CollectionStatus.SERVING;
            }
            catch { }
            return false;
        }

        /// <inheritdoc/>
        public async Task<MemoryRecord?> GetAsync(string collectionName, string key, bool withEmbedding = false, CancellationToken cancellationToken = default)
        {
            var results = await client.FetchDocAsync(new Models.Requests.FetchDocRequest()
            {
                Ids = [key]
            }, collectionName, cancellationToken).ConfigureAwait(false);

            if (results.Code == 0 && results.OutPut?.Count == 1)
            {
                var doc = results.OutPut.First().Value;
                var metaData = ToMetaData(doc.Fields!);

                return MemoryRecord.FromMetadata(metaData, doc.Vector, doc.Id);
            }
            return null;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<MemoryRecord> GetBatchAsync(string collectionName, IEnumerable<string> keys, bool withEmbeddings = false, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var results = await client.FetchDocAsync(new Models.Requests.FetchDocRequest()
            {
                Ids = keys.ToList()
            }, collectionName, cancellationToken).ConfigureAwait(false);

            if (results.OutPut != null)
            {
                foreach (var (id, doc) in results.OutPut)
                {
                    var metaData = ToMetaData(doc.Fields!);

                    yield return MemoryRecord.FromMetadata(metaData, doc.Vector, doc.Id);
                }
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<string> GetCollectionsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var collections = await client.GetCollectionListAsync(cancellationToken).ConfigureAwait(false);

            if (collections.OutPut != null)
            {
                foreach (var collection in collections.OutPut)
                {
                    yield return collection;
                }
            }
        }

        /// <inheritdoc/>
        public async Task<(MemoryRecord, double)?> GetNearestMatchAsync(string collectionName, ReadOnlyMemory<float> embedding, double minRelevanceScore = 0, bool withEmbedding = false, CancellationToken cancellationToken = default)
        {
            var result = await client.QueryDocAsync(new Models.Requests.QueryDocRequest()
            {
                IncludeVector = withEmbedding,
                Vector = embedding.ToArray(),
                TopK = 1,
            }, collectionName, cancellationToken).ConfigureAwait(false);

            if (result.OutPut?.Count != 1)
            {
                return null;
            }

            var doc = result.OutPut[0];
            var isScoreValid = options.Metric switch
            {
                CollectionInfo.Metric.Cosine or CollectionInfo.Metric.Euclidean => doc.Score <= minRelevanceScore,
                _ => doc.Score >= minRelevanceScore,
            };
            if (!isScoreValid)
            {
                return null;
            }

            var metadata = ToMetaData(doc.Fields!);
            return (MemoryRecord.FromMetadata(metadata, doc.Vector, doc.Id),
                doc.Score);

        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<(MemoryRecord, double)> GetNearestMatchesAsync(string collectionName, ReadOnlyMemory<float> embedding, int limit, double minRelevanceScore = 0, bool withEmbeddings = false,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var results = await client.QueryDocAsync(new Models.Requests.QueryDocRequest()
            {
                IncludeVector = withEmbeddings,
                Vector = embedding.ToArray(),
                TopK = limit,
            }, collectionName, cancellationToken).ConfigureAwait(false);

            if (results.OutPut?.Count > 0)
            {
                foreach (var doc in results.OutPut)
                {
                    var isScoreValid = options.Metric switch
                    {
                        CollectionInfo.Metric.Cosine or CollectionInfo.Metric.Euclidean => doc.Score <= minRelevanceScore,
                        _ => doc.Score >= minRelevanceScore,
                    };

                    if (!isScoreValid)
                    {
                        break;
                    }

                    var metadata = ToMetaData(doc.Fields!);
                    yield return (MemoryRecord.FromMetadata(metadata, doc.Vector, doc.Id),
                        doc.Score);
                }
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string collectionName, string key, CancellationToken cancellationToken = default)
        {
            await client.DeleteDocAsync(new Models.Requests.DeleteDocRequest()
            {
                Ids = [key]
            }, collectionName, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task RemoveBatchAsync(string collectionName, IEnumerable<string> keys, CancellationToken cancellationToken = default)
        {
            await client.DeleteDocAsync(new Models.Requests.DeleteDocRequest()
            {
                Ids = keys.ToList()
            }, collectionName, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> UpsertAsync(string collectionName, MemoryRecord record, CancellationToken cancellationToken = default)
        {
            var id = string.IsNullOrEmpty(record.Key) ? Guid.NewGuid().ToString("N") : record.Key;
            await client.UpsertDocAsync(new Models.Requests.UpsertDocRequest()
            {
                Docs = [
                     new Doc()
                     {
                         Id = id,
                         Fields = ToFieldValues(record.Metadata),
                         Vector = record.Embedding.ToArray()
                     }
                ]
            }, collectionName, cancellationToken).ConfigureAwait(false);
            return id;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<string> UpsertBatchAsync(string collectionName, IEnumerable<MemoryRecord> records, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var docs = new List<Doc>();

            foreach (var record in records)
            {
                var id = string.IsNullOrEmpty(record.Key) ? Guid.NewGuid().ToString("N") : record.Key;

                docs.Add(new Doc()
                {
                    Id = id,
                    Fields = ToFieldValues(record.Metadata),
                    Vector = record.Embedding.ToArray()
                });
            }

            await client.UpsertDocAsync(new Models.Requests.UpsertDocRequest()
            {
                Docs = docs,
            }, collectionName, cancellationToken).ConfigureAwait(false);
            foreach (var doc in docs)
            {
                yield return doc.Id;
            }
        }
    }

    public class DashVectorCollectionOptions
    {
        public int Dimension { get; set; }
        public DataType DataType { get; set; } = DataType.FLOAT;
        public string Metric { get; set; } = CollectionInfo.Metric.Cosine;
        public Dictionary<string, string>? ExtraParams { get; set; }
    }
}
