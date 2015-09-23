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
    public class ContentTypeManagerTests
    {
        private IDocumentFinder _documentFinder;

        private IUmbracoService _umbracoFactory;

        private IRetryableContentTypeService _retryableContentTypeService;

        private IDataTypeService _dataTypeService;

        private IContentTypeService _contentTypeService;

        private IDataTypeManager _dataTypeManager;

        private IContentWriteRepository _contentWriteRepository;

        private IFileService _fileService;

        [SetUp]
        public void SetUp()
        {
            _documentFinder = MockRepository.GenerateStub<IDocumentFinder>();
            _umbracoFactory = MockRepository.GenerateStub<IUmbracoService>();
            _retryableContentTypeService = MockRepository.GenerateStub<IRetryableContentTypeService>();
            _dataTypeService = MockRepository.GenerateStub<IDataTypeService>();
            _contentTypeService = MockRepository.GenerateMock<IContentTypeService>();
            _dataTypeManager = MockRepository.GenerateStub<IDataTypeManager>();
            _contentWriteRepository = MockRepository.GenerateStub<IContentWriteRepository>();
            _fileService = MockRepository.GenerateMock<IFileService>();
        }

        [Test]
        public void Should_Handle_Empty_Collection_Of_Documents()
        {
            // Arrange
            _documentFinder.Expect(m => m.GetAllIDocumentTypesBase(true)).Repeat.Once().Return(new List<Type>());
            _dataTypeManager.Expect(m => m.GetDataTypeDefinition(Arg<DocumentTypePropertyAttribute>.Is.Anything)).Return(new DataTypeDefinition(1, Guid.NewGuid()));
            IContentTypeManager contentTypeManager = new ContentTypeManager(_documentFinder, _retryableContentTypeService, _contentWriteRepository, _umbracoFactory);

            // Act
            contentTypeManager.SaveContentType();

            // Assert
            _documentFinder.VerifyAllExpectations();
        }

        [Test]
        public void Should_Handle_Non_Existing_DocumentType()
        {
            // Arrange
            var documentTypeManager = MockRepository.GenerateStub<IContentTypeManager>();

            // Act
            _contentTypeService.Expect(m => m.GetContentType("NonExistingDocumenttypeName"))
                .Repeat.Once()
                .Return(null);
            documentTypeManager.Expect(m => m.DoesContentTypeExists("NonExistingDocumenttypeName"))
                .Repeat.Once()
               .Return(false);

            // Assert
            documentTypeManager.VerifyAllExpectations();
        }

        [Test]
        public void DoesDocumentTypeExists_ShouldReturnFalseWhenDocumentTypeDoesNotExists_ReturnFalse()
        {
            // Arrange
            IContentTypeService contentTypeService = MockRepository.GenerateMock<IContentTypeService>();
            IFileService fileService = MockRepository.GenerateMock<IFileService>();
            IUmbracoService apiFactory = MockRepository.GenerateStrictMock<IUmbracoService>();
            apiFactory.Expect(m => m.GetContentTypeService()).Return(contentTypeService);
            apiFactory.Expect(m => m.GetFileService()).Return(fileService);
            IContentType contentType = MockRepository.GenerateStub<IContentType>();
            contentTypeService.Expect(m => m.GetContentType("Bar")).Return(contentType);

            // Act
            IContentTypeManager contentTypeManager = new ContentTypeManager(_documentFinder, _retryableContentTypeService, _contentWriteRepository, apiFactory);
            var actual = contentTypeManager.DoesContentTypeExists("Foo");

            // Assert
            Assert.That(actual, Is.EqualTo(false));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DoesDocumentTypeExists_ShouldThrowArgumentNullExceptionWhenArgumentIsEmpty_ReturnException()
        {
            // Arrange
            IUmbracoService apiFactory = MockRepository.GenerateStrictMock<IUmbracoService>();
            apiFactory.Expect(m => m.GetContentTypeService()).Return(_contentTypeService);
            apiFactory.Expect(m => m.GetFileService()).Return(_fileService);

            // Act
            IContentTypeManager contentTypeManager = new ContentTypeManager(_documentFinder, _retryableContentTypeService, _contentWriteRepository, apiFactory);
            contentTypeManager.DoesContentTypeExists(string.Empty);

            // Assert
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DoesDocumentTypeExists_ShouldThrowArgumentNullExceptionWhenArgumentIsNull_ReturnException()
        {
            // Arrange
            IUmbracoService apiFactory = MockRepository.GenerateStrictMock<IUmbracoService>();
            apiFactory.Expect(m => m.GetContentTypeService()).Return(_contentTypeService);
            apiFactory.Expect(m => m.GetFileService()).Return(_fileService);

            // Act
            IContentTypeManager contentTypeManager = new ContentTypeManager(_documentFinder, _retryableContentTypeService, _contentWriteRepository, apiFactory);
            contentTypeManager.DoesContentTypeExists(null);

            // Assert
        }

        [Test]
        public void Should_Save_DocumentType()
        {
            // Arrange
            IDocumentFinder documentFinder = MockRepository.GenerateMock<IDocumentFinder>();
            List<Type> listOfTypes = new List<Type> 
                               {
                                   typeof(TestDocumentTypeBase), 
                                   typeof(TestDocumentTypeBaseEmpty),
                                   typeof(TestDocumentTypeBaseMissingAttribute)
                               };

            // Act
            documentFinder.Expect(m => m.GetAllIDocumentTypesBase(true))
                .Repeat
                .Once()
                .Return(listOfTypes);

            _umbracoFactory.Expect(m => m.GetDataTypeService()).Return(_dataTypeService);
            _umbracoFactory.Expect(m => m.GetContentTypeService()).Return(_contentTypeService);

            IContentWriteRepository repository = MockRepository.GenerateMock<IContentWriteRepository>();
            repository.Expect(m => m.SaveList(listOfTypes)).Repeat.Once();

            IContentTypeManager contentTypeManager = new ContentTypeManager(documentFinder, _retryableContentTypeService, repository, _umbracoFactory);

            contentTypeManager.SaveContentType();

            // Assert
            documentFinder.VerifyAllExpectations();
            repository.VerifyAllExpectations();
        }
    }
}