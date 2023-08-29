using DashScope.Models;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashScope.SemanticKernel
{
    public class DashScopeChatResult : IChatStreamingResult, ITextStreamingResult
    {
        private readonly CompletionResponse _response;

        public DashScopeChatResult(CompletionResponse response)
        {
            this._response = response;
            this.ModelResult = new ModelResult(response);
        }

        public ModelResult ModelResult { get; private set; }

        public Task<ChatMessageBase> GetChatMessageAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCompletionAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<string> GetCompletionStreamingAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<ChatMessageBase> GetStreamingChatMessageAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}
