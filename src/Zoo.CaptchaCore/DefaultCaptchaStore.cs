using System.Collections.Generic;

namespace Zoo.CaptchaCore
{
    public interface ICaptchaStore
    {
        void Add(Captcha captcha);
        Captcha Get(string id);
    }
    public class MemoryCaptchaStore : ICaptchaStore
    {
        public static IDictionary<string, Captcha> dictionary = new Dictionary<string, Captcha>();

        public void Add(Captcha captcha)
        {
            dictionary.Add(captcha.Id, captcha);
        }

        public Captcha Get(string id)
        {
            Captcha captcha;
            dictionary.TryGetValue(id, out captcha);
            return captcha;
        }
    }
}
