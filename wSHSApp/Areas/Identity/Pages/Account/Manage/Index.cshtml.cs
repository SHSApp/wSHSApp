#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wSHSApp.Areas.Identity.Data;

namespace wSHSApp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AkusUser> _userManager;
        private readonly SignInManager<AkusUser> _signInManager;

        public IndexModel(
            UserManager<AkusUser> userManager,
            SignInManager<AkusUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Display(Name = "Логин пользователя")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [DataType(DataType.Text)]
            [Display(Name = "Имя пользователя")]
            public string Firtsname { get; set; }
            
            [DataType(DataType.Text)]
            [Display(Name = "Фамилия пользователя")]
            public string Surname { get; set; }
        }

        private async Task LoadAsync(AkusUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);
            var firstName = from claim in claims where claim.Type == "firstname" select claim.Value;
            var surName = from claim in claims where claim.Type == "surname" select claim.Value;
            Username = userName;

            Input = new InputModel
            {
                Firtsname = firstName.First(),
                Surname = surName.First()
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Какая-то хуйня с юзером '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Какая-то хуйня с юзером '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            IList<Claim> claims = await _userManager.GetClaimsAsync(user);
            var firstName = from claim in claims where claim.Type == "firstname" select claim;
            var surName = from claim in claims where claim.Type == "surname" select claim;
            if (Input.Firtsname != firstName.First().Value)
            {
                var setResult = await _userManager.ReplaceClaimAsync(user, firstName.First(), new Claim("firstname", Input.Firtsname));
                if (!setResult.Succeeded)
                {
                    StatusMessage = "Произошла ошибка при изменении Имени пользователя.";
                    return RedirectToPage();
                }
                var setResult2 = await _userManager.ReplaceClaimAsync(user, surName.First(), new Claim("surname", Input.Surname));
                if (!setResult2.Succeeded)
                {
                    StatusMessage = "Произошла ошибка при изменении Фамилии пользователя.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Профиль пользователя изменен";
            return RedirectToPage();
        }
    }
}
