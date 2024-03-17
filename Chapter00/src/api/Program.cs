using Microsoft.Azure.Cosmos;
using SimpleTodo.Api;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

builder.Services.AddSingleton<ListsRepository>();

CosmosClientOptions options = new()
{
    SerializerOptions = new CosmosSerializationOptions
    {
        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
    }
};

var cosmosEndpoint = builder.Configuration["AZURE_COSMOS_ENDPOINT"];
var authKeyOrResourceToken = builder.Configuration["AZURE_COSMOS_KEY"];

 builder.Services.AddSingleton(_ => new CosmosClient(
   accountEndpoint: cosmosEndpoint, 
   authKeyOrResourceToken: authKeyOrResourceToken, 
   options
));
 


builder.Services.AddCors();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseCors(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
});

// Swagger UI
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("./openapi.yaml", "v1");
    options.RoutePrefix = "";
});

app.UseStaticFiles(new StaticFileOptions
{
    // Serve openapi.yaml file
    ServeUnknownFileTypes = true,
});

app.MapGroup("/lists")
    .MapTodoApi()
    .WithOpenApi();
app.Run();