using EmailService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TabibApp.Application.Implementation;
using TabibApp.Application.Interfaces;
using TabibApp.Infrastructure.Repository;

namespace TabibApp.Application.RegisterServices;

public static class AddApplicationServicesExtention
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<ISpecializationRepository, SpecializationRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IClinicRepository, ClinicRepository>();
        services.AddScoped<IMedicalHistoryService,MedicalHistoryService>();
        services.AddScoped<IMedicalHistoryRecordsRepository,MedicalHistoryRecordsRepository>();
        services.AddScoped<IAdminDashboardRepository,AdminDashboardRepository>();
        services.AddScoped<IAssistantRepository,AssistantRepository>();
        services.AddScoped<ISessionRepository,SessionRepository>();
        services.AddScoped<ISessionService,SessionService>();
        services.AddScoped<IChatRepository,ChatRepository>();
        services.AddScoped<IDoctorDashboardRepository,DoctorDashboardRepository>();
        services.AddScoped<IClinicService, ClinicService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<ISpecializationService, SpecializationService>();
        services.AddScoped<IAppointmentService,AppointmentService>();
        
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IEmailSender, EmailSender>();

       
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Doctor", policy =>
                policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Doctor"));
            options.AddPolicy("Patient", policy =>
                policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Patient"));
            options.AddPolicy("Admin", policy =>
                policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin"));
        });

        return services;
    }
}