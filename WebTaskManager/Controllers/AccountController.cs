using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTaskManager.Models;

namespace WebTaskManager.Controllers
{
    public class AccountController : Controller
    {
        private AppConnectionContext _context;

        public AccountController(AppConnectionContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(user => user.Name == model.Name);
                if (user == null)
                {
                    user = new User() { Name = model.Name, Password = model.Password };
                    var role = await _context.Roles.FirstOrDefaultAsync(role => role.Name == "user");
                    user.Role = role;

                    _context.Users.Add(user);

                    await _context.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("GetTask", "Home");
                }
                else
                    ModelState.AddModelError("", "Ошибка регистрации пользователя");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .Include(user => user.Role)
                    .FirstOrDefaultAsync(user => user.Name == model.Name && user.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("GetTask", "Home");
                }
                else
                    ModelState.AddModelError("", "Ошибка авторизации");
            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claim = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };

            var claimsId = new ClaimsIdentity(claim,
                "AppCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsId));
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
