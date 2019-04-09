using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zoo.Captcha.Web.Controllers
{
    public class ExamplesController : Controller
    {


        private GraphicsPath GraphicsPathDeform(GraphicsPath path, int width, int height)
        {
            var WarpFactor = 3;
            var xAmp = WarpFactor * width / 100d;
            var yAmp = WarpFactor * height / 50d;
            var xFreq = 2d * Math.PI / width;
            var yFreq = 2d * Math.PI / height;
            Random rng = new Random();
            var deformed = new PointF[path.PathPoints.Length];
            var xSeed = rng.NextDouble() * 2 * Math.PI;
            var ySeed = rng.NextDouble() * 2 * Math.PI;
            var i = 0;
            foreach (var original in path.PathPoints)
            {
                var val = xFreq * original.X + yFreq * original.Y;
                var xOffset = (int)(xAmp * Math.Sin(val + xSeed));
                var yOffset = (int)(yAmp * Math.Sin(val + ySeed));
                deformed[i++] = new PointF(original.X + xOffset, original.Y + yOffset);
            }


            return new GraphicsPath(deformed, path.PathTypes);
        }
        public IActionResult Index()
        {

            int width = 100, height = 100;
            using (Bitmap image = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.White);
                    g.FillRectangle(Brushes.White, 0, 0, width, height);
                    using (var gp = new GraphicsPath())
                    {
                        gp.AddString("G", new FontFamily("Arial"), (int)FontStyle.Bold, 48f, new Point(0, 0), StringFormat.GenericDefault);
                        using (var gpp = GraphicsPathDeform(gp, width, height))
                        {
                            g.DrawPath(new Pen(Color.FromArgb(133, 127, 166), 2.0f), gpp);
                        }
                    }
                    using (var gp = new GraphicsPath())
                    {
                        gp.AddString("B", new FontFamily("Arial"), (int)FontStyle.Bold, 48f, new Point(30, 0), StringFormat.GenericDefault);
                        using (var gpp = GraphicsPathDeform(gp, width, height))
                        {
                            g.DrawPath(new Pen(Color.FromArgb(133, 127, 166), 2.0f), gpp);
                        }
                    }
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    var data = stream.ToArray();

                    return File(data, "image/png");
                }
            }

        }


        public IActionResult Index2()
        {
            using (Bitmap image = new Bitmap(100, 100))
            {
                using (Graphics g = Graphics.FromImage(image))
                {


                    GraphicsPath path = new GraphicsPath(FillMode.Alternate);
                    path.AddString("A", new FontFamily("Consolas"), (int)FontStyle.Bold, 50, new Point(0, 0), StringFormat.GenericTypographic);

                      

                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    var data = stream.ToArray();

                    return File(data, "image/png");
                }
            }



        }
    }
}