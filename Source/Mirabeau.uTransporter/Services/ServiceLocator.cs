
using StructureMap;

namespace Mirabeau.uTransporter.Services
{
    /// <summary>
    ///  uTransporter ServiceLocator - Custom Inversion fo control
    ///  and dependency injection class
    /// </summary>
    public class ServiceLocator : IBootstrapper
    {
        private static volatile bool _hasStarted;

        /// <summary>
        /// Bootstraps this instance.
        /// </summary>
        public static void Bootstrap()
        {
            new ServiceLocator().BootstrapStructureMap();
        }

        /// <summary>
        ///  Init ObjectFactory and add Registry classes
        /// </summary>
        public void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(
                x =>
                {
                    x.AddRegistry(new CoreRegistry());
                    x.AddRegistry(new ScanningRegistry());
                });
        }

        /// <summary>
        /// Restarts this instance.
        /// </summary>
        public void Restart()
        {
            if (_hasStarted)
            {
                Bootstrap();
            }
            else
            {
                Bootstrap();
                _hasStarted = true;
            }
        }
    }
}