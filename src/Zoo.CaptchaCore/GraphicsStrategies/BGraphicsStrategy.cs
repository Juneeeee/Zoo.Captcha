using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zoo.CaptchaCore.GraphicsStrategies
{
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


                    //GraphicsPath path = new GraphicsPath(FillMode.Alternate);
                    //path.AddString("A", new FontFamily("Consolas"), (int)FontStyle.Bold, 50, new Rectangle(10, 0, 60, 60), StringFormat.GenericTypographic);
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
            int factor = RandomUtils.ToNumber(3, 10);
            int w = image.Width;
            int h = image.Height;

            // 透过公式进行水波纹的採样 
            Point[,] points = new Point[w, h];

            double newX, newY;
            double xo, yo;

            //先取样将水波纹座标跟RGB取出
            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    xo = (factor * Math.Sin(2.0 * Math.PI * (float)y / 120.0));
                    yo = (factor * Math.Cos(2.0 * Math.PI * (float)x / 120.0));

                    newX = (x + xo);
                    newY = (y + yo);

                    if (newX > 0 && newX < w)
                        points[x, y].X = (int)Math.Round(newX, 0);
                    else
                        points[x, y].X = 0;
                    if (newY > 0 && newY < h)
                        points[x, y].Y = (int)Math.Round(newY, 0);
                    else
                        points[x, y].Y = 0;
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
                        xOffset = points[x, y].X;
                        yOffset = points[x, y].Y;

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
