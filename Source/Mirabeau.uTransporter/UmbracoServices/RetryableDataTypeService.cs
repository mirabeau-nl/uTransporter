using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Mirabeau.uTransporter.Interfaces;

using Palmer;

using Umbraco.Core.Models;
using Umbraco.Core.Services;

using umbraco.interfaces;

namespace Mirabeau.uTransporter.UmbracoServices
{
    public class RetryableDataTypeService : IRetryableDataTypeService
    {
        private readonly IDataTypeService _dataTypeService;

        public RetryableDataTypeService(IUmbracoService umbracoFactory)
        {
            _dataTypeService = umbracoFactory.GetDataTypeService();
        }

        public IDataTypeDefinition GetDataTypeDefinitionById(int id)
        {
            IDataTypeDefinition dataTypeDefinition = null;

            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .AndOn<DuplicateNameException>()
                .For(5)
                .With(context => dataTypeDefinition = _dataTypeService.GetDataTypeDefinitionById(id));

            return dataTypeDefinition;
        }

        public IDataTypeDefinition GetDataTypeDefinitionById(Guid id)
        {
            IDataTypeDefinition dataTypeDefinition = null;

            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .AndOn<DuplicateNameException>()
                .For(5)
                .With(context => dataTypeDefinition = _dataTypeService.GetDataTypeDefinitionById(id));

            return dataTypeDefinition;
        }

        public IEnumerable<IDataTypeDefinition> GetAllDataTypeDefinitions(params int[] ids)
        {
            IEnumerable<IDataTypeDefinition> dataTypeDefinition = null;

            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .AndOn<DuplicateNameException>()
                .For(5)
                .With(context => dataTypeDefinition = _dataTypeService.GetAllDataTypeDefinitions(ids));

            return dataTypeDefinition;
        }

        public void Save(IDataTypeDefinition dataTypeDefinition, int userId = 0)
        {
            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
               .For(5)
               .AndOn<DuplicateNameException>()
               .For(5)
               .With(context => _dataTypeService.Save(dataTypeDefinition));
        }

        public void Save(IEnumerable<IDataTypeDefinition> dataTypeDefinitions, int userId = 0)
        {
            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .AndOn<DuplicateNameException>()
                .For(5)
                .With(context => _dataTypeService.Save(dataTypeDefinitions));
        }

        public void Delete(IDataTypeDefinition dataTypeDefinition, int userId = 0)
        {
            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .AndOn<DuplicateNameException>()
                .For(5)
                .With(context => _dataTypeService.Delete(dataTypeDefinition));
        }

        public IDataType GetDataTypeById(Guid id)
        {
            IDataType dataType = null;

            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .AndOn<DuplicateNameException>()
                .For(5)
                .With(context => dataType = _dataTypeService.GetDataTypeById(id));

            return dataType;
        }

        public IEnumerable<IDataType> GetAllDataTypes()
        {
            IEnumerable<IDataType> dataTypes = null;

            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .AndOn<DuplicateNameException>()
                .For(5)
                .With(context => dataTypes = _dataTypeService.GetAllDataTypes());

            return dataTypes;
        }

        public IEnumerable<IDataTypeDefinition> GetDataTypeDefinitionByControlId(Guid id)
        {
            IEnumerable<IDataTypeDefinition> dataTypes = null;

            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .AndOn<DuplicateNameException>()
                .For(5)
                .With(context => dataTypes = _dataTypeService.GetDataTypeDefinitionByControlId(id));

            return dataTypes;
        }
    }
}
