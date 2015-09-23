using System.Collections.Generic;

using Mirabeau.uTransporter.Attributes;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface ITemplateManager
    {
        IList<ITemplate> CreateAllowedTemplateList(DocumentTypeAttribute attribute);

        IList<ITemplate> GetAllowedTemplateList(DocumentTypeAttribute attribute);

        ITemplate GetTheDefaultTemplateOrCreateIt(DocumentTypeAttribute documentType);
    }
}