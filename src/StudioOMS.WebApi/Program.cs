using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StudioOMS.Endpoints;
using StudioOMS.Middlewares;
using StudioOMS.Serializer;
using StudioOMS.Serializer.Converters;

var builder = WebApplication.CreateSlimBuilder(args);

// Json
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateTimeOffsetConverter());
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
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

// Doc
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Api
app.MapLoginEndpoints()
   .MapUserEndpoints()
   .MapEmployeeEndpoints()
   .MapCustomerEndpoints()
   .MapOrderEndpoints();
app.UseMiddleware<CurrentSessionMiddleware>();


app.Run();