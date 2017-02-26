using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models.ManageViewModels
{
    public class EditLaerlingViewModel
    {
        public int LaerlingID { get; set; }

        [Required(ErrorMessage = "Du skal vælge dit fag.")]
        [Display(Name = "Fag")]
        public int FagId { get; set; }

        public string Speciale { get; set; }
        public string Firma { get; set; }
        public int? Hovedforløb { get; set; }

        [Required(ErrorMessage = "Du skal skrive en profil tekst.")]
        [Display(Name = "Profiltekst")]
        public string ProfilTekst { get; set; }

        public List<City> CurrentCitys { get; set; }

        [Required(ErrorMessage = "Du skal vælge hvilke byer du vil arbejde i.")]
        [Display(Name = "Arbejdsbyer")]
        public List<int> SelectedCitys { get; set; }
    }
}
