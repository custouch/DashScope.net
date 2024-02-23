using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashVector
{
    public class Defaults
    {

        /// <summary>
        /// 获取API Endpoint
        /// </summary>
        public static string GetApiEndpoint(string endpoint, string apiVersion = "v1", string? collectionName = null, List<string>? functionNames = null, Dictionary<string, string>? query = null)
        {
            StringBuilder urlBuilder = new($"https://{endpoint}/{apiVersion}/collections");

            var path = new List<string>();
            if (!string.IsNullOrEmpty(collectionName))
            {
                path.Add(collectionName);
            }

            if (functionNames?.Count > 0)
            {
                path.AddRange(functionNames);
            }

            if (path.Count > 0)
            {
                urlBuilder.Append($"/{string.Join("/", path)}");
            }

            if (query?.Count > 0)
            {
                urlBuilder.Append($"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}"))}");
            }


            return urlBuilder.ToString();
        }
    }
}
