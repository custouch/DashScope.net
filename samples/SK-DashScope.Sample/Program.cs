using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddScoped(svc =>
{
    var kernel = new KernelBuilder().WithDashScopeCompletionService(builder.Configuration["DashScope:ApiKey"]!)
    .Build();
    return kernel;
});

builder.Services.AddScoped(svc =>
{
#pragma warning disable SKEXP0052 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    return new MemoryBuilder()
    .WithDashScopeTextEmbeddingGenerationService(builder.Configuration["DashScope:ApiKey"]!)
    .Build();
#pragma warning restore SKEXP0052 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();