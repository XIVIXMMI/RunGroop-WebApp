using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroopWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManeger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManeger,
            SignInManager<AppUser> signInManager,ApplicationDbContext context)
        {
            _userManeger = userManeger;
            _signInManager = signInManager;
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            //User is found check password
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManeger.FindByEmailAsync(loginViewModel.EmailAddress);

            if(user != null)
            {
                var passwordCheck = await _userManeger.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //Password correct sigin
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                }
                //passwork is incorrect
                TempData["Error"] = "Wrong credentials. Please, try again";
                return View(loginViewModel);
            }
            //User not found
            TempData["Error"] = "Wrong credentials. Please, try again";
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View();
        }
    }
}

