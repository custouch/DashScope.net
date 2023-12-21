using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddScoped(svc =>
{
    var kernel = Kernel.CreateBuilder().WithDashScopeCompletionService(builder.Configuration["DashScope:ApiKey"]!)
    .Build();
    return kernel;
});

builder.Services.AddScoped(svc =>
{
    return new MemoryBuilder()
    .WithDashScopeTextEmbeddingGenerationService(builder.Configuration["DashScope:ApiKey"]!)
    .Build();
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