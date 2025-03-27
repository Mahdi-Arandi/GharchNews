// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using GharchNews.Models;
using GharchNews.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GharchNews.Pages.Admin.User
{
    [Authorize(Roles = SD.Admin)]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
        }


        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }

        public IFormFile imgUp { get; set; }

        public class InputModel
        {
            [Display(Name = "نام")]
            public string Name { get; set; }

            [Display(Name = "نام خانوادگی")]
            public string Family { get; set; }

            [Display(Name = "شماره تماس")]
            public string PhoneNumber { get; set; }

            [Display(Name = "تصویر پروفایل")]
            public string ProfileImage { get; set; }

            [Required(ErrorMessage ="لطفا نام کاربری را وارد کنید")]
            [Display(Name = "نام کاربری")]
            public string UserName { get; set; }


            [Required(ErrorMessage = "لطفا کلمه عبور را وارد کنید")]
            [StringLength(100, ErrorMessage = "{0} باید حداقل {2} کاراکتر و حداکثر {1} کاراکتر داشته باشد.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "کلمه عبور")]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "تکرار کلمه عبور")]
            [Compare("Password", ErrorMessage = "کلمه عبور با تکرار کلمه عبور تطابق ندارد.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "سطح دسترسی")]
            public string Inrole { get; set; }
        }


        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if(! await _roleManager.RoleExistsAsync(SD.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Admin));
            }

            initRoles();
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                user.UserName = Input.UserName;
                user.Name = Input.Name;
                user.Family = Input.Family;
                user.PhoneNumber = Input.PhoneNumber;
                user.InRole = Input.Inrole;
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, Input.Password);

                if (imgUp != null)
                {
                    string SaveDir = "wwwroot/Img/ProfileImages";
                    if (!Directory.Exists(SaveDir))
                        Directory.CreateDirectory(SaveDir);

                    user.ProfileImage = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), SaveDir, user.ProfileImage);
                    using (var FileSream = new FileStream(savePath, FileMode.Create))
                    {
                        imgUp.CopyTo(FileSream);
                    }
                }


                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(SD.User))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.User));
                    }

                    await _userManager.AddToRoleAsync(user, Input.Inrole);

                    return RedirectToPage("./index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            initRoles();
            // If we got this far, something failed, redisplay form
            return Page();
        }

        void initRoles()
        {
            ViewData["Inrole"] = new SelectList(_roleManager.Roles, "Name", "Name");
        }

        private Users CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Users>();
            }
            catch
            {
                throw new InvalidOperationException($"خطایی در هنگام ایجاد کاربر رخ داده است.");
            }
        }
    }
}
