using DashScope.Models;
using Microsoft;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using System.Runtime.CompilerServices;
using Microsoft.SemanticKernel.AI.TextGeneration;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel;

namespace DashScope.SemanticKernel
{
    public class DashScopeTextCompletion : IChatCompletionService, ITextGenerationService
    {
        private readonly string _model;
        private readonly DashScopeClient _client;

        private readonly Dictionary<string, object?> _attributes = new();
        public IReadOnlyDictionary<string, object?> Attributes => this._attributes;

        public DashScopeTextCompletion(string apiKey, string model = DashScopeModels.QWenTurbo, HttpClient? client = null)
        {
            Requires.NotNullOrWhiteSpace(apiKey, nameof(apiKey));
            Requires.NotNullOrWhiteSpace(model, nameof(model));

            this._model = model;

            _client = new DashScopeClient(apiKey, client);
        }

        public ChatHistory CreateNewChat(string? instructions = null)
        {
            var history = new ChatHistory();
            if (!string.IsNullOrWhiteSpace(instructions))
            {
                history.AddSystemMessage(instructions);
            }
            return history;
        }
        public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chat, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            var settings = DashScopeAIRequestSettings.FromRequestSettings(executionSettings);

            var response = await _client.GenerationAsync(new CompletionRequest()
            {
                Input = {
                    Messages = ChatHistoryToMessages(chat),
                },
                Model = this._model,
                Parameters = ToParameters(settings)
            }, cancellationToken);

            return new List<ChatMessageContent>() { new DashScopeChatMessage(response) }.AsReadOnly();
        }

        public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var settings = DashScopeAIRequestSettings.FromRequestSettings(executionSettings);

            var responses = _client.GenerationStreamAsync(new CompletionRequest()
            {
                Input =
                {
                    Messages = ChatHistoryToMessages(chatHistory),
                },
                Parameters = ToParameters(settings, true),
                Model = this._model
            }, cancellationToken);
            await foreach (var response in responses)
            {
                yield return new DashScopeStreamingChatMessage(response);
            }

        }

        public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            var settings = DashScopeAIRequestSettings.FromRequestSettings(executionSettings);

            var response = await _client.GenerationAsync(new CompletionRequest()
            {
                Input = {
                    Messages =
                    [
                        new Message()
                        {
                            Role = MessageRole.User,
                            Content = prompt
                        }
                    ]
                },
                Model = this._model,
                Parameters = ToParameters(settings)
            }, cancellationToken);

            return new List<TextContent>() { new(response.Output.Text) }.AsReadOnly();
        }

        public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var settings = DashScopeAIRequestSettings.FromRequestSettings(executionSettings);

            var responses = _client.GenerationStreamAsync(new CompletionRequest()
            {
                Input =
                {
                    Messages =
                    [
                        new Message()
                        {
                            Role = MessageRole.User,
                            Content = prompt
                        }
                    ]
                },
                Parameters = ToParameters(settings, true),
                Model = this._model
            }, cancellationToken);
            await foreach (var response in responses)
            {
                yield return new StreamingTextContent(response.Output.Text);
            }

        }

        private static CompletionParameters ToParameters(DashScopeAIRequestSettings? settings, bool? stream = null)
        {
            if (settings == null)
            {
                return new CompletionParameters();
            }

            return new CompletionParameters()
            {
                TopP = settings.TopP,
                Temperature = settings.Temperature,
                TopK = settings.TopK,
                Seed = settings.Seed,
                IncrementalOutput = stream,
                EnableSearch = settings.EnableSearch,
                ResultFormat = "text"
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