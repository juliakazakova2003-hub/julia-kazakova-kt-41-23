using JuliaKazakovaKt_41_23.Interfaces.DisciplineInterfaces;
using JuliaKazakovaKt_41_23.Interfaces.GradeInterfaces;
using JuliaKazakovaKt_41_23.Interfaces.GroupInterfaces;
using JuliaKazakovaKt_41_23.Interfaces.StudentsInterfaces;
//using JuliaKazakovaKt_41_23.Interfaces.StudentsInterfaces.JuliaKazakovaKt_41_23.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JuliaKazakovaKt_41_23.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IDisciplineService, DisciplineService>();
            services.AddScoped<IGradeService, GradeService>();
            return services;
        }
    }
}
