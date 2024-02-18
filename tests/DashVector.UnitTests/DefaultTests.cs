using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DashVector.UnitTests
{

    public class DefaultTests
    {
        [Theory]
        [InlineData("https://example.com/v1/collections", "example.com", "v1", null, null, null, null)]
        [InlineData("https://example.com/v1/collections/collectionName", "example.com", "v1", "collectionName", null, null, null)]
        [InlineData("https://example.com/v1/collections/collectionName/stats", "example.com", "v1", "collectionName", new string[] { "stats" }, null, null)]
        [InlineData("https://example.com/v1/collections/collectionName/docs/upsert", "example.com", "v1", "collectionName", new string[] { "docs", "upsert" }, null, null)]
        [InlineData("https://example.com/v1/collections/collectionName/docs?ids=id1,id2&partition=partitionName", "example.com", "v1", "collectionName", null, new string[] { "id1", "id2" }, "partitionName")]
        public void GetApiEndpoint_ReturnsCorrectUrl(string expectedUrl, string endpoint, string? apiVersion = "v1", string? collectionName = null, string[]? functionNames = null, string[]? ids = null, string? partitionName = null)
        {
            var result = Defaults.GetApiEndpoint(endpoint, apiVersion, collectionName, functionNames != null ? new List<string>(functionNames) : null, ids != null ? new List<string>(ids) : null, partitionName);

            Assert.Equal(expectedUrl, result);
        }
    }
}
