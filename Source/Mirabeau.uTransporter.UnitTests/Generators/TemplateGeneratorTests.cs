using Mirabeau.uTransporter.Generators;
using Mirabeau.uTransporter.Interfaces;

using NUnit.Framework;

using Rhino.Mocks;

namespace Mirabeau.uTransporter.UnitTests.Generators
{
    [TestFixture]
    public class TemplateGeneratorTests
    {
        private IFileHelper _fileWriter;

        private ITemplateReadRepository _templateReadRepository;

        private IClassNameHelper _classNameHelper;

        [SetUp]
        public void SetUp()
        {
            _fileWriter = MockRepository.GenerateMock<IFileHelper>();
            _templateReadRepository = MockRepository.GenerateMock<ITemplateReadRepository>();
            _classNameHelper = MockRepository.GenerateMock<IClassNameHelper>();
        }

        [Test]
        public void CreateTargetPath_ShouldReturnValidPath_ReturnValidPathWithFilenameAndExtension()
        {
            // Arrange
            ITemplateGenerator templateGenerator = new TemplateGenerator(_fileWriter, _templateReadRepository, _classNameHelper);

            // Act
            var actual = templateGenerator.CreateTargetPath("filename", "c:/targetPath/");

            // Assert
            Assert.That(actual, Is.EqualTo("c:/targetPath/Templates\\Filename.cs"));

        }
    }
}
