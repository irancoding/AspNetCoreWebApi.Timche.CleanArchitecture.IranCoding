using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using WebAPI.Infratructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Add(new TextSingleValueFormatter());
})
    .AddXmlSerializerFormatters()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = (context) =>
        {
            var responseObj = new
            {
                Path = context.HttpContext.Request.Path.ToString(),
                method = context.HttpContext.Request.Method.ToString(),
                controller = (context.ActionDescriptor as ControllerActionDescriptor)?.ControllerName,
                action = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName,
                errors = context.ModelState.Keys.Select(x =>
                {
                    return new
                    {
                        field = x,
                        message = context.ModelState[x].Errors.Select(c => c.ErrorMessage)
                    };
                })
            };

            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError("model state error");

            return new BadRequestObjectResult(responseObj);
        };

        //diable auto modelState invalid filter=true
        options.SuppressModelStateInvalidFilter = true;

        // Disable inference rules=true
        options.SuppressInferBindingSourcesForParameters = false;
    });

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-version"),
        new MediaTypeApiVersionReader("ver")
        );
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

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
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
if (pendingMigrations?.Any() == true)
{
    await context.Database.MigrateAsync();
}
app.Run();
