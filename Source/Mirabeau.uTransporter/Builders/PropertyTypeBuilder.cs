// PropertyTypeBuilder.cs

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Builders
{
    /// <summary>
    /// Builder class for constructing property objects
    /// </summary>
    /// <remarks></remarks>
    public class PropertyTypeBuilder 
    {
        private readonly IDataTypeDefinition _dataTypeDefinition;
        
        private string _name;
        
        private string _alias;
        
        private string _description;
        
        private bool _mandatory;
        
        private string _validationRegEx;
        
        private int _sortOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTypeBuilder"/> class.
        /// </summary>
        /// <param name="dataTypeDefinition">The data type definition.</param>
        public PropertyTypeBuilder(IDataTypeDefinition dataTypeDefinition)
        {
            this._dataTypeDefinition = dataTypeDefinition;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="PropertyTypeBuilder"/> to <see cref="PropertyType"/>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator PropertyType(PropertyTypeBuilder instance)
        {
            return instance.Build();
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>PropertyType object</returns>
        public PropertyType Build()
        {
            PropertyType propertyType = new PropertyType(_dataTypeDefinition)
            {
                Name = _name,
                Alias = _alias,
                Description = _description,
                Mandatory = _mandatory,
                ValidationRegExp = _validationRegEx,
                SortOrder = _sortOrder
            };
           
            return propertyType;
        }

        /// <summary>
        /// Set the name of the property type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public PropertyTypeBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Set the alias of the property type.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public PropertyTypeBuilder WithAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        /// <summary>
        /// Set the description of the property type..
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>PropertyTypeBuilder object</returns>
        public PropertyTypeBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        /// <summary>
        /// Determines whether the specified mandatory is mandatory.
        /// </summary>
        /// <param name="mandatory">if set to <c>true</c> [mandatory].</param>
        /// <returns>PropertyTypeBuilder object</returns>
        public PropertyTypeBuilder IsMandatory(bool mandatory)
        {
            _mandatory = mandatory;
            return this;
        }

        /// <summary>
        /// Set the validation reg ex of the property type.
        /// </summary>
        /// <param name="validationRegEx">The validation reg ex.</param>
        /// <returns>PropertyTypeBuilder object</returns>
        public PropertyTypeBuilder WithValidationRegEx(string validationRegEx)
        {
            this._validationRegEx = validationRegEx;
            return this;
        }

        /// <summary>
        /// Set the sort order of the property type.
        /// </summary>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>PropertyTypeBuilder object</returns>
        public PropertyTypeBuilder WithSortOrder(int sortOrder)
        {
            _sortOrder = sortOrder;
            return this;
        }
    }
}