using Mirabeau.uTransporter.Builders;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core;
using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Builders
{
    [TestFixture]
    public class PropertyTypeBuilderTests
    {
        private IDataTypeDefinition _dataTypeDefinition;
        private string _propertyTypeAlias;

        [SetUp]
        public void SetUp()
        {
            _dataTypeDefinition = MockRepository.GenerateStub<IDataTypeDefinition>();
            _propertyTypeAlias = string.Format("{0}_propertytypealias", Constants.PropertyEditors.InternalGenericPropertiesPrefix);
        }

        [Test]
        public void Build_WhenPropertyTypeIsBuildNameIsAdded_ReturnName()
        {
            //Arrange
            PropertyType propertyType = new PropertyTypeBuilder(_dataTypeDefinition)
                       .WithAlias(_propertyTypeAlias)
                       .WithName("propertyTypeName");
            //Act
            string name = propertyType.Name;
            //Assert
            Assert.That(name, Is.EqualTo("propertyTypeName"));
        }

        [Test]
        public void Build_WhenPropertyTypeIsBuildAliasIsAdded_ReturnAlias()
        {
            //Arrange
            PropertyType propertyType = new PropertyTypeBuilder(_dataTypeDefinition).WithAlias(_propertyTypeAlias);
            //Act
            string alias = propertyType.Alias;
            //Assert
            Assert.That(alias, Is.EqualTo(_propertyTypeAlias));
        }

        [Test]
        public void Build_WhenPropertyTypeIsBuildDesctipionIsAdded_ReturnDescription()
        {
            //Arrange
            PropertyType propertyType = new PropertyTypeBuilder(_dataTypeDefinition)
                       .WithAlias(_propertyTypeAlias)
                       .WithDescription("propertyTypeName");
            //Act
            string description = propertyType.Description;
            //Assert
            Assert.That(description, Is.EqualTo("propertyTypeName"));
        }

        [Test]
        public void Build_WhenPropertyTypeIsBuildMandatoryIsAdded_ReturnMandatory()
        {
            //Arrange
            PropertyType propertyType = new PropertyTypeBuilder(_dataTypeDefinition)
                .WithAlias(_propertyTypeAlias)
                .IsMandatory(true);

            //Act
            bool mandatory = propertyType.Mandatory;
            //Assert
            Assert.That(mandatory, Is.EqualTo(true));
        }

        [Test]
        public void Build_WhenPropertyTypeIsBuildValadationRegExIsAdded_ReturnRegEx()
        {
            //Arrange
            PropertyType propertyType = new PropertyTypeBuilder(_dataTypeDefinition)
                       .WithAlias(_propertyTypeAlias)
                       .WithValidationRegEx(@"(?<=\.) {2,}(?=[A-Z])");
            //Act
            string RegEx = propertyType.ValidationRegExp;
            //Assert
            Assert.That(RegEx, Is.EqualTo(@"(?<=\.) {2,}(?=[A-Z])"));
        }

        [Test]
        public void Build_WhenPropertyTypeIsBuildSortOrderIsAdded_ReturnSortOrder()
        {
            //Arrange
            PropertyType propertyType = new PropertyTypeBuilder(_dataTypeDefinition)
                       .WithAlias(_propertyTypeAlias)
                       .WithSortOrder(2);
            //Act
            int sortOrder = propertyType.SortOrder;
            //Assert
            Assert.That(sortOrder, Is.EqualTo(2));
        }
    }
}