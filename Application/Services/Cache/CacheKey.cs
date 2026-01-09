using Application.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Application.Domain.Finance;
using Application.Domain.Finance.ExpenseTracking;

namespace Application.Services.Cache
{
    public static class CacheKeys
    {
        public static string UserByEmail(string email) => $"Get_UserByEmail_{email}";
        public static string UserById(int id) => $"Get_UserById_{id}";
        public static string SalaryByUserId (int userId) => $"Get_SalaryByUserId_{userId}";
    }

    public interface IEntityCacheKey<T>
    {
        string ListKey(params object[] args);
    }

    public class UserAccountCacheKeys : IEntityCacheKey<UserAccount>
    {
        public string ListKey(params object[] args)
        {
            return args.Length switch
            {
                1 => $"AccountsByUserId_{args[0]}",
                2 => $"AccountsByUserId_{args[0]}_type_{args[1]}",
                _ => throw new ArgumentException("Invalid arguments for account cache key")
            };
        }
    }

    public class SalaryCacheKeys : IEntityCacheKey<Salary>
    {
        public string ListKey(params object[] args)
        {
            return $"UserByEmail_{args[0]}";
        }
    }

    public class CategoryCacheKeys : IEntityCacheKey<Category>
    {
        public string ListKey(params object[] args)
        {
            return $"UserByEmail_{args[0]}";
        }
    }

    public class SubCategoryCacheKeys : IEntityCacheKey<SubCategory>
    {
        public string ListKey(params object[] args)
        {
            return $"UserByEmail_{args[0]}";
        }
    }

}
