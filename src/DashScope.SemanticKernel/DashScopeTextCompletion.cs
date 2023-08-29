﻿using DashScope.Models;
using Microsoft;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DashScope.SemanticKernel
{
    public class DashScopeTextCompletion : IChatCompletion, ITextCompletion
    {
        private readonly string _model;
        private readonly DashScopeClient _client;

        public DashScopeTextCompletion(string apiKey, string model = DashScopeModels.QWenV1, HttpClient? client = null)
        {
            Requires.NotNullOrWhiteSpace(apiKey, nameof(apiKey));
            Requires.NotNullOrWhiteSpace(model, nameof(model));

            this._model = model;

            _client = new DashScopeClient(apiKey, client);
        }

        /// <summary>
        /// Unsupported instructions
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        public ChatHistory CreateNewChat(string? instructions = null)
        {
            return new ChatHistory();
        }

        public async Task<IReadOnlyList<IChatResult>> GetChatCompletionsAsync(ChatHistory chat, ChatRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
        {
            var response = await _client.GenerationAsync(new CompletionRequest()
            {
                Input = {
                    Prompt = chat.Last().Content,
                    History = ToHistory(chat.Take(chat.Count - 1)).ToList()
                },
                Model = this._model,
                Parameters = ToParameters(requestSettings)
            }, cancellationToken);

            return new List<DashScopeChatResult>() { new DashScopeChatResult(response) }.AsReadOnly();
        }

        public async Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
        {
            var response = await _client.GenerationAsync(new CompletionRequest()
            {
                Input = {
                    Prompt = text,
                },
                Model = this._model,
                Parameters = ToParameters(requestSettings)
            }, cancellationToken);

            return new List<DashScopeChatResult>() { new DashScopeChatResult(response) }.AsReadOnly();

        }

        public async IAsyncEnumerable<IChatStreamingResult> GetStreamingChatCompletionsAsync(ChatHistory chat, ChatRequestSettings? requestSettings = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responses = _client.GenerationStreamAsync(new CompletionRequest()
            {
                Input =
                {
                    Prompt = chat.Last().Content,
                    History = ToHistory(chat.Take(chat.Count - 1)).ToList()
                },
                Parameters = ToParameters(requestSettings),
                Model = this._model
            }, cancellationToken);
            var lastTextIndex = 0;
            await foreach (var result in responses)
            {
                lastTextIndex += result.Output.Text.Length;
                yield return new DashScopeChatResult(result);
            }
        }

        public async IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, CompleteRequestSettings requestSettings, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responses = _client.GenerationStreamAsync(new CompletionRequest()
            {
                Input =
                {
                    Prompt = text,
                },
                Parameters = ToParameters(requestSettings),
                Model = this._model
            }, cancellationToken);
            var lastTextIndex = 0;
            await foreach (var result in responses)
            {
                lastTextIndex += result.Output.Text.Length;
                yield return new DashScopeChatResult(result);
            }
        }

        private static IEnumerable<CompletionHistoryItem> ToHistory(IEnumerable<ChatMessageBase> history)
        {
            var item = new CompletionHistoryItem();
            foreach (var historyItem in history)
            {
                if (historyItem.Role == AuthorRole.User)
                {
                    item.User = historyItem.Content;
                }
                if (historyItem.Role == AuthorRole.Assistant)
                {
                    item.Bot = historyItem.Content;
                    yield return item;
                    item = new CompletionHistoryItem();
                }
            }
        }

        private static CompletionParameters ToParameters(ChatRequestSettings? settings)
        {
            if (settings == null)
            {
                return new CompletionParameters();
            }
            else
            {
                return new CompletionParameters()
                {
                    TopK = (int)(settings.Temperature * 100 % 100),
                    TopP = (float)settings.TopP,
                };
            }
        }


        private CompletionParameters ToParameters(CompleteRequestSettings settings)
        {
            return new CompletionParameters()
            {
                TopK = (int)(settings.Temperature * 100 % 100),
                TopP = (float)settings.TopP,
            };
        }

    }
}
