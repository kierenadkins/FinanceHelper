using FinanceHelper.Domain.Objects.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceHelper.Domain.Objects.Users
{
    public class UserAccount : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}
