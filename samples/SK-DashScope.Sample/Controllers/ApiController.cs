using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.TextCompletion;
using SK_DashScope.Sample.Models;
using System.Text;
using DashScope.SemanticKernel;
using Microsoft.SemanticKernel.AI;

namespace SK_DashScope.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IKernel kernel;

        public ApiController(IKernel kernel)
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

            var chat = kernel.GetService<IChatCompletion>();
            var history = chat.CreateNewChat();
            history.AddUserMessage(input.Text);

            var result = await chat.GenerateMessageAsync(history);
            return Ok(result);
        }

        [HttpPost("text")]
        public async Task<IActionResult> TextAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var completion = kernel.GetService<ITextCompletion>();
            // CompleteRequestSettings 已经被弃用且删除，新版改用 AIRequestSettings
            // 自定义参数在属性ExtensionData里
            // 或者使用 DashScopeAIRequestSettings，用法请看下面的函数 TextWithDashScopeSettingsAsync
            var settings = new AIRequestSettings
            {
                ExtensionData = new Dictionary<string, object>
                {
                    { "top_p", 0.8 }
                }
            };
            var result = await completion.GetCompletionsAsync(input.Text, settings, cancellationToken);

            var text = await result.First().GetCompletionAsync();
            return Ok(text);
        }

        [HttpPost("stream")]
        public async Task ChatStreamAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                await Response.CompleteAsync();
            }

            var chat = kernel.GetService<IChatCompletion>();
            var history = chat.CreateNewChat();
            history.AddUserMessage(input.Text);
            
            // 可以使用IncrementalOutput来控制stream输出的方式
            var settings = new DashScopeAIRequestSettings()
            {
                IncrementalOutput = true
            };
            var results = chat.GenerateMessageStreamAsync(history, settings, cancellationToken: cancellationToken);

            await foreach (var result in results)
            {
                await Response.WriteAsync("data: " + result + "\n\n", Encoding.UTF8);
                await Response.Body.FlushAsync();
            }

            await Response.CompleteAsync();
        }

        [HttpPost("embedding")]
        public async Task<IActionResult> EmbeddingAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var embedding = kernel.GetService<ITextEmbeddingGeneration>();

            ReadOnlyMemory<float> result = await embedding.GenerateEmbeddingAsync(input.Text, cancellationToken);

            var serializableList = result.ToArray().ToList();

            return Ok(serializableList);
        }
        
        [HttpPost("text_with_settings")]
        public async Task<IActionResult> TextWithDashScopeSettingsAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                return NoContent();
            }

            var completion = kernel.GetService<ITextCompletion>();
            var settings = new DashScopeAIRequestSettings()
            {
                TopP = 0.5f,
                TopK = 10,
                Seed = 1234,
                Temperature = 0.5f,
                IncrementalOutput = false,
                EnableSearch = true,
                ResultFormat = "text"
            };
            var result = await completion.GetCompletionsAsync(input.Text, settings, cancellationToken);

            var text = await result.First().GetCompletionAsync(cancellationToken);
            return Ok(text);
        }
        [HttpPost("text_stream_with_settings")]
        public async Task TextStreamWithDashScopeSettingsAsync([FromBody] UserInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.Text))
            {
                await Response.CompleteAsync();
            }

            var completion = kernel.GetService<ITextCompletion>();
            
            // 可以使用IncrementalOutput来控制stream输出的方式
            var settings = new DashScopeAIRequestSettings()
            {
                IncrementalOutput = true
            };
            var streamingResults = completion.GetStreamingCompletionsAsync(input.Text, settings, cancellationToken: cancellationToken);
            
            await foreach (var streamingResult in streamingResults)
            {
                var results = streamingResult.GetCompletionStreamingAsync(cancellationToken);
                await foreach (var result in results)
                {
                    await Response.WriteAsync("data: " + result + "\n\n", Encoding.UTF8, cancellationToken);
                    await Response.Body.FlushAsync(cancellationToken);
                }
            }

            await Response.CompleteAsync();
        }
    }
}