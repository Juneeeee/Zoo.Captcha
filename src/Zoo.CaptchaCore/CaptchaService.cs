using System;

namespace Zoo.CaptchaCore
{
    public class CaptchaService: ICaptchaService
    {
        private readonly ICaptchaStore _captchaStore;
        private readonly IGraphicsStrategy _graphicsStrategy;
        public CaptchaService(ICaptchaStore captchaStore,
            IGraphicsStrategy graphicsStrategy)
        {
            _captchaStore = captchaStore;
            _graphicsStrategy = graphicsStrategy;
        }

        public Captcha CreateCaptcha(int imgWidth, int imgHeight, int minCharsLength, int maxCharsLength)
        {
            if (imgWidth < 0)
                throw new ArgumentException("图片绘制宽度不能小于0");
            if (imgHeight < 0)
                throw new ArgumentException("图片绘制高度不能小于0");
            if (minCharsLength < 1 || minCharsLength > 10)
                throw new ArgumentException("随机最少字符长度范围为[1~10]之间");
            if (maxCharsLength < 1 || maxCharsLength > 10)
                throw new ArgumentException("随机最多字符长度范围为[1~10]之间");
            if (maxCharsLength < minCharsLength)
                throw new ArgumentException("随机最多字符长度不能少于最少字符长度");


            var length = RandomUtils.ToNumber(minCharsLength, maxCharsLength);
            var code = RandomUtils.ToChars(length);
            var captcha = _graphicsStrategy.Drawing(code, imgWidth, imgHeight);

            //存储验证码 
            _captchaStore.Add(captcha);
            return captcha;
        }
        public Captcha FindCaptcha(string id)
        {
            return _captchaStore.Get(id);
        }
        public bool Validate(string id, string code)
        {
            var captcha = _captchaStore.Get(id);
            if (captcha == null || string.IsNullOrEmpty(captcha.Code))
                return false;
            if (string.IsNullOrEmpty(code))
                return false;
            return captcha.Code.ToLower() == code.ToLower();
        }
    }
}
