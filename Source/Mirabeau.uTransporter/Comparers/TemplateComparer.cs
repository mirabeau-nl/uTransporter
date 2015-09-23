using System;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Interfaces;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Comparers
{
    public class TemplateComparer : ITemplateComparer
    {
        private readonly ITemplateReadRepository _templateReadRepository;

        public TemplateComparer(ITemplateReadRepository templateReadRepository)
        {
            _templateReadRepository = templateReadRepository;
        }

        public bool Comparer(Type existingTemplate, ITemplate template)
        {
            TemplateAttribute templateAttributes = _templateReadRepository.GetTemplateAttributes<TemplateAttribute>(existingTemplate);

            if (templateAttributes.Name != template.Name)
            {
                return false;
            }

            if (templateAttributes.Alias != template.Alias)
            {
                return false;
            }

            if (templateAttributes.Content != template.Content)
            {
                return false;
            }

            return true;
        }
    }
}
