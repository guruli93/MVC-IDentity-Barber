using Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dai.Controllers
{
    public class LoginController : Controller
    {
       
        private readonly UserManager<IdentityUser> _userManager;
     
        private readonly SignInManager<IdentityUser> _signInManager;
        
        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;


            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;


            return View();
        }
        public IActionResult BlockAccount()
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;
            ViewData["Title"] = "Access Denied one mintes";
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {
            
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("ShowRezervation", "Services");
                    }
                    else if (await _userManager.IsInRoleAsync(user, "User"))
                    {

                        return RedirectToAction("ShowRezervation", "Services");
                    }


                }
                if (result.RequiresTwoFactor)
                {
                }
                if (result.IsLockedOut)
                {

                    return RedirectToAction("BlockAccount", "Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }




    }
}
