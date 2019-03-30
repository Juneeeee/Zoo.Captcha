using System;
using System.Text;

namespace Zoo.CaptchaCore
{
    public class RandomUtils
    {
        private static Random _random;
        static RandomUtils()
        {
            _random = new Random();
        }
        public static string ToChars(int length)
        {
            var seeds = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; 
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(seeds[_random.Next(0, seeds.Length)]);
            }
            return builder.ToString();
        }
        public static int ToNumber(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
        public static double ToDouble()
        {
            return _random.NextDouble();
        }
    }
}
