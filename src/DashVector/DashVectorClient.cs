using DashVector.Enums;
using DashVector.Models;
using DashVector.Models.Requests;
using DashVector.Models.Responses;
using System.Text;
using System.Text.Json;


namespace DashVector
{
    /// <summary>
    /// DashVector Http Client
    /// <see href="https://help.aliyun.com/document_detail/2510275.html?spm=a2c4g.2510280.0.0.51607c08tlxYWE"/>
    /// </summary>
    public class DashVectorClient
    {
        const string AUTH_TOKEN = "dashvector-auth-token";

        private readonly string _apiKey;
        private readonly string _endPoint;

        private readonly HttpClient _client;

        public DashVectorClient(string apiKey, string endPoint, HttpClient? client = null)
        {
            _apiKey = apiKey;
            _endPoint = endPoint;
            _client = client ?? DefaultHttpClientProvider.CreateClient();
        }

        #region CollectionService
        /// <summary>
        /// 新建Collection
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase> CreateCollectionAsync(CreateCollectionRequest request, CancellationToken cancellationToken = default)
        {
            var apiEndpoint = Defaults.GetApiEndpoint(_endPoint);

            var response = await RequestAsync(HttpMethod.Post, apiEndpoint, request, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase>(response).ConfigureAwait(false);

        }

        /// <summary>
        /// 描述Collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase<CollectionMeta>> DescribeCollectionAsync(string name, CancellationToken cancellationToken = default)
        {
            var apiEndPoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: name);

            var response = await RequestAsync(HttpMethod.Get, apiEndPoint, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase<CollectionMeta>>(response).ConfigureAwait(false);

        }

        /// <summary>
        /// 获取Collection列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseBase<List<string>>> GetCollectionListAsync(CancellationToken cancellationToken = default)
        {
            var apiEndpoint = Defaults.GetApiEndpoint(_endPoint);

            var response = await RequestAsync(HttpMethod.Get, apiEndpoint, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase<List<string>>>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// 统计Collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase<CollectionStats>> StatsCollectionAsync(string name, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = [FunctionNames.Stats];
            var apiEndPoint = Defaults.GetApiEndpoint(_endPoint, collectionName: name, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Get, apiEndPoint, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase<CollectionStats>>(response).ConfigureAwait(false);

        }

        /// <summary>
        /// 删除Collection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ResponseBase> DeleteCollectionAsync(string name, CancellationToken cancellationToken = default)
        {
            var apiEndpoint = Defaults.GetApiEndpoint(_endPoint, collectionName: name);

            var response = await RequestAsync(HttpMethod.Delete, apiEndpoint, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase>(response).ConfigureAwait(false);
        }

        #endregion

        #region DocService

        /// <summary>
        /// 删除Doc
        /// </summary>
        /// <param name="deleteDocRequest"></param>
        /// <param name="collectionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase> DeleteDocAsync(DeleteDocRequest deleteDocRequest, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = [FunctionNames.Docs];

            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Delete, apiEndpoint, deleteDocRequest, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取Doc
        /// </summary>
        /// <param name="fetchDocRequest"></param>
        /// <param name="collectionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase<Dictionary<string, Doc>>> FetchDocAsync(FetchDocRequest fetchDocRequest, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = [FunctionNames.Docs];

            Dictionary<string, string> query = new()
            {
                ["ids"] = string.Join(",", fetchDocRequest.Ids)
            };
            if (fetchDocRequest.PartitionName != null)
            {
                query["partitionName"] = fetchDocRequest.PartitionName;
            }

            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames, query: query);

            var response = await RequestAsync(HttpMethod.Get, apiEndpoint, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase<Dictionary<string, Doc>>>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// 插入Doc
        /// </summary>
        /// <param name="request"></param>
        /// <param name="collectionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase> InsertDocAsync(InsertDocRequest request, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = [FunctionNames.Docs];
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Post, apiEndpoint, request, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// 检索Doc
        /// </summary>
        /// <param name="queryDocRequest"></param>
        /// <param name="collectionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase<List<Doc>>> QueryDocAsync(QueryDocRequest queryDocRequest, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = [FunctionNames.Query];
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Post, apiEndpoint, queryDocRequest, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase<List<Doc>>>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// 更新Doc
        /// </summary>
        /// <param name="updateDocRequest"></param>
        /// <param name="collectionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase> UpdateDocAsync(UpdateDocRequest updateDocRequest, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = [FunctionNames.Docs];
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Put, apiEndpoint, updateDocRequest, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// 插入或者更新Doc
        /// </summary>
        /// <param name="updateDocRequest"></param>
        /// <param name="collectionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase> UpsertDocAsync(UpsertDocRequest updateDocRequest, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = [FunctionNames.Docs, FunctionNames.Upsert];

            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Post, apiEndpoint, updateDocRequest, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase>(response).ConfigureAwait(false);
        }

        #endregion

        #region PartitionService

        /// <summary>
        /// 创建Partition
        /// </summary>
        /// <param name="request"></param>
        /// <param name="collectionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase> CreatePartitionAsync(CreatePartitionRequest request, string collectionName, CancellationToken cancellationToken = default)
        {
            List<string> functionName = [FunctionNames.Partitions];

            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionName);

            var response = await RequestAsync(HttpMethod.Post, apiEndpoint, request, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// 删除Partition
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="partitionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase> DeletePartitionAsync(string collectionName, string partitionName, CancellationToken cancellationToken = default)
        {
            List<string> functionName = [FunctionNames.Partitions, partitionName];

            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionName);

            var response = await RequestAsync(HttpMethod.Delete, apiEndpoint, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// 描述Partition
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="partitionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase<string>> DescribePartitionAsync(string collectionName, string partitionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = [FunctionNames.Partitions, partitionName];
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Get, apiEndpoint, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase<string>>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取Partition列表
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="cancellationTokenn"></param>
        /// <returns></returns>
        public async Task<ResponseBase<List<string>>> ListPartitionsAsync(string collectionName, CancellationToken cancellationTokenn = default)
        {
            List<string> functionNames = [FunctionNames.Partitions];
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Get, apiEndpoint, apiEndpoint, cancellationTokenn).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase<List<string>>>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// 统计Partition
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="partitionName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBase<PartitionStats>> StatesPartitionAsync(string collectionName, string partitionName, CancellationToken cancellationToken = default)
        {
            List<string> functionNames = [FunctionNames.Partitions, partitionName, FunctionNames.Stats];
            var apiEndpoint = Defaults.GetApiEndpoint(endpoint: _endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await RequestAsync(HttpMethod.Get, apiEndpoint, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<ResponseBase<PartitionStats>>(response).ConfigureAwait(false);
        }

        #endregion

        private async Task<HttpResponseMessage> RequestAsync(HttpMethod method, string ApiEndPoint, CancellationToken cancellationToken = default)
        {
            return await RequestAsync<object>(method, ApiEndPoint, null, cancellationToken).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> RequestAsync<TRequest>(HttpMethod method, string ApiEndPoint, TRequest? requestBody = null, CancellationToken cancellationToken = default)
            where TRequest : class
        {
            var request = new HttpRequestMessage();
            request.Headers.Add(AUTH_TOKEN, _apiKey);
            request.RequestUri = new Uri(ApiEndPoint);
            request.Method = method;

            if (requestBody != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            }

            var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);

            return response;
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response) where T : ResponseBase
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<T>(content)
                        ?? throw new DashVectorException("Failed to deserialize response");

                if (result.Code != 0)
                {
                    throw new DashVectorException(result.Code, result.Message);
                }

                return result;
            }
            catch (DashVectorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DashVectorException(ex.Message);
            }
        }
    }
}
