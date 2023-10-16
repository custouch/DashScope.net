﻿using DashScope.Models;
using Microsoft;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System.Runtime.CompilerServices;
using Microsoft.SemanticKernel.AI;

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
            var history = new ChatHistory();
            if (!string.IsNullOrWhiteSpace(instructions))
            {
                history.AddSystemMessage(instructions);
            }
            return history;
        }

        public async Task<IReadOnlyList<IChatResult>> GetChatCompletionsAsync(ChatHistory chat, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
        {
            var response = await _client.GenerationAsync(new CompletionRequest()
            {
                Input = {
                    Messages = ChatHistoryToMessages(chat),
                },
                Model = this._model,
                Parameters = ToParameters(requestSettings)
            }, cancellationToken);

            return new List<DashScopeChatResult>() { new DashScopeChatResult(response) }.AsReadOnly();
        }

        public async Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, AIRequestSettings? requestSettings, CancellationToken cancellationToken = default)
        {
            var response = await _client.GenerationAsync(new CompletionRequest()
            {
                Input = {
                    Messages = new List<Message>()
                    {
                        new Message()
                        {
                            Role = MessageRole.User,
                            Content = text
                        }
                    }
                },
                Model = this._model,
                Parameters = ToParameters(requestSettings)
            }, cancellationToken);

            return new List<DashScopeChatResult>() { new DashScopeChatResult(response) }.AsReadOnly();
        }

        public async IAsyncEnumerable<IChatStreamingResult> GetStreamingChatCompletionsAsync(ChatHistory chat, AIRequestSettings? requestSettings = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responses = _client.GenerationStreamAsync(new CompletionRequest()
            {
                Input =
                {
                    Messages = ChatHistoryToMessages(chat),
                },
                Parameters = ToParameters(requestSettings),
                Model = this._model
            }, cancellationToken);
            yield return new DashScopeChatResult(responses);
            await Task.CompletedTask;
        }

        public async IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, AIRequestSettings? requestSettings, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var responses = _client.GenerationStreamAsync(new CompletionRequest()
            {
                Input =
                {
                    Messages = new List<Message>()
                    {
                        new Message()
                        {
                            Role = MessageRole.User,
                            Content = text
                        }
                    }
                },
                Parameters = ToParameters(requestSettings),
                Model = this._model
            }, cancellationToken);
            yield return new DashScopeChatResult(responses);
            await Task.CompletedTask;
        }

        private static CompletionParameters ToParameters(AIRequestSettings? settings)
        {
            if (settings == null)
            {
                return new CompletionParameters();
            }
            else
            {
                // 因为新版已弃用且删除 ChatRequestSettings，统一改用AIRequestSettings
                // 需要自己从 ExtensionData 里通过 key 取数据
                // 而不是像以前一样通过属性访问
                var parameters = new CompletionParameters();
                
                if (settings.ExtensionData.TryGetValue("temperature", out object temperature))
                {
                    parameters.TopK = Math.Max((int)((double)temperature * 100 % 100), 1);
                }

                if (settings.ExtensionData.TryGetValue("top_p", out object topP))
                {
                    parameters.TopP = (float)(double)topP;
                }

                return parameters;
            }
        }

        private List<Message> ChatHistoryToMessages(ChatHistory chatHistory)
        {
            return chatHistory.Select(m => new Message()
            {
                Role = AuthorRoleToMessageRole(m.Role),
                Content = m.Content
            }).ToList();
        }

        private static readonly Dictionary<AuthorRole, string> AuthorRoleToMessageRoleMap = new Dictionary<AuthorRole, string>
        {
            { AuthorRole.User, MessageRole.User },
            { AuthorRole.Assistant, MessageRole.Assistant },
            { AuthorRole.System, MessageRole.System }
        };

        private string AuthorRoleToMessageRole(AuthorRole role)
        {
            if (AuthorRoleToMessageRoleMap.TryGetValue(role, out var messageRole))
            {
                return messageRole;
            }
            return MessageRole.User;
        }
    }
}