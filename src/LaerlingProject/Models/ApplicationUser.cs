using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LaerlingProject.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birth { get; set; }
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        public int? LaerlingProfilID { get; set; }
        public LaerlingProfil LaerlingProfil { get; set; }

    }
}
