using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsSystem.App.Areas.Identity.Pages.Account;
using NewsSystem.Common;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;

namespace NewsSystem.App.Areas.Administration.Controllers
{
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class UsersController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;

        public UsersController(UserManager<ApplicationUser> userManager,  ILogger<RegisterModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        [Route("users")]

        public async Task<IActionResult> All()
        {
            var users = await userManager.Users.ToListAsync();

            return this.View(users);
        }
        
        public IActionResult Create()
        {
            return this.View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserCreatInputModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser{ UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName};
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    logger.LogInformation("User created a new account with password.");

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);
//
//                    await emailSender.SendEmailAsync(model.Email, "Confirm your email",
//                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
//
//                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


            }

            return LocalRedirect("/users");
        }

        public async Task<IActionResult> Promote(string email, string roleName)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            await this.userManager.AddToRoleAsync(user, roleName);
            return LocalRedirect("/users");
        }

        public async Task<IActionResult> Demote(string email, string roleName)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            await this.userManager.RemoveFromRoleAsync(user, roleName);
            return LocalRedirect("/users");
        }
    }
}