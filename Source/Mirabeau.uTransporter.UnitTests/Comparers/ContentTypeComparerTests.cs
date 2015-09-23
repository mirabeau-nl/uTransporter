using System;
using System.Collections.Generic;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Builders;
using Mirabeau.uTransporter.Comparers;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Comparers
{
    [TestFixture]
    public class ContentTypeComparerTests
    {
        private IContentReadRepository _contentReader;

        private IPropertyComparer _propertyComparer;

        private ITemplate _template;

        private IContentType contentTypeClean;

        private IAttributeManager _attributeManager;

        private IManagerFactory _managerFactory;

        private IContentTypeFactory _contentTypeFactory;

        [SetUp]
        public void SetUp()
        {
            _contentReader = MockRepository.GenerateMock<IContentReadRepository>();
            _propertyComparer = MockRepository.GenerateStub<IPropertyComparer>();
            _template = MockRepository.GenerateMock<ITemplate>();
            _attributeManager = MockRepository.GenerateStrictMock<IAttributeManager>();
            _managerFactory = MockRepository.GenerateMock<IManagerFactory>();
            _contentTypeFactory = MockRepository.GenerateMock<IContentTypeFactory>();

            var contentTypeBuilder = new ContentTypeWithoutAliasBuilder(-1)
                .WithName("BarPage")
                // .WithAlias("barpage")
                .WithDescription("A Description")
                .IsAllowedAtRoot(true)
                .AllowedTemplateList(new List<ITemplate>())
                .WithIcon("folder.gif")
                .WithThumbnail("folder.png");

            contentTypeClean = contentTypeBuilder.Build();
        }

        [Test]
        [Ignore("Need fixing")]
        public void Compare_WhenTheDocumentTypeAttributeAndIContentTypeAreTheSame_ReturnTrue()
        {
            // Arrange
            DocumentTypeAttribute documentTypeAttribute = new DocumentTypeAttribute
            {
                Name = "BarPage",
                //Alias = "barpage",
                Description = "A Description",
                AllowAtRoot = true,
                AllowedTemplates = new Type[] { },
                Icon = "folder.gif",
                Thumbnail = "folder.png"
            };

            // Act
            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(_managerFactory, _propertyComparer, _contentTypeFactory);
            _attributeManager.Expect(m => m.GetContentTypeAttributes<DocumentTypeAttribute>(typeof(TestDocumentTypeBase))).Repeat.Once().Return(documentTypeAttribute);
            _propertyComparer.Expect(m => m.Compare(Arg<Type>.Is.Anything, Arg<IContentType>.Is.Anything)).Return(true);
            bool actual = contentTypeComparer.Compare(typeof(TestDocumentTypeBase), contentTypeClean);

            // Assert
            Assert.That(actual, Is.EqualTo(true));
            _attributeManager.VerifyAllExpectations();

            _propertyComparer.VerifyAllExpectations();
        }

        [Test]
        [Ignore("Need fixing")]
        public void Compaire_WhenTheDocumentTypeAttributeAndIContentTypeAreNotTheSame_ReturnFalse()
        {
            // Arrange
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

            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(_managerFactory, _propertyComparer, _contentTypeFactory);
            _attributeManager.Expect(m => m.GetContentTypeAttributes<DocumentTypeAttribute>(typeof(TestDocumentTypeBase))).Repeat.Once().Return(documentTypeAttribute);

            // Act
            bool actual = contentTypeComparer.Compare(typeof(TestDocumentTypeBase), contentTypeClean);

            // Assert
            Assert.That(actual, Is.EqualTo(false));
            _contentReader.VerifyAllExpectations();
        }

        [Test]
        [Ignore("Need fixing")]
        public void Compaire_WhenTheDocumentTypeAttributeThumbnailAndIContentTypeThumbnailAreNotTheSame_ReturnFalse()
        {
            // Arrange
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

            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(_managerFactory, _propertyComparer, _contentTypeFactory);
            _attributeManager.Expect(m => m.GetContentTypeAttributes<DocumentTypeAttribute>(typeof(TestDocumentTypeBase))).Repeat.Once().Return(documentTypeAttribute);

            // Act
            bool actual = contentTypeComparer.Compare(typeof(TestDocumentTypeBase), contentTypeClean);

            // Assert
            Assert.That(actual, Is.EqualTo(false));
            _contentReader.VerifyAllExpectations();
        }

        [Test]
        [Ignore("Need fixing")]
        public void Compaire_WhenTheDocumentTypeAttributeTemplateAndIContentTypeTemplateAreNotTheSame_ReturnFalse()
        {
            // Arrange
            ITemplate template = new Template(string.Empty, "template", "template");
            ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(-1)
                .WithName("BarPage")
                .WithAlias("barpage")
                .WithDescription("A Different Description")
                .IsAllowedAtRoot(true)
                .AllowedTemplateList(new List<ITemplate> { template })
                .WithDefaultTemplate(_template)
                .WithIcon("folder.gif")
                .WithThumbnail("folder.png");

            IContentType contentType = contentTypeBuilder.Build();

            DocumentTypeAttribute documentTypeAttribute = new DocumentTypeAttribute
            {
                Name = "BarPage",
                Alias = "barpage",
                Description = "A Description",
                AllowAtRoot = true,
                AllowedTemplates = new[] { typeof(DefaultTemplate) },
                DefaultTemplate = typeof(DefaultTemplate),
                Icon = "folder.gif",
                Thumbnail = "folder.png"
            };

            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(_managerFactory, _propertyComparer, _contentTypeFactory);
            _attributeManager.Expect(m => m.GetContentTypeAttributes<DocumentTypeAttribute>(typeof(TestDocumentTypeBase))).Repeat.Once().Return(documentTypeAttribute);

            // Act
            bool actual = contentTypeComparer.Compare(typeof(TestDocumentTypeBase), contentType);

            // Assert
            Assert.That(actual, Is.EqualTo(false));
            _contentReader.VerifyAllExpectations();
        }

        [Test]
        [Ignore("Need fixing")]
        public void CompaireAllowedTemplates_WhenTemplatesAreNull_ReturnTrue()
        {
            // Arrange
            ITemplate template = new Template("path", "template", "template");

            ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(-1)
                .WithName("BarPage")
                .WithAlias("barpage")
                .WithDescription("A Different Description")
                .IsAllowedAtRoot(true)
                //.WithDefaultTemplate(template)
                .WithIcon("folder.gif")
                .WithThumbnail("folder.png");
            IContentType contentType = contentTypeBuilder.Build();

            DocumentTypeAttribute documentTypeAttribute = new DocumentTypeAttribute
            {
                Name = "BarPage",
                Alias = "barpage",
                Description = "A Description",
                AllowAtRoot = true,
                AllowedTemplates = new Type[] { },
                DefaultTemplate = typeof(DefaultTemplate),
                Icon = "folder.gif",
                Thumbnail = "folder.png"
            };

            // Act
            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(_managerFactory, _propertyComparer, _contentTypeFactory);
            var actual = contentTypeComparer.CompareAllowedTemplates(documentTypeAttribute, contentType);

            // Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        [Ignore("Need fixing")]
        public void CompaireAllowedTemplates_WhenAllowedTemplateCountIsNotEqual_ReturnFalse()
        {
            // Arrange
            ITemplateManager templateManager = MockRepository.GenerateMock<ITemplateManager>();

            ITemplate templateOne = new Template(string.Empty, "templateOne", "templateOne");
            ITemplate templateTwo = new Template(string.Empty, "templateTwo", "templateTwo");
            ITemplate templateThree = new Template(string.Empty, "templateThree", "templateThree");

            ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(-1)
                .WithName("BarPage")
                .WithAlias("barpage")
                .WithDescription("A Different Description")
                .IsAllowedAtRoot(true)
                .WithDefaultTemplate(_template)
                .AllowedTemplateList(new List<ITemplate> { templateTwo, templateThree })
                .WithIcon("folder.gif")
                .WithThumbnail("folder.png");
            IContentType contentType = contentTypeBuilder.Build();

            DocumentTypeAttribute documentTypeAttribute = new DocumentTypeAttribute
            {
                Name = "BarPage",
                Alias = "barpage",
                Description = "A Description",
                AllowAtRoot = true,
                AllowedTemplates = new[] { typeof(DefaultTemplate) },
                DefaultTemplate = typeof(DefaultTemplate),
                Icon = "folder.gif",
                Thumbnail = "folder.png"
            };

            templateManager.Expect(m => m.GetAllowedTemplateList(documentTypeAttribute))
                .IgnoreArguments()
                .Return(new List<ITemplate> { templateOne });

            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(
                _managerFactory,
                _propertyComparer,
                _contentTypeFactory);

            // Act
            var actual = contentTypeComparer.CompareAllowedTemplates(documentTypeAttribute, contentType);

            // Assert
            Assert.That(actual, Is.EqualTo(false));
            templateManager.VerifyAllExpectations();
        }

        [Test]
        [Ignore("Need fixing")]
        public void CompaireAllowedTemapltes_WhenAllowedTemplateCountIsEqualButAliasesAreDifferent_ReturnFalse()
        {
            // Arrange
            ITemplateManager templateManager = MockRepository.GenerateMock<ITemplateManager>();

            ITemplate templateOne = new Template(string.Empty, "templateOne", "templateOne");
            ITemplate templateTwo = new Template(string.Empty, "templateTwo", "templateTwo");
            ITemplate templateThree = new Template(string.Empty, "templateThree", "templateThree");
            ITemplate templateFour = new Template(string.Empty, "templateFour", "templateFour");

            ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(-1)
                .WithName("BarPage")
                .WithAlias("barpage")
                .WithDescription("A Different Description")
                .IsAllowedAtRoot(true)
                .WithDefaultTemplate(_template)
                .AllowedTemplateList(new List<ITemplate> { templateOne, templateTwo })
                .WithIcon("folder.gif")
                .WithThumbnail("folder.png");

            IContentType contentType = contentTypeBuilder.Build();

            DocumentTypeAttribute documentTypeAttribute = new DocumentTypeAttribute
            {
                Name = "BarPage",
                Alias = "barpage",
                Description = "A Description",
                AllowAtRoot = true,
                AllowedTemplates = new[] { typeof(DefaultTemplate) },
                DefaultTemplate = typeof(DefaultTemplate),
                Icon = "folder.gif",
                Thumbnail = "folder.png"
            };

            templateManager.Expect(m => m.GetAllowedTemplateList(documentTypeAttribute))
                .IgnoreArguments()
                .Return(new List<ITemplate> { templateThree, templateFour });

            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(
                _managerFactory,
                _propertyComparer,
                _contentTypeFactory);

            // Act
            var actual = contentTypeComparer.CompareAllowedTemplates(documentTypeAttribute, contentType);

            // Assert
            Assert.That(actual, Is.EqualTo(false));
            templateManager.VerifyAllExpectations();
        }

        [Test]
        [Ignore("Need fixing")]
        public void CompaireAllowedTemapltes_WhenAllowedTemplateAreTheSame_ReturnTrue()
        {
            // Arrange
            ITemplateManager templateManager = MockRepository.GenerateMock<ITemplateManager>();
            ITemplate templateOne = new Template(string.Empty, "templateOne", "templateOne");

            ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(-1)
                .WithName("BarPage")
                .WithAlias("barpage")
                .WithDescription("A Different Description")
                .IsAllowedAtRoot(true)
                .WithDefaultTemplate(_template)
                .AllowedTemplateList(new List<ITemplate> { templateOne })
                .WithIcon("folder.gif")
                .WithThumbnail("folder.png");

            IContentType contentType = contentTypeBuilder.Build();

            DocumentTypeAttribute documentTypeAttribute = new DocumentTypeAttribute
            {
                Name = "BarPage",
                Alias = "barpage",
                Description = "A Description",
                AllowAtRoot = true,
                AllowedTemplates = new[] { typeof(DefaultTemplate) },
                DefaultTemplate = typeof(DefaultTemplate),
                Icon = "folder.gif",
                Thumbnail = "folder.png"
            };

            templateManager.Expect(m => m.GetAllowedTemplateList(documentTypeAttribute))
                .IgnoreArguments()
                .Return(new List<ITemplate> { templateOne });

            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(
                _managerFactory,
                _propertyComparer,
                _contentTypeFactory);

            // Act
            var actual = contentTypeComparer.CompareAllowedTemplates(documentTypeAttribute, contentType);

            // Assert
            Assert.That(actual, Is.EqualTo(true));
            templateManager.VerifyAllExpectations();
        }

        [Test]
        [Ignore("Need fixing")]
        public void CompareAllowedChildNodeTypes_ShouldReturnFalseWhenDocumentTypeAllowedChildNodeTypeAreNotTheSame_ReturnFalse()
        {
            // Arrange
            IContentTypeFactory contentTypeFactory = MockRepository.GenerateMock<IContentTypeFactory>();
            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(
                _managerFactory,
                _propertyComparer,
                _contentTypeFactory);

            List<ContentTypeSort> contentTypeSorts = new List<ContentTypeSort>();
            contentTypeSorts.Add(new ContentTypeSort { Alias = "testdoc", Id = new Lazy<int>(() => 324) });

            contentTypeFactory.Expect(m => m.CreateAllowedChildNodeTypeStructure(Arg<DocumentTypeAttribute>.Is.Anything)).IgnoreArguments().Repeat.Once().Return(contentTypeSorts);

            ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(-1)
                .WithName("BarPage")
                .WithAlias("barpage")
                .WithDescription("A Different Description")
                .IsAllowedAtRoot(true)
                .WithIcon("folder.gif")
                .WithThumbnail("folder.png");

            IContentType contentType = contentTypeBuilder.Build();

            // Act
            var actual = contentTypeComparer.CompareAllowedChildNodeTypes(new DocumentTypeAttribute(), contentType);

            // Assert
            Assert.That(actual, Is.EqualTo(false));
            contentTypeFactory.VerifyAllExpectations();
        }

        [Test]
        [Ignore("Need fixing")]
        public void CompareAllowedChildNodeTypes_ShouldReturnFalseWhenChildNodeTypesCountIsTheSameButAliasesAreDifferent_ReturnFalse()
        {
            // Arrange
            IContentTypeFactory contentTypeFactory = MockRepository.GenerateMock<IContentTypeFactory>();
            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(
                _managerFactory,
                _propertyComparer,
                _contentTypeFactory);

            List<ContentTypeSort> contentTypeSortsOne = new List<ContentTypeSort>();
            contentTypeSortsOne.Add(new ContentTypeSort { Alias = "testdoc", Id = new Lazy<int>(() => 324) });

            List<ContentTypeSort> contentTypeSortsTwo = new List<ContentTypeSort>();
            contentTypeSortsTwo.Add(new ContentTypeSort { Alias = "testdocWithDifferentAlias", Id = new Lazy<int>(() => 3454) });

            contentTypeFactory.Expect(m => m.CreateAllowedChildNodeTypeStructure(Arg<DocumentTypeAttribute>.Is.Anything)).IgnoreArguments().Repeat.Once().Return(contentTypeSortsOne);

            ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(-1)
                .WithName("BarPage")
                .WithAlias("barpage")
                .WithDescription("A Different Description")
                .IsAllowedAtRoot(true)
                .WithIcon("folder.gif")
                .WithThumbnail("folder.png");

            IContentType contentType = contentTypeBuilder.Build();
            contentType.AllowedContentTypes = contentTypeSortsTwo;

            // Act
            var actual = contentTypeComparer.CompareAllowedChildNodeTypes(new DocumentTypeAttribute(), contentType);

            // Assert
            Assert.That(actual, Is.EqualTo(false));
            contentTypeFactory.VerifyAllExpectations();
        }

        [Test]
        [Ignore("Need fixing")]
        public void CompareAllowedChildNodeTypes_ShouldReturnTrueWhenChildNodeTypesCountIsTheSameAndAliasesAreTheSame_ReturnTrue()
        {
            // Arrange
            IContentTypeFactory contentTypeFactory = MockRepository.GenerateMock<IContentTypeFactory>();
            IContentTypeComparer contentTypeComparer = new ContentTypeComparer(
                _managerFactory,
                _propertyComparer,
                _contentTypeFactory);

            List<ContentTypeSort> contentTypeSorts = new List<ContentTypeSort>();
            contentTypeSorts.Add(new ContentTypeSort { Alias = "testdoc", Id = new Lazy<int>(() => 324) });

            contentTypeFactory.Expect(m => m.CreateAllowedChildNodeTypeStructure(Arg<DocumentTypeAttribute>.Is.Anything)).IgnoreArguments().Repeat.Once().Return(contentTypeSorts);

            ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(-1)
                .WithName("BarPage")
                .WithAlias("barpage")
                .WithDescription("A Different Description")
                .IsAllowedAtRoot(true)
                .WithIcon("folder.gif")
                .WithThumbnail("folder.png");

            IContentType contentType = contentTypeBuilder.Build();
            contentType.AllowedContentTypes = contentTypeSorts;

            // Act
            var actual = contentTypeComparer.CompareAllowedChildNodeTypes(new DocumentTypeAttribute(), contentType);

            // Assert
            Assert.That(actual, Is.EqualTo(true));
            contentTypeFactory.VerifyAllExpectations();
        }
    }
}
