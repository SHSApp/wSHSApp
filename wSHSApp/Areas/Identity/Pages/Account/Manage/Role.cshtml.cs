#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using wSHSApp.Areas.Identity.Data;
using wSHSApp.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace wSHSApp.Areas.Identity.Pages.Account.Manage
{
    public class RoleModel : PageModel
    {
        private readonly UserManager<AkusUser> _userManager;
        private readonly SignInManager<AkusUser> _signInManager;

        public RoleModel(
            UserManager<AkusUser> userManager,
            SignInManager<AkusUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        private List<string> allRoles = new List<string> { "Admin", "Personal" };
        public IEnumerable<SelectListItem> Roles { get; set; } 

        [Display(Name = "Логин пользователя")]      
        public string Username { get; set; }

        public string Role { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Группа пользователя")]
            public string Role { get; set; }
        }

        private async Task LoadAsync(AkusUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            Username = userName;

            var roles = await _userManager.GetRolesAsync(user);
            var roleId = allRoles.IndexOf(roles[0]);
            Roles = allRoles.Select(x => new SelectListItem() { Text = x, Value = x }).ToList();
            Roles.ElementAt(roleId).Selected = true;
            Role = roles[0];
            Input = new InputModel
            {
                Role = roles[0]
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
                        
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeRoleAsync()
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

            var roles = await _userManager.GetRolesAsync(user);
            if (Input.Role.ToUpper() != roles[0].ToUpper())
            {
                var res = await _userManager.RemoveFromRoleAsync(user, roles[0].ToUpper());
                var res2 = await _userManager.AddToRoleAsync(user, Input.Role.ToUpper()); 
                if (res.Succeeded && res2.Succeeded) StatusMessage = "Группа изменена.";
                return RedirectToPage();
            }

            StatusMessage = "Изменение группы не произведено.";
            return RedirectToPage();
        }

    }
}
