using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KauRestaurant.Areas.Identity.Data
{
    public class KauRestaurantUser : IdentityUser
    {
        [Required]
        [PersonalData]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [PersonalData]
        [StringLength(50)]
        public string? LastName { get; set; }
    }
}