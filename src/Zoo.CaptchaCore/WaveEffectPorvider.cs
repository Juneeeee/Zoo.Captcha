using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Zoo.CaptchaCore
{
    public class WaveEffectPorvider : IEffectPorvider
    {
        public void Effect(Bitmap image)
        {
            int nWave = 10;
            int w = image.Width;
            int h = image.Height;

            // 透过公式进行水波纹的採样 
            PointF[,] fp = new PointF[w, h];
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
                    xo = ((double)nWave * Math.Sin(2.0 * 3.1415 * (float)y / 128.0));
                    yo = ((double)nWave * Math.Cos(2.0 * 3.1415 * (float)x / 128.0));

                    newX = (x + xo);
                    newY = (y + yo);

                    if (newX > 0 && newX < w)
                    {
                        fp[x, y].X = (float)newX;
                        pt[x, y].X = (int)newX;
                    }
                    else
                    {
                        fp[x, y].X = (float)0.0;
                        pt[x, y].X = 0;
                    }


                    if (newY > 0 && newY < h)
                    {
                        fp[x, y].Y = (float)newY;
                        pt[x, y].Y = (int)newY;
                    }
                    else
                    {
                        fp[x, y].Y = (float)0.0;
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
