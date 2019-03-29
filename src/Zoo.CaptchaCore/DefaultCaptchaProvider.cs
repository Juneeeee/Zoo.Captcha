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
        private readonly ICodeProvider _codeProvider;
        private readonly ICaptchaStore _captchaStore;
        public DefaultCaptchaProvider(ICaptchaStore captchaStore,
            ICodeProvider codeProvider)
        {
            _codeProvider = codeProvider;
            _captchaStore = captchaStore;
        }
        public Captcha CreateCaptcha()
        {
            var code = _codeProvider.Generate();


            IGraphicsStrategy graphicsStrategy = GraphicsStrategyManager.Instance.GetRandomStrategy();
            var captcha = graphicsStrategy.Drawing(code, 216, 96);

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
