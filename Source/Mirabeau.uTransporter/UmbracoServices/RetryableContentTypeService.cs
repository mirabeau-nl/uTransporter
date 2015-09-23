using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Mirabeau.uTransporter.Interfaces;

using Palmer;

using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.UmbracoServices
{
    /// <summary>
    /// 
    /// </summary>
    public class RetryableContentTypeService : IRetryableContentTypeService
    {
        private readonly IUmbracoService _umbracoContentTypeService;
        private IContentTypeService ContentTypeService
        {
            get
            {
                return _umbracoContentTypeService.GetContentTypeService();
            }
        }

        public RetryableContentTypeService(IUmbracoService umbracoContentTypeService)
        {
            _umbracoContentTypeService = umbracoContentTypeService;
        }

        public IContentType GetContentType(int id)
        {
            return ContentTypeService.GetContentType(id);
        }

        public IContentType GetContentType(string alias)
        {
            IContentType contentType = null;

            contentType = ContentTypeService.GetContentType(alias);

            return contentType;
        }

        public IEnumerable<IContentType> GetAllContentTypes(params int[] ids)
        {
            IEnumerable<IContentType> contentTypes = null;

            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .With(context => contentTypes = ContentTypeService.GetAllContentTypes(ids));

            return contentTypes;
        }

        public IEnumerable<IContentType> GetContentTypeChildren(int id)
        {
            return ContentTypeService.GetContentTypeChildren(id);
        }

        public void Save(IContentType contentType, int userId = 0)
        {
            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .AndOn<DuplicateNameException>().For(5)
                .With(context => ContentTypeService.Save(contentType, userId));
        }

        public void Save(IEnumerable<IContentType> contentTypes, int userId = 0)
        {
            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .AndOn<DuplicateNameException>().For(5)
                .With(context => ContentTypeService.Save(contentTypes, userId));
        }

        public void Delete(IContentType contentType, int userId = 0)
        {
            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .With(context => ContentTypeService.Delete(contentType, userId));
        }

        public void Delete(IEnumerable<IContentType> contentTypes, int userId = 0)
        {
            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .With(context => ContentTypeService.Delete(contentTypes, userId));
        }

        public string GetDtd()
        {
            string Dtd = null;
            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .With(context => Dtd = ContentTypeService.GetDtd());

            return Dtd;
        }

        public string GetContentTypesDtd()
        {
            string Dtd = null;
            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .With(context => Dtd = ContentTypeService.GetContentTypesDtd());

            return Dtd;
        }

        public bool HasChildren(int id)
        {
            bool hasChild = false;
            Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205)
                .For(5)
                .With(context => hasChild = ContentTypeService.HasChildren(id));

            return hasChild;
        }
    }
}
