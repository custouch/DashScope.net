
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DashVector;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI;
using Microsoft.KernelMemory.MemoryStorage;

public class DashVectorMemoryDb : IMemoryDb
{
    private readonly DashVectorClient client;
    private readonly ITextEmbeddingGenerator textEmbedding;
    const string id_field = "id";

    public DashVectorMemoryDb(DashVectorClient client, ITextEmbeddingGenerator textEmbedding)
    {
        this.client = client;
        this.textEmbedding = textEmbedding;
    }

    public async Task CreateIndexAsync(string index, int vectorSize, CancellationToken cancellationToken = default)
    {
        try
        {
            await this.client.DescribeCollectionAsync(index, cancellationToken);
        }
        catch (DashVectorException ex)
        {
            await this.client.CreateCollectionAsync(new DashVector.Models.Requests.CreateCollectionRequest()
            {
                Name = index,
                DataType = DashVector.Enums.DataType.FLOAT,
                Dimension = vectorSize,
                FieldsSchema = new Dictionary<string, DashVector.Enums.FieldType>()
                {
                    { id_field, DashVector.Enums.FieldType.STRING }
                }
            }, cancellationToken);
        }


    }

    public async Task DeleteAsync(string index, MemoryRecord record, CancellationToken cancellationToken = default)
    {
        var doc = await this.client.QueryDocAsync(new DashVector.Models.Requests.QueryDocRequest()
        {
            Filter = $"{id_field} = {record.Id}"
        }, index, cancellationToken);

        if (doc.OutPut?.Count > 0)
        {
            await this.client.DeleteDocAsync(new DashVector.Models.Requests.DeleteDocRequest()
            {
                Ids = doc.OutPut.Select(_ => _.Id).ToList()
            }, index, cancellationToken);
        }
    }

    public async Task DeleteIndexAsync(string index, CancellationToken cancellationToken = default)
    {
        await this.client.DeleteCollectionAsync(index, cancellationToken);
    }

    public async Task<IEnumerable<string>> GetIndexesAsync(CancellationToken cancellationToken = default)
    {
        var result = await this.client.GetCollectionListAsync(cancellationToken);
        return result?.OutPut;
    }

    public async IAsyncEnumerable<MemoryRecord> GetListAsync(string index, ICollection<MemoryFilter>? filters = null, int limit = 1, bool withEmbeddings = false,[EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var results = await this.client.QueryDocAsync(new DashVector.Models.Requests.QueryDocRequest()
        {
            IncludeVector = withEmbeddings,
            TopK = limit,
            Filter = BuildFilterString(filters)

        }, index, cancellationToken);

        if (results.OutPut != null)
        {
            foreach (var result in results.OutPut)
            {
                yield return new MemoryRecord()
                {
                    Id = result.Fields[id_field].GetValue<string>(),
                    Vector = result.Vector,
                    Payload = result.Fields.Where(_ => _.Value != null).ToDictionary(_ => _.Key, _ => _.Value.RawValue),
                };
            }
        }
    }

    public async IAsyncEnumerable<(MemoryRecord, double)> GetSimilarListAsync(string index, string text, ICollection<MemoryFilter>? filters = null, double minRelevance = 0, int limit = 1, bool withEmbeddings = false,[EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var embedding = await this.textEmbedding.GenerateEmbeddingAsync(text, cancellationToken);
        var results = await this.client.QueryDocAsync(new DashVector.Models.Requests.QueryDocRequest()
        {
            IncludeVector = withEmbeddings,
            TopK = limit,
            Filter = BuildFilterString(filters),
            Vector = embedding.Data.ToArray(),
        }, index, cancellationToken);

        if (results.OutPut != null)
        {
            foreach (var result in results.OutPut)
            {
                if (result != null)
                    yield return (new MemoryRecord()
                    {
                        Id = result.Fields[id_field].GetValue<string>(),
                        Vector = result.Vector,
                        Payload = result.Fields.Where(_ => _.Value != null).ToDictionary(_ => _.Key, _ => _.Value.RawValue),
                    }, result.Score);
            }
        }
    }

    public async Task<string> UpsertAsync(string index, MemoryRecord record, CancellationToken cancellationToken = default)
    {
        var payload = record.Payload.ToDictionary(_ => _.Key, _ => new DashVector.Models.FieldValue(_.Value));
        payload.Add("id", new DashVector.Models.FieldValue(record.Id));
        var result = await this.client.UpsertDocAsync(new DashVector.Models.Requests.UpsertDocRequest()
        {
            Docs = new List<DashVector.Models.Doc>()
            {
                new DashVector.Models.Doc()
                {
                    Vector = record.Vector.Data.ToArray(),
                    Fields = payload,
                }
            }
        }, index, cancellationToken);

        return record.Id;
    }

    private string? BuildFilterString(ICollection<MemoryFilter>? filters)
    {
        if (filters == null || filters.Count == 0)
        {
            return null;
        }
        var filter = string.Join(" or ", filters.Select(f =>
        {
            return "(" + string.Join(" and ", f.Select(kv => $"{kv.Key} = {kv.Value}")) + ")";
        }));
        return filter;
    }

}