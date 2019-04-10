using log4net;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace Zoo.CaptchaCore
{
    public class DefaultGraphicsProvider : IGraphicsProvider
    {
        private readonly IRandomProvider _randomProvider;
        private ILog log;
        public DefaultGraphicsProvider(IRandomProvider randomProvider)
        {
            log = LogManager.GetLogger("NETCoreRepository", typeof(DefaultGraphicsProvider));
            _randomProvider = randomProvider;
        }
        public Captcha Drawing(string code, int width, int height)
        {


            var length = _randomProvider.ToNumber(7, 10);
            var chars = _randomProvider.ToChars(length);


            using (Bitmap image = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.Clear(Color.FromArgb(243, 241, 241));
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.High;
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    var fontFamily = new FontFamily("Arial");
                    var pen = new Pen(Color.Red);
                    var pen2 = new Pen(Color.Black);

                    var ch = new Chars();
                    string c;
                    int toLeft = 10, toTop = 5, maxHeight = 0;
                    int perLineCount = length / 2;
                    for (int i = 0; i < length; i++)
                    {
                        c = chars[i].ToString();
                        var rectangle = ch[chars[i]];
                        using (GraphicsPath path = new GraphicsPath(FillMode.Alternate))
                        {
                            path.AddString(c, fontFamily, (int)FontStyle.Regular, 50, new Point(toLeft + rectangle.X, toTop + rectangle.Y), StringFormat.GenericTypographic);
                            g.DrawPath(pen, path);
                            //g.DrawRectangle(pen2, new Rectangle(toLeft, toTop, rectangle.Width, rectangle.Height));
                        }
                        if (maxHeight < rectangle.Height)
                            maxHeight = rectangle.Height;
                        if (i == perLineCount)
                        {
                            toTop = maxHeight + 2;
                            toLeft = 10;
                        }
                        toLeft += rectangle.Width;
                    }


                    //AddWaveEffectGraphics(width, height, image);
                    using (var stream = new MemoryStream())
                    {

                        image.Save(stream, ImageFormat.Png);
                        return new Captcha(chars, stream.ToArray(), "image/png");
                    }
                }
            }
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
