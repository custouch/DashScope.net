using System;
using System.Collections;
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
        [ClassData(typeof(DefaultsData))]
        public void GetApiEndpoint_ReturnsCorrectUrl(string expectedUrl, string endpoint, string apiVersion = "v1", string? collectionName = null, string[]? functionNames = null, Dictionary<string, string>? query = null)
        {
            var result = Defaults.GetApiEndpoint(endpoint, apiVersion, collectionName, functionNames != null ? new List<string>(functionNames) : null, query);

            Assert.Equal(expectedUrl, result);
        }
    }

    public class DefaultsData : IEnumerable<object[]>
    {
        private List<object?[]> data = [
            ["https://example.com/v1/collections", "example.com", "v1", null, null, null],
            ["https://example.com/v1/collections/collectionName", "example.com", "v1", "collectionName", null, null],
            ["https://example.com/v1/collections/collectionName/stats", "example.com", "v1", "collectionName", new string[] { "stats" }, null],
            ["https://example.com/v1/collections/collectionName/docs/upsert", "example.com", "v1", "collectionName", new string[] { "docs", "upsert" }, null],
            ["https://example.com/v1/collections/collectionName/docs?ids=id1,id2&partition=partitionName", "example.com", "v1", "collectionName", new[] { "docs" }, new Dictionary<string, string>() { ["ids"] = "id1,id2", ["partition"] = "partitionName" }]
            ];
        public IEnumerator<object[]> GetEnumerator() => this.data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
