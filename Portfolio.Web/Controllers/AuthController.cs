using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Web.Context;
using Portfolio.Web.Entities;
using Portfolio.Web.Models;
using System.Security.Claims;

namespace Portfolio.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly PortfolioContext context;

        public AuthController(PortfolioContext context)
        {
            this.context = context;
        }

        // ======= LOGIN =======
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = context.Users.FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("fullName", string.Join(" ", user.FirstName, user.LastName))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                IsPersistent = false
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            HttpContext.Session.SetString("UserName", user.UserName);
            return RedirectToAction("Index", "Statistics");
        }

        // ======= LOGOUT =======
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserName");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Default");
        }

        // ======= CHANGE PASSWORD =======
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var username = User.Identity.Name;
            var user = context.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View(model);
            }

            // Mevcut şifre kontrolü
            if (user.Password != model.OldPassword)
            {
                ModelState.AddModelError("", "Mevcut şifreniz hatalı.");
                return View(model);
            }

            // Yeni şifre ve tekrar şifre eşleşme kontrolü
            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Yeni şifre ve tekrar şifre eşleşmiyor.");
                return View(model);
            }

            // Şifre güncelleme
            user.Password = model.NewPassword;
            context.SaveChanges();

            ViewBag.Success = "Parolanız başarıyla güncellendi.";
            return View();
        }
    }
}
