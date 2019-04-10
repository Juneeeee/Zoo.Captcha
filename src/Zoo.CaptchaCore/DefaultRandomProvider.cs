using System;
using System.Text;

namespace Zoo.CaptchaCore
{
    public class DefaultRandomProvider : IRandomProvider
    {
        private   Random _random;
        public DefaultRandomProvider()
        {
            _random = new Random();
        }
        public string ToChars(int length)
        {
            var seeds = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
                builder.Append(seeds[_random.Next(0, seeds.Length)]);
            return builder.ToString();
        }

        public double ToDouble()
        {
            return _random.NextDouble();
        }

        public int ToNumber(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
