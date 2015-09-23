using System;
using System.Collections.Generic;

using Umbraco.Core.Models;

using umbraco.interfaces;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IRetryableDataTypeService
    {
        IDataTypeDefinition GetDataTypeDefinitionById(int id);

        IDataTypeDefinition GetDataTypeDefinitionById(Guid id);

        IEnumerable<IDataTypeDefinition> GetAllDataTypeDefinitions(params int[] ids);

        void Save(IDataTypeDefinition dataTypeDefinition, int userId = 0);

        void Save(IEnumerable<IDataTypeDefinition> dataTypeDefinitions, int userId = 0);

        void Delete(IDataTypeDefinition dataTypeDefinition, int userId = 0);

        IDataType GetDataTypeById(Guid id);

        IEnumerable<IDataType> GetAllDataTypes();

        IEnumerable<IDataTypeDefinition> GetDataTypeDefinitionByControlId(Guid id);
    }
}
