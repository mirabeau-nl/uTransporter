using System;

namespace Mirabeau.uTransporter.Attributes 
{
    public class DocumentTypeAttribute : Attribute 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeAttribute"/> class.
        /// </summary>
        public DocumentTypeAttribute() 
        {
            Icon = "folder.gif"; 
            Thumbnail = "folder.png";
        }

        /// <summary>
        /// Gets or sets the name of the document type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the alias of this document type
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the icon of this document type
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail of this document type
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets the description of this document type
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets allowed templates of this document type
        /// </summary>
        public Type[] AllowedTemplates { get; set; }

        /// <summary>
        /// Gets or sets the default template of this document type
        /// </summary>
        public Type DefaultTemplate { get; set; }

        /// <summary>
        ///  Gets or sets which child node types are allowed..
        /// </summary>
        public Type[] AllowedChildNodeTypes { get; set; }

        /// <summary>
        /// Gets or sets a value
        /// </summary>
        public bool AllowAtRoot { get; set; }

        /// <summary>
        /// Gets the DefaultTemplate as string
        /// </summary>
        public string DefaultTemplateAsString 
        {
            get 
            {
                if (DefaultTemplate != null) 
                {
                    return ((Type)DefaultTemplate).Name;
                }

                return string.Empty;
            }
        }
    }
}