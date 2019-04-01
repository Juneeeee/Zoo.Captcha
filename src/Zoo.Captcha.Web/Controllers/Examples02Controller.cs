using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zoo.Captcha.Web.Controllers
{
    public class Examples02Controller : Controller
    {
        public IActionResult Index()
        {
            int width = 1000, height = 1000;
            using (Bitmap image = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.Clear(Color.White);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    GraphicsPath path = new GraphicsPath();
                    path.AddString("A", new FontFamily("黑体"), (int)FontStyle.Bold, 100f, new Point(100, 100), StringFormat.GenericDefault);

                    Matrix m = new Matrix();
                    m.Scale(1f, 0.2f);   //缩放
                    m.RotateAt(10, new PointF(110, 110));
                    path.Transform(m);
                    g.DrawPath(new Pen(Color.Blue), path);

                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    var data = stream.ToArray();

                    return File(data, "image/jpeg");

                }
            }
        }

    }
}