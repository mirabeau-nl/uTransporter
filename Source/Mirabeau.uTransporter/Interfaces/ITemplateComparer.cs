using System;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface ITemplateComparer
    {
        bool Comparer(Type existingTemplate, ITemplate template); 
    }
}