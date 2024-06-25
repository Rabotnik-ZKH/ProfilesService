using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Polly.Extensions.Http;
using Polly;
using Profiles.Contracts.DTOs.Doctor;
using Profiles.Contracts.DTOs.OuterServicesModels;
using Profiles.Contracts.DTOs.Patient;
using Profiles.Contracts.DTOs.Receptionist;
using Profiles.Domain.Interfaces;
using Profiles.Infrastructure.Data;
using Profiles.Infrastructure.Repositories;
using Profiles.Presentation.Validators;
using Profiles.Services.Abstractions;
using Profiles.Services.Services;

namespace Profiles.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IHttpRepository<OfficeDTO>, OfficesHttpRepository<OfficeDTO>>()
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
        .AddPolicyHandler(GetRetryPolicy());
        services.AddScoped<IValidator<DoctorCreateDTO>, DoctorCreateValidator>();
        services.AddScoped<IValidator<DoctorUpdateDTO>, DoctorUpdateValidator>();
        services.AddScoped<IValidator<PatientCreateDTO>, PatientCreateValidator>();
        services.AddScoped<IValidator<PatientUpdateDTO>, PatientUpdateValidator>();
        services.AddScoped<IValidator<ReceptionistCreateDTO>, ReceptionistCreateValidator>();
        services.AddScoped<IValidator<ReceptionistUpdateDTO>, ReceptionistUpdateValidator>();
        services.AddScoped<IDoctorsService, DoctorsService>();
        services.AddScoped<IPatientsService, PatientsService>();
        services.AddScoped<IReceptionistsService, ReceptionistsService>();
        services.AddScoped<IDoctorsRepository, DoctorsRepository>();
        services.AddScoped<IAccountsRepository, AccountRepository>();
        services.AddScoped<IPatientsRepository, PatientsRepository>();
        services.AddScoped<IReceptionistsRepository, ReceptionistsRepository>();
        services.AddAutoMapper(typeof(MapperProfile));
        services.AddHttpClient();
        services.AddDbContext<ProfilesDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        services.AddControllers()
            .AddApplicationPart(typeof(Profiles.Presentation.Controllers.DoctorsController).Assembly);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "ProfilesAPI", Version = "v1" });

            var xmlFilename = Path.Combine(AppContext.BaseDirectory, "Profiles.Presentation.xml");
            options.IncludeXmlComments(xmlFilename);
        });
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAtempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAtempt)));
    }
}
