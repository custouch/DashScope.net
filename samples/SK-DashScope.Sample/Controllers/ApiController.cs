using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using SK_DashScope.Sample.Models;
using System.Text;
using DashScope.SemanticKernel;
using Microsoft.SemanticKernel.TextGeneration;

namespace SK_DashScope.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly Kernel kernel;

        public ApiController(Kernel kernel)
        {
            this.kernel = kernel;
        }

        [HttpPost]
        public async Task<IActionResult> ChatAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var chat = kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();
            history.AddUserMessage(input.Text);

            var result = await chat.GetChatMessageContentAsync(history);
            return Ok(result.Content);
        }

        [HttpPost("text")]
        public async Task<IActionResult> TextAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var completion = kernel.GetRequiredService<ITextGenerationService>();

            var settings = new PromptExecutionSettings()
            {
                ExtensionData = new Dictionary<string, object>
                {
                    { "top_p", 0.8 }
                }
            };
            var result = await completion.GetTextContentAsync(input.Text, settings, cancellationToken: cancellationToken);

            var text = result.Text;
            return Ok(text);
        }

        [HttpPost("stream")]
        public async Task ChatStreamAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                await Response.CompleteAsync();
            }

            var chat = kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();
            history.AddUserMessage(input.Text);

            var settings = new DashScopeAIRequestSettings();
            var results = chat.GetStreamingChatMessageContentsAsync(history, settings, cancellationToken: cancellationToken);

            await foreach (var result in results)
            {
                await Response.WriteAsync("data: " + result + "\n\n", Encoding.UTF8);
                await Response.Body.FlushAsync();
            }

            await Response.CompleteAsync();
        }

        [HttpPost("text_with_settings")]
        public async Task<IActionResult> TextWithDashScopeSettingsAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var completion = kernel.GetRequiredService<ITextGenerationService>();
            var settings = new DashScopeAIRequestSettings()
            {
                TopP = 0.5f,
                TopK = 10,
                Seed = 1234,
                Temperature = 0.5f,
                EnableSearch = true,
            };
            var result = await completion.GetTextContentsAsync(input.Text, settings, null, cancellationToken);

            var text = result[0].Text;
            return Ok(text);
        }

        [HttpPost("text_stream_with_settings")]
        public async Task TextStreamWithDashScopeSettingsAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                await Response.CompleteAsync();
            }

            var completion = kernel.GetRequiredService<ITextGenerationService>();

            var settings = new DashScopeAIRequestSettings();
            var streamingResults = completion.GetStreamingTextContentsAsync(input.Text, settings, cancellationToken: cancellationToken);

            await foreach (var streamingResult in streamingResults)
            {
                await Response.WriteAsync("data: " + streamingResult + "\n\n", Encoding.UTF8, cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }

            await Response.CompleteAsync();
        }

        [HttpPost("semantic")]
        public async Task<IActionResult> SemanticAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var prompt =
                """

                翻译以下内容为英文：

                {{$input}}

                """;
            var result = await kernel.InvokePromptAsync(prompt, new KernelArguments() { ["input"] = input.Text }, cancellationToken: cancellationToken);
            var value = result.GetValue<string>();
            var usage = result.Metadata?["Usage"];

            return Ok(new { value, usage });

        }
    }
}