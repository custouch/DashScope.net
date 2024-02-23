using DashVector.Enums;
using DashVector.Models;
using DashVector.Models.Requests;
using DashVector.Models.Responses;
using System.Text;
using System.Text.Json;


namespace DashVector
{
    public class DashVectorClient
    {
        private readonly string _apiKey;
        public readonly string _endPoint;
        private readonly HttpClient _client;

        public DashVectorClient(string apiKey, string endPoint, HttpClient? client = null)
        {
            _apiKey = apiKey;
            _endPoint = endPoint;
            _client = client ?? DefaultHttpClientProvider.CreateClient();
        }

        #region CollectionService

        public async Task<NormalResponse<object>> CreateCollectionAsync(CreateCollectionRequest request, CancellationToken cancellationToken = default)
        {
            var apiEndpoint = Defaults.GetApiEndpoint(_endPoint);

            var response = await RequestAsync(HttpMethod.Post, apiEndpoint, request, cancellationToken);

            return await HandleResponseAsync<object>(response);

        }

        public async Task<NormalResponse<CollectionMeta>> DescribeCollectionAsync(string name, CancellationToken cancellationToken = default)
        {
            var apiEndPoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: name);

            var response = await RequestAsync<object>(HttpMethod.Get, apiEndPoint, cancellationToken);

            return await HandleResponseAsync<CollectionMeta>(response);

        }

        public async Task<NormalResponse<List<string>>> GetCollectionListAsync()
        {
            var apiEndpoint = Defaults.GetApiEndpoint(_endPoint);

            var response = await RequestAsync<object>(HttpMethod.Get, apiEndpoint);

            return await HandleResponseAsync<List<string>>(response);
        }

        public async Task<NormalResponse<CollectionStats>> StatsCollectionAsync(string name, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Stats });
            var apiEndPoint = Defaults.GetApiEndpoint(_endPoint, collectionName: name, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Get, apiEndPoint, cancellationToken);

            return await HandleResponseAsync<CollectionStats>(response);

        }

        public async Task<NormalResponse<object>> DeleteCollectionAsync(string name)
        {
            var apiEndpoint = Defaults.GetApiEndpoint(_endPoint, collectionName: name);

            var response = await RequestAsync<object>(HttpMethod.Delete, apiEndpoint);

            return await HandleResponseAsync<object>(response);
        }

        #endregion

        #region DocService

        public async Task<NormalResponse<object>> DeleteDocAsync(DeleteDocRequest deleteDocRequest, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Docs });

            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Delete, apiEndpoint, deleteDocRequest, cancellationToken);

            return await HandleResponseAsync<object>(response);
        }

        public async Task<NormalResponse<Dictionary<string, Doc>>> FetchDocAsync(FetchDocRequest fetchDocRequest, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Docs });

            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames, ids: fetchDocRequest.Ids, partitionName: fetchDocRequest.PartitionName);

            var response = await RequestAsync(HttpMethod.Get, apiEndpoint, cancellationToken);

            return await HandleResponseAsync<Dictionary<string, Doc>>(response);
        }

        public async Task<NormalResponse<object>> InsertDocAsync(InsertDocRequest request, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Docs });
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Post, apiEndpoint, request, cancellationToken);

            return await HandleResponseAsync<object>(response);
        }

        public async Task<NormalResponse<List<Doc>>> QueryDocAsync(QueryDocRequest queryDocRequest, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Query });
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Post, apiEndpoint, queryDocRequest, cancellationToken);

            return await HandleResponseAsync<List<Doc>>(response);
        }

        public async Task<NormalResponse<object>> UpdateDocAsync(InsertDocRequest updateDocRequest, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Docs });
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Put, apiEndpoint, updateDocRequest, cancellationToken);

            return await HandleResponseAsync<object>(response);
        }

        public async Task<NormalResponse<object>> UpsertDocAsync(InsertDocRequest updateDocRequest, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Docs, FunctionNames.Upsert });

            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Post, apiEndpoint, updateDocRequest, cancellationToken);

            return await HandleResponseAsync<object>(response);
        }

        #endregion

        #region PartitionService

        public async Task<NormalResponse<object>> CreatePartitionAsync(CreatePartitionRequest request, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionName = new List<string>(new string[] { FunctionNames.Partitions });

            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionName);

            var response = await RequestAsync(HttpMethod.Post, apiEndpoint, request, cancellationToken);

            return await HandleResponseAsync<object>(response);
        }

        public async Task<NormalResponse<object>> DeletePartitionAsync(string collectionName, string partitionName, CancellationToken cancellationToken = default)
        {
            List<string> functionName = new List<string>(new string[] { FunctionNames.Partitions, partitionName });

            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionName);

            var response = await RequestAsync(HttpMethod.Delete, apiEndpoint, cancellationToken);

            return await HandleResponseAsync<object>(response);
        }

        public async Task<NormalResponse<string>> DescribePartitionAsync(string collectionName, string partitionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Partitions, partitionName });
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Get, apiEndpoint, cancellationToken);

            return await HandleResponseAsync<string>(response);
        }

        public async Task<NormalResponse<List<string>>> ListPartitionsAsync(string collectionName, CancellationToken cancellationTokenn = default)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Partitions });
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Get, apiEndpoint, apiEndpoint, cancellationTokenn);

            return await HandleResponseAsync<List<string>>(response);
        }

        public async Task<NormalResponse<PartitionStats>> StatesPartitionAsync(string collectionName, string partitionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Partitions, partitionName, FunctionNames.Stats });
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Get, apiEndpoint, cancellationToken);

            return await HandleResponseAsync<PartitionStats>(response);
        }

        #endregion

        public async Task<HttpResponseMessage> RequestAsync<TRequest>(HttpMethod method, string ApiEndPoint, TRequest requestBody = default, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("dashvector-auth-token", _apiKey);
            request.RequestUri = new Uri(ApiEndPoint);
            request.Method = method;

            if (method == HttpMethod.Post || method == HttpMethod.Put)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            }

            var response = await _client.SendAsync(request, cancellationToken);

            return response;
        }

        public async Task<NormalResponse<T>> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = new NormalResponse<T>();

            try
            {
                result = JsonSerializer.Deserialize<NormalResponse<T>>(content) ?? throw new DashVectorException("Not found Content");
            }
            catch (JsonException)
            {
                try
                {
                    ErrorResponse errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content) ?? throw new DashVectorException("Not found Content");
                    result = new NormalResponse<T>()
                    {
                        Code = errorResponse.HttpStatusCode,
                        Message = $"dashVector internal error:{errorResponse.Message}",
                        RequestId = errorResponse.RequestId,
                    };
                }
                catch (JsonException)
                {
                    throw new DashVectorException("JsonException Error");
                }
            }

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
