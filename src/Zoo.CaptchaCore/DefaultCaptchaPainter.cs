using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zoo.CaptchaCore
{
    public interface ICaptchaPainter
    {
        Captcha Paint(string code);
    }
    public class DefaultCaptchaPainter : ICaptchaPainter
    {
        private readonly IEffectPorvider _effectPorvider;
        public DefaultCaptchaPainter(
            IEffectPorvider effectPorvider)
        {
            _effectPorvider = effectPorvider;
        } 
        //// 是否遮盖其他字体 
        //private bool IsCover(Rectangle rectangle, Rectangle[] rectangles)
        //{
            
        //    foreach (var r in rectangles)
        //    {
        //        if (rectangle.X < r.X + r.Width && rectangle.Y < r.Height + r.Height)
        //            return true;
        //    }
        //    return false;
        //}
        //private Rectangle GetSuitableRectangle(Rectangle rectangle, Rectangle[] rectangles)
        //{
        //    int i = 0;
        //    while (true)
        //    {
        //        if (i % 2 == 0)
        //            rectangle.X += 1;
        //        else
        //            rectangle.Y += 1;
        //        return rectangle;
        //    } 
        //}
        public Captcha Paint(string code)
        {
            using (Bitmap image = new Bitmap(216, 96))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.Clear(Color.FromArgb(254, 248, 248));
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    //生成随机字符
                    var length = RandomUtils.ToNumber(7, 10);
                    var chars = RandomUtils.ToChars(length);

                    //绘制字体 
                    GraphicsPath path = new GraphicsPath(FillMode.Alternate);

                    Rectangle[] points = new Rectangle[length];
                    var firstLineCount = length / 2;
                    var charWidth = 0;
                    var isUpper = false;
                    var c = "";
                    for (int i = 0; i < length; i++)
                    {
                        c = chars[i].ToString();
                        isUpper = c == c.ToUpper();
                        charWidth = isUpper ? 30 : 25;
                        var point = new Rectangle(0, 0, 60, 60);
                        if (i < firstLineCount)
                        {
                            if (i == 0)
                                point.X = RandomUtils.ToNumber(10, 40);
                            else
                                point.X = points[i - 1].X + charWidth;
                            point.Y = 0;
                        }
                        else
                        {
                            if (i == firstLineCount)
                                point.X = RandomUtils.ToNumber(10, 40);
                            else
                                point.X = points[i - 1].X + charWidth;
                            point.Y = 35;
                        }
                        points[i] = point;
                        path.AddString(c, new FontFamily("Consolas"), (int)FontStyle.Bold, 50, point, StringFormat.GenericTypographic);
                    }
                    RectangleF srcRect = new RectangleF(0, 0, 100, 200);

                    g.DrawPath(new Pen(Color.FromArgb(133, 127, 166), 1.5f), path);



                    //字体增加特效影响
                    //_effectPorvider.Effect(image);


                    //写入数据流
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    var data = stream.ToArray();

                    return new Captcha(chars, data, "image/png");//content-type同ImageFormat需为同一类型 
                }
            }

        }
    }
}
