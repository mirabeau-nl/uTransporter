﻿using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Mirabeau.uTransporter.DependencyResolution.Registries
{
    public class ScanningRegistry : Registry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanningRegistry"/> class.
        /// </summary>
        public ScanningRegistry()
        {
            Scan(
                x =>
                {
                    x.TheCallingAssembly();
                    x.Assembly("Mirabeau.uTransporter");
                    x.WithDefaultConventions();
                });
        }
    }
}