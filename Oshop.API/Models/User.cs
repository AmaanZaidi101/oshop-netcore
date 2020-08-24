using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Oshop.API.Models
{
    public class User: IdentityUser<string>
    {
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Shipping> Shippings { get; set; }
        public virtual Cart Cart { get; set; }
    }
}