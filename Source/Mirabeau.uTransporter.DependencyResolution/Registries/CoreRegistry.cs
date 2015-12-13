using System.Diagnostics;

using StructureMap.Configuration.DSL;


namespace Mirabeau.uTransporter.DependencyResolution.Registries
{
    public class CoreRegistry : Registry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreRegistry"/> class.
        /// </summary>
        public CoreRegistry()
        {
            //    For<Stopwatch>().Use<Timer>();
        }
    }
}
