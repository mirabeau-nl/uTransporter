// ContentTypeBuilder.cs
using System.Collections.Generic;

using Mirabeau.uTransporter.Utils;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Builders
{
    /// <summary>
    /// Builder class for constructing content/document types objects
    /// </summary>
    /// <remarks></remarks>
    public class ContentTypeBuilder
    {
        #region Private Fields

        protected readonly int _parentId;

        protected string _name;

        protected string _alias;

        protected bool _allowedAtRoot;

        protected List<ITemplate> _allowedTemplates;

        protected ITemplate _defaultTemplate;

        protected string _description;

        protected string _icon;

        protected string _thumbnail;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeBuilder"/> class.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        public ContentTypeBuilder(int parentId)
        {
            _parentId = parentId;
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>ContentType object</returns>
        public virtual IContentType Build()
        {
            IContentType contentType = new ContentType(_parentId);
            contentType.Name = _name;
            contentType.Alias = _alias;
            contentType.AllowedAsRoot = _allowedAtRoot;
            contentType.AllowedTemplates = _allowedTemplates;
            contentType.Description = _description;
            contentType.Icon = _icon;
            contentType.Thumbnail = _thumbnail;
            contentType.SetDefaultTemplate(_defaultTemplate);

            return contentType;
        }

        /// <summary>
        /// Set the name of the content type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ContentTypeBuilder object</returns>
        public ContentTypeBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Set the alias of the content type.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>ContentTypeBuilder object</returns>
        public ContentTypeBuilder WithAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        /// <summary>
        /// Set the icon of the content type.
        /// </summary>
        /// <param name="icon">The icon.</param>
        /// <returns>ContentTypeBuilder object</returns>
        public ContentTypeBuilder WithIcon(string icon)
        {
            _icon = icon;
            return this;
        }

        /// <summary>
        /// Set the thumbnail of the content type.
        /// </summary>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <returns>ContentTypeBuilder object</returns>
        public ContentTypeBuilder WithThumbnail(string thumbnail)
        {
            _thumbnail = thumbnail;
            return this;
        }

        /// <summary>
        /// Set the description of the content type.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>ContentTypeBuilder object</returns>
        public ContentTypeBuilder WithDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                _description = string.Empty;
            }
            else
            {
                _description = Util.TrimLength(description, 1500);
            }

            return this;
        }

        /// <summary>
        /// Alloweds the template list.
        /// </summary>
        /// <param name="allowedTemplates">The allowed templates.</param>
        /// <returns>ContentTypeBuilder object</returns>
        public ContentTypeBuilder AllowedTemplateList(List<ITemplate> allowedTemplates)
        {
            _allowedTemplates = allowedTemplates;
            return this;
        }

        /// <summary>
        /// Set the default template of the content type.
        /// </summary>
        /// <param name="defaulTemplate">The defaul template.</param>
        /// <returns>ContentTypeBuilder object</returns>
        public ContentTypeBuilder WithDefaultTemplate(ITemplate defaulTemplate)
        {
            _defaultTemplate = defaulTemplate;
            return this;
        }

        /// <summary>
        /// Determines whether [is allowed at root] [the specified allowed at root].
        /// </summary>
        /// <param name="allowedAtRoot">if set to <c>true</c> [allowed at root].</param>
        /// <returns>ContentTypeBuilder object</returns>
        public ContentTypeBuilder IsAllowedAtRoot(bool allowedAtRoot)
        {
            _allowedAtRoot = allowedAtRoot;
            return this;
        }
        #endregion
    }
}