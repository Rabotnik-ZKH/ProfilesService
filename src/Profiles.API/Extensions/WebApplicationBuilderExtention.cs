using FluentValidation;
using Polly.Extensions.Http;
using Polly;
using Profiles.Contracts.DTOs.Doctor;
using Profiles.Contracts.DTOs.OuterServicesModels;
using Profiles.Contracts.DTOs.Patient;
using Profiles.Contracts.DTOs.Receptionist;
using Profiles.Domain.Interfaces;
using Profiles.Infrastructure.Repositories;
using Profiles.Presentation.Validators;
using Profiles.Services.Abstractions;
using Profiles.Services.Services;
using Microsoft.OpenApi.Models;
using Profiles.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Profiles.API.Extensions;

public static class WebApplicationBuilderExtention
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<DoctorCreateDTO>, DoctorCreateValidator>();
        builder.Services.AddScoped<IValidator<DoctorUpdateDTO>, DoctorUpdateValidator>();
        builder.Services.AddScoped<IValidator<PatientCreateDTO>, PatientCreateValidator>();
        builder.Services.AddScoped<IValidator<PatientUpdateDTO>, PatientUpdateValidator>();
        builder.Services.AddScoped<IValidator<ReceptionistCreateDTO>, ReceptionistCreateValidator>();
        builder.Services.AddScoped<IValidator<ReceptionistUpdateDTO>, ReceptionistUpdateValidator>();
        builder.Services.AddScoped<IDoctorsService, DoctorsService>();
        builder.Services.AddScoped<IPatientsService, PatientsService>();
        builder.Services.AddScoped<IReceptionistsService, ReceptionistsService>();
        builder.Services.AddScoped<IDoctorsRepository, DoctorsRepository>();
        builder.Services.AddScoped<IAccountsRepository, AccountRepository>();
        builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();
        builder.Services.AddScoped<IReceptionistsRepository, ReceptionistsRepository>();

        builder.Services.AddHttpClient<IHttpRepository<OfficeDTO>, OfficesHttpRepository<OfficeDTO>>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy());

        builder.Host.UseSerilog((ctx, lc) =>
            lc.WriteTo.Console()
            .ReadFrom.Configuration(ctx.Configuration));

        builder.Logging.ClearProviders();

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5005";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "profiles.api");
            });
        });

        builder.Services.AddAutoMapper(typeof(MapperProfile));
        builder.Services.AddHttpClient();

        builder.Services.AddDbContext<ProfilesDbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

        builder.Services.AddControllers()
            .AddApplicationPart(typeof(Presentation.Controllers.DoctorsController).Assembly);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                    },
                    new List<string>()
                }
            });
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
