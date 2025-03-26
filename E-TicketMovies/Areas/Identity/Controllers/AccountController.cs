using E_TicketMovies.Email_Sender;
using E_TicketMovies.Models;
using E_TicketMovies.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace E_TicketMovies.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager , IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }

       

        public async Task<IActionResult> Register()
        {
            if (roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Company"));
                await roleManager.CreateAsync(new IdentityRole("Customer"));

                var adminUser =new ApplicationUser()
                {
                    UserName = "Sara",
                    Email = "Admin@gmail.com",
                    EmailConfirmed = true,
                };

                var result =  await userManager.CreateAsync(adminUser, "123456789@Sa");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser appUser = new()
                {
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                    EmailConfirmed = false
                };


                var result = await userManager.CreateAsync(appUser, registerVM.Password);

                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(appUser, "Customer");

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(appUser);

                    // Create confirmation link
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = appUser.Id, token = token }, Request.Scheme);

                    // Send confirmation email
                    await emailSender.SendEmailAsync(appUser.Email, "Confirm Your Email",
                        $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.");

                    TempData["Message"] = "Registration successful! Please check your email to confirm your account.";

                    return RedirectToAction("Login", "Account", new { area = "Identity" });

                }
                else
                {
                    ModelState.AddModelError("Password", "Passwords Don't Match");
                }

            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await userManager.FindByEmailAsync(loginVM.Email);
                if (currentUser != null)
                {
                    if (!currentUser.EmailConfirmed)
                    {
                        ModelState.AddModelError("", "Please confirm your email before logging in.");
                        return View();
                    }
                    var result = await userManager.CheckPasswordAsync(currentUser, loginVM.Password);
                    if (result)
                    {
                        await signInManager.SignInAsync(currentUser, loginVM.RememberMe);
                        return RedirectToAction("Index", "Home", new { area = "End User" });

                    }
                    else
                    {
                        ModelState.AddModelError("Email", "The Email Not Found");
                        ModelState.AddModelError("Password", "The Password Doesn't Match");
                    }
                }

            }
            return View();

        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "End User" });
        }

        public IActionResult AccessDenied()
        {

            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest("Invalid email confirmation request.");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmailSuccess"); // Create a success page
            }
            else
            {
                return View("ConfirmEmailFailed"); // Create a failure page
            }
        }

    }
}
