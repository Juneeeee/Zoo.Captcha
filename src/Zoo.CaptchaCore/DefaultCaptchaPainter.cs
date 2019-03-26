using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zoo.CaptchaCore
{
    public interface ICaptchaPainter
    {
        Captcha Paint(string code);
    }
    public class DefaultCaptchaPainter : ICaptchaPainter
    {
        private readonly IEffectPorvider _effectPorvider;
        public DefaultCaptchaPainter(
            IEffectPorvider effectPorvider)
        {
            _effectPorvider = effectPorvider;
        }
        public Captcha Paint(string code)
        {
            using (Bitmap image = new Bitmap(216, 96))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.Clear(Color.FromArgb(247, 247, 247));
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    //生成随机字符
                    var length = RandomUtils.ToNumber(7, 10);
                    var chars = RandomUtils.ToChars(length);

                    //绘制字体 
                    GraphicsPath path = new GraphicsPath(FillMode.Alternate);
                    var firstLineCount = (int)length / 2;
                    var toLeft = 0;
                    var charWidth = 25;
                    for (int i = 0; i < length; i++)
                    {
                        int x = 0, y = 0;
                        if (i < firstLineCount)
                        {
                            if (i == 0)
                                toLeft = RandomUtils.ToNumber(10, 60);
                            y = 5;
                            x = toLeft + (i * charWidth);
                        }
                        else
                        {
                            y = 40;
                            if (i - firstLineCount == 0)
                                toLeft = RandomUtils.ToNumber(5, 40);
                            x = toLeft + ((i - firstLineCount) * charWidth);
                        }
                        path.AddString(chars[i].ToString(), new FontFamily("Consolas"), (int)FontStyle.Bold, 50, new PointF(x, y), StringFormat.GenericTypographic);
                    }
                    g.DrawPath(new Pen(Color.FromArgb(51, 122, 183), 2.5f), path);

                    //字体增加特效影响
                    _effectPorvider.Effect(image);

                    //写入数据流
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    var data = stream.ToArray();
                    g.Dispose();
                    image.Dispose();

                    return new Captcha(chars, data, "image/png");//content-type同ImageFormat需为同一类型
                }
            }

        }
    }
}
