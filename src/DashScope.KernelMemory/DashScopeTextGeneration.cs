using DashScope.Models;
using Microsoft.KernelMemory.AI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace DashScope.KernelMemory
{
    /// <summary>
    /// Implementation of <see cref="ITextGeneration"/> using DashScope.
    /// </summary>
    public class DashScopeTextGeneration : ITextGeneration
    {
        private readonly DashScopeClient _client;
        private readonly DashScopeOptions _options;

        public DashScopeTextGeneration(DashScopeClient client, DashScopeOptions options)
        {
            this._client = client;
            this._options = options;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<string> GenerateTextAsync(string prompt, TextGenerationOptions options, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var streamResult = _client.GenerationStreamAsync(new Models.CompletionRequest()
            {
                Input = new Models.CompletionInput()
                {
                    Messages =
                     {
                         new Models.Message()
                         {
                            Role =   MessageRole.User,
                            Content = prompt
                         }
                     }
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
