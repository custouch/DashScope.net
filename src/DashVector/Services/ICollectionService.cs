using DashVector.Models;
using DashVector.Models.Requests;
using DashVector.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DashVector.Requests
{
    public interface ICollectionService
    {
        public Task<Response<object>> CreateCollectionAsync(CreateCollectionRequest request, CancellationToken cancellationToken = default);

        public Task<Response<CollectionMeta>> DescribeCollectionAsync(string name, CancellationToken cancellationToken);

        public Task<Response<List<string>>> GetCollectionListAsync();

        public Task<Response<CollectionStats>> StatsCollectionAsync(string name, CancellationToken cancellationToken);

        public Task<Response<object>> DeleteCollectionAsync(string name);

    }
}
