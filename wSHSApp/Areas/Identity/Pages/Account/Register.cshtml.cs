#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using wSHSApp.Areas.Identity.Data;
using wSHSApp.Models;

namespace wSHSApp.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AkusUser> _signInManager;
        private readonly UserManager<AkusUser> _userManager;
        private readonly IUserStore<AkusUser> _userStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AkusUser> userManager,
            IUserStore<AkusUser> userStore,
            SignInManager<AkusUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }
        private List<string> allRoles = new List<string> { "Admin", "Personal" };
        public IEnumerable<SelectListItem> Roles { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Логин")]
            public string UserName { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Имя пользователя")]
            public string Firstname { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Фамилия пользователя")]
            public string Surname { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Группа пользователя")]
            public string Role { get; set; }
            [Required]
            [StringLength(20, ErrorMessage = "{0} должен быть длиной от {2} до {1} символов.", MinimumLength = 5)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтверждение пароля")]
            [Compare("Password", ErrorMessage = "Обрати внимание, пароли должны совпадать.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Roles = allRoles.Select(x => new SelectListItem() { Text = x, Value = x }).ToList();
            Roles.ElementAt(1).Selected = true;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.Firstname = Input.Firstname;
                user.Surname = Input.Surname;

                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);
                var roleResult = await _userManager.AddToRoleAsync(user, Input.Role);
                Claim[] Claims = { new Claim("firstname", Input.Firstname), new Claim("surname", Input.Surname) };
                var claimsResult = await _userManager.AddClaimsAsync(user, Claims);

                if (result.Succeeded && roleResult.Succeeded && claimsResult.Succeeded)
                {
                    _logger.LogInformation("Создан новый аккаунт!");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }

        private AkusUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AkusUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AkusUser)}'. " +
                    $"Ensure that '{nameof(AkusUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
