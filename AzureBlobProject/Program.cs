using Azure.Storage.Blobs;
using AzureBlobProject.Services;
using AzureBlobProject.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton(u => new BlobServiceClient(
    builder.Configuration.GetValue<string>("BlobConnection")
    ));

builder.Services.AddScoped<IContainerService, ContainerService>();
builder.Services.AddScoped<IBlobService, BlobService>();
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
