using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text;

namespace Zoo.CaptchaCore
{
    public class DefaultGraphicsProvider : IGraphicsProvider
    {
        private readonly IRandomProvider _randomProvider;
        //private ILog log;
        public DefaultGraphicsProvider(IRandomProvider randomProvider)
        {
            //log = LogManager.GetLogger("NETCoreRepository", typeof(AGraphicsStrategy));
            _randomProvider = randomProvider;
        }
        public Captcha Drawing(string code, int width, int height)
        {

            var fColor = Color.Black;
            var length = _randomProvider.ToNumber(7, 10);
            var chars = _randomProvider.ToChars(length);


            using (Bitmap image = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.High;
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    var fontFamily = new FontFamily("Arial");
                    var pen = new Pen(Color.Black);
                    using (GraphicsPath path = new GraphicsPath(FillMode.Alternate))
                    {
                        var splitChars= SplitChars(chars);
                        path.AddString(splitChars[0], fontFamily, (int)FontStyle.Regular, 50, new RectangleF(10, 0, width, height), StringFormat.GenericTypographic);
                        path.AddString(splitChars[1], fontFamily, (int)FontStyle.Regular, 50, new RectangleF(10, 40, width, height), StringFormat.GenericTypographic);

                        /*每个字体单独绘制，保证紧凑
                         * 动态调节x轴坐标
                         */

                        g.DrawPath(pen, path);
                    }
                    AddWaveEffectGraphics(width, height, image);
                    using (var stream = new MemoryStream())
                    {
                       
                        image.Save(stream, ImageFormat.Png);
                        return new Captcha(chars, stream.ToArray(), "image/png");
                    }
                }
            }
        }
        private string[] SplitChars(string chars)
        {
            var lineCount = chars.Length / 2;
            var firstLineChars = chars.Substring(0, lineCount);
            var secondLineChars = chars.Substring(lineCount);
            return new string[] { firstLineChars, secondLineChars };
        }
        private void AddWaveEffectGraphics(int width, int height, Bitmap pic)
        {
            using (var copy = (Bitmap)pic.Clone())
            {
                double distort = 10;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    { 
                        int newX = (int)(x + (distort * Math.Sin(Math.PI * y / 84.0)));
                        int newY = (int)(y + (distort * Math.Cos(Math.PI * x / 44.0)));
                        if (newX < 0 || newX >= width) newX = 0;
                        if (newY < 0 || newY >= height) newY = 0;
                        pic.SetPixel(x, y, copy.GetPixel(newX, newY));
                    }
                }
            }
        }


    }
}
