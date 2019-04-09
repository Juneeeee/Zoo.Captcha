using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Zoo.Captcha.Web.Controllers
{
    public class Examples03Controller : Controller
    {
        public IActionResult Index()
        {
            int width = 1000, height = 1000;
            using (Bitmap image = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.FillRectangle(Brushes.White, 0, 0, width, height);


                    using (var gp = new GraphicsPath())
                    {
                        gp.AddString("asdcze", new FontFamily("Arial"), (int)FontStyle.Bold, 48f, new Point(0, 0), StringFormat.GenericDefault);


                        using (var gpp = GraphicsPathDeform(gp, width, height))
                        {
                            var bounds = gpp.GetBounds();
                            var matrix = new Matrix();
                            var x = (width - bounds.Width) / 2 - bounds.Left;
                            var y = (height - bounds.Height) / 2 - bounds.Top;
                            matrix.Translate(x, y);
                            gpp.Transform(matrix);
                            g.FillPath(Brushes.Black, gpp);
                        }
                    }
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    var data = stream.ToArray();
                    return File(data, "image/png");
                }
            }
        }
        private GraphicsPath GraphicsPathDeform(GraphicsPath path, int width, int height)
        {
            var WarpFactor = 3;
            var xAmp = WarpFactor * width / 100d;
            var yAmp = WarpFactor * height / 50d;
            var xFreq = 2d * Math.PI / width;
            var yFreq = 2d * Math.PI / height;
            Random rng = new Random();
            var deformed = new PointF[path.PathPoints.Length];
            var xSeed =rng.NextDouble()* 2 * Math.PI;
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
    }

}