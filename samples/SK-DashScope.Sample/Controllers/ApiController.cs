using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.TextCompletion;
using SK_DashScope.Sample.Models;
using System.Text;
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

            var results = chat.GenerateMessageStreamAsync(history, cancellationToken: cancellationToken);

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
    }
}