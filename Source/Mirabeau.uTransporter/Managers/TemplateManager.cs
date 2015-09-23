using System;
using System.Collections.Generic;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Builders;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;

using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.Managers
{
    public class TemplateManager : ITemplateManager
    {
        private static object lockObject = new object();

        private readonly IFileService _fileService;

        private readonly ITemplateReadRepository _templateReadRepository;

        private readonly IAttributeManager _attributeManager;
        
        private readonly ILog4NetWrapper _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateManager"/> class.
        /// </summary>
        /// <param name="umbracoFactory">The umbraco factory.</param>
        /// <param name="templateReadRepository">The template read repository.</param>
        public TemplateManager(IUmbracoService umbracoFactory, ITemplateReadRepository templateReadRepository, IAttributeManager attributeManager)
        {
            _fileService = umbracoFactory.GetFileService();
            _templateReadRepository = templateReadRepository;
            _attributeManager = attributeManager;
            _log = LogManagerWrapper.GetLogger("Mirabeau.uTransporter");
        }

        /// <summary>
        /// Creates the allowed template list.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>List of Template objects</returns>
        public IList<ITemplate> CreateAllowedTemplateList(DocumentTypeAttribute attribute)
        {
            IList<ITemplate> allowedTemplates = new List<ITemplate>();
            if (attribute.AllowedTemplates != null)
            {
                foreach (Type templateType in attribute.AllowedTemplates)
                {
                    TemplateAttribute templateAttribute = _attributeManager.GetTemplateAttributes<TemplateAttribute>(templateType);
                    ITemplate template = _templateReadRepository.GetATemplate(templateAttribute.Alias);

                    if (template != null)
                    {
                        allowedTemplates.Add(template);
                    }
                    else
                    {
                        ITemplate retVal = CreateTemplate(templateAttribute);
                        allowedTemplates.Add(retVal);
                    }
                }
            }

            return allowedTemplates;
        }

        /// <summary>
        /// Gets the allowed template list.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>List of allowed templates</returns>
        public IList<ITemplate> GetAllowedTemplateList(DocumentTypeAttribute attribute)
        {
            IList<ITemplate> allowedTemplates = new List<ITemplate>();
            if (attribute.AllowedTemplates != null || attribute.AllowedTemplates.Length != 0)
            {
                foreach (Type templateType in attribute.AllowedTemplates)
                {
                    TemplateAttribute templateAttribute = _attributeManager.GetTemplateAttributes<TemplateAttribute>(templateType);
                    ITemplate template = _templateReadRepository.GetATemplate(templateAttribute.Alias);

                    if (template != null)
                    {
                        allowedTemplates.Add(template);
                    }
                }
            }

            return allowedTemplates;
        }

        /// <summary>
        /// Creates the template.
        /// </summary>
        /// <param name="templateDefintion">The template defintion.</param>
        /// <returns>Returns an ITemplate object</returns>
        public ITemplate CreateTemplate(TemplateAttribute templateDefintion)
        {
            TemplateBuilder templateBuilder = new TemplateBuilder(templateDefintion.Name, templateDefintion.Alias);
            templateBuilder.WithContent(templateDefintion.Content);
            templateBuilder.WithCreateDate(DateTime.Now);
            templateBuilder.WithUpdateDate(DateTime.Now);

            ITemplate template = templateBuilder.Build();

            lock (lockObject)
            {
                _fileService.SaveTemplate(template);
            }
            
            _log.Indent(3);
            _log.Info("Template with name {0} did not exist, created it.", templateDefintion.Name);

            return _templateReadRepository.GetATemplate(template.Alias);
        }

        /// <summary>
        /// Gets the default template or create it.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns>ITemplate object</returns>
        public ITemplate GetTheDefaultTemplateOrCreateIt(DocumentTypeAttribute documentType)
        {
            ITemplate template = null;
            if (documentType.DefaultTemplate != null)
            {
                TemplateAttribute templateAttribute = _attributeManager.GetTemplateAttributes<TemplateAttribute>(documentType.DefaultTemplate);
                template = _templateReadRepository.GetATemplate(templateAttribute.Alias);

                // if the updated template does not exists, add it
                if (template == null)
                {
                    return CreateTemplate(templateAttribute);
                }
                
            }

            return template;
        }
    }
}