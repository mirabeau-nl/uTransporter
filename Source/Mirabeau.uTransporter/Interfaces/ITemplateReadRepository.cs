using System;
using System.Collections.Generic;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface ITemplateReadRepository
    {
        ITemplate GetATemplate(string alias);

        IEnumerable<ITemplate> GetAllTemplates();

        T GetTemplateAttributes<T>(Type type);

        int CountAllTemplates();
    }
}