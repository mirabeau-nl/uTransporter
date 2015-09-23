using System;

using Mirabeau.uTransporter.Builders;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Builders
{

    [TestFixture]
    public class TemplateBuilderTests
    {
        private TemplateBuilder templateBuilder;
        [SetUp]
        public void SetUp()
        {
            templateBuilder = new TestCustomTemplateBuilder("Name", "Alias");
        }

        [Test]
        public void Build_WhenTemplateIsBuildNameIsInitThroughConstructor_ReturnTemplateName()
        {
            //Arrange
            //Act
            ITemplate template = templateBuilder.Build();
            //Assert
            Assert.That(template.Name, Is.EqualTo("Name"));
        }

        [Test]
        public void Build_WhenTemplateIsBuildAliasIsAdded_ReturnAlias()
        {
            //Arrange
            //Act
            ITemplate template = templateBuilder.Build();
            //Assert
            Assert.That(template.Alias, Is.EqualTo("alias"));
        }

        [Test]
        public void Build_WhenTemplateIsBuildContentIsAdded_ReturnContent()
        {
            //Arrage
            templateBuilder.WithContent("<html><head><title>TheTitle</title></head><body><p>The Body</p></body></html>");
            //Act
            ITemplate template = templateBuilder.Build();
            //Assert
            Assert.That(template.Content, Is.EqualTo("<html><head><title>TheTitle</title></head><body><p>The Body</p></body></html>"));
        }

        [Test]
        public void Build_WhenTemplateISBuildCreateDateIsAdded_ReturnDate()
        {
            //Arrage
            templateBuilder.WithCreateDate(DateTime.Now.Date);
            //Act
            ITemplate template = templateBuilder.Build();
            //Assert
            Assert.That(template.CreateDate, Is.EqualTo(DateTime.Now.Date));
        }

        [Test]
        public void Build_WhenTemplateISBuildUpdateDateIsAdded_ReturnDate()
        {
            //Arrage
            templateBuilder.WithUpdateDate(DateTime.Now.Date);
            //Act
            ITemplate template = templateBuilder.Build();
            //Assert
            Assert.That(template.UpdateDate, Is.EqualTo(DateTime.Now.Date));
        }
    }
}