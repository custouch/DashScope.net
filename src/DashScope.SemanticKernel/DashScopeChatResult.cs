using DashScope.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DashScope.SemanticKernel
{
    public class DashScopeChatMessage : ChatMessageContent
    {
        public DashScopeChatMessage(CompletionResponse response, IReadOnlyDictionary<string, object?>? metadata = null)
            : base(AuthorRole.Assistant, response.Output.Text!, metadata: metadata)
        {

        }
    }
    public class DashScopeStreamingChatMessage : StreamingChatMessageContent
    {
        public DashScopeStreamingChatMessage(CompletionResponse response, IReadOnlyDictionary<string, object?>? metadata = null)
            : base(AuthorRole.Assistant, response.Output.Text!, response, metadata: metadata)
        {

        }
    }
}
