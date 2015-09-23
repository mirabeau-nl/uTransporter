using System;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Comparers;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Comparers
{
    [TestFixture]
    public class TemplateComparerTests
    {
        private ITemplateComparer _templateComparer;

        private ITemplate _template;

        private Type _existingTemplate;

        private ITemplateReadRepository _templateReadRepository;

        private TemplateAttribute _templateAttributes;

        [SetUp]
        public void SetUp()
        {
            _templateReadRepository = MockRepository.GenerateMock<ITemplateReadRepository>();
           _templateComparer = new TemplateComparer(_templateReadRepository);
           _template = MockRepository.GenerateMock<ITemplate>();
           _existingTemplate = typeof(Master);
            _templateAttributes = CreateTemplateAttribute();
        }

        [Test]
        public void Comparer_ShouldReturnTrueWhenTemplateObjectsNamesAreTheSame_ReturnTrue()
        {
            // Arrange
            this.MockTemplate("Master", "Master", string.Empty);

            // Act
            _templateReadRepository.Expect(m => m.GetTemplateAttributes<TemplateAttribute>(_existingTemplate)).Repeat.Once().Return(_templateAttributes);
            var actual = _templateComparer.Comparer(_existingTemplate, _template);

            // Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void Comparer_ShouldReturnTrueWhenTemplateObjectsAliasAreTheSame_ReturnTrue()
        {
            // Arrange
            this.MockTemplate("Master", "Master", string.Empty);

            // Act
            _templateReadRepository.Expect(m => m.GetTemplateAttributes<TemplateAttribute>(_existingTemplate)).Repeat.Once().Return(_templateAttributes);
            var actual = _templateComparer.Comparer(_existingTemplate, _template);

            // Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void Comparer_ShouldReturnTrueWhenTemplateObjectsContentAreTheSameUsingStringEmpty_ReturnTrue()
        {
             // Arrange
            this.MockTemplate("Master", "Master", string.Empty);

            // Act
            _templateReadRepository.Expect(m => m.GetTemplateAttributes<TemplateAttribute>(_existingTemplate)).Repeat.Once().Return(_templateAttributes);
            var actual = _templateComparer.Comparer(_existingTemplate, _template);

            // Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void Comparer_ShouldReturnTrueWhenTemplateObjectsContentAreTheSameUsingQuotation_ReturnTrue()
        {
            // Arrange
            this.MockTemplate("Master", "Master", string.Empty);

            // Act
            _templateReadRepository.Expect(m => m.GetTemplateAttributes<TemplateAttribute>(_existingTemplate)).Repeat.Once().Return(_templateAttributes);
            var actual = _templateComparer.Comparer(_existingTemplate, _template);

            // Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void Comparer_ShouldReturnFalseWhenTemplateObjectsNameAreNotTheSame_ReturnFalse()
        {
            // Arrange
            this.MockTemplate("DifferentName", "Master", string.Empty);

            // Act
            _templateReadRepository.Expect(m => m.GetTemplateAttributes<TemplateAttribute>(_existingTemplate)).Repeat.Once().Return(_templateAttributes);
            var actual = _templateComparer.Comparer(_existingTemplate, _template);

            // Assert
            Assert.That(actual, Is.EqualTo(false));
        }

        [Test]
        public void Comparer_ShouldReturnFalseWhenTemplateObjectsAliasAreNotTheSame_ReturnFalse()
        {
            // Arrange
            this.MockTemplate("Master", "DifferentAlias", string.Empty);

            // Act
            _templateReadRepository.Expect(m => m.GetTemplateAttributes<TemplateAttribute>(_existingTemplate)).Repeat.Once().Return(_templateAttributes);
            var actual = _templateComparer.Comparer(_existingTemplate, _template);

            // Assert
            Assert.That(actual, Is.EqualTo(false));
        }

        [Test]
        public void Comparer_ShouldReturnFalseWhenTemplateObjectsContentAreNotTheSame_ReturnFalse()
        {
            // Arrange
            this.MockTemplate("Master", "Master", "SomeContent");

            // Act
            _templateReadRepository.Expect(m => m.GetTemplateAttributes<TemplateAttribute>(_existingTemplate)).Repeat.Once().Return(_templateAttributes);
            var actual = _templateComparer.Comparer(_existingTemplate, _template);

            // Assert
            Assert.That(actual, Is.EqualTo(false));
        }

        private TemplateAttribute CreateTemplateAttribute()
        {
            return new TemplateAttribute { Name = "Master", Alias = "Master", Content = string.Empty };
        }

        private void MockTemplate(string name, string alias, string content)
        {
            _template.Expect(m => m.Name)
                .Repeat.Once()
                .Return(name);

            _template.Expect(m => m.Alias)
                .Repeat.Once()
                .Return(alias);

            _template.Expect(m => m.Content)
                .Repeat.Once()
                .Return(content);    
        }
    }
}
