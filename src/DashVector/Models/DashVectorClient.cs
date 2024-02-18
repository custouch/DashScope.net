using DashVector.Models.Requests;
using DashVector.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DashVector.Models
{
    public class DashVectorClient
    {
        private readonly string _apiKey;
        public readonly string _endPoint;
        private readonly HttpClient _client;

        public DashVectorClient(String apiKey, String endPoint, HttpClient? client = null)
        {
            this._apiKey = apiKey;
            this._endPoint = endPoint;
            this._client = client ?? DefaultHttpClientProvider.CreateClient();
        }

        public async Task<HttpResponseMessage> RequestAsync<TRequest>(HttpMethod method, string ApiEndPoint, TRequest requestBody = default, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("dashvector-auth-token", _apiKey);
            request.RequestUri = new Uri(ApiEndPoint);
            request.Method = method;

            if (method == HttpMethod.Post || method == HttpMethod.Put)
            {
                request.Headers.Add("Content-Type", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            }

            var response = await _client.SendAsync(request, cancellationToken);

            return response;
        }

        public async Task<Response<T>> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<T>>(content) ?? throw new DashVectorException("Not found Content");

            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                throw new DashVectorException(result.Code, result.Message);
            }
        }
    }
}
