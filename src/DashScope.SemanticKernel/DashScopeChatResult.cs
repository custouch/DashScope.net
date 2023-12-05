using DashScope.Models;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
