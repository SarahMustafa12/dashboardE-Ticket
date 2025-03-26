 using E_TicketMovies.Models;
using E_TicketMovies.Repositories;
using E_TicketMovies.Repositories.IRepositories;
using E_TicketMovies.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_TicketMovies.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(IUserRepository userRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;

        }
        public async Task<IActionResult> Index(string query, int page = 1)
        {
            var users = userRepository.Get();

            if (query != null)
            {
                var usersByRole = await userManager.GetUsersInRoleAsync(query);
                users = userRepository.Get(e => e.UserName.Contains(query) || e.Email.Contains(query) || usersByRole.Contains(e));
                
            }
            int totalCount = users.Count();
            int pageSize = 3;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            if (page > totalPages && totalPages > 0)
                return RedirectToAction("NotFoundPage", "Home", new { area = "End User" });

            users = users.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.totalPages = totalPages;


            var userWithRole = new List<UserWithRoleVM>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userWithRole.Add(new UserWithRoleVM
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = string.Join(", ", roles)
                });
            }

            return View(userWithRole);
        }



        [HttpGet]
        public IActionResult Create()
        {
            var roles = new RegisterVM
            {
                Roles = roleManager.Roles.Select(e => e.Name).ToList()
            };
            ViewBag.Roles = roles.Roles;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterVM registerVM)
        {
            var roles = new RegisterVM
            {
                Roles = roleManager.Roles.Select(e => e.Name).ToList()
            };
            ViewBag.Roles = roles.Roles;

            if (ModelState.IsValid)
            {

                ApplicationUser appUser = new()
                {
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                    EmailConfirmed = true,

                };
                // need to check the email is unique and need to displayy the errros
                var usersEmail = userRepository.Get().Select(e => e.Email).ToList();
                if (!usersEmail.Contains(appUser.Email))
                {
                    var result = await userManager.CreateAsync(appUser, registerVM.Password);
                    if (result.Succeeded)
                    {
                        if (registerVM.Roles.Count > 0)
                        {
                            await userManager.AddToRolesAsync(appUser, registerVM.Roles);
                        }
                        return RedirectToAction("Index", "User");
                    }

                    else
                    {
                        ModelState.AddModelError("UserName", "This User Name Is Already Exist");
                        ModelState.AddModelError("Password", "Passwords Don't Match");
                        
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "This Email Is Already Exist");
                }
            }
            return View();
        }
    }
}
