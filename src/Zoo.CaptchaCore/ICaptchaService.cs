namespace Zoo.CaptchaCore
{
    public interface ICaptchaService
    {
        Captcha CreateCaptcha(CaptchaOptions options);
        Captcha FindCaptcha(string id);
        bool Validate(string id, string code);
    }
}
