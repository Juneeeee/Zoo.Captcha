namespace Zoo.CaptchaCore.GraphicsStrategies
{
    public interface IGraphicsStrategy
    {
        Captcha Drawing(string code, int width, int height);
    }
}
