namespace Zoo.CaptchaCore
{
    public interface IRandomProvider
    {
        string ToChars(int length);
        int ToNumber(int minValue, int maxValue);
        double ToDouble();
    }
}
