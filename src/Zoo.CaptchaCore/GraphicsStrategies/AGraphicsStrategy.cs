using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zoo.CaptchaCore.GraphicsStrategies
{

    public class AGraphicsStrategy : GraphicsStrategyBase
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
                                int maxToLeft = width - firstLineCharsWidth - 10;//距离左侧最大X坐标
                                rectangles[i].X = RandomUtils.ToNumber(0, maxToLeft);
                            }
                            else
                                rectangles[i].X = rectangles[i - 1].X + rectangles[i - 1].Width;
                            rectangles[i].Y = -10;
                        }
                        else
                        {
                            if (i == firstLineCount)
                            {
                                int maxToLeft = width - secondLineCharsWidth - 10;//距离左侧最大X坐标
                                rectangles[i].X = RandomUtils.ToNumber(0, maxToLeft);
                            }
                            else
                                rectangles[i].X = rectangles[i - 1].X + rectangles[i - 1].Width;
                            rectangles[i].Y = 30;
                        }
                        var matrix = new Matrix();
                        matrix.Translate(rectangles[i].X, rectangles[i].Y);
                        var path = transformData.Path;
                        path.Transform(matrix);
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
        private TransformData Transform(string c, FontFamily fontFamily)
        {
            using (var path = new GraphicsPath())
            {
                //以(0,0)绘制字体，通过散布的坐标点获取该字体的宽高
                path.AddString(c, fontFamily, (int)FontStyle.Bold, 60, new PointF(0, 0), StringFormat.GenericTypographic);
                var property = GetCharProperty(path.PathPoints);

                var xAmp = property.Width * 0.03;
                var yAmp = property.Height * 0.06;
                var xFreq = 2d * Math.PI / property.Width;
                var yFreq = 2d * Math.PI / property.Height;
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

                return new TransformData(newPath, property);
            }
        }
        private Rectangle GetCharProperty(PointF[] points)
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

    }

}