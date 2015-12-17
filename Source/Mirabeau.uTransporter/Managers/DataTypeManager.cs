using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Management;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Enums;
using Mirabeau.uTransporter.Exceptions;
using Mirabeau.uTransporter.Extensions;
using Mirabeau.uTransporter.Interfaces;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Managers
{
    public class DataTypeManager : IDataTypeManager
    {
        private readonly IRetryableDataTypeService _dataTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeManager"/> class.
        /// </summary>
        /// <param name="umbracoFactory">The umbraco factory.</param>
        public DataTypeManager(IRetryableDataTypeService dataTypeService)
        {
            _dataTypeService = dataTypeService;
        }

        /// <summary>
        /// Gets the data type definition.
        /// </summary>
        /// <param name="propertyAttribute">The property attribute.</param>
        /// <returns>DataTypeDefinition object</returns>
        public IDataTypeDefinition GetDataTypeDefinition(DocumentTypePropertyAttribute propertyAttribute)
        {
            IDataTypeDefinition dataTypeDefinition = null;
            if (propertyAttribute.Type == null && propertyAttribute.OtherTypeName == null)
            {
                dataTypeDefinition = this.CreateDefaultDataTypeDefinition();
            }
            else if (propertyAttribute.Type == UmbracoPropertyType.Other)
            {
                try
                {
                    dataTypeDefinition = this.GetCustomDataTypeDefinition(propertyAttribute);
                }
                catch (CustomDataTypeArgumentNullException exception)
                {
                    Logger.WriteErrorLine<DataTypeManager>("{0}", exception.Message);
                }
            }
            else
            {
                if (propertyAttribute.Type != null)
                {
                    dataTypeDefinition = _dataTypeService.GetDataTypeDefinitionById((int)propertyAttribute.Type);
                }
            }

            return dataTypeDefinition;
        }

        /// <summary>
        /// Gets a data type identifier.
        /// </summary>
        /// <param name="propertyAttribute">The property attribute.</param>
        /// <returns>Id of an datatype</returns>
        public int GetDataTypeId(DocumentTypePropertyAttribute propertyAttribute)
        {
            return this.GetDataTypeDefinition(propertyAttribute).Id;
        }

        /// <summary>
        /// Gets a custom data type definition.
        /// </summary>
        /// <param name="propertyAttribute">The property attribute.</param>
        /// <returns>DataTypeDefinition object</returns>
        /// <exception cref="CustomDataTypeArgumentNullException"></exception>
        public IDataTypeDefinition GetCustomDataTypeDefinition(DocumentTypePropertyAttribute propertyAttribute)
        {
            if (propertyAttribute.OtherTypeName == null)
            {
                throw new CustomDataTypeArgumentNullException(string.Format("When creating a custom data type, the property Othername can't be empty in {0}", this.GetType().Name));
            }

            IDataTypeDefinition customDataTypeDefinition = null;
            if (propertyAttribute.OtherTypeName != null)
            {
                IEnumerable<IDataTypeDefinition> dataTypeList = _dataTypeService.GetAllDataTypeDefinitions();
                customDataTypeDefinition = dataTypeList.FirstOrDefault(p => p.Name.Equals(propertyAttribute.OtherTypeName));
            }

            if (customDataTypeDefinition == null && propertyAttribute.OtherTypeName != null)
            {
                try
                {
                    customDataTypeDefinition = this.CreateNewDataTypeDefinition(propertyAttribute.OtherTypeName);
                }
                catch (ArgumentNullException exception)
                {
                    Logger.WriteErrorLine<DataTypeManager>("{0}", exception.Message);
                }
            }

            return _dataTypeService.GetDataTypeDefinitionById(customDataTypeDefinition.Id);
        }

        /// <summary>
        /// Creates a new data type definition.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DataTypeDefinition object</returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        /// <exception cref="System.Web.Management.SqlExecutionException"></exception>
        public IDataTypeDefinition CreateNewDataTypeDefinition(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            IDataTypeDefinition dataTypeDefinition = new DataTypeDefinition(-1, new Guid());
            dataTypeDefinition.Name = name;

            try
            {
                _dataTypeService.Save(dataTypeDefinition);
            }
            catch (Exception)
            {
                throw new SqlExecutionException(
                     string.Format("Can't add data type with name: {0}, please check the data type definition", dataTypeDefinition.Name));
            }

            Logger.WriteInfoLine<DataTypeManager>("DataType with name {0} did not exist, created it.", name);

            return dataTypeDefinition;
        }

        /// <summary>
        /// Gets the data type definition by unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DataTypeDefinition object</returns>
        public IDataTypeDefinition GetDataTypeDefinitionByGuid(int id)
        {
            return _dataTypeService.GetDataTypeDefinitionById(id);
        }

        /// <summary>
        /// Creates the default data type definition.
        /// </summary>
        /// <returns>DataTypeDefinition object</returns>
        public IDataTypeDefinition CreateDefaultDataTypeDefinition()
        {
            return _dataTypeService.GetDataTypeDefinitionById((int)UmbracoPropertyType.Richtexteditor);
        }
    }
}