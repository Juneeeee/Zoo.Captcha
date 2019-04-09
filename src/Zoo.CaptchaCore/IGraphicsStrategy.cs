namespace Zoo.CaptchaCore
{
    public interface IGraphicsStrategy
    {
        Captcha Drawing(string code, int width, int height);
    }
}
