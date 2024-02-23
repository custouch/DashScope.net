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

        public async Task<ResponseBase> CreateCollection([FromBody] CreateCollectionRequest request)
        {
            return await _client.CreateCollectionAsync(request);
        }

        [HttpGet("collection/{name}")]
        public async Task<ResponseBase<CollectionMeta>> DescribeCollection(string name)
        {
            return await _client.DescribeCollectionAsync(name);
        }

        [HttpGet("collections")]
        public async Task<ResponseBase<List<string>>> GetCollectionList()
        {
            return await _client.GetCollectionListAsync();
        }

        [HttpGet("collection/{name}/stats")]
        public async Task<ResponseBase<CollectionStats>> StatsCollection(string name)
        {
            return await _client.StatsCollectionAsync(name);
        }

        [HttpDelete("collection/{name}")]
        public async Task<ResponseBase> DeleteCollection(string name)
        {
            return await _client.DeleteCollectionAsync(name);
        }

        #endregion

        #region Doc Controllers


        [HttpPost("doc/{collectionName}/insert")]
        public async Task<ResponseBase> InsertDoc([FromBody] InsertDocRequest request, string collectionName)
        {
            return await _client.InsertDocAsync(request, collectionName);
        }

        [HttpPost("doc/{collectionName}/query")]
        public async Task<ResponseBase<List<Doc>>> QueryDoc([FromBody] QueryDocRequest request, string collectionName)
        {
            return await _client.QueryDocAsync(request, collectionName);
        }

        [HttpPost("doc/{collectionName}/upsert")]
        public async Task<ResponseBase> UpsertDoc([FromBody] UpsertDocRequest request, string collectionName)
        {
            return await _client.UpsertDocAsync(request, collectionName);
        }

        [HttpPost("doc/{collectionName}/update")]
        public async Task<ResponseBase> UpdateDoc([FromBody] UpdateDocRequest request, string collectionName)
        {
            return await _client.UpdateDocAsync(request, collectionName);
        }

        [HttpGet("doc/{collectionName}/fetch")]
        public async Task<ResponseBase<Dictionary<string, Doc>>> FetchDoc([FromQuery] FetchDocRequest request, string collectionName)
        {
            return await _client.FetchDocAsync(request, collectionName);
        }

        [HttpDelete("doc/{collectionName}/delete")]
        public async Task<ResponseBase> DeleteDoc([FromBody] DeleteDocRequest request, string collectionName)
        {
            return await _client.DeleteDocAsync(request, collectionName);
        }

        #endregion


        #region Partitions Controller

        [HttpPost("partition/{collectionName}/create")]
        public async Task<ResponseBase> CreatePartition(CreatePartitionRequest request, string collectionName)
        {
            return await _client.CreatePartitionAsync(request, collectionName);
        }

        [HttpGet("partition/{collectionName}/{partitionName}/describe")]
        public async Task<ResponseBase<string>> DescripbePartion(string collectionName, string partitionName)
        {
            return await _client.DescribePartitionAsync(collectionName, partitionName);
        }

        [HttpGet("partition/{collectionName}/list")]
        public async Task<ResponseBase<List<string>>> ListPartitions(string collectionName)
        {
            return await _client.ListPartitionsAsync(collectionName);
        }

        [HttpGet("partition/{collectionName}/{partitionName}/stats")]
        public async Task<ResponseBase<PartitionStats>> StatPartion(string collectionName, string partitionName)
        {
            return await _client.StatesPartitionAsync(collectionName, partitionName);
        }

        [HttpDelete("partition/{collectionName}/{partitionName}/delete")]
        public async Task<ResponseBase> DeletePartition(string collectionName, string partitionName)
        {
            return await _client.DeletePartitionAsync(collectionName, partitionName);
        }
        #endregion



    }
}
