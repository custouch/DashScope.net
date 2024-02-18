using DashVector.Models;
using DashVector.Models.Requests;
using DashVector.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashVector.Requests
{
    public interface IDocService
    {
        public Task<Response<object>> InsertDocAsync(InsertDocRequest request, string collectionName, CancellationToken cancellationToken);

        public Task<Response<List<Doc>>> QueryDocAsync(QueryDocRequest queryDocRequest, string collectionName, CancellationToken cancellationToken);

        public Task<Response<object>> UpsertDocAsync(InsertDocRequest updateDocRequest, string collectionName, CancellationToken cancellationToken);

        public Task<Response<object>> UpdateDocAsync(InsertDocRequest updateDocRequest, string collectionName, CancellationToken cancellationToken);

        public Task<Response<Dictionary<string, Doc>>> FetchDocAsync(FetchDocRequest fetchDocRequest, string collectionName, CancellationToken cancellationToken);

        public Task<Response<object>> DeleteDocAsync(DeleteDocRequest deleteDocRequest, string collectionName, CancellationToken cancellationToken);
    }
}
