using System.Collections.Generic;

namespace Zoo.CaptchaCore
{
    public class Chars
    {
        private Dictionary<char, Rectangle> _chars = new Dictionary<char, Rectangle>();
        public Chars()
        {
            _chars.Add('A', new Rectangle(0, -9, 33, 37));
            _chars.Add('B', new Rectangle(-3, -9, 27, 36));
            _chars.Add('C', new Rectangle(-3, -9, 32, 37));
            _chars.Add('D', new Rectangle(-4, -9, 30, 36));
            _chars.Add('E', new Rectangle(-4, -10, 27, 36));
            _chars.Add('F', new Rectangle(-4, -10, 24, 36));
            _chars.Add('G', new Rectangle(-3, -9, 33, 37));
            _chars.Add('H', new Rectangle(-4, -10, 28, 36));
            _chars.Add('I', new Rectangle(-5, -10, 4, 36));
            _chars.Add('J', new Rectangle(-1, -9, 20, 37));
            _chars.Add('K', new Rectangle(-4, -9, 30, 36));
            _chars.Add('L', new Rectangle(-4, -10, 22, 36));
            _chars.Add('M', new Rectangle(-4, -10, 34, 36));
            _chars.Add('N', new Rectangle(-4, -9, 28, 36));
            _chars.Add('O', new Rectangle(-3, -9, 34, 37));
            _chars.Add('P', new Rectangle(-4, -10, 27, 36));
            _chars.Add('Q', new Rectangle(-3, -10, 35, 39));
            _chars.Add('R', new Rectangle(-5, -10, 32, 36));
            _chars.Add('S', new Rectangle(-3, -9, 28, 37));
            _chars.Add('T', new Rectangle(-1, -10, 28, 36));
            _chars.Add('U', new Rectangle(-4, -9, 28, 37));
            _chars.Add('V', new Rectangle(-2, -9, 31, 37));
            _chars.Add('W', new Rectangle(-3, -10, 46, 36));
            _chars.Add('X', new Rectangle(-3, -9, 33, 36));
            _chars.Add('Y', new Rectangle(-1, -9, 32, 36));
            _chars.Add('Z', new Rectangle(-1, -10, 28, 36));
            _chars.Add('a', new Rectangle(-2, -19, 24, 27));
            _chars.Add('b', new Rectangle(-3, -10, 22, 36));
            _chars.Add('c', new Rectangle(-1, -19, 23, 27));
            _chars.Add('d', new Rectangle(-2, -10, 22, 36));
            _chars.Add('e', new Rectangle(-2, -19, 24, 27));
            _chars.Add('f', new Rectangle(0, -10, 15, 36));
            _chars.Add('g', new Rectangle(-2, -19, 22, 37));
            _chars.Add('h', new Rectangle(-3, -10, 21, 36));
            _chars.Add('i', new Rectangle(-3, -10, 5, 36));
            _chars.Add('j', new Rectangle(2, -10, 10, 46));
            _chars.Add('k', new Rectangle(-3, -10, 21, 36));
            _chars.Add('l', new Rectangle(-3, -10, 4, 36));
            _chars.Add('m', new Rectangle(-3, -19, 35, 27));
            _chars.Add('n', new Rectangle(-3, -19, 21, 27));
            _chars.Add('o', new Rectangle(-2, -19, 24, 27));
            _chars.Add('p', new Rectangle(-3, -19, 24, 37));
            _chars.Add('q', new Rectangle(-2, -19, 22, 36));
            _chars.Add('r', new Rectangle(-3, -19, 14, 27));
            _chars.Add('s', new Rectangle(-2, -19, 22, 27));
            _chars.Add('t', new Rectangle(-1, -11, 13, 35));
            _chars.Add('u', new Rectangle(-3, -19, 21, 27));
            _chars.Add('v', new Rectangle(-1, -20, 24, 26));
            _chars.Add('w', new Rectangle(0, -19, 36, 26));
            _chars.Add('x', new Rectangle(-1, -19, 24, 26));
            _chars.Add('y', new Rectangle(-1, -19, 24, 37));
            _chars.Add('z', new Rectangle(-1, -19, 23, 27));
            _chars.Add('0', new Rectangle(-3, -10, 23, 37));
            _chars.Add('1', new Rectangle(-5, -10, 14, 36));
            _chars.Add('2', new Rectangle(-2, -10, 23, 36));
            _chars.Add('3', new Rectangle(-3, -10, 23, 37));
            _chars.Add('4', new Rectangle(-1, -9, 25, 36));
            _chars.Add('5', new Rectangle(-3, -10, 24, 36));
            _chars.Add('6', new Rectangle(-2, -9, 24, 37));
            _chars.Add('7', new Rectangle(-3, -10, 23, 36));
            _chars.Add('8', new Rectangle(-2, -9, 24, 37));
            _chars.Add('9', new Rectangle(-2, -10, 24, 37));
        }
        /// <summary>
        /// X代表字符绘制X轴偏移值
        /// Y代表字符绘制Y轴偏移值
        /// Width代表绘制字符的宽度
        /// Height代表绘制字符的高度
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Rectangle this[char c]
        {
            get
            {
                Rectangle r;
                if (_chars.TryGetValue(c, out r))
                {
                    return r;
                }
                return new Rectangle(0, 0, 0, 0);
            }
        }

    }
}
