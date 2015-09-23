using System.Collections.Generic;
using System.Threading.Tasks;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IUmbracoSyncConsole
    {
        Task<bool> RunSyncFromConsole(string dllFilePath);

        Task<bool> RunSyncFromConsoleWithDryRun(string dllFilePath);

        void Export(string targetPath);

        void RemoveDocumentTypes();

        void RemoveDocumentType(int id);

        IEnumerable<IContentType> ListDocumentTypes();
    }
}