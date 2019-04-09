namespace Zoo.CaptchaCore
{
    public interface ICaptchaStore
    {
        void Add(Captcha captcha);
        Captcha Get(string id);
    }
}
