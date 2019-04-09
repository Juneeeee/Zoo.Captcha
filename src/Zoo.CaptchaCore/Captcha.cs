using System;

namespace Zoo.CaptchaCore
{
    public class Captcha
    {
        public Captcha(string code, byte[] data, string contentType)
        {
            Id = Guid.NewGuid().ToString("n");
            Code = code;
            Data = data;
            ContentType = contentType;
        }
        public string Id { get; }
        public byte[] Data { get; }
        public string ContentType { get; }
        public string Code { get; }
    }
}
