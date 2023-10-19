using DashScope;
using Microsoft;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.Diagnostics;

public class DashScopeEmbeddingGeneration : ITextEmbeddingGeneration
{
    private readonly DashScopeClient _client;
    private readonly string _model;

    public DashScopeEmbeddingGeneration(string apiKey, string model = DashScopeModels.TextEmbeddingV1, HttpClient? client = null)
    {
        Requires.NotNullOrWhiteSpace(apiKey, nameof(apiKey));
        Requires.NotNullOrWhiteSpace(model, nameof(model));

        this._model = model;
        _client = new DashScopeClient(apiKey, client);
    }

    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken = default)
    {
        try
        {
            var embeddings = await _client.TextEmbeddingAsync(
                new DashScope.Models.EmbeddingRequest()
                {
                    Model = _model,
                    Input =
                    {
                        Texts = data.ToArray<string>()
                    }
                }, cancellationToken);

            return embeddings.Output.Embeddings.Select(d => new ReadOnlyMemory<float>(d.Embedding.Select(e => (float)e).ToArray())).ToList();
        }
        catch (DashScopeException ex)
        {
            throw new SKException(ex.Message, ex);
        }
    }
}