using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOpenTelemetry().UseAzureMonitor(o =>
            {
                o.ConnectionString = builder.Configuration.GetValue<string>("AppInsights:ConnectionString");
                o.Credential = new DefaultAzureCredential();
            });
builder.Services.ConfigureOpenTelemetryTracerProvider((sp, b) =>
{
    b.ConfigureResource(resourceBuilder => resourceBuilder.AddAttributes(new Dictionary<string, object> { { "service.name", "adp-flux-notification-api" } }));
});
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
