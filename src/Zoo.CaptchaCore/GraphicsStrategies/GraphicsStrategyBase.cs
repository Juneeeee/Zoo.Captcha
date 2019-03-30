namespace Zoo.CaptchaCore.GraphicsStrategies
{
    public abstract class GraphicsStrategyBase : IGraphicsStrategy
    {
        public abstract Captcha Drawing(string code, int width, int height);
        private void ChangeCharPosition()
        {

        }
    }
}
