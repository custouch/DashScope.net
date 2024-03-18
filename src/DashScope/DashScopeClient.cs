using DashScope.Models;
using Microsoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DashScope
{
    /// <summary>
    /// <see href="https://help.aliyun.com/document_detail/2399481.html"></see>
    /// </summary>
    public class DashScopeClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;

        public DashScopeClient(string apiKey, HttpClient? client = null)
        {
            this._apiKey = apiKey;
            this._client = client ?? DefaultHttpClientProvider.CreateClient();
        }

        public async Task<CompletionResponse> GenerationAsync(CompletionRequest request, CancellationToken cancellationToken = default)
        {
            var response = await RequestAsync(request, cancellationToken: cancellationToken);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<CompletionResponse>(content) ?? throw new DashScopeException("Not found Content");

            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                throw new DashScopeException(result.Code, result.Message);
            }
        }

        public async IAsyncEnumerable<CompletionResponse> GenerationStreamAsync(CompletionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var response = await RequestAsync(request, true, cancellationToken: cancellationToken);

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var line = await reader.ReadLineAsync();

                if (line.StartsWith("data:"))
                {
                    var data = RemovePrefix(line, "data:");
                    var result = JsonSerializer.Deserialize<CompletionResponse>(data)!;
                    yield return result;
                }
            }
        }

        private string RemovePrefix(string text, string prefix)
        {
            if (text.StartsWith(prefix))
            {
                return text.Substring(prefix.Length);
            }
            else
            {
                return text;
            }
        }

        public async Task<EmbeddingResponse> TextEmbeddingAsync(EmbeddingRequest request, CancellationToken cancellationToken = default)
        {
            var endpoint = Defaults.GetApiEndpoint(taskGroup: "embeddings", task: "text-embedding", functionCall: "text-embedding");
            var response = await RequestAsync(request, endpoint: endpoint, cancellationToken: cancellationToken);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<EmbeddingResponse>(content) ?? throw new DashScopeException("Not found Content");

            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                throw new DashScopeException(result.Code, result.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="negative_prompt"></param>
        /// <param name="model"></param>
        /// <exception cref="NotImplementedException"></exception>
        [Obsolete("not implemented")]
        public void ImageSynthesis(string prompt, string? negative_prompt = null, string? model = null)
        {
            //TODO: Implement The ImageSynthesis
            throw new NotImplementedException();
        }


        private async Task<HttpResponseMessage> RequestAsync<TRequest>(TRequest requestBody, bool isStream = false, bool dataInspection = false, string? endpoint = null, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage();
            if (isStream)
            {
                request.Headers.Add("X-DashScope-SSE", "enable");
            }

            if (dataInspection)
            {
                request.Headers.Add("X-DashScope-DataInspection", "enable");
            }
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            request.RequestUri = new Uri(endpoint ?? Defaults.GetApiEndpoint());
            request.Method = HttpMethod.Post;

            var response =
                isStream ? await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                         : await _client.SendAsync(request, cancellationToken);

            return response;
        }

        public async Task<TokenizerResponse> TokenCountsAsync(TokenizerRequest requestBody, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            request.RequestUri = new Uri(Defaults.GetTokenizerEndpoint());
            request.Method = HttpMethod.Post;

            var response = await _client.SendAsync(request, cancellationToken);
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TokenizerResponse>(content) ?? throw new DashScopeException("Not found Content");

            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                throw new DashScopeException(result.Code, result.Message);
            }
        }
    }
}
