using Microsoft.Azure.Cosmos;
using UserServiceAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add CosmosClient to the DI container
// Add CosmosClient to the DI container
builder.Services.AddSingleton<CosmosClient>(sp =>
{
    // Retrieve the configuration values
    var cosmosDbConfig = builder.Configuration.GetSection("CosmosDb");
    var cosmosDbAccount = cosmosDbConfig["Account"];
    var cosmosDbKey = cosmosDbConfig["Key"];

    return new CosmosClient(cosmosDbAccount, cosmosDbKey);
});
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
