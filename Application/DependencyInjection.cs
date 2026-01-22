using FinanceHelper.Application.Usecases.Users.Command;
using FinanceHelper.Application.Usecases.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceHelper.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
                AppDomain.CurrentDomain.GetAssemblies()
            ));


        //validator behavior, run per command / query
        services.AddValidatorsFromAssemblyContaining<SignupCommandHandler.SignupCommandValidator>();

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>)
        );

        return services;
    }
}