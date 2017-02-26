using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models
{
    public class LaerlingProfil
    {
        public LaerlingProfil()
        {
            LaerlingCity = new HashSet<LaerlingCity>(); 
        }

        [Key]
        public int LaerlingID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int FagId { get; set; }
        public virtual Fag Fag { get; set; }

  
        public string Speciale { get; set; }


        public string Firma { get; set; }

   
        public int? Hovedforløb { get; set; }

        [Required]
        public string ProfilTekst { get; set; }

        public ICollection<LaerlingCity> LaerlingCity { get; set; }
    }
}
