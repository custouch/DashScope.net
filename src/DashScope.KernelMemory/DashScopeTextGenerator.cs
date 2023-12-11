using DashScope.Models;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory.AI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DashScope.KernelMemory
{
    /// <summary>
    /// Implementation of <see cref="ITextGenerator"/> using DashScope.
    /// </summary>
    public class DashScopeTextGenerator : ITextGenerator
    {
        private readonly DashScopeClient _client;
        private readonly DashScopeOptions _options;

        public DashScopeTextGenerator(DashScopeClient client, DashScopeOptions options)
        {
            this._client = client;
            this._options = options;
        }

        public int MaxTokenTotal => this._options.MaxTokenTotals;

        public int CountTokens(string text)
        {
            return
               this._client.TokenCountsAsync(new Models.TokenizerRequest()
               {
                   Model = _options.Model,
                   Input =
                {
                    Messages =
                     [
                         new Message()
                         {
                            Role =   MessageRole.User,
                            Content = text
                         }
                     ]
                }
               }).ConfigureAwait(false).GetAwaiter().GetResult().InputTokens;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<string> GenerateTextAsync(string prompt, TextGenerationOptions options, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var streamResult = _client.GenerationStreamAsync(new Models.CompletionRequest()
            {
                Input = new Models.CompletionInput()
                {
                    Messages =
                     [
                         new Models.Message()
                         {
                             Role = MessageRole.User,
                             Content = prompt
                         }
                     ]
                },
                Model = _options.Model,
                Parameters = MergeCompletionParameters(options, _options.DefaultCompletionParameters)
            }, cancellationToken);

            await foreach (var result in streamResult)
            {
                if (result.IsTextResponse)
                {
                    yield return result.Output.Text!;
                }
                else
                {
                    yield return result.Output.Choices![0].Message.Content;
                }
            }
        }

        private CompletionParameters MergeCompletionParameters(TextGenerationOptions generationOptions, CompletionParameters? defaultCompletionParameters)
        {
            defaultCompletionParameters ??= new CompletionParameters();

            if (generationOptions.Temperature != default)
            {
                defaultCompletionParameters.Temperature = (float)generationOptions.Temperature;
            }

            if (generationOptions.TopP != default)
            {
                defaultCompletionParameters.TopP = (float)generationOptions.TopP;
            }

            defaultCompletionParameters.IncrementalOutput = true;
            defaultCompletionParameters.ResultFormat = "text";

            return defaultCompletionParameters;
        }

    }
}
