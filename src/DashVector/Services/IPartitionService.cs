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
    public interface IPartitionService
    {
        public Task<Response<object>> CreatePartitionAsync(string collectionName, CancellationToken cancellationToken);

        public Task<Response<Status>> DescribePartitionAsync(string collectionName, string partitionName, CancellationToken cancellationToken);

        public Task<Response<List<string>>> ListPartitions(string collectionName, CancellationToken cancellationTokenn);

        public Task<Response<PartitionStats>> StatesPartition(string collectionName, string partitionName, CancellationToken cancellationToken);

        public Task<Response<object>> DeletePartition(string collectionName, string partitionName, CancellationToken cancellationToken);
    }
}
