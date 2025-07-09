using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS_Identity;
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
            return View(new LoginViewModel { IsStudent = true });
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
                    ModelState.AddModelError("", "You are not eligible to register. Please contact admin to be added as a student first.");
                    var viewModel = new LoginViewModel { IsStudent = true };
                    ViewBag.ActiveTab = "register";
                    return View("StudentAuth", viewModel);
                }

                // Check if user already exists in Identity
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "An account with this email already exists.");
                    var viewModel = new LoginViewModel { IsStudent = true };
                    ViewBag.ActiveTab = "register";
                    return View("StudentAuth", viewModel);
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
                    ModelState.AddModelError("", error.Description);
                }
            }

            ViewBag.ActiveTab = "register";
            return View("StudentAuth", new LoginViewModel { IsStudent = true });
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
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("AdminAuth", model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // First check if the user exists
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View("StudentAuth", model);
                }

                // Check if student trying to login is in the students table
                if (model.IsStudent)
                {
                    var studentExists = await _studentService.GetStudentByEmail(model.Email);
                    if (studentExists == null)
                    {
                        ModelState.AddModelError("", "You are not registered as a student in the system.");
                        return View("StudentAuth", model);
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, "Student"))
                    {
                        if (model.IsStudent)
                        {
                            return RedirectToAction("StudentDashboard", "Student");
                        }
                        else
                        {
                            await _signInManager.SignOutAsync();
                            ModelState.AddModelError("", "Please use student login.");
                            return View("AdminAuth", model);
                        }
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        if (!model.IsStudent)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            await _signInManager.SignOutAsync();
                            ModelState.AddModelError("", "Please use admin login.");
                            return View("StudentAuth", model);
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid email or password.");
            }

            return View(model.IsStudent ? "StudentAuth" : "AdminAuth", model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("RoleSelection");
        }
    }
} 