using DashVector.Enums;
using DashVector.Models;
using DashVector.Models.Requests;
using DashVector.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DashVector.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ApiController : ControllerBase
    {
        private readonly DashVectorClient _client;

        public ApiController(DashVectorClient client)
        {
            _client = client;
        }

        #region Collection Controllers
        [HttpPost("collection")]

        public async Task<ResponseBase> CreateCollectionAsync([FromBody] CreateCollectionRequest request)
        {
            return await _client.CreateCollectionAsync(request).ConfigureAwait(false);
        }

        [HttpGet("collection/{name}")]
        public async Task<ResponseBase<CollectionMeta>> DescribeCollectionAsync(string name)
        {
            return await _client.DescribeCollectionAsync(name).ConfigureAwait(false);
        }

        [HttpGet("collections")]
        public async Task<ResponseBase<List<string>>> GetCollectionListAsync()
        {
            return await _client.GetCollectionListAsync().ConfigureAwait(false);
        }

        [HttpGet("collection/{name}/stats")]
        public async Task<ResponseBase<CollectionStats>> StatsCollectionAsync(string name)
        {
            return await _client.StatsCollectionAsync(name).ConfigureAwait(false);
        }

        [HttpDelete("collection/{name}")]
        public async Task<ResponseBase> DeleteCollectionAsync(string name)
        {
            return await _client.DeleteCollectionAsync(name).ConfigureAwait(false);
        }

        #endregion

        #region Doc Controllers


        [HttpPost("doc/{collectionName}/insert")]
        public async Task<ResponseBase> InsertDocAsync([FromBody] InsertDocRequest request, string collectionName)
        {
            return await _client.InsertDocAsync(request, collectionName).ConfigureAwait(false);
        }

        [HttpPost("doc/{collectionName}/query")]
        public async Task<ResponseBase<List<Doc>>> QueryDocAsync([FromBody] QueryDocRequest request, string collectionName)
        {
            return await _client.QueryDocAsync(request, collectionName).ConfigureAwait(false);
        }

        [HttpPost("doc/{collectionName}/upsert")]
        public async Task<ResponseBase> UpsertDocAsync([FromBody] UpsertDocRequest request, string collectionName)
        {
            return await _client.UpsertDocAsync(request, collectionName).ConfigureAwait(false);
        }

        [HttpPost("doc/{collectionName}/update")]
        public async Task<ResponseBase> UpdateDocAsync([FromBody] UpdateDocRequest request, string collectionName)
        {
            return await _client.UpdateDocAsync(request, collectionName).ConfigureAwait(false);
        }

        [HttpGet("doc/{collectionName}/fetch")]
        public async Task<ResponseBase<Dictionary<string, Doc>>> FetchDocAsync([FromQuery] FetchDocRequest request, string collectionName)
        {
            return await _client.FetchDocAsync(request, collectionName).ConfigureAwait(false);
        }

        [HttpDelete("doc/{collectionName}/delete")]
        public async Task<ResponseBase> DeleteDocAsync([FromBody] DeleteDocRequest request, string collectionName)
        {
            return await _client.DeleteDocAsync(request, collectionName).ConfigureAwait(false);
        }

        #endregion


        #region Partitions Controller

        [HttpPost("partition/{collectionName}/create")]
        public async Task<ResponseBase> CreatePartitionAsync(CreatePartitionRequest request, string collectionName)
        {
            return await _client.CreatePartitionAsync(request, collectionName).ConfigureAwait(false);
        }

        [HttpGet("partition/{collectionName}/{partitionName}/describe")]
        public async Task<ResponseBase<string>> DescripbePartionAsync(string collectionName, string partitionName)
        {
            return await _client.DescribePartitionAsync(collectionName, partitionName).ConfigureAwait(false);
        }

        [HttpGet("partition/{collectionName}/list")]
        public async Task<ResponseBase<List<string>>> ListPartitionsAsync(string collectionName)
        {
            return await _client.ListPartitionsAsync(collectionName).ConfigureAwait(false);
        }

        [HttpGet("partition/{collectionName}/{partitionName}/stats")]
        public async Task<ResponseBase<PartitionStats>> StatPartionAsync(string collectionName, string partitionName)
        {
            return await _client.StatesPartitionAsync(collectionName, partitionName).ConfigureAwait(false);
        }

        [HttpDelete("partition/{collectionName}/{partitionName}/delete")]
        public async Task<ResponseBase> DeletePartitionAsync(string collectionName, string partitionName)
        {
            return await _client.DeletePartitionAsync(collectionName, partitionName).ConfigureAwait(false);
        }
        #endregion



    }
}
