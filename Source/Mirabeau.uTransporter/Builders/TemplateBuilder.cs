// TemplateBuilde.cs

using System;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Builders
{
    /// <summary>
    /// Builder class for constructing template objects
    /// </summary>
    public class TemplateBuilder
    {
        private readonly string _name;
        
        private readonly string _alias;
        
        private string _content;
        
        private DateTime _creationDate;
        
        private DateTime _updateDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateBuilder"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        public TemplateBuilder(string name, string alias)
        {
            this._name = name;
            this._alias = alias;
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>Template object</returns>
        public ITemplate Build()
        {
            ITemplate template = CreateTemplateInstance();
            template.Content = _content;
            template.CreateDate = _creationDate;
            template.UpdateDate = _updateDate;

            return template;
        }

        /// <summary>
        /// Sets the content of the template.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>Template object</returns>
        public TemplateBuilder WithContent(string content)
        {
            this._content = content;
            return this;
        }

        /// <summary>
        /// Sets the creation date of the template.
        /// </summary>
        /// <param name="creationDate">The creation date.</param>
        /// <returns>Template object</returns>
        public TemplateBuilder WithCreateDate(DateTime creationDate)
        {
            this._creationDate = creationDate;
            return this;
        }

        /// <summary>
        /// Sets the update date of the template
        /// </summary>
        /// <param name="updateDate">The update date.</param>
        /// <returns>Template object</returns>
        public TemplateBuilder WithUpdateDate(DateTime updateDate)
        {
            this._updateDate = updateDate;
            return this;
        }

        public virtual ITemplate CreateTemplateInstance()
        {
            return new Template("~/Masterpages/" + _name, _name, _alias.ToLower());            
        }
    }
}