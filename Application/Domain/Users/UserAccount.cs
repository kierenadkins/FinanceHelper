using Application.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Domain.Users
{
    public class UserAccount : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}
