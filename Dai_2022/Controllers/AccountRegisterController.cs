
using Application;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Dai.Controllers
{

    public class AccountRegisterController : Controller
    {
        
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccountRegisterController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;

        }





        public IActionResult Register()
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;
            return View();
        }
  
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
              
                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {

                    throw new UserAlreadyExistsException();
                }

                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("ShowRezervation", "Services");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("LastVisitedPage");
            HttpContext.Response.Cookies.Delete("Last");
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
        }
    }
}
