using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FinanceHelper.Application.Tools
{
    public class StringTools
    {
        private static readonly Regex ValidEmailRegex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.Compiled);
        private static readonly Regex PasswordRegex = new Regex("^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&+*!=]).*$", RegexOptions.Compiled);

        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && ValidEmailRegex.IsMatch(email);
        }

        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrEmpty(password) && PasswordRegex.IsMatch(password);
        }
    }
}
