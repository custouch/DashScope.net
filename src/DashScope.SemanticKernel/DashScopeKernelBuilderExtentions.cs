using DashScope;
using DashScope.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.TextGeneration;

namespace Microsoft.SemanticKernel
{
    public static class DashScopeKernelBuilderExtensions
    {
        public static IKernelBuilder WithDashScopeCompletionService(
            this IKernelBuilder builder,
            string apiKey,
            string? model = null,
            HttpClient? httpClient = null,
            string? serviceId = null
            )
        {
            model ??= DashScopeModels.QWenTurbo;
            var generation = new DashScopeTextCompletion(apiKey, model, httpClient);
            builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId, generation);
            builder.Services.AddKeyedSingleton<ITextGenerationService>(serviceId, generation);
            return builder;
        }

        public static MemoryBuilder WithDashScopeTextEmbeddingGenerationService(
            this MemoryBuilder builder,
            string apiKey,
            string? model = null,
            HttpClient? httpClient = null
            )
        {
            model ??= DashScopeModels.TextEmbeddingV1;
            builder.WithTextEmbeddingGeneration(new DashScopeEmbeddingGeneration(apiKey, model, httpClient));

            return builder;
        }
    }
}