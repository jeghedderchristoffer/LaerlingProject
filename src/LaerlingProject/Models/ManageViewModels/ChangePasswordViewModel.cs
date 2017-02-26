using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Du mangler at skrive dit gamle kodeord.")]
        [DataType(DataType.Password)]
        [Display(Name = "Gamle kodeord")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Du mangler at skrive dit nye kodeord.")]
        [StringLength(100, ErrorMessage = "Kodeordet skal være mellem {2} og {1} karakter lang.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nyt kodeord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekræft nyt kodeord")]
        [Compare("NewPassword", ErrorMessage = "Kodeordende er ikke ens.")]
        public string ConfirmPassword { get; set; }
    }
}
