namespace Zoo.CaptchaCore
{
    public interface ICaptchaProvider
    {
        Captcha CreateCaptcha();
        bool Validate(string id, string code);
        Captcha GetCaptcha(string id);
    }
    public class DefaultCaptchaProvider : ICaptchaProvider
    {
        private readonly ICaptchaPainter _captchaPainter;
        private readonly ICodeProvider _codeProvider;
        private readonly ICaptchaStore _captchaStore;
        public DefaultCaptchaProvider(ICaptchaPainter captchaPainter, ICaptchaStore captchaStore,
            ICodeProvider codeProvider)
        {
            _captchaPainter = captchaPainter;
            _codeProvider = codeProvider;
            _captchaStore = captchaStore;
        }
        public Captcha CreateCaptcha()
        {
            var code = _codeProvider.Generate();
            var captcha = _captchaPainter.Paint(code);

            //存储验证码 
            _captchaStore.Add(captcha);
            return captcha;
        }

        public Captcha GetCaptcha(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new System.ArgumentNullException(nameof(id));
            return _captchaStore.Get(id);
        }

        public bool Validate(string id, string code)
        {
            var captcha = _captchaStore.Get(id);
            if (string.IsNullOrEmpty(code))
                return false;
            return captcha.Code.ToLower() == code.ToLower();
        }
    }
}
