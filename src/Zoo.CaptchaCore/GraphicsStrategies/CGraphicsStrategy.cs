using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

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

                    var fontFamily = new FontFamily("Arial");
                    //当前绘制字体
                    string c;
                    List<GraphicsPath> paths = new List<GraphicsPath>();
                    for (int i = 0; i < length; i++)
                    {
                        c = chars[i].ToString();
                        var path = Transform(c, fontFamily);
                        if (i > 0)
                        {

                        }
                        paths.Add(path);

                    }

                }
            }

            throw new NotImplementedException();
        }

        private void Move(List<Point> points, Point[] point)
        {
            while (true)
            {
                if (!IsCover(points, point))
                    break;
                for (int i = 0; i < point.Length; i++)
                {
                    point[i].X += 1;
                }
            }
            
        }
        //判断像素数组是否存在像素数组的集合中
        private bool IsCover(List<Point> points, Point[] point)
        {
            for (int i = 0; i < points.Count; i++)
            {

            }
            return false;
        }

        private GraphicsPath Transform(string c, FontFamily fontFamily)
        {
            using (var path = new GraphicsPath())
            {
                path.AddString(c, fontFamily, (int)FontStyle.Bold, 60, new PointF(0, 0), StringFormat.GenericTypographic);
                return path;
            }
        }
    }
}
