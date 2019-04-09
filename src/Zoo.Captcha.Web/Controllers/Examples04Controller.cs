using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zoo.CaptchaCore;

namespace Zoo.Captcha.Web.Controllers
{
    public class Examples04Controller : Controller
    {
        private int yo;

        public IActionResult Index()
        {
            // 创建位图
            Bitmap bmp = new Bitmap("C:/Users/Administrator/GitHub/Zoo.Captcha/src/Zoo.Captcha.Web/wwwroot/images/8786105_112724961000_2.jpg");


            // 锁定bitmap
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData =
                bmp.LockBits(rect, ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // 复制RGB值到数组.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            //将byte数组转换成二维数组
            int w = bmp.Width, h = bmp.Height;
            Point[,] points = new Point[bmp.Width, bmp.Height];
            int factor = 3;
            double xOffset, yOffset,newX,newY;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    xOffset = (factor * Math.Sin(2.0 * Math.PI * (float)y / 120.0));
                    yOffset = (factor * Math.Cos(2.0 * Math.PI * (float)x / 120.0));

                    newX = x + xOffset;
                    newY = y + yOffset;

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

            //使用三角函数打散数组

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    //xo = (factor * Math.Sin(2.0 * Math.PI * (float)y / 120.0));
                    //yo = (factor * Math.Cos(2.0 * Math.PI * (float)x / 120.0));

                    //newX = (x + xo);
                    //newY = (y + yo);
                }
            }



            // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 0; counter < rgbValues.Length; counter += 3)
                rgbValues[counter] = 255;





            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            using (Bitmap image = new Bitmap(bmp.Width, bmp.Height))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.DrawImage(bmp, 0, 150);

                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Jpeg);

                    return File(stream.ToArray(), "image/Jpeg");
                }
            }
        }
    }
}