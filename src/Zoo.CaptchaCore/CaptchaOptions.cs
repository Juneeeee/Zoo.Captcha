namespace Zoo.CaptchaCore
{
    public class CaptchaOptions
    {
        public int ImgWidth { get; set; }
        public int ImgHeight { get; set; }
        public int MinCharsLength { get; set; }
        public int MaxCharsLength { get; set; }
        public string FontColor { get; set; }
        public string BackgroundColor { get; set; }
    }
}
