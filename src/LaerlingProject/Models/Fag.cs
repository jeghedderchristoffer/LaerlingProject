using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models
{
    public class Fag
    {
        [Key]
        public int FagID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
