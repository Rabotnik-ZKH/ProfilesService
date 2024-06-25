using FluentValidation;
using Profiles.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

builder.Logging.ClearProviders();

ValidatorOptions.Global.LanguageManager.Enabled = false;

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

var logger = app.Services.GetRequiredService<Serilog.ILogger>();

app.ConfigureExceptionHandler(logger);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
