using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GoodsStore.Models
{
    public class AppUser : IdentityUser
    {
        public string? Address { get; set; }
        public ICollection<Orders> Orders { get; set; }

    }
}
