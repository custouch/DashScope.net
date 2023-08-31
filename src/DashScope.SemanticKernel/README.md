# DashScope Semantic Kernel

DashScope(灵积模型服务) Semantic Kernel 集成

## Install

## Usage

```
builder.Services.AddScoped(svc =>
{
    var kernel = Kernel.Builder.WithDashScopeCompletionService(configuration["ApiKey"])
    .WithDashScopeTextEmbeddingGenerationService(configuration["ApiKey"])
    .Build();
    return kernel;
});

```

## Features

- [x] IChatCompletion
- [x] ITextCompletion
- [x] ITextEmbeddingGeneration
