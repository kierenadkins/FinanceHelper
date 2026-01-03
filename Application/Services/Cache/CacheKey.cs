using Application.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Cache
{
    public static class CacheKeys
    {
        public static string UserByEmail(string email) => $"Get_UserByEmail_{email}";
        public static string UserById(int id) => $"Get_UserById_{id}";
    }

    public interface IEntityCacheKey<T>
    {
        string ListKey(params object[] args);
    }

    public class UserAccountCacheKeys : IEntityCacheKey<UserAccount>
    {
        public string ListKey(params object[] args)
        {
            return $"UserByEmail_{args[0]}";
        }
    }

}
