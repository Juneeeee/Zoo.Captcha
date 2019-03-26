using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Zoo.Captcha.Web.Models;
using Zoo.CaptchaCore;

namespace Zoo.Captcha.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICaptchaProvider _captchaProvider;
        public HomeController(ICaptchaProvider captchaProvider)
        {
            _captchaProvider = captchaProvider;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Captcha(string id)
        {
            var captcha = _captchaProvider.GetCaptcha(id);
            return File(captcha.Data, captcha.ContentType);

        }
        public IActionResult Validate(string id, string code)
        {
            var result = _captchaProvider.Validate(id, code);
            return Json(result);
        }
        public IActionResult CreateToken()
        {
            var captcha = _captchaProvider.CreateCaptcha();
            return Json(captcha.Id);
        } 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
