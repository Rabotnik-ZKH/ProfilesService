using Appointments.API;
using FluentValidation;
using MassTransit;
using Profiles.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

ValidatorOptions.Global.LanguageManager.Enabled = false;

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.SetInMemorySagaRepositoryProvider();

    var assembly = typeof(Program).Assembly;
    x.AddConsumer<OfficeUpdatedConsumer>();

    x.AddSagaStateMachines(assembly);
    x.AddSagas(assembly);
    x.AddActivities(assembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("office-updated-appointments", queueConfigurator =>
        {
            queueConfigurator.Consumer<OfficeUpdatedConsumer>(context);
        });
    });
});

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
