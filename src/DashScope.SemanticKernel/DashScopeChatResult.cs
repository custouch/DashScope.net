﻿using DashScope.Models;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DashScope.SemanticKernel
{
    public class DashScopeChatResult : IChatStreamingResult, ITextStreamingResult
    {
        private readonly CompletionResponse? _response;
        private readonly IAsyncEnumerable<CompletionResponse>? responses;

        public DashScopeChatResult(CompletionResponse response)
        {
            this._response = response;
            this.ModelResult = new ModelResult(response);
        }

        public DashScopeChatResult(IAsyncEnumerable<CompletionResponse> responses)
        {
            this.responses = responses;
            this.ModelResult = new ModelResult(responses);
        }

        public ModelResult ModelResult { get; private set; }

        public Task<ChatMessageBase> GetChatMessageAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<ChatMessageBase>(new DashScopeChatMessage(AuthorRole.Assistant, _response!.Output.Text));
        }

        public Task<string> GetCompletionAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<string>(_response!.Output.Text);
        }

        public async IAsyncEnumerable<string> GetCompletionStreamingAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var response in responses!)
            {
                yield return response.Output.Text;
            }
        }

        public async IAsyncEnumerable<ChatMessageBase> GetStreamingChatMessageAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var response in responses!)
            {
                yield return new DashScopeChatMessage(AuthorRole.Assistant, response.Output.Text);
            }
        }

    }
    public class DashScopeChatMessage : ChatMessageBase
    {
        public DashScopeChatMessage(AuthorRole role, string content) : base(role, content)
        {
        }
    }
}
