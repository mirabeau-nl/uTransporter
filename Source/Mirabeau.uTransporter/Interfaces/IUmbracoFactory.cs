using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.Interfaces 
{
    public interface IUmbracoFactory 
    {
        IContentTypeService CreateContentTypeService();

        IContentService CreateContentService();

        IDataTypeService CreateDataTypeService();

        IFileService CreateFileService();

        IMediaService CreateMediaService();
    }
}
