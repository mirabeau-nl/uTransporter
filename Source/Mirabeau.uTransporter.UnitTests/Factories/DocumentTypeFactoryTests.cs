using System;
using System.Collections.Generic;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Exceptions;
using Mirabeau.uTransporter.Factories;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Factories
{
    public class DocumentTypeFactoryTests
    {
        private IManagerFactory managerFactory;

        private IPropertyFactory propertyFactory;

        private IContentReadRepository contentReadRepository;

        [SetUp]
        public void SetUp()
        {
            managerFactory = MockRepository.GenerateStub<IManagerFactory>();
            propertyFactory = MockRepository.GenerateStub<IPropertyFactory>();
            contentReadRepository = MockRepository.GenerateStub<IContentReadRepository>();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDocumentType_ShouldThrowArgumentNullExceptionWhenBothAgrumentsAreNull_ThrowException()
        {
            // Arrange
            IContentTypeFactory factory = new ContentTypeFactory(managerFactory, propertyFactory, contentReadRepository);

            factory.CreateDocumentType(null, null, 0);

            // Assert
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDocumentType_ShouldThrowArgumentNullExceptionWhenOneOutOfTwoAgrumentsAreNull_ThrowException()
        {
            // Arrange
            IContentTypeFactory factory = new ContentTypeFactory(managerFactory, propertyFactory, contentReadRepository);

            // Act
            factory.CreateDocumentType(typeof(TestDocumentTypeBase), null, 0);

            // Assert
        }

        [Test]
        [ExpectedException(typeof(AliasNullException))]
        public void CreateDocumentType_ShouldThrowExceptionWhenDocAliasIsNull_ThrowException()
        {
            // Arrange
            IContentTypeFactory factory = new ContentTypeFactory(managerFactory, propertyFactory, contentReadRepository);

            DocumentTypeAttribute documentTypeAttribute = new DocumentTypeAttribute
            {
                Name = "BarPage",
                Description = "A Description",
                AllowAtRoot = true,
                AllowedTemplates = new Type[] { },
                Icon = "folder.gif",
                Thumbnail = "folder.png"
            };

            // Act
            factory.CreateDocumentType(typeof(TestDocumentTypeBase), documentTypeAttribute, 0);

            // Assert
        }

        [Test]
        public void CreateDocumentType_ShouldCreateAnContentType_ReturnContentTypeObject()
        {
            // Arrange
            var templateManager = MockRepository.GenerateMock<ITemplateManager>();
            managerFactory.Expect(m => m.CreateTemplateManager()).Return(templateManager);

            templateManager.Expect(m => m.CreateAllowedTemplateList(Arg<DocumentTypeAttribute>.Is.Anything)).Return(new List<ITemplate>());
            templateManager.Expect(m => m.GetTheDefaultTemplateOrCreateIt(Arg<DocumentTypeAttribute>.Is.Anything)).Return(null);

            IContentTypeFactory factory = new ContentTypeFactory(managerFactory, propertyFactory, contentReadRepository);

            DocumentTypeAttribute documentTypeAttribute = new DocumentTypeAttribute
            {
                Name = "BarPage",
                Alias = "barpage",
                Description = "A Description",
                AllowAtRoot = true,
                AllowedTemplates = new Type[] { },
                Icon = "folder.gif",
                Thumbnail = "folder.png"
            };

            // Act
            var actual = factory.CreateDocumentType(typeof(TestDocumentTypeBase), documentTypeAttribute, -1, new ContentTypeWithoutAliasBuilder(-1));

            // Assert
            Assert.That(actual, Is.TypeOf<ContentType>());
        }
    }
}
