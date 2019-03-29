using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zoo.Captcha.Web.Controllers
{
    public class ExamplesController : Controller
    {
        public IActionResult Index()
        {
            Bitmap image = new Bitmap(1000, 1000);
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            // Create a new Path.
            GraphicsPath myPath = new GraphicsPath();

            // Call AddBezier.
            myPath.StartFigure();
            myPath.AddBezier(50, 50, 70, 0, 100, 120, 150, 50);

            // Close the curve.
            myPath.CloseFigure();

            // Draw the path to screen.
            g.DrawPath(new Pen(Color.Red, 2), myPath);



            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            var data = stream.ToArray();

            return File(data, "image/png");
        }
    }
}