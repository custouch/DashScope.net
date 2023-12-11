using Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.KernelMemory.AI;
using Microsoft.KernelMemory;
using DashScope.Models;

namespace DashScope.KernelMemory
{
    internal class DashScopeTextEmbeddingGenerator : ITextEmbeddingGenerator
    {
        private readonly DashScopeClient _client;
        private readonly string _model;

        public DashScopeTextEmbeddingGenerator(DashScopeClient client, DashScopeOptions options)
        {

            _client = client;
            this._model = options.EmbeddingModel;
        }

        public int MaxTokens => 2048;

        /// <inheritdoc/>
        public async Task<Embedding> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
        {
            var embeddings = await _client.TextEmbeddingAsync(
                   new Models.EmbeddingRequest()
                   {
                       Model = _model,
                       Input =
                       {
                            Texts = [text]
                       }
                   }, cancellationToken);
            return new Embedding(embeddings.Output.Embeddings[0].Embedding.Select(e => (float)e).ToArray());
        }

        public int CountTokens(string text)
        {
            return
              this._client.TokenCountsAsync(new Models.TokenizerRequest()
              {
                  // TODO: Tokenizer Api Not Support Embedding Model
                  Model = DashScopeModels.QWenTurbo,
                  Input =
               {
                      Prompt = text,
               }
              }).ConfigureAwait(false).GetAwaiter().GetResult().InputTokens;
        }
    }
}
