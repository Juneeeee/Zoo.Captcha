using System;
using System.Drawing;
using Xunit;

namespace Zoo.CaptchaCore.Tests
{
    public class GraphicsStrategyTest
    {
        [Fact]
        public void Test1()
        {
            //var strategy = new CGraphicsStrategy();
            //strategy.Drawing("A", 10, 10);
        }
        [Fact]
        public void MaxWidthAndHeight()
        {
            var points = new PointF[] { new PointF(0, 1), new PointF(1, 2), new PointF(3, 4), new PointF(6, 7) };
            int w = 0, h = 0;
            for (int i = 0; i < points.Length - 1; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    var xValue = Math.Abs(points[i].X - points[j].X);
                    if (w < xValue)
                        w = (int)xValue;
                    var yValue = Math.Abs(points[i].Y - points[j].Y);
                    if (h < yValue)
                        h = (int)yValue;
                }
            }


        }
    }
}
