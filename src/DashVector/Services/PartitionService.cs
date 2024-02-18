using DashVector.Enums;
using DashVector.Models;
using DashVector.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashVector.Requests
{
    public class PartitionService : IPartitionService
    {
        private readonly DashVectorClient _client;

        public PartitionService(DashVectorClient client)
        {
            this._client = client;
        }

        public async Task<Response<object>> CreatePartitionAsync(string collectionName, CancellationToken cancellationToken)
        {
            List<string> functionName = new List<string>(new string[] { FunctionNames.Partitions });

            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName, functionNames: functionName);

            var response = await _client.RequestAsync(HttpMethod.Post, apiEndpoint, cancellationToken);

            return await _client.HandleResponseAsync<object>(response);
        }

        public async Task<Response<object>> DeletePartition(string collectionName, string partitionName, CancellationToken cancellationToken)
        {
            List<string> functionName = new List<string>(new string[] { FunctionNames.Partitions, partitionName });

            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName, functionNames: functionName);

            var response = await _client.RequestAsync(HttpMethod.Delete, apiEndpoint, cancellationToken);

            return await _client.HandleResponseAsync<object>(response);
        }

        public async Task<Response<Status>> DescribePartitionAsync(string collectionName, string partitionName, CancellationToken cancellationToken)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Partitions, partitionName });
            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName, functionNames: functionNames);

            var response = await _client.RequestAsync(HttpMethod.Get, apiEndpoint, cancellationToken);

            return await _client.HandleResponseAsync<Status>(response);
        }

        public async Task<Response<List<string>>> ListPartitions(string collectionName, CancellationToken cancellationTokenn)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Partitions });
            var apiEndpoint = Defaults.GetApiEndpoint(this._client._endPoint, collectionName, functionNames: functionNames);

            var response = await _client.RequestAsync(HttpMethod.Get, apiEndpoint, apiEndpoint, cancellationTokenn);

            return await _client.HandleResponseAsync<List<string>>(response);
        }

        public async Task<Response<PartitionStats>> StatesPartition(string collectionName, string partitionName, CancellationToken cancellationToken)
        {
            List<string> functionNames = new List<string>(new string[] { FunctionNames.Partitions, partitionName, FunctionNames.Stats });
            var apiEndpoint = Defaults.GetApiEndpoint(_client._endPoint, collectionName, functionNames: functionNames);

            var response = await _client.RequestAsync(HttpMethod.Get, apiEndpoint, cancellationToken);

            return await _client.HandleResponseAsync<PartitionStats>(response);
        }
    }
}
