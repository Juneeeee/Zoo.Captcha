using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zoo.CaptchaCore.GraphicsStrategies
{
    public class CGraphicsStrategy : GraphicsStrategyBase
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

                    //当前绘制字体
                    //随机字体
                    var fontFamily = RandomFontFamily();
                    var color = Color.FromArgb(133, 127, 166);
                    var brush = new SolidBrush(color);
                    var pen = new Pen(color);
                    string c;
                    List<GraphicsPath> paths = new List<GraphicsPath>();
                    for (int i = 0; i < length; i++)
                    {
                        c = chars[i].ToString();
                        var path = Transform(c, fontFamily);
                        //设置字体位置
                        var matrix = new Matrix();
                        matrix.Translate(20 * i, 20);
                        path.Transform(matrix);
                        //随机绘制路径还是填充字体
                        var isFill = RandomUtils.ToBoolean();
                        if (isFill)
                            g.FillPath(brush, path);
                        else
                            g.DrawPath(pen, path);
                    }
                    //写入数据流
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    var data = stream.ToArray();

                    return new Captcha(chars, data, "image/png");
                }
            }

        } 
        private GraphicsPath Transform(string c, FontFamily fontFamily)
        {
            using (var path = new GraphicsPath())
            {
                path.AddString(c, fontFamily, (int)FontStyle.Bold, 60, new PointF(0, 0), StringFormat.GenericTypographic);
                Matrix m = new Matrix();
                //随机缩放
                var xScale = (float)RandomUtils.ToDouble();
                var yScale = (float)RandomUtils.ToDouble();
                m.Scale(xScale, yScale);
                //随机旋转
                var angleValue = RandomUtils.ToNumber(-20, 20);
                var rectangle = CalculateSingleCharRectangle(path.PathPoints);
                m.RotateAt(angleValue, new Point(rectangle.Width / 2, rectangle.Height / 2));

                path.Transform(m);

                return new GraphicsPath(path.PathPoints, path.PathTypes);
            }
        }
        private FontFamily RandomFontFamily()
        {
            var fonts = new string[] {
                //"Tempus Sans ITC",
                "Arial"
            };
            var index = RandomUtils.ToNumber(0, fonts.Length);
            return new FontFamily(fonts[index]);
        }
        private Rectangle CalculateSingleCharRectangle(PointF[] points)
        {
            float w = 0, h = 0;
            for (int i = 0; i < points.Length - 1; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    var xValue = Math.Abs(points[i].X - points[j].X);
                    if (w < xValue)
                        w = xValue;
                    var yValue = Math.Abs(points[i].Y - points[j].Y);
                    if (h < yValue)
                        h = yValue;
                }
            }
            return new Rectangle() { X = 0, Y = 0, Width = (int)Math.Round(w, 0), Height = (int)Math.Round(h, 0) };
        }
    }
}
