using Mirabeau.uTransporter.Generators;
using Mirabeau.uTransporter.Interfaces;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Generators
{
    [TestFixture]
    public class DocumentTypeGeneratorTests
    {
        private IFileHelper _fileWriter;

        private IContentReadRepository _contentReadRepo;

        private IDataTypeManager _dataTypeManager;

        private IPropertyReadRepository _propertyReadRepository;

        private IClassNameHelper _classNameHelper;

        [SetUp]
        public void SetUp()
        {
            _fileWriter = MockRepository.GenerateMock<IFileHelper>();
            _contentReadRepo = MockRepository.GenerateMock<IContentReadRepository>();
            _dataTypeManager = MockRepository.GenerateMock<IDataTypeManager>();
            _propertyReadRepository = MockRepository.GenerateMock<IPropertyReadRepository>();
            _classNameHelper = MockRepository.GenerateMock<IClassNameHelper>();
        }

        [Test]
        public void CreateClass_ShouldCreateClassWithName_ReturnCodeDomObject()
        {
            // Arrange
            IDocumentTypeGenerator documentTypeGenerator = new DocumentTypeGenerator(_contentReadRepo, _fileWriter, _dataTypeManager, _propertyReadRepository, _classNameHelper);
            IContentType contentType = this.CreateContentType(-1);

            // Act
            _classNameHelper.Expect(m => m.CreateSafeClassName("Name"))
                .Repeat.Once()
                .Return("Name");
            var actual = documentTypeGenerator.CreateClass(contentType);

            // Assert
            Assert.That(actual.Name, Is.EqualTo("Name"));
            _classNameHelper.VerifyAllExpectations();
        }

        [Test]
        public void CreateClass_ShouldCreateAClassWithIDocumentTypeBaseAsBaseType_ReturnCodeTypeReference()
        {
            // Arrange
            IDocumentTypeGenerator documentTypeGenerator = new DocumentTypeGenerator(_contentReadRepo, _fileWriter, _dataTypeManager, _propertyReadRepository, _classNameHelper);
            IContentType contentType = this.CreateContentType(-1);

            // Act
            var actual = documentTypeGenerator.CreateClass(contentType);

            // Assert
            Assert.That(actual.BaseTypes[0].BaseType, Is.EqualTo("IDocumentTypeBase"));
        }

        [Test]
        public void CreateClass_BaseTypePropertyShouldOnlyhaveOneBaseTypeClass_ReturnCodetypeReference()
        {
            // Arrange
            IDocumentTypeGenerator documentTypeGenerator = new DocumentTypeGenerator(_contentReadRepo, _fileWriter, _dataTypeManager, _propertyReadRepository, _classNameHelper);
            IContentType contentType = this.CreateContentType(-1);

            // Act
            var actual = documentTypeGenerator.CreateClass(contentType);

            // Assert
            Assert.That(actual.BaseTypes.Count, Is.EqualTo(1));
        }

        [Test]
        [Ignore]
        public void CreateClass_ShouldDo_ShouldReturn()
        {
            // Arrange
            IDocumentTypeGenerator documentTypeGenerator = new DocumentTypeGenerator(_contentReadRepo, _fileWriter, _dataTypeManager, _propertyReadRepository, _classNameHelper);
            IContentType contentType = this.CreateContentType(20);
            IContentType baseContentType = this.CreateBaseType(20);

            // Act
            _contentReadRepo.Expect(m => m.GetContentTypesBasedOnId(20))
                .Repeat.Once()
                .Return(baseContentType);

            var actual = documentTypeGenerator.CreateClass(contentType);

            // Assert
            Assert.That(actual.BaseTypes[0].BaseType, Is.EqualTo(baseContentType.Alias));
            _contentReadRepo.VerifyAllExpectations();
        }

        private IContentType CreateContentType(int parentId)
        {
            IContentType contentType = new ContentType(parentId);
            contentType.Name = "Name";

            return contentType;
        }

        private IContentType CreateBaseType(int id)
        {
            IContentType contentType = new ContentType(id);
            contentType.Name = "BasTypeName";

            return contentType;
        }
    }
}