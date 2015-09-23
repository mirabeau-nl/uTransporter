using System;
using System.Collections.Generic;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Repositories;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Repositories
{
    public class PropertyWriteRepositoryTests
    {
        private IDataTypeDefinition _dataTypeDefinition;

        private IPropertyFactory _propertyFactory;

        private IManagerFactory _managerFactory;

        private IPropertyReadRepository _propertyReadRepository;

        private IRetryableContentTypeService _retryableContentTypeService;

        private IContentType _contentType;

        [SetUp]
        public void SetUp()
        {
            _dataTypeDefinition = MockRepository.GenerateMock<IDataTypeDefinition>();
            _propertyFactory = MockRepository.GenerateMock<IPropertyFactory>();
            _managerFactory = MockRepository.GenerateStub<IManagerFactory>();
            _propertyReadRepository = MockRepository.GenerateMock<IPropertyReadRepository>();
            _retryableContentTypeService = MockRepository.GenerateMock<IRetryableContentTypeService>();
            _contentType = new ContentType(-1);
        }

        [Test]
        public void CreateRedundantPropertiesList_ShouldComparerTheListsAndREturnTheDiffencence_ReturnList()
        {
            List<PropertyType> properties = new List<PropertyType>();

            PropertyType propertyTypeOne = CreatePropertyType(_dataTypeDefinition, "PropertyOne");
            PropertyType propertyTypeNewTwo = CreatePropertyType(_dataTypeDefinition, "newPropertyTwo");

            properties.Add(propertyTypeOne);
            properties.Add(propertyTypeNewTwo);

            _propertyReadRepository.Expect(m => m.CountPropertiesFromDocumentTypeAttribute(Arg<Type>.Is.Anything))
                .Repeat.Once()
                .Return(1);

            _propertyReadRepository.Expect(m => m.CountPropertiesFromContentType(Arg<IContentType>.Is.Anything))
                .Repeat.Once()
                .Return(1);

            _propertyReadRepository.Expect(m => m.CreateMissingPropertiesList(Arg<Type>.Is.Anything, Arg<IContentType>.Is.Anything))
                .Repeat.Once()
                .Return(properties);

            _propertyFactory.Expect(
                m => m.CreatePropertyGroup(Arg<Type>.Is.Anything, Arg<IContentType>.Is.Anything, Arg<PropertyType>.Is.Anything))
                .Repeat.Once();

            PropertyWriteRepository propertyWriteRepository = new PropertyWriteRepository(_propertyFactory, _managerFactory, _propertyReadRepository, _retryableContentTypeService);
            propertyWriteRepository.CreateMissingProperties(typeof(PropertyWriteRepository), _contentType, new DocumentTypePropertyAttribute());
        }

        private static PropertyType CreatePropertyType(IDataTypeDefinition dataTypeDefinition, string name)
        {
            PropertyType propertyTypeExistingOne = new PropertyType(dataTypeDefinition) { Name = name };
            return propertyTypeExistingOne;
        }
    }
}