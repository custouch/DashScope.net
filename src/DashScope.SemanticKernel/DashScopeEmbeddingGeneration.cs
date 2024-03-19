﻿using DashScope;
using Microsoft;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;

public class DashScopeEmbeddingGeneration : ITextEmbeddingGenerationService
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

    private readonly Dictionary<string, object?> _attributes = new();
    public IReadOnlyDictionary<string, object?> Attributes => this._attributes;

    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel? kernel = null, CancellationToken cancellationToken = default)
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
                }, cancellationToken).ConfigureAwait(false);

            return embeddings.Output.Embeddings.Select(d => new ReadOnlyMemory<float>(d.Embedding.Select(e => (float)e).ToArray())).ToList();
        }
        catch (DashScopeException ex)
        {
            throw new KernelException(ex.Message, ex);
        }
    }
}