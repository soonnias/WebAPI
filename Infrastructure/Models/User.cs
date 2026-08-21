using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBitrh { get; set; }

        // Навігаційні властивості для зв'язків

        public virtual Cart Cart { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    }

    public enum UserRole
    {
        Admin,
        Customer,
        Manager
    }
}
