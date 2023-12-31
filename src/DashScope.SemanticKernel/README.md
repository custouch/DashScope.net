﻿# DashScope Semantic Kernel

DashScope(灵积模型服务) Semantic Kernel integration

## Install

```
dotnet add package DashScope.SemanticKernel --prerelease
```

## Usage

```
builder.Services.AddScoped(svc =>
{
    var kernel = Kernel.Builder.WithDashScopeCompletionService(configuration["ApiKey"])
    .Build();
    return kernel;
});

builder.Services.AddScoped(svc =>
{
    return new MemoryBuilder()
    .WithDashScopeTextEmbeddingGenerationService(builder.Configuration["ApiKey"])
    .Build();
});
```

## Features

- [x] IChatCompletion
- [x] ITextCompletion
- [x] ITextEmbeddingGeneration
