using System.Diagnostics;

using Mirabeau.uTransporter.Extensions;

using StructureMap.Configuration.DSL;

namespace Mirabeau.uTransporter.Services
{
    public class CoreRegistry : Registry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreRegistry"/> class.
        /// </summary>
        public CoreRegistry()
        {
            For<Stopwatch>().Use<Timer>();
        }
    }
}
