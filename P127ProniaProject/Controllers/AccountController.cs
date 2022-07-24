using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P127ProniaProject.Models;
using P127ProniaProject.ViewModels;
using System.Threading.Tasks;

namespace P127ProniaProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public SignInManager<AppUser> _signInManager { get; }
        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if(!ModelState.IsValid) return View();
            if (!register.Terms)
            {
                ModelState.AddModelError("Terms", "SErtleri qebul ele !!!");
                return View();
            }
            AppUser user = new AppUser
            {
                Firstname = register.Firstname,
                Lastname = register.Lastname,
                Email = register.Email,
                UserName = register.Username
            };
            IdentityResult result = await _userManager.CreateAsync(user,register.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task <IActionResult> Login(LoginVM login)
        
        {
            if(!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(login.Username);
            if (user is null) return View();
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user,login.Password,login.RememberMe,true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Due to over trying you have been blocked about 10 minutes");
                }
                ModelState.AddModelError("", "Your password and username is incorrect");
                return View();
            }
            return RedirectToAction("Index","Home") ;
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public JsonResult ShowAuthentication()
        {
            return Json(User.Identity.IsAuthenticated);
        }
    }
}
