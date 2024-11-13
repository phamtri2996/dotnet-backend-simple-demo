using Command;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Query;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var conf = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExtensions(conf);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
// app.UseAntiforgery();
// // Get token endpoint
// app.MapGet("antiforgery/token", (IAntiforgery forgeryService, HttpContext context) =>
// {
//     var tokens = forgeryService.GetAndStoreTokens(context);
//     var xsrfToken = tokens.RequestToken!;
//     return TypedResults.Content(xsrfToken, "text/plain");
// });
app.MapPost("/upload", async (IMediator mediator, [FromForm]UploadProfile profile) =>
{
    var res = await mediator.Send(profile);
    return res;
})
.DisableAntiforgery()
.WithName("UploadProfile")
.WithOpenApi();

app.MapGet("/profiles", async (IMediator mediator) =>
{
    var query = new GetProfileQuery();
    var res = await mediator.Send(query);
    return res;
})
.WithName("GetListProfiles")
.WithOpenApi();

app.MapGet("/profiles/onboarding-file/{id}", async (IMediator mediator, string id) =>
{
    var query = new GetOnboardingFileQuery{Id = id};
    var res = await mediator.Send(query);
    return res;
})
.WithName("GetProfilesOnboardingFile")
.WithOpenApi();

app.Run();