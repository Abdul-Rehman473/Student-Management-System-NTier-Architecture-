using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NTier_Final.Areas.Identity.Data;
using NTier_Final.Models;
using SMS_BLL;
using System.Threading.Tasks;

namespace NTier_Final.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStudentService _studentService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IStudentService studentService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _studentService = studentService;
        }

        public IActionResult RoleSelection()
        {
            return View();
        }

        public IActionResult StudentAuth()
        {
            return View();
        }

        public IActionResult AdminAuth()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StudentRegister(StudentRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if student exists in the system
                var studentExists = await _studentService.GetStudentByEmail(model.Email);
                if (studentExists == null)
                {
                    ModelState.AddModelError(string.Empty, "You are not eligible to register. Please contact admin.");
                    return View("StudentAuth", model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    IsStudent = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Student");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("StudentDashboard", "Student");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("StudentAuth", model);
        }

        [HttpPost]
        public async Task<IActionResult> AdminRegister(AdminRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    IsStudent = false
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("AdminAuth", model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        if (await _userManager.IsInRoleAsync(user, "Student"))
                        {
                            return RedirectToAction("StudentDashboard", "Student");
                        }
                        else if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model.IsStudent ? "StudentAuth" : "AdminAuth", model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("RoleSelection");
        }
    }

    public class StudentRegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class AdminRegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public bool IsStudent { get; set; }
    }
} 