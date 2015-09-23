using System;
using System.Collections.Generic;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Managers;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.UnitTests.Managers
{
    [TestFixture]
    public class TemplateManagerTests
    {
        private IUmbracoService _umbracoService;

        private IAttributeManager _attributeManager;

        private ITemplateReadRepository _templateReadRepository;

        [SetUp]
        public void SetUp()
        {
            _umbracoService = MockRepository.GenerateStub<IUmbracoService>();
            _attributeManager = MockRepository.GenerateStrictMock<IAttributeManager>();
            _templateReadRepository = MockRepository.GenerateMock<ITemplateReadRepository>();
        }

        [Test]
        public void CreateTemplateList_WithEmptyList_ReturnList()
        {
            // Arrange
            IUmbracoService factory = MockRepository.GenerateStrictMock<IUmbracoService>();
            IFileService fileService = MockRepository.GenerateStrictMock<IFileService>();
            factory.Expect(m => m.GetFileService())
                .Return(fileService);

            // Act
            TemplateManager manager = new TemplateManager(factory, _templateReadRepository, _attributeManager);
            DocumentTypeAttribute attribute = new DocumentTypeAttribute();
            attribute.AllowedTemplates = new Type[] { };
            manager.CreateAllowedTemplateList(attribute);

            // Assert
            fileService.VerifyAllExpectations();
            factory.VerifyAllExpectations();
        }

        [Test]
        [Ignore]
        public void CreateTemplateList_WithOneItemList_ReturnList()
        {
            // Arrange
            ITemplate template = MockRepository.GenerateStub<ITemplate>();

            _templateReadRepository.Expect(m => m.GetATemplate("defaulttemplate"))
                .Repeat.Once()
                .Return(template);

            // Act
            TemplateManager manager = new TemplateManager(_umbracoService, _templateReadRepository, _attributeManager);
            DocumentTypeAttribute attribute = new DocumentTypeAttribute();
            attribute.AllowedTemplates = new[] { typeof(DefaultTemplate) };
            manager.CreateAllowedTemplateList(attribute);

            // Assert
            _templateReadRepository.VerifyAllExpectations();
        }

        [Test]
        [Ignore]
        public void GetAllowedTemplateList_ShouldReturnCollectionOfTemplates_ReturnCollection()
        {
            Type[] templates = { typeof(DefaultTemplate) };
            IUmbracoService factory = MockRepository.GenerateMock<IUmbracoService>();
            TemplateManager manager = new TemplateManager(factory, _templateReadRepository, _attributeManager);

            _templateReadRepository.Expect(m => m.GetATemplate(Arg<string>.Is.Anything)).Return(new Template("path", "name", "alias"));

            DocumentTypeAttribute documentTypeAttribute = new DocumentTypeAttribute();
            documentTypeAttribute.AllowedTemplates = templates;
            IList<ITemplate> allowedTemplateList = manager.GetAllowedTemplateList(documentTypeAttribute);

            CollectionAssert.IsNotEmpty(allowedTemplateList);
        }
    }
}