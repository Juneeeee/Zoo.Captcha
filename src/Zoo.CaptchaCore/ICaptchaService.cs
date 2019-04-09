namespace Zoo.CaptchaCore
{
    public interface ICaptchaService
    {
        Captcha CreateCaptcha(int imgWidth, int imgHeight, int minCharsLength, int maxCharsLength);
        Captcha FindCaptcha(string id);
        bool Validate(string id, string code);
    }
}
