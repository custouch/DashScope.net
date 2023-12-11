using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DashScope.Models
{
    public class CompletionRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = DashScopeModels.QWenTurbo;

        [JsonPropertyName("input")]
        public CompletionInput Input { get; set; } = new();

        [JsonPropertyName("parameters")]
        public CompletionParameters Parameters { get; set; } = new();
    }
    public class CompletionInput
    {

        [JsonPropertyName("prompt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Prompt { get; set; }

        [JsonPropertyName("messages")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Message>? Messages { get; set; }

    }
    public class CompletionHistoryItem
    {
        public string User { get; set; } = string.Empty;
        public string Bot { get; set; } = string.Empty;
    }

    public class CompletionParameters
    {
        /// <summary>
        /// 生成时，核采样方法的概率阈值。例如，取值为0.5时，仅保留累计概率之和大于等于0.5的概率分布中的token，作为随机采样的候选集。取值范围为(0,1.0)，取值越大，生成的随机性越高；取值越低，生成的随机性越低。默认值 0.5。注意，取值不要大于等于1
        /// </summary>
        [JsonPropertyName("top_p")]
        public float? TopP { get; set; } = 0.5f;

        /// <summary>
        /// 默认值为null，表示不启用top_k策略，取值范围：[1, 100]，生成时，采样候选集的大小。例如，取值为50时，仅将单次生成中得分最高的50个token组成随机采样的候选集。取值越大，生成的随机性越高；取值越小，生成的确定性越高。注意：如果top_k的值大于100，则不会启用top_k策略，此时仅有top_p策略生效。
        /// </summary>
        [JsonPropertyName("top_k")]
        public int? TopK { get; set; }

        /// <summary>
        /// 生成时，随机数的种子，用于控制模型生成的随机性。如果使用相同的种子，每次运行生成的结果都将相同；当需要复现模型的生成结果时，可以使用相同的种子。seed参数支持无符号64位整数类型。默认值 1234
        /// </summary>
        [JsonPropertyName("seed")]
        public long? Seed { get; set; } = 1234;

        /// <summary>
        /// 生成时，是否参考夸克搜索的结果。注意：打开搜索并不意味着一定会使用搜索结果；如果打开搜索，模型会将搜索结果作为prompt，进而“自行判断”是否生成结合搜索结果的文本，默认为false
        /// </summary>
        [JsonPropertyName("enable_search")]
        public bool? EnableSearch { get; set; } = false;

        /// <summary>
        /// "text"表示旧版本的text，"message"表示兼容openai的message
        /// </summary>
        [JsonPropertyName("result_format")]
        public string? ResultFormat { get; set; }

        /// <summary>
        ///用于控制随机性和多样性的程度。具体来说，temperature值控制了生成文本时对每个候选词的概率分布进行平滑的程度。较高的temperature值会降低概率分布的峰值，使得更多的低概率词被选择，生成结果更加多样化；而较低的temperature值则会增强概率分布的峰值，使得高概率词更容易被选择，生成结果更加确定。取值范围： (0, 2),系统默认值1.0
        /// </summary>
        [JsonPropertyName("temperature")]
        public float? Temperature { get; set; } = 1.0f;

        /// <summary>
        /// 控制流式输出模式，默认False，即后面内容会包含已经输出的内容；设置为True，将开启增量输出模式，后面输出不会包含已经输出的内容，您需要自行拼接整体输出
        /// <example> 流式输出示例代码
        /// <code>
        /// 值为False时:
        /// I
        /// I like
        /// I like apple
        /// 值为True时:
        /// I
        /// like
        /// apple
        /// </code>
        /// </example>
        /// </summary>
        [JsonPropertyName("incremental_output")]
        public bool? IncrementalOutput { get; set; } = false;
    }

    public class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

    }

    public static class MessageRole
    {
        public const string User = "user";
        public const string Assistant = "assistant";
        public const string System = "system";
    }
}