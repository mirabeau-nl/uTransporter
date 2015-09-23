
using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IUmbracoService
    {
        IContentTypeService GetContentTypeService();

        IContentService GetContentService();

        IDataTypeService GetDataTypeService();

        IFileService GetFileService();

        IMediaService GetMediaService();
    }
}
