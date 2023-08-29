using Microsoft;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashScope.SemanticKernel
{
    public class DashScopeTextCompletion : IChatCompletion, ITextCompletion
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly DashScopeClient _client;

        public DashScopeTextCompletion(string apiKey, string model = DashScopeModels.QWenV1, HttpClient? client = null)
        {
            Requires.NotNullOrWhiteSpace(apiKey, nameof(apiKey));
            Requires.NotNullOrWhiteSpace(model, nameof(model));

            this._apiKey = apiKey;
            this._model = model;

            _client = new DashScopeClient(apiKey, client);
        }
        public ChatHistory CreateNewChat(string? instructions = null)
        {
            return new ChatHistory();
        }

        public Task<IReadOnlyList<IChatResult>> GetChatCompletionsAsync(ChatHistory chat, ChatRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
        {
            var response = await _client.GenerationAsync(new Models.CompletionRequest()
            {
                Input = {
                    Prompt = text,
                },
                Model = this._model,
                Parameters =
                {
                     TopP = (float)requestSettings.TopP
                }
            });

            return new List<DashScopeChatResult>() { new DashScopeChatResult(response) }.AsReadOnly();

        }

        public IAsyncEnumerable<IChatStreamingResult> GetStreamingChatCompletionsAsync(ChatHistory chat, ChatRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
