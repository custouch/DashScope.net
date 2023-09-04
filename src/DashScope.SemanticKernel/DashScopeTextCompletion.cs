using DashScope.Models;
using Microsoft;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System.Runtime.CompilerServices;

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

        public async Task<IReadOnlyList<IChatResult>> GetChatCompletionsAsync(ChatHistory chat, ChatRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
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

        public async Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
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

        public async IAsyncEnumerable<IChatStreamingResult> GetStreamingChatCompletionsAsync(ChatHistory chat, ChatRequestSettings? requestSettings = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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

        public async IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, CompleteRequestSettings requestSettings, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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
                    TopK = Math.Max((int)(settings.Temperature * 100 % 100), 1),
                    TopP = (float)settings.TopP,
                };
            }
        }

        private CompletionParameters ToParameters(CompleteRequestSettings settings)
        {
            return new CompletionParameters()
            {
                TopK = Math.Max((int)(settings.Temperature * 100 % 100), 1),
                TopP = (float)settings.TopP,
            };
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