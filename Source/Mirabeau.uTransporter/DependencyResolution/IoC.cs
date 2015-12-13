namespace Mirabeau.uTransporter.DependencyResolution
{
    public class IoC
    {
        /// <summary>
        /// Bootstraps this instance.
        /// </summary>
        public static void Bootstrap()
        {
            new IoC().Initialize();
        }

        /// <summary>
        ///  Init ObjectFactory and add Registry classes
        /// </summary>
        /// 
        public void Initialize()
        {
            ServiceLocator.Bootstrap();
        }
    }
}
