using System;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IPropertyComparer
    {
        bool Compare(Type documentType, IContentType contentType);
    }
}