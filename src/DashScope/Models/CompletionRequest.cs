using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DashScope.Models
{
    public class CompletionRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = DashScopeModels.QWenV1;

        [JsonPropertyName("input")]
        public CompletionInput Input { get; set; } = new();

        [JsonPropertyName("parameters")]
        public CompletionParameters Parameters { get; set; } = new();
    }
    public class CompletionInput
    {
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;

        [JsonPropertyName("history")]
        public List<CompletionHistoryItem> History { get; set; } = new List<CompletionHistoryItem>();

    }
    public class CompletionHistoryItem
    {
        public string User { get; set; } = string.Empty;
        public string Bot { get; set; } = string.Empty;
    }

    public class CompletionParameters
    {
        /// <summary>
        /// 生成时，核采样方法的概率阈值。例如，取值为0.8时，仅保留累计概率之和大于等于0.8的概率分布中的token，作为随机采样的候选集。取值范围为(0,1.0)，取值越大，生成的随机性越高；取值越低，生成的随机性越低。默认值 0.8。注意，取值不要大于等于1
        /// </summary>
        [JsonPropertyName("top_p")]
        public float? TopP { get; set; } = 0.8f;

        /// <summary>
        /// 生成时，采样候选集的大小。例如，取值为50时，仅将单次生成中得分最高的50个token组成随机采样的候选集。取值越大，生成的随机性越高；取值越小，生成的确定性越高。注意：如果top_k的值大于100，top_k将采用默认值100
        /// </summary>
        [JsonPropertyName("top_k")]
        public int? TopK { get; set; }

        /// <summary>
        /// 生成时，随机数的种子，用于控制模型生成的随机性。如果使用相同的种子，每次运行生成的结果都将相同；当需要复现模型的生成结果时，可以使用相同的种子。
        /// </summary>
        [JsonPropertyName("seed")]
        public long? Seed { get; set; } = 1234;

        /// <summary>
        /// 生成时，是否参考夸克搜索的结果。
        /// </summary>
        [JsonPropertyName("enable_search")]
        public bool? EnableSearch { get; set; } = false;
        /// <summary>
        /// 接口输入和输出的信息是否通过绿网过滤，默认不调用绿网。
        /// </summary>
        [JsonIgnore]
        public bool? DataInspection { get; set; }
    }
}
