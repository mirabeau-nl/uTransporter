using System;

using Mirabeau.uTransporter.Enums;

namespace Mirabeau.uTransporter.Attributes 
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DocumentTypePropertyAttribute : Attribute 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypePropertyAttribute"/> class.
        /// </summary>
        public DocumentTypePropertyAttribute()
        {
            Mandatory = false;
            ValidationRegExp = string.Empty;
            Description = string.Empty;
            Alias = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypePropertyAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public DocumentTypePropertyAttribute(UmbracoPropertyType type) : this() 
        { 
            Type = type;
        }

        /// <summary>
        /// Gets or sets the name of the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the alias of the property
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the property of the property
        /// </summary>
        public UmbracoPropertyType? Type { get; set; }

        /// <summary>
        /// Gets or sets the type of the custom datatype.
        /// </summary>
        /// <value>
        /// The type of the other custom datatype.
        /// </value>
        public Type OtherType { get; set; }

        /// <summary>
        /// Gets or sets the name of the a data type.
        /// </summary>
        public string OtherTypeName { get; set; }

        /// <summary>
        /// Gets or sets the tabname of the property
        /// </summary>
        public Type Tab { get; set; }

        /// <summary>
        /// Gets or sets the sort order of the property within a tab
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets a value the property name mandatory
        /// </summary>
        public bool Mandatory { get; set; }

        /// <summary>
        /// Gets or sets the validation RegEx
        /// </summary>
        public string ValidationRegExp { get; set; }

        /// <summary>
        /// Gets or sets the description of the property
        /// </summary>
        public string Description { get; set; }
    }
}