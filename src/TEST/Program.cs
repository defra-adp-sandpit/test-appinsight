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
                //string userAssignedClientId = "9e5badc8-bb65-4b34-835b-ab2f5117e88e";
                o.ConnectionString = builder.Configuration.GetValue<string>("AppInsights:ConnectionString");
                string resourid = "/subscriptions/7dc5bbdf-72d7-42ca-ac23-eb5eea3764b4/resourcegroups/SSVADPDEVRG3401/providers/Microsoft.ManagedIdentity/userAssignedIdentities/SSVADPDEVMI3401-adp-portal-web";
                o.Credential = new ManagedIdentityCredential(resourid, new Azure.Identity.TokenCredentialOptions());
                // o.Credential = new DefaultAzureCredential(
                // new DefaultAzureCredentialOptions
                // {
                //     ManagedIdentityClientId = userAssignedClientId
                // });
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
