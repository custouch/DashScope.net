using DashScope;
using DashScope.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.TextGeneration;
using Microsoft.SemanticKernel.Plugins.Memory;

namespace Microsoft.SemanticKernel
{
    public static class DashScopeKernelBuilderExtensions
    {
        public static KernelBuilder WithDashScopeCompletionService(
            this KernelBuilder builder,
            string apiKey,
            string? model = null,
            HttpClient? httpClient = null,
            string? serviceId = null
            )
        {
            builder.WithServices(c =>
            {
                model ??= DashScopeModels.QWenTurbo;
                var generation = new DashScopeTextCompletion(apiKey, model, httpClient);
                c.AddKeyedSingleton<IChatCompletionService>(serviceId, generation);
                c.AddKeyedSingleton<ITextGenerationService>(serviceId, generation);
            });
            return builder;
        }

#pragma warning disable SKEXP0052 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
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
#pragma warning restore SKEXP0052 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    }
}