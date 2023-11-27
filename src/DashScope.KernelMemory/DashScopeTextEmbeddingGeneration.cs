using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.Diagnostics;
using Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DashScope.KernelMemory
{
    internal class DashScopeTextEmbeddingGeneration : ITextEmbeddingGeneration
    {
        private readonly DashScopeClient _client;
        private readonly string _model;

        public DashScopeTextEmbeddingGeneration(DashScopeClient client, DashScopeOptions options)
        {

            _client = client;
            this._model = options.EmbeddingModel;
        }

        private readonly Dictionary<string, string> _attributes = new();
        public IReadOnlyDictionary<string, string> Attributes => this._attributes;

        public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken = default)
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
    }
}
