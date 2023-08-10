using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Physico_BAL.Contracts;
using Physico_BAL.Repoisitories;
using Physico_DAL.Data;
using Physico_DAL.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Physico.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                    b => b.MigrationsAssembly("Physico"));
            }
            );
        public static void ConfigureIdentity<T>(this IServiceCollection services) where T : User
        {
            var authBuilder = services.AddIdentityCore<T>
                (o =>
                {
                    o.Password.RequireDigit = true;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 10;
                    o.User.RequireUniqueEmail = true;
                });
            authBuilder = new IdentityBuilder(authBuilder.UserType, typeof(IdentityRole), services);
            authBuilder.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes("ResearchersAPIKey");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                        ValidAudience = jwtSettings.GetSection("validAudience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                    };
                });
              //  .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"), "jwtBearerScheme2");

        }
        public static void ConfigureLifeTime(this IServiceCollection services)
        {
            services.AddScoped<User, Doctor>();
            services.AddScoped<RepositoryBase<Doctor>, DoctorRepoistory>();
            services.AddScoped<RepositoryBase<Appointment>, AppointmentRepoistory>();
            services.AddScoped<RepositoryBase<DoctorDays>, DoctorDaysRepoistory>();

            services.AddScoped<IRepositoryManager,RepositoryManager>();
            services.AddScoped<IFilesManager, FilesManager>();
            services.AddScoped<IFilesRepository, FilesRepository>();
            services.AddScoped<IDoctorRepoistory, DoctorRepoistory>();
            services.AddScoped<IDoctorDaysRepoistory, DoctorDaysRepoistory>();
            services.AddScoped<IAppointmentRepoistory, AppointmentRepoistory>();
            services.AddScoped<IAuthService , AuthService>();
            services.AddScoped<IEmailService, EmailService>();
        }

        }
}
