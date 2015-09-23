using System;

using Mirabeau.uTransporter.Attributes;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IContentTypeComparer
    {
        bool Compare(Type type, IContentType contentType);

        bool CompareAllowedTemplates(DocumentTypeAttribute attributes, IContentType contentType);

        bool CompaireDefaultTemplates(ITemplate existingAlias, TemplateAttribute newAlias);

        bool CompareAllowedChildNodeTypes(DocumentTypeAttribute attributes, IContentType contentType);
    } 
}