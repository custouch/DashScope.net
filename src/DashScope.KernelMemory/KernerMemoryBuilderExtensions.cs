using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI;
using Microsoft.SemanticKernel.AI.Embeddings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DashScope.KernelMemory;

public static class KernerMemoryBuilderExtensions
{
    public static IKernelMemoryBuilder WithDashScopeTextGeneration(this IKernelMemoryBuilder builder, DashScopeClient client, DashScopeOptions? options = null)
    {
        return builder.AddSingleton<ITextGeneration>(new DashScopeTextGeneration(client, options ?? new DashScopeOptions()));
    }

    public static IKernelMemoryBuilder WithDashScopeEmbeddingGeneration(this IKernelMemoryBuilder builder, DashScopeClient client, DashScopeOptions? options = null)
    {

        var generation = new DashScopeTextEmbeddingGeneration(client, options ?? new DashScopeOptions());

        builder.AddSingleton<ITextEmbeddingGeneration>(generation);
        builder.AddIngestionEmbeddingGenerator(generation);

        return builder;
    }

    public static IKernelMemoryBuilder WithDefaultDashScopes(this IKernelMemoryBuilder builder, DashScopeClient client, DashScopeOptions? options = null)
    {
        return builder.WithDashScopeTextGeneration(client, options)
                      .WithDashScopeEmbeddingGeneration(client, options);
    }
}
