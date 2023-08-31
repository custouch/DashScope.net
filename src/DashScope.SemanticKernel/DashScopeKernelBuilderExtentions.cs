using DashScope;
using DashScope.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.TextCompletion;

namespace Microsoft.SemanticKernel
{
    public static class DashScopeKernelBuilderExtensions
    {
        public static KernelBuilder WithDashScopeCompletionService(
            this KernelBuilder builder,
            string apiKey,
            string? model = null,
            HttpClient? httpClient = null,
            bool alsoAsTextCompletion = true,
            string? serviceId = null,
            bool setAsDefault = false
            )
        {
            model ??= DashScopeModels.QWenV1;
            var generation = new DashScopeTextCompletion(apiKey, model, httpClient);
            builder.WithAIService<IChatCompletion>(serviceId, generation, setAsDefault);

            if (alsoAsTextCompletion)
            {
                builder.WithAIService<ITextCompletion>(serviceId, generation, setAsDefault);
            }

            return builder;
        }

        public static KernelBuilder WithDashScopeTextEmbeddingGenerationService(
            this KernelBuilder builder,
            string apiKey,
            string? model = null,
            HttpClient? httpClient = null,
            string? serviceId = null
            )
        {
            model ??= DashScopeModels.TextEmbeddingV1;
            builder.WithAIService<ITextEmbeddingGeneration>(serviceId, new DashScopeEmbeddingGeneration(apiKey, model, httpClient));

            return builder;
        }

        private static DashScopeClient CreateDashScopeClient(string apiKey, HttpClient? httpClient)
        {
            Requires.NotNullOrWhiteSpace(apiKey, nameof(apiKey));

            return new DashScopeClient(apiKey, httpClient);
        }
    }
}