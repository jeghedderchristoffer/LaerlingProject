using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models.ManageViewModels
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "Du mangler at skrive dit fornavn")]
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Du mangler at skrive dit efternavn")]
        [Display(Name = "Efternavn")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Du mangler at skrive din Email")]
        [EmailAddress(ErrorMessage = "Dette er ikke en gyldig email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Fødselsdag")]
        public DateTime? Birth { get; set; }

        [Display(Name = "Telefon nummer")]
        public string PhoneNumber { get; set; }

        [Display(Name = "By")]
        public string CityName { get; set; }
    }
}
