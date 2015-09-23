using System.Collections.Generic;

using Mirabeau.uTransporter.Builders;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Builders
{
    [TestFixture]
    public class ContentTypeBuilderTests
    {
        private ContentTypeBuilder contentTypeBuilder;

        [SetUp]
        public void SetUp()
        {
            contentTypeBuilder = new ContentTypeWithoutAliasBuilder(-1);
            contentTypeBuilder.WithName("DocumentType");
            contentTypeBuilder.WithAlias("documenttype");
            contentTypeBuilder.WithDescription(uTransporter.Utils.Util.TrimLength("The Description", 1500));
            contentTypeBuilder.IsAllowedAtRoot(true);
            contentTypeBuilder.AllowedTemplateList(new List<ITemplate>());
            contentTypeBuilder.WithIcon("folder.gif");
            contentTypeBuilder.WithThumbnail("folder.png");
        }

        [Test]
        public void Build_WhenContentTypeIsBuildNameIsAdded_ReturnName()
        {
            // Arrange
            IContentType contentType = contentTypeBuilder.Build();

            // Act
            string name = contentType.Name;

            // Assert
            Assert.That(name, Is.EqualTo("DocumentType"));
        }

        [Test]
        public void Build_WhenContentTypeIsBuildDescriptionIsAdded_ReturnDescription()
        {
            // Arrange
            IContentType contentType = contentTypeBuilder.Build();

            // Act
            string description = contentType.Description;

            // Assert
            Assert.That(description, Is.EqualTo("The Description"));
        }

        [Test]
        public void Build_WhenContentTypeIsBuildAllowedASRootBoolIsSet_ReturnAllowedAtRootBool()
        {
            // Arrange
            IContentType contentType = contentTypeBuilder.Build();

            // Act
            bool allowedAtRoot = contentType.AllowedAsRoot;
            
            // Assert
            Assert.That(allowedAtRoot, Is.EqualTo(true));
        }

        [Test]
        public void Build_WhenContentTypeIsBuildTemplateListIsAdded_ReturnEmptyList()
        {
            // Act
            List<ITemplate> templateList = new List<ITemplate>();

            // Assert
            Assert.That(templateList, Is.EqualTo(new List<ITemplate>()));
        }

        [Test]
        public void Build_WhenContentTypeIsBuildIconIsAdded_ReturnIcon()
        {
            // Arrange
            IContentType contentType = contentTypeBuilder.Build();
            
            // Act
            string icon = contentType.Icon;
           
            // Assert
            Assert.That(icon, Is.EqualTo("folder.gif"));
        }

        [Test]
        public void Build_WhenContentTypeIsBuildThumbnailIsAdded_ReturnThumbnail()
        {
            // Arrange
            IContentType contentType = contentTypeBuilder.Build();

            // Act
            string thumbnail = contentType.Thumbnail;

            // Assert
            Assert.That(thumbnail, Is.EqualTo("folder.png"));
        }
    }
}