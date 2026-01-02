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
}
