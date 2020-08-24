using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Oshop.API.Models
{
    public class Role : IdentityRole<string>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}