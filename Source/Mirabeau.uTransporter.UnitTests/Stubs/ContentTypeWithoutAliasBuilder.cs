using Mirabeau.uTransporter.Builders;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Stubs
{
    public class ContentTypeWithoutAliasBuilder : ContentTypeBuilder
    {
        public ContentTypeWithoutAliasBuilder(int parentId)
            : base(parentId)
        {
        }

        public override IContentType Build()
        {
            IContentType contentType = new ContentType(_parentId);
            contentType.Name = _name;
            contentType.AllowedAsRoot = _allowedAtRoot;
            contentType.AllowedTemplates = _allowedTemplates;
            contentType.Description = _description;
            contentType.Icon = _icon;
            contentType.Thumbnail = _thumbnail;
            contentType.SetDefaultTemplate(_defaultTemplate);

            return contentType;
        }
    }
}
