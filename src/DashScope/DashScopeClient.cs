using Microsoft;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.Json;

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

        public async Task<DashScopeResponse> GenerationAsync(CompletionRequest request)
        {
            var response = await RequestAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<DashScopeResponse>(content) ?? throw new DashScopeException("Not found Content");
            
            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                throw new DashScopeException(result.Code, result.Message);
            }
        }

        public Task<IAsyncEnumerable<DashScopeResponse>> GenerationStreamAsync(string prompt, string model)
        {

        }

        // TODO:
        public void TextEmbedding(string input, string? model)
        {

        }

        // TODO:
        public void ImageSynthesis(string prompt, string? negative_prompt = null, string? model = null)
        {

        }


        private async Task<HttpResponseMessage> RequestAsync(CompletionRequest requestBody, bool isStream = false)
        {
            var request = new HttpRequestMessage();
            if (isStream)
            {
                request.Headers.Add("X-DashScope-SSE", "enable");
            }
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            request.RequestUri = new Uri(Defaults.GetApiEndpoint());

            var response = await _client.SendAsync(request);

            return response;
        }
    }
}
