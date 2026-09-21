using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StudioOMS;
using StudioOMS.Me;
using StudioOMS.Serializer;
using StudioOMS.Serializer.Converters;

var builder = WebApplication.CreateSlimBuilder(args);

// Json
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateTimeOffsetConverter());
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, DtoJsonSerializerContext.Default);
});

// WebApi
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Blazor", policy =>
        policy.WithOrigins("https://localhost:7140")  // Blazor 前端的地址
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// StudioOMS
builder.Services.AddStudioOMSHandlers();
var connectString = builder.Configuration.GetConnectionString("sqlite");
builder.Services.AddInfrastructure(x => x.UseSqlite(connectString));


// Build
var app = builder.Build();

app.UseCors("Blazor");
app.MapStudioOMS();
app.UseMiddleware<CurrentSessionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Run
app.Run();