using System.Reflection;
using Behaviors;
using ExceptionHandler;
using FluentValidation;
using Repository;

namespace Extensions;
public static class Extensions{
    public static IServiceCollection AddExtensions(this IServiceCollection services, ConfigurationManager conf)
    {
        services.AddMediatR(cfg =>{
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        //services.AddAntiforgery(opt => opt.HeaderName = "X-XSRF-TOKEN");
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.Configure<EnvConfig.EnvConfig>(conf.GetSection("EnvConfig"));
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddTransient<IUserRepository, UserRepository>();
        return services;
    }
}