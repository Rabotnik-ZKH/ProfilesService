using FluentValidation;
using Profiles.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

ValidatorOptions.Global.LanguageManager.Enabled = false;

builder.ConfigureServices();

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
//.RequireAuthorization("ApiScope");
await app.MigrateDatabaseAsync();

app.Run();
