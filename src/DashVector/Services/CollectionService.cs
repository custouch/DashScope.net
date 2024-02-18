using DashVector.Models;
using DashVector.Models.Requests;
using DashVector.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DashVector.Requests
{
    public class CollectionService : ICollectionService
    {
        private readonly DashVectorClient _client;

        public CollectionService(DashVectorClient client)
        {
            this._client = client;
        }

        public async Task<Response<object>> CreateCollectionAsync(CreateCollectionRequest request, CancellationToken cancellationToken = default)
        {
            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint);

            var response = await _client.RequestAsync(HttpMethod.Post, apiEndpoint, request, cancellationToken);

            return await _client.HandleResponseAsync<object>(response);

        }

        public async Task<Response<CollectionMeta>> DescribeCollectionAsync(string name, CancellationToken cancellationToken)
        {
            var apiEndPoint = Defaults.GetApiEndpoint(endpoint: _client._endPoint, collectionName: name);

            var response = await _client.RequestAsync<object>(HttpMethod.Get, apiEndPoint, cancellationToken);

            return await _client.HandleResponseAsync<CollectionMeta>(response);

        }

        public async Task<Response<List<string>>> GetCollectionListAsync()
        {
            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint);

            var response = await _client.RequestAsync<object>(HttpMethod.Get, apiEndpoint);

            return await _client.HandleResponseAsync<List<string>>(response);
        }

        public async Task<Response<CollectionStats>> StatsCollectionAsync(string name, CancellationToken cancellationToken)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Stats });
            var apiEndPoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName: name, functionNames: functionNames);

            var response = await _client.RequestAsync(HttpMethod.Get, apiEndPoint, cancellationToken);

            return await _client.HandleResponseAsync<CollectionStats>(response);

        }

        public async Task<Response<object>> DeleteCollectionAsync(string name)
        {
            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName: name);

            var response = await _client.RequestAsync<object>(HttpMethod.Delete, apiEndpoint);

            return await _client.HandleResponseAsync<object>(response);
        }

    }
}
