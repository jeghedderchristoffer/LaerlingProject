using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LaerlingProject.Models;
using LaerlingProject.Models.ManageViewModels;
using LaerlingProject.Services;
using LaerlingProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using LaerlingProject.Repository;

namespace LaerlingProject.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        #region Properties&Constructor
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        private readonly ILaerlingProfilRepository _laerlingProfilRepo;
        private readonly ICityRepository _cityRepo;
        private readonly IFagRepository _fagRepo;
        private readonly ApplicationDbContext _context;

        public ManageController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ISmsSender smsSender,
        ILoggerFactory loggerFactory,
        ILaerlingProfilRepository laerlingProfilRepo,
        ICityRepository cityRepo,
        IFagRepository fagRepo,
        ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _laerlingProfilRepo = laerlingProfilRepo;
            _cityRepo = cityRepo;
            _fagRepo = fagRepo;
            _context = context;
        }
        #endregion


        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                    message == ManageMessageId.ChangePasswordSuccess ? "Dit kodeord er blevet skiftet."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.EditUserSuccess ? "Dine oplysninger blev gemt."
                : message == ManageMessageId.LaerlingSuccess ? "Du er blevet registreret som arbejder."
                : "";

            ViewData["ErrorMessage"] =
                message == ManageMessageId.Error ? "Der skete en fejl."
                : message == ManageMessageId.EmailError ? "Der er allerede en bruger med denne email. Dine oplysninger blev ikke gemt."
                : "";

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                CurrentUser = await _userManager.FindByIdAsync(user.Id),
            };

            if (user.LaerlingProfilID != null)
            {
                model.LaerlingProfil = _laerlingProfilRepo.GetLaerlingProfil(user.LaerlingProfilID.Value);
            }

            if (user.CityId != null)
            {
                var city = _cityRepo.GetCity(user.CityId.Value);
                ViewData["CityName"] = city.Name;
            }
            else
            {
                ViewData["CityName"] = "Ikke angivet"; 
            }

            if (user.LaerlingProfilID != null)
            {
                var fag = _fagRepo.GetFag(model.LaerlingProfil.FagId);
                ViewData["Fag"] = fag.Name;

                ViewBag.Cities = GetLaerlingCity();
            }



            return View(model);
        }

        #region AutoAdded 
        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        {
            ManageMessageId? message = ManageMessageId.Error;
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    message = ManageMessageId.RemoveLoginSuccess;
                }
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await _smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(1, "User enabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(2, "User disabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Failed to verify phone number");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                //AddErrors(result);
                ModelState.AddModelError(string.Empty, "Dit gamle kodeord er forkert.");
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/SetPassword
        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //GET: /Manage/ManageLogins
        [HttpGet]
        public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.AddLoginSuccess ? "The external login was added."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var otherLogins = _signInManager.GetExternalAuthenticationSchemes().Where(auth => userLogins.All(ul => auth.AuthenticationScheme != ul.LoginProvider)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        //
        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(user, info);
            var message = result.Succeeded ? ManageMessageId.AddLoginSuccess : ManageMessageId.Error;
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }
        #endregion

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            EditUserSuccess,
            EmailError,
            PropertyNotSet,
            LaerlingSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion

        #region CustomAdded


        [HttpGet]
        public IActionResult EditUser(ManageMessageId? message = null)
        {
            ViewData["Error"] =
                message == ManageMessageId.PropertyNotSet ? "Du skal angive dit telefon nummer, by og fødselsdato for at blive arbejder."
                : "";

            var user = GetCurrentUserAsync().Result;
            var model = new EditUserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Birth = user.Birth,
                PhoneNumber = user.PhoneNumber,
            };

            if (user.CityId != null)
                model.CityName = _cityRepo.GetCity(user.CityId.Value).Name;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model, ManageMessageId? message = null)
        { 
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                var userToUpdate = await _userManager.FindByIdAsync(user.Id);

                userToUpdate.FirstName = model.FirstName;
                userToUpdate.LastName = model.LastName;
                userToUpdate.Birth = model.Birth;
                userToUpdate.Email = model.Email;
                userToUpdate.UserName = model.Email;
                userToUpdate.PhoneNumber = model.PhoneNumber;

                if (!string.IsNullOrEmpty(model.CityName))
                    userToUpdate.CityId = _cityRepo.GetCityName(model.CityName).CityId;

                var result = await _userManager.UpdateAsync(userToUpdate);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.EditUserSuccess });
                else
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.EmailError }); // Sikre sig, at der ikke står opdateret, hvis du f.eks ændre din mail til en eksisterne...

            }

            ViewData["Error"] = "";

            return View(model);

            //return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        [HttpGet]
        public IActionResult RegisterLaerling()
        {
            if (IsPropertySet())
            {
                return View();
            }
            return RedirectToAction(nameof(EditUser), new { Message = ManageMessageId.PropertyNotSet });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterLaerling(RegisterLaerlingViewModel model)
        {
            var user = await GetCurrentUserAsync();

            if (ModelState.IsValid)
            {
                var laerlingProfil = new LaerlingProfil() { FagId = model.FagId, Speciale = model.Speciale, Firma = model.Firma, ProfilTekst = model.Profiltekst, Hovedforløb = model.HovedForløb };
                _laerlingProfilRepo.CreateLaerlingProfil(laerlingProfil, model.SelectedCitys, user);

                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.LaerlingSuccess });
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditLaerling()
        {
            var user = GetCurrentUserAsync().Result;
            var laerling = _laerlingProfilRepo.GetLaerlingProfil(user.LaerlingProfilID.Value); 

            var model = new EditLaerlingViewModel()
            {
                LaerlingID = laerling.LaerlingID,
                FagId = laerling.FagId, 
                Speciale = laerling.Speciale, 
                Firma = laerling.Firma, 
                Hovedforløb = laerling.Hovedforløb, 
                ProfilTekst = laerling.ProfilTekst,
                CurrentCitys = GetLaerlingCity()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditLaerling(EditLaerlingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var laerlingProfil = new LaerlingProfil() { LaerlingID = model.LaerlingID, FagId = model.FagId, Speciale = model.Speciale, Firma = model.Firma, ProfilTekst = model.ProfilTekst, Hovedforløb = model.Hovedforløb };
                 _laerlingProfilRepo.UpdateLaerlingProfil(laerlingProfil, model.SelectedCitys);

                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.EditUserSuccess });
            }
            model.CurrentCitys = GetLaerlingCity();
            return View(model);
        }

        private bool IsPropertySet()
        {
            var user = GetCurrentUserAsync().Result;

            if (user.Birth == null || user.CityId == null || user.PhoneNumber == null)
            {
                return false;
            }
            return true;
        }
        private List<City> GetLaerlingCity()
        {
            var cc = _context.Citys.Include(c => c.LaerlingCity).ThenInclude(x => x.City).ToList();

            var cities = new List<City>();

            foreach (City c in cc)
            {
                foreach (var v in c.LaerlingCity)
                {
                    if (v.LaerlingID == GetCurrentUserAsync().Result.LaerlingProfilID)
                    {
                        cities.Add(c);
                    }
                }
            }
            return cities;
        }

        #endregion
    }
}