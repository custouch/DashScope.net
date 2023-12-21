using DashScope.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DashScope.SemanticKernel
{
    public class DashScopeChatMessage : ChatMessageContent
    {
        public DashScopeChatMessage(CompletionResponse response) : base(AuthorRole.Assistant, response.Output.Text!)
        {

        }
    }
    public class DashScopeStreamingChatMessage : StreamingChatMessageContent
    {
        public DashScopeStreamingChatMessage(CompletionResponse response) : base(AuthorRole.Assistant, response.Output.Text!, response)
        {

        }
    }
}
