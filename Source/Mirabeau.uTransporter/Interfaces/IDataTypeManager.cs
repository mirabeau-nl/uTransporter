using Mirabeau.uTransporter.Attributes;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IDataTypeManager
    {
        IDataTypeDefinition GetDataTypeDefinition(DocumentTypePropertyAttribute docProperties);

        int GetDataTypeId(DocumentTypePropertyAttribute propertyAttribute);

        IDataTypeDefinition GetCustomDataTypeDefinition(DocumentTypePropertyAttribute propertyAttribute);

        IDataTypeDefinition CreateNewDataTypeDefinition(string name);

        IDataTypeDefinition GetDataTypeDefinitionByGuid(int id);

        IDataTypeDefinition CreateDefaultDataTypeDefinition();
    }
}