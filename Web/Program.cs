using Application.Data;
using Application.Domain.Finance;
using Application.Domain.Finance.ExpenseTracking;
using Application.Domain.Users;
using Application.Services;
using Application.Services.Cache;
using Application.Services.Encryption;
using Application.Services.Finance;
using Application.Services.Finance.ExpenseTracking;
using Application.Services.Sterializer;
using Application.Services.Tax;
using Application.Services.User;
using Core.Services.Session;
using Microsoft.EntityFrameworkCore;

namespace FinanceBuddy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(
                    AppDomain.CurrentDomain.GetAssemblies()
                ));
            builder.Services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQL")), ServiceLifetime.Transient);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });


            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddSingleton<ICacheManagerService, CacheManagerService>();

            builder.Services.AddScoped<ISessionManagerService, SessionManagerService>();
            builder.Services.AddScoped<ISterializerService, SterializerService>();
            builder.Services.AddScoped<IEncryptionService, EncryptionService>();
            builder.Services.AddScoped<IHashingService, HashingService>();
            builder.Services.AddScoped<IUserAccountService, UserAccountService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
            builder.Services.AddScoped<ITaxService, TaxService>();
            builder.Services.AddScoped<ISalaryService, SalaryService>();

            builder.Services.AddScoped<IEntityCacheKey<UserAccount>, UserAccountCacheKeys>();
            builder.Services.AddScoped<IEntityCacheKey<Salary>, SalaryCacheKeys>();
            builder.Services.AddScoped<IEntityCacheKey<Category>, CategoryCacheKeys>();
            builder.Services.AddScoped<IEntityCacheKey<SubCategory>, SubCategoryCacheKeys>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}