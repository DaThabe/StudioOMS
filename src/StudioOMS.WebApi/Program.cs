using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StudioOMS.Serializer;
using StudioOMS.Serializer.Converters;
using StudioOMS.WebApi.Endpoints;
using StudioOMS.WebApi.Middlewares;
using StudioOMS.WebApi.Serializer;

var builder = WebApplication.CreateSlimBuilder(args);

// Json
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateTimeOffsetConverter());
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, DtoJsonSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Insert(1, ResponseJsonSerializerContext.Default);
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
builder.Services.AddInfrastructure(x => x.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));
builder.Services.AddStudioOMSExceptionConverters();


// Build
var app = builder.Build();

app.UseMiddleware<ExceptionConverterMiddleware>();
app.UseMiddleware<CurrentSessionMiddleware>();
app.MapStudioOMS();


app.UseCors("Blazor");
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Run
app.Run();