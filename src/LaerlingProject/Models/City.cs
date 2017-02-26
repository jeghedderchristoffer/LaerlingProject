using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models
{
    public class City
    {
        public City()
        {
            LaerlingCity = new HashSet<LaerlingCity>(); 
        }

        [Key]
        public int CityId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int PostNo { get; set; }

        public ICollection<LaerlingCity> LaerlingCity { get; set; } 
    }
}
