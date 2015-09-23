
namespace Mirabeau.uTransporter.Interfaces
{
    public interface IContentTypeManager
    {
        /// <summary>
        /// Saves the contenttype.
        /// </summary>
        void SaveContentType();

        /// <summary>
        /// Does the contenttype exists.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        bool DoesContentTypeExists(string alias);

        /// <summary>
        /// Removes the contenttypes.
        /// </summary>
        /// <returns></returns>
        int RemoveContentTypes();

        /// <summary>
        /// Removes the templates.
        /// </summary>
        /// <returns></returns>
        int RemoveTemplates();

        /// <summary>
        /// Removes content
        /// <param name="id">The identifier.</param>
        void RemoveContentType(int id);
    }
}