namespace Zoo.CaptchaCore.GraphicsStrategies
{
    public class GraphicsStrategyManager
    {
        static GraphicsStrategyManager()
        {
            Instance = new GraphicsStrategyManager();
        }
        public static GraphicsStrategyManager Instance { get; private set; }

        public GraphicsStrategyBase GetRandomStrategy()
        {

            return new AGraphicsStrategy();

        }
    }
}
