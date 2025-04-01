using SalesApp.Infrastructure;
using SalesApp.Infrastructure.Contracts;
using SalesApp.Services;
using SalesApp.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddScoped<IFileManager, FileManager>();


var apiCorsPolicy = "ApiCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: apiCorsPolicy,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000", "https://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(apiCorsPolicy);

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


