using DashVector.Models;
using DashVector.Models.Requests;
using DashVector.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DashVector.Requests
{
    public class DocService : IDocService
    {
        private readonly DashVectorClient _client;

        public DocService(DashVectorClient client)
        {
            this._client = client;
        }

        public async Task<Response<object>> DeleteDocAsync(DeleteDocRequest deleteDocRequest, string collectionName, CancellationToken cancellationToken)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Docs });

            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName, functionNames: functionNames);

            var response = await _client.RequestAsync(HttpMethod.Delete, apiEndpoint, deleteDocRequest, cancellationToken);

            return await _client.HandleResponseAsync<object>(response);
        }

        public async Task<Response<Dictionary<string, Doc>>> FetchDocAsync(FetchDocRequest fetchDocRequest, string collectionName, CancellationToken cancellationToken)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Docs });

            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName, functionNames: functionNames, ids: fetchDocRequest.Ids, partitionName: fetchDocRequest.Partition);

            var response = await _client.RequestAsync(HttpMethod.Get, apiEndpoint, cancellationToken);

            return await _client.HandleResponseAsync<Dictionary<string, Doc>>(response);
        }

        public async Task<Response<object>> InsertDocAsync(InsertDocRequest request, string collectionName, CancellationToken cancellationToken)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Docs });
            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName, functionNames: functionNames);

            var response = await _client.RequestAsync(HttpMethod.Post, apiEndpoint, request, cancellationToken);

            return await _client.HandleResponseAsync<object>(response);
        }

        public async Task<Response<List<Doc>>> QueryDocAsync(QueryDocRequest queryDocRequest, string collectionName, CancellationToken cancellationToken)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Query });
            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName, functionNames: functionNames);

            var response = await _client.RequestAsync(HttpMethod.Post, apiEndpoint, queryDocRequest, cancellationToken);

            return await _client.HandleResponseAsync<List<Doc>>(response);
        }

        public async Task<Response<object>> UpdateDocAsync(InsertDocRequest updateDocRequest, string collectionName, CancellationToken cancellationToken)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Docs });
            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName, functionNames: functionNames);

            var response = await _client.RequestAsync(HttpMethod.Put, apiEndpoint, updateDocRequest, cancellationToken);

            return await _client.HandleResponseAsync<object>(response);
        }

        public async Task<Response<object>> UpsertDocAsync(InsertDocRequest updateDocRequest, string collectionName, CancellationToken cancellationToken)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Docs, FunctionNames.Upsert });

            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName: collectionName, functionNames: functionNames);

            var response = await _client.RequestAsync(HttpMethod.Post, apiEndpoint, updateDocRequest, cancellationToken);

            return await _client.HandleResponseAsync<object>(response);
        }
    }
}
