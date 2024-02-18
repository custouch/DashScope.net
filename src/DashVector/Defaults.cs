using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashVector
{
    public class Defaults
    {
        public static string ApiBase = "https:/";

        /// <summary>
        /// 获取API
        /// </summary>
        public static string GetApiEndpoint(string endpoint, string apiVersion = "v1", string? collectionName = null, List<string>? functionNames = null, List<string>? ids = null, string? partitionName = null)
        {
            StringBuilder urlBuilder = new StringBuilder($"{ApiBase}/{endpoint}/{apiVersion}/collections");

            if (!string.IsNullOrEmpty(collectionName))
            {
                urlBuilder.Append($"/{collectionName}");

                if (functionNames != null && functionNames.Count > 0)
                {
                    foreach (var function in functionNames)
                    {
                        urlBuilder.Append($"/{function}");
                    }
                }
                else if (ids != null)
                {
                    urlBuilder.Append($"/docs?ids={string.Join(",", ids)}");
                    if (!string.IsNullOrEmpty(partitionName))
                    {
                        urlBuilder.Append($"&partition={partitionName}");
                    }
                }
            }
            else if (functionNames != null && functionNames.Count > 0)
            {
                foreach (var function in functionNames)
                {
                    urlBuilder.Append($"/{function}");
                }
            }

            return urlBuilder.ToString();
        }
    }

}
