namespace Zoo.CaptchaCore
{
    public interface IGraphicsProvider
    {
        Captcha Drawing(string code, int width, int height, int length, string fontColor, string backgroundColor);
    }
}
