using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;
using Mirabeau.uTransporter.Models;

namespace Mirabeau.uTransporter.Extensions
{
    /// <summary>
    ///  uTransporter DocumentFinder class - Finds all classes within an assembly 
    ///  that implement a specific interface during umbraco startup.
    /// </summary>
    public class DocumentFinder : IDocumentFinder
    {
        private readonly ILog4NetWrapper _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFinder"/> class.
        /// </summary>
        public DocumentFinder()
        {
            _log = LogManagerWrapper.GetLogger("Mirabeau.uTransporter");
        }

        /// <summary>
        /// Gets the sub class.
        /// </summary>
        /// <param name="implementedInterface">The implemented interface.</param>
        /// <param name="dllFilepath">The DLL filepath.</param>
        /// <param name="checkForSystemObject">if set to <c>true</c> [check for system object].</param>
        /// <returns></returns>
        public List<Type> GetSubClass(Type implementedInterface, string dllFilepath, bool checkForSystemObject)
        {
            List<Type> documentObjectList = new List<Type>();
            try
            {
                Assembly assembly = Assembly.UnsafeLoadFrom(dllFilepath);
                documentObjectList = GetSubClassOf(implementedInterface, checkForSystemObject, assembly);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }

            return documentObjectList;
        }

        /// <summary>
        /// Gets the sub class of.
        /// </summary>
        /// <param name="implementedInterface">The implemented interface.</param>
        /// <param name="checkForSystemObject">if set to <c>true</c> [check for system object].</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        public List<Type> GetSubClassOf(Type implementedInterface, bool checkForSystemObject, params System.Reflection.Assembly[] assemblies)
        {
            var documentObjectList = new List<Type>();

            IEnumerable<Assembly> assembliesToScan = this.GetAssembliesToScan(assemblies);

            Parallel.ForEach(assembliesToScan, assembly =>
            {
                Parallel.ForEach(assembly.GetTypes(), type =>
                {
                    GetImplementedMembers(implementedInterface, checkForSystemObject, type, documentObjectList);
                });
            });

            return documentObjectList;
        }

        /// <summary>
        /// Gets the child types from a base class.
        /// </summary>
        /// <param name="baseClass">The base class.</param>
        /// <returns></returns>
        public List<Type> GetChildTypes(Type baseClass)
        {
            var childTypes = new List<Type>();
            List<Type> objectList = GetSubClassOf(typeof(IDocumentTypeBase), false, Thread.GetDomain().GetAssemblies());

            foreach (Type objects in objectList)
            {
                if (objects.BaseType != null)
                {
                    if (objects.BaseType == baseClass || (baseClass.IsGenericType && baseClass.Name == objects.BaseType.Name)
                        || objects.GetInterfaces().FirstOrDefault(i => i == baseClass) != null)
                    {
                        _log.Indent(4);
                        _log.Info("Subclass found {0}", objects);
                        childTypes.Add(objects);
                    }
                }
            }

            return childTypes;
        }

        /// <summary>
        /// Gets all document types base.
        /// </summary>
        /// <param name="checkForSystemObject">if set to <c>true</c> [check for system object].</param>
        /// <returns></returns>
        public List<Type> GetAllIDocumentTypesBase(bool checkForSystemObject)
        {
            return GetSubClassOf(typeof(IDocumentTypeBase), checkForSystemObject, Thread.GetDomain().GetAssemblies());
        }

        private IEnumerable<Assembly> GetAssembliesToScan(IEnumerable<Assembly> assemblies)
        {
            return assemblies.ToList().Where(
                assembly => assembly.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase) == false
                            && assembly.FullName.StartsWith("DynamicProxyGenAssembly2,", StringComparison.OrdinalIgnoreCase) == false
                            && assembly.FullName.StartsWith("mscorelib") == false
                            && assembly.FullName.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase) == false);
        }

        private void GetImplementedMembers(Type implementedInterface, bool checkForSystemObject, Type type,
           List<Type> documentObjectList)
        {
            try
            {
                if (implementedInterface.IsInterface)
                {
                    if (type.GetInterface(implementedInterface.FullName) != null)
                    {
                        if (checkForSystemObject == false || type.BaseType == typeof(object))
                        {
                            lock (documentObjectList)
                            {
                                documentObjectList.Add(type);
                            }
                        }
                    }
                }
                else if (type.IsSubclassOf(implementedInterface))
                {
                    documentObjectList.Add(type);
                }
            }
            catch (Exception reflectionException)
            {
                HandleException(reflectionException);
            }
        }

        private void HandleException(Exception exception)
        {
            if (exception is ReflectionTypeLoadException)
            {
                ReflectionTypeLoadException typeLoadException = exception as ReflectionTypeLoadException;

                _log.Error("exception occured", typeLoadException);

                Exception[] loaderExceptions = typeLoadException.LoaderExceptions;

                foreach (Exception loaderException in loaderExceptions)
                {
                    _log.Error("exception occured, current loaderException", loaderException);
                }
            }
            else
            {
                _log.Error("Error occured", exception);
            }
        }
    }
}