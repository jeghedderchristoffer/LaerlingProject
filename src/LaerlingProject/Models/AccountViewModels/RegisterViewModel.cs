using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Du skal angive et fornavn.")]
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Du skal angive et efternavn.")]
        [Display(Name = "Efternavn")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Du skal skrive din email.")]
        [EmailAddress(ErrorMessage = "Det skal være en gyldig email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Du skal angive et kodeord.")]
        [StringLength(100, ErrorMessage = "Kodeordet skal være mellem {2} og {1} karaktere langt.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Kodeord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekræft kodeord")]
        [Compare("Password", ErrorMessage = "Kodeordene passer ikke sammen.")]
        public string ConfirmPassword { get; set; }
    }
}
