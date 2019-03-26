namespace Zoo.CaptchaCore
{
    public interface ICodeProvider
    {
        string Generate();
    }
    public class DefaultCodeProvider : ICodeProvider
    {
        public string Generate()
        {
            var length = RandomUtils.ToNumber(7, 10);
            return RandomUtils.ToChars(length);
        }
    }
}
