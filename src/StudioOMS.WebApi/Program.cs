using Scalar.AspNetCore;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;

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
builder.Services.AddMessaging();
builder.Services.AddRepository();


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
app.MapOrderEndpoints();
app.MapTimingOrderEndpoints();


app.Run();