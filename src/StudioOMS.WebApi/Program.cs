using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StudioOMS.Endpoints;
using StudioOMS.Endpoints.Orders;
using StudioOMS.Middlewares;

var builder = WebApplication.CreateSlimBuilder(args);

// Json
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));


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
builder.Services.AddInfrastructure(x => x.UseSqlite("Data Source=studio_oms.db"));


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
app.MapStudioOMSEndpoints();
app.MapTimingOrderEndpoints();
app.UseMiddleware<CurrentUserMiddleware>();


app.Run();