using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Zoo.CaptchaCore
{
    public class DefaultGraphicsStrategy : IGraphicsStrategy
    {
        //private ILog log;
        public DefaultGraphicsStrategy()
        {
            //log = LogManager.GetLogger("NETCoreRepository", typeof(AGraphicsStrategy));
        }
        public Captcha Drawing(string code, int width, int height)
        {
            using (Bitmap image = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.Clear(Color.FromArgb(254, 248, 248));
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    //生成随机字符
                    var length = 7;// RandomUtils.ToNumber(7, 10);
                    var chars = "aBcdEfG";// RandomUtils.ToChars(length);

                    var fontFamily = new FontFamily("Arial");
                    //所有字体绘制信息
                    TransformData[] transformDatas = new TransformData[length];
                    //当前绘制字体
                    string c;


                    //①采用两行字体 
                    var firstLineCount = length / 2;
                    var firstLineCharsWidth = 0;
                    var secondLineCharsWidth = 0;
                    for (int i = 0; i < length; i++)
                    {
                        c = chars[i].ToString();
                        var transform = Transform(c, fontFamily);
                        transformDatas[i] = transform;
                        if (i < firstLineCount)
                            firstLineCharsWidth += transform.Rectangle.Width + 1;
                        else
                            secondLineCharsWidth += transform.Rectangle.Width + 1;
                    }
                    //开始绘制  
                    Rectangle[] rectangles = new Rectangle[length];
                    for (int i = 0; i < length; i++)
                    {
                        var transformData = transformDatas[i];
                        rectangles[i].Width = transformData.Rectangle.Width;
                        rectangles[i].Height = transformData.Rectangle.Height;
                        if (i < firstLineCount)
                        {
                            if (i == 0)
                            {
                                int maxToLeft = (width - firstLineCharsWidth) / 2;//距离左侧最大X坐标
                                rectangles[i].X = RandomUtils.ToNumber(0, maxToLeft);
                            }
                            else
                                rectangles[i].X = rectangles[i - 1].X + rectangles[i - 1].Width;
                            rectangles[i].Y = 5;
                        }
                        else
                        {
                            if (i == firstLineCount)
                            {
                                int maxToLeft = (width - firstLineCharsWidth) / 2;//距离左侧最大X坐标
                                rectangles[i].X = RandomUtils.ToNumber(0, maxToLeft);
                            }
                            else
                                rectangles[i].X = rectangles[i - 1].X + rectangles[i - 1].Width;
                            rectangles[i].Y = 40;
                        }
                        var matrix = new Matrix();
                        matrix.Translate(rectangles[i].X, rectangles[i].Y - 8);
                        var path = transformData.Path;
                        path.Transform(matrix);
                        g.DrawPath(new Pen(Color.FromArgb(133, 127, 166), 1.7f), path);
                        g.DrawRectangle(new Pen(Color.Blue), rectangles[i]);
                        Log(i, path.PathPoints, rectangles[i]);
                    }
                    //var transform = Transform("B", fontFamily);
                    //var matrix = new Matrix();
                    //matrix.Translate(52, 40 - 8);
                    //var path = transform.Path;
                    //path.Transform(matrix);
                    //g.DrawPath(new Pen(Color.FromArgb(133, 127, 166), 1.7f), path);
                    //g.DrawRectangle(new Pen(Color.Blue), new Rectangle(56, 40, 30, 39));

                    //写入数据流
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    var data = stream.ToArray();

                    return new Captcha(chars, data, "image/png");
                }
            }
        }
        private TransformData Transform(string c, FontFamily fontFamily)
        {
            using (var path = new GraphicsPath())
            {
                //以(0,0)绘制字体，通过散布的坐标点获取该字体的宽高
                path.AddString(c, fontFamily, (int)FontStyle.Bold, 50, new PointF(0, 0), StringFormat.GenericTypographic);
                var rectangle = CalculateSingleCharRectangle(path.PathPoints);

                //Matrix m = new Matrix();
                ////随机旋转
                //var angleValue = RandomUtils.ToNumber(-20, 20);

                //m.RotateAt(angleValue, new Point(rectangle.Width / 2, rectangle.Height / 2));
                //path.Transform(m);


                var xAmp = rectangle.Width * 0.03;
                var yAmp = rectangle.Height * 0.06;
                var xFreq = 2d * Math.PI / rectangle.Width;
                var yFreq = 2d * Math.PI / rectangle.Height;
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

                rectangle = CalculateSingleCharRectangle(points);

                var newPath = new GraphicsPath(points, path.PathTypes);

                return new TransformData(newPath, rectangle);
            }
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

        public class TransformData
        {
            public TransformData(GraphicsPath path, Rectangle rectangle)
            {
                Path = path;
                Rectangle = rectangle;
            }
            public GraphicsPath Path { get; }
            public Rectangle Rectangle { get; set; }

        }

        private void Log(int i, PointF[] points, Rectangle rectangle)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var point in points)
            {
                builder.Append($"x:{point.X},y:{point.Y}\r\n");
            }

            //log.Debug($@"{i} Rectangle[x:{rectangle.X},y:{rectangle.Y},width:{rectangle.Width},height:{rectangle.Height}] Points:{builder.ToString()}");
        }

    }
}
