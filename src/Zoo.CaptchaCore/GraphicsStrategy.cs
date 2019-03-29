using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zoo.CaptchaCore
{
    public interface IGraphicsStrategy
    {
        Captcha Drawing(string code, int width, int height);
    }
    public abstract class GraphicsStrategyBase : IGraphicsStrategy
    {
        protected int CharWidth(string c)
        {
            return IsUpper(c) ? UpperCharWidth : LowerCharWidth;
        }
        protected int UpperCharWidth { get; set; } = 30;
        protected int LowerCharWidth { get; set; } = 28;
        public abstract Captcha Drawing(string code, int width, int height);

        protected int CalculateWidth(string code, int length = 0)
        {
            if (length == 0)
                length = code.Length;
            int width = 0;
            for (int i = 0; i < length; i++)
            {
                width += IsUpper(code[i].ToString()) ? UpperCharWidth : LowerCharWidth;
            }
            return width;
        }
        protected bool IsUpper(string c)
        {
            return c == c.ToUpper();
        }


    }

    public class GraphicsStrategyManager
    {
        static GraphicsStrategyManager()
        {
            Instance = new GraphicsStrategyManager();
        }
        public static GraphicsStrategyManager Instance { get; private set; }

        public GraphicsStrategyBase GetRandomStrategy()
        {

            return new AGraphicsStrategy();

        }
    }

    public class AGraphicsStrategy : GraphicsStrategyBase
    {
        //字体的宽高
        private PointF MaxWidthAndHeight(PointF[] points)
        {
            int w = 0, h = 0;
            for (int i = 0; i < points.Length - 1; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    var xValue = Math.Abs(points[i].X - points[j].X);
                    if (w < xValue)
                        w = (int)xValue;
                    var yValue = Math.Abs(points[i].Y - points[j].Y);
                    if (h < yValue)
                        h = (int)yValue;
                }
            }
            return new PointF(w, h);
        }
        private GraphicsPath Transform(string c, Rectangle rectangle, FontFamily fontFamily)
        {
            using (var path = new GraphicsPath())
            {
                path.AddString(c, fontFamily, (int)FontStyle.Bold, 60, new PointF(rectangle.X, rectangle.Y), StringFormat.GenericTypographic);
                var wh = MaxWidthAndHeight(path.PathPoints);

                var xAmp = wh.X * 0.03;
                var yAmp = wh.Y * 0.06;
                var xFreq = 2d * Math.PI / wh.X;
                var yFreq = 2d * Math.PI / wh.Y;
                var points = new PointF[path.PathPoints.Length];
                var xSeed = RandomUtils.ToDouble() * 2 * Math.PI;
                var ySeed = RandomUtils.ToDouble() * 2 * Math.PI;
                var i = 0;
                foreach (var original in path.PathPoints)
                {
                    var val = xFreq * original.X + yFreq * original.Y;
                    var xOffset = (int)(xAmp * Math.Sin(val + xSeed));
                    var yOffset = (int)(yAmp * Math.Sin(val + ySeed));
                    points[i++] = new PointF(original.X + xOffset, original.Y + yOffset);
                }
                var newPath = new GraphicsPath(points, path.PathTypes);

                return newPath;
            }
        }
        public override Captcha Drawing(string code, int width, int height)
        {
            using (Bitmap image = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.Clear(Color.FromArgb(254, 248, 248));
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    //生成随机字符
                    var length = RandomUtils.ToNumber(7, 10);
                    var chars = RandomUtils.ToChars(length);

                    var fontFamily = new FontFamily("Arial");
                    Rectangle[] points = new Rectangle[length];
                    var firstLineCount = length / 2;
                    int charWidth;//字体宽度
                    bool isUpper;//绘制字体是否为大写
                    string c;//每次绘制的字体 
                    for (int i = 0; i < length; i++)
                    {
                        c = chars[i].ToString();
                        isUpper = c == c.ToUpper();
                        charWidth = CharWidth(c);

                        var point = new Rectangle(0, 0, 40, 60);
                        if (i < firstLineCount)
                        {
                            if (i == 0)
                            {
                                int maxToLeft = width - CalculateWidth(chars, firstLineCount) - 10;//距离左侧最大X坐标
                                point.X = RandomUtils.ToNumber(0, maxToLeft);
                            }
                            else
                                point.X = points[i - 1].X + charWidth;
                            point.Y = -10;
                        }
                        else
                        {
                            if (i == firstLineCount)
                            {
                                int maxToLeft = width - CalculateWidth(chars, length - firstLineCount) - 10;//距离左侧最大X坐标
                                point.X = RandomUtils.ToNumber(0, maxToLeft);
                            }
                            else
                                point.X = points[i - 1].X + charWidth;
                            point.Y = 30;
                        }
                        points[i] = point;
                        var path = Transform(c, point, fontFamily);
                        g.DrawPath(new Pen(Color.FromArgb(133, 127, 166), 2.0f), path);
                    }

                    //写入数据流
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    var data = stream.ToArray();

                    return new Captcha(chars, data, "image/png");

                }
            }
        }

    }
     
    public class BGraphicsStrategy : GraphicsStrategyBase
    {
        public override Captcha Drawing(string code, int width, int height)
        {
            using (Bitmap image = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.Clear(Color.FromArgb(254, 248, 248));
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    //生成随机字符
                    var length = RandomUtils.ToNumber(7, 10);
                    var chars = RandomUtils.ToChars(length);

                    GraphicsPath path = new GraphicsPath(FillMode.Alternate);
                    Rectangle[] points = new Rectangle[length];
                    var firstLineCount = length / 2;
                    //var charWidth = 0;
                    //var isUpper = false;
                    //var c = "";
                    //for (int i = 0; i < length; i++)
                    //{
                    //    c = chars[i].ToString();
                    //    isUpper = c == c.ToUpper();
                    //    charWidth = isUpper ? 30 : 25;
                    //    var point = new Rectangle(0, 0, 60, 60);
                    //    if (i < firstLineCount)
                    //    {
                    //        if (i == 0)
                    //            point.X = RandomUtils.ToNumber(10, 40);
                    //        else
                    //            point.X = points[i - 1].X + charWidth;
                    //        point.Y = 0;
                    //    }
                    //    else
                    //    {
                    //        if (i == firstLineCount)
                    //            point.X = RandomUtils.ToNumber(10, 40);
                    //        else
                    //            point.X = points[i - 1].X + charWidth;
                    //        point.Y = 35;
                    //    }
                    //    points[i] = point;
                    //    path.AddString(c, new FontFamily("Consolas"), (int)FontStyle.Bold, 50, point, StringFormat.GenericTypographic);
                    //}
                    path.AddString("A", new FontFamily("Consolas"), (int)FontStyle.Bold, 50, new Rectangle(10, 0, 60, 60), StringFormat.GenericTypographic);
                    g.DrawPath(new Pen(Color.FromArgb(133, 127, 166), 1.8f), path);

                    Effect(image);

                    //写入数据流
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    var data = stream.ToArray();

                    return new Captcha(chars, data, "image/png");
                }
            }
        }
        public void Effect(Bitmap image)
        {
            int nWave = 10;
            int w = image.Width;
            int h = image.Height;

            // 透过公式进行水波纹的採样 
            Point[,] pt = new Point[w, h];

            Point mid = new Point();
            mid.X = w / 2;
            mid.Y = h / 2;

            double newX, newY;
            double xo, yo;

            //先取样将水波纹座标跟RGB取出
            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    xo = (nWave * Math.Sin(2.0 * Math.PI * (float)y / 120.0));
                    yo = (nWave * Math.Cos(2.0 * Math.PI * (float)x / 120.0));

                    newX = (x + xo);
                    newY = (y + yo);

                    if (newX > 0 && newX < w)
                    {
                        pt[x, y].X = (int)Math.Round(newX, 0);
                    }
                    else
                    {
                        pt[x, y].X = 0;
                    }


                    if (newY > 0 && newY < h)
                    {
                        pt[x, y].Y = (int)Math.Round(newY, 0);
                    }
                    else
                    {
                        pt[x, y].Y = 0;
                    }
                }
            }
            //进行合成
            Bitmap bSrc = (Bitmap)image.Clone();

            // 依照 Format24bppRgb 每三个表示一 Pixel 0: 蓝 1: 绿 2: 红
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int scanline = bitmapData.Stride;

            IntPtr Scan0 = bitmapData.Scan0;
            IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = bitmapData.Stride - image.Width * 3;

                int xOffset, yOffset;

                for (int y = 0; y < h; ++y)
                {
                    for (int x = 0; x < w; ++x)
                    {
                        xOffset = pt[x, y].X;
                        yOffset = pt[x, y].Y;

                        if (yOffset >= 0 && yOffset < h && xOffset >= 0 && xOffset < w)
                        {
                            p[0] = pSrc[(yOffset * scanline) + (xOffset * 3)];
                            p[1] = pSrc[(yOffset * scanline) + (xOffset * 3) + 1];
                            p[2] = pSrc[(yOffset * scanline) + (xOffset * 3) + 2];
                        }

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            image.UnlockBits(bitmapData);
            bSrc.UnlockBits(bmSrc);
        }


    }
   
}
