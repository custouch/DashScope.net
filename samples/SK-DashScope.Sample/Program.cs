using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddUserSecrets(typeof(Program).Assembly)
                        .Build();

var apiKey = configuration["DashScope:ApiKey"];

builder.Services.AddScoped(svc =>
{
    var kernel = Kernel.Builder.WithDashScopeCompletionService(configuration["DashScope:ApiKey"])
    .WithDashScopeTextEmbeddingGenerationService(configuration["DashScope:ApiKey"])
    .Build();
    return kernel;
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