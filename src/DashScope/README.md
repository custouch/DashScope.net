# DashScope

DashScope(灵积模型服务）.NET SDK

## Install

```
dotnet add package DashScope --prerelease
```

## Usage

```
var apiKey = builder.configuration["ApiKey"]
var client = DefaultHttpClientProvider.CreateClient();

var DScopeClient = new DashScopeClient(apiKey, client);
```

## Features

- [x] qwen-v1
- [x] text-embedding-v1