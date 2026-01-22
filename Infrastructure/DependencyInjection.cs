using FinanceHelper.Application.Common;
using FinanceHelper.Application.Interfaces;
using FinanceHelper.Application.Services;
using FinanceHelper.Application.Services.Cache;
using FinanceHelper.Application.Services.Encryption;
using FinanceHelper.Application.Services.Finance;
using FinanceHelper.Application.Services.Finance.ExpenseTracking;
using FinanceHelper.Application.Services.Salarys;
using FinanceHelper.Application.Services.Session;
using FinanceHelper.Application.Services.Sterializer;
using FinanceHelper.Application.Services.Tax;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Domain.Objects.Finance;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using FinanceHelper.Domain.Objects.Users;
using FinanceHelper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceHelper.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LocalDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SQL")));

        services.AddScoped<IFinanceHelperDbContext>(provider =>
            provider.GetRequiredService<LocalDbContext>());

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<ICacheManagerService, CacheManagerService>();

        services.AddScoped<ISessionManagerService, SessionManagerService>();
        services.AddScoped<ISterializerService, SterializerService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IHashingService, HashingService>();
        services.AddScoped<IUserAccountService, UserAccountService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<ITaxService, TaxService>();
        services.AddScoped<ISalaryService, SalaryService>();
        services.AddScoped<ISalaryCalculatorService, SalaryCalculatorService>();


        services.AddScoped<IEntityCacheKey<UserAccount>, UserAccountCacheKeys>();
        services.AddScoped<IEntityCacheKey<Salary>, SalaryCacheKeys>();
        services.AddScoped<IEntityCacheKey<Category>, CategoryCacheKeys>();
        services.AddScoped<IEntityCacheKey<SubCategory>, SubCategoryCacheKeys>();
        return services;
    }
    
}