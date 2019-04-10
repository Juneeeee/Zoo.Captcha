using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Zoo.Captcha.Web.Models;
using Zoo.CaptchaCore;

namespace Zoo.Captcha.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICaptchaService _captchaService;
        public HomeController(ICaptchaService captchaService)
        {
            _captchaService = captchaService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Captcha(string id)
        {

            var captcha = _captchaService.FindCaptcha(id);
            return File(captcha.Data, captcha.ContentType);

        }
        public IActionResult Validate(string id, string code)
        {
            var result = _captchaService.Validate(id, code);
            return Json(result);
        }
        public IActionResult CreateToken()
        {
            var captcha = _captchaService.CreateCaptcha(new CaptchaOptions()
            {
                ImgWidth = 216,
                ImgHeight = 96,
                MinCharsLength = 5,
                MaxCharsLength = 10,
                BackgroundColor = "#fff",
                FontColor = "#337ab7"
            });
            return Json(captcha.Id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
