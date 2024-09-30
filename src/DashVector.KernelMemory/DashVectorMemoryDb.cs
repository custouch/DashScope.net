
using System;
using System.Collections.Generic;
using System.Linq;
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

    public DashVectorMemoryDb(DashVectorClient client, ITextEmbeddingGenerator textEmbedding)
    {
        this.client = client;
        this.textEmbedding = textEmbedding;
    }

    public async Task CreateIndexAsync(string index, int vectorSize, CancellationToken cancellationToken = default)
    {
        await this.client.CreateCollectionAsync(new DashVector.Models.Requests.CreateCollectionRequest()
        {
            Name = index,
            DataType = DashVector.Enums.DataType.FLOAT,
            Dimension = vectorSize,
        }, cancellationToken);
    }

    public async Task DeleteAsync(string index, MemoryRecord record, CancellationToken cancellationToken = default)
    {
        await this.client.DeleteDocAsync(new DashVector.Models.Requests.DeleteDocRequest()
        {
            Ids = [record.Id]
        }, index, cancellationToken);
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

    public async IAsyncEnumerable<MemoryRecord> GetListAsync(string index, ICollection<MemoryFilter>? filters = null, int limit = 1, bool withEmbeddings = false, CancellationToken cancellationToken = default)
    {
        var results = await this.client.QueryDocAsync(new DashVector.Models.Requests.QueryDocRequest()
        {
            IncludeVector = withEmbeddings,
            TopK = limit,
            Filter = BuildFilterString(filters)

        }, index, cancellationToken);

        foreach (var result in results.OutPut)
        {
            yield return new MemoryRecord()
            {
                Id = result.Id,
                Vector = result.Vector,
                Payload = result.Fields.ToDictionary(_ => _.Key, _ => _.Value.RawValue),
            };
        }
    }

    public async IAsyncEnumerable<(MemoryRecord, double)> GetSimilarListAsync(string index, string text, ICollection<MemoryFilter>? filters = null, double minRelevance = 0, int limit = 1, bool withEmbeddings = false, CancellationToken cancellationToken = default)
    {
        var embedding = await this.textEmbedding.GenerateEmbeddingAsync(text, cancellationToken);
        var results = await this.client.QueryDocAsync(new DashVector.Models.Requests.QueryDocRequest()
        {
            IncludeVector = withEmbeddings,
            TopK = limit,
            Filter = BuildFilterString(filters),
            Vector = embedding.Data.ToArray(),
        }, index, cancellationToken);

        foreach (var result in results.OutPut)
        {
            yield return (new MemoryRecord()
            {
                Id = result.Id,
                Vector = result.Vector,
                Payload = result.Fields.ToDictionary(_ => _.Key, _ => _.Value.RawValue),
            }, result.Score);
        }
    }

    public async Task<string> UpsertAsync(string index, MemoryRecord record, CancellationToken cancellationToken = default)
    {
        var result = await this.client.UpsertDocAsync(new DashVector.Models.Requests.UpsertDocRequest()
        {
            Docs = new List<DashVector.Models.Doc>()
            {
                new DashVector.Models.Doc()
                {
                    Id = record.Id,
                    Vector = record.Vector.Data.ToArray(),
                    Fields = record.Payload.ToDictionary(_=> _.Key , _=> new DashVector.Models.FieldValue(_.Value)),
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