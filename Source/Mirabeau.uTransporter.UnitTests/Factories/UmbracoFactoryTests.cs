using Mirabeau.uTransporter.Interfaces;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.UnitTests.Factories
{
    [TestFixture]
    public class UmbracoFactoryTests
    {
        private IUmbracoFactory umbracoFactory;

        [SetUp]
        public void SetUp()
        {
            umbracoFactory = MockRepository.GenerateStrictMock<IUmbracoFactory>();
        }

        [Test]
        public void CreateContentService_ShouldReturnContentService_ReturnService()
        {
            // Arrange
            IContentService contentService = MockRepository.GenerateStrictMock<IContentService>();
            umbracoFactory.Expect(m => m.CreateContentService())
                .Repeat.Once()
                .Return(contentService);
            
            // Act
            umbracoFactory.CreateContentService();

            // Assert
            umbracoFactory.VerifyAllExpectations();
        }

        [Test]
        public void CreateDataTypeService_ShouldReturnDataTypeService_ReturnService()
        {
            // Arrange
            IDataTypeService dataTypeService = MockRepository.GenerateMock<IDataTypeService>();
            umbracoFactory.Expect(m => m.CreateDataTypeService())
                .Repeat.Once()
                .Return(dataTypeService);
            
            // Act
            umbracoFactory.CreateDataTypeService();
            
            // Assert
            umbracoFactory.VerifyAllExpectations();
        }

        [Test]
        public void CreateFileService_ShouldReturnFileService_ReturnService()
        {
            // Arrange
            IFileService fileService = MockRepository.GenerateMock<IFileService>();
            umbracoFactory.Expect(m => m.CreateFileService())
                .Repeat.Once()
                .Return(fileService);
            
            // Act
            umbracoFactory.CreateFileService();

            // Assert
            umbracoFactory.VerifyAllExpectations();
        }

        [Test]
        public void CreateMediaService_ShouldReturnMediaService_ReturnService()
        {
            // Arrange
            IMediaService mediaService = MockRepository.GenerateMock<IMediaService>();
            umbracoFactory.Expect(m => m.CreateMediaService())
                .Repeat.Once()
                .Return(mediaService);
            
            // Act
            umbracoFactory.CreateMediaService();

            // Assert
            umbracoFactory.VerifyAllExpectations();
        }
    }
}
