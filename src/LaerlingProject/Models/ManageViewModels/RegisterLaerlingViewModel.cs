using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models.ManageViewModels
{
    public class RegisterLaerlingViewModel
    {
        [Required(ErrorMessage = "Du skal vælge dit fag")]
        [Display(Name = "Fag")]
        public int FagId { get; set; }

        public string Speciale { get; set; }
        public string Firma { get; set; }

        [Display(Name = "Hovedforløb")]
        public int? HovedForløb { get; set; }

        [Required (ErrorMessage = "Du skal skrive en profil tekst")]
        [Display(Name = "Profiltekst")]
        public string Profiltekst { get; set; }

        [Required (ErrorMessage = "Du skal vælge hvilke byer du vil arbejde i")]
        [Display(Name = "Arbejdsbyer")]
        public List<int> SelectedCitys { get; set; } 
    }
}
