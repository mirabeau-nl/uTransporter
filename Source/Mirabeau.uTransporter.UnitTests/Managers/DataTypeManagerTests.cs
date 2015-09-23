using System;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Enums;
using Mirabeau.uTransporter.Exceptions;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Managers;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.UnitTests.Managers
{
    [TestFixture]
    public class DataTypeManagerTests
    {
        private IRetryableDataTypeService _retryableDataTypeService;
        private IDataTypeService _dataTypeServiceStub;

        [SetUp]
        public void SetUp()
        {
            _retryableDataTypeService = MockRepository.GenerateStub<IRetryableDataTypeService>();
            _dataTypeServiceStub = MockRepository.GenerateStub<IDataTypeService>();
        }

        [Test]
        public void GetDataTypeDefinition_GetDataTypeDefinitionWithEmptyProperties_ReturnDefaultDefinition()
        {
            // Arrange


            // Act
            DocumentTypePropertyAttribute attribute = new DocumentTypePropertyAttribute();
            IDataTypeManager dataTypeManager = new DataTypeManager(_retryableDataTypeService);
            dataTypeManager.GetDataTypeDefinition(attribute);

            // Assert
            _retryableDataTypeService.VerifyAllExpectations();
            _dataTypeServiceStub.VerifyAllExpectations();
        }

        [Test]
        public void GetDataTypeDefinition_WithDefaultProperty_ReturnDefinition()
        {
            // Arrange

            // Act
            DocumentTypePropertyAttribute attribute = new DocumentTypePropertyAttribute
            {
                Type = UmbracoPropertyType.ApprovedColor
            };

            IDataTypeManager dataTypeManager = new DataTypeManager(_retryableDataTypeService);
            dataTypeManager.GetDataTypeDefinition(attribute);

            // Assert
            _retryableDataTypeService.VerifyAllExpectations();
        }

        [Test]
        public void CreateNewDataTypeDefinition_ShouldCreateDataTypeDefinitionWithName_ReturnDataTypeDefinition()
        {
            // Arrange

            IDataTypeManager dataTypeManager = new DataTypeManager(_retryableDataTypeService);

            // Act
            var result = dataTypeManager.CreateNewDataTypeDefinition("name");

            // Assert
            Assert.That(result.Name, Is.EqualTo("name"));
        }

        [Test]
        public void SaveDataTypeDefinition_ShouldSaveTheDataTypeDefinition_ReturnTrue()
        {
            // Arrange
            IDataTypeService dataTypeService = MockRepository.GenerateStub<IDataTypeService>();

            // Act
            IDataTypeManager dataTypeManager = new DataTypeManager(_retryableDataTypeService);
            IDataTypeDefinition dataTypeDefinition = dataTypeManager.CreateNewDataTypeDefinition("CustomDataType");
            dataTypeService.Expect(m => m.Save(dataTypeDefinition));

            // Assert
            dataTypeService.VerifyAllExpectations();
            Assert.That(dataTypeDefinition.Name, Is.EqualTo("CustomDataType"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDataTypeDefinition_ShouldThrowExceptionWhenNameIsNullOrEmpty_ReturnArgumentNullException()
        {
            // Arrange
            IDataTypeService dataTypeService = MockRepository.GenerateStrictMock<IDataTypeService>();

            // Act
            IDataTypeManager dataTypeManager = new DataTypeManager(_retryableDataTypeService);
            dataTypeManager.CreateNewDataTypeDefinition(string.Empty);

            // Assert
        }

        [Test]
        [ExpectedException(typeof(CustomDataTypeArgumentNullException))]
        public void GetCustomDataTypeDefinition_ShouldThrowExceptionWhenPropertyAttributeOtherTypeNameIsEmptyOrNull_ReturnException()
        {
            // Arrange
            IDataTypeService dataTypeService = MockRepository.GenerateStrictMock<IDataTypeService>();

            // Act
            DocumentTypePropertyAttribute attribute = new DocumentTypePropertyAttribute();
            attribute.OtherTypeName = null;
            IDataTypeManager dataTypeManager = new DataTypeManager(_retryableDataTypeService);
            dataTypeManager.GetCustomDataTypeDefinition(attribute);

            // Assert
        }
    }
}