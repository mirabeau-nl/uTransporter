using System;
using System.Collections.Generic;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IDocumentFinder
    {
        List<Type> GetAllIDocumentTypesBase(bool checkForSystemObject);

        List<Type> GetSubClassOf(Type implementedInterface, bool checkForSystemObject,
            params System.Reflection.Assembly[] assemblies);

        List<Type> GetChildTypes(Type baseClass);
    }
}
