using System.Collections.Generic;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Repositories
{
    [TestFixture]
    public class ContentReadRepositoryTests
    {
        private List<IContentType> CreateContentTypes(int numberOfContentTypes)
        {
            List<IContentType> contentTypeList = new List<IContentType>();

            for (int i = 1; i <= numberOfContentTypes; i++)
            {
                ContentType contentType = new ContentType(-1);
                contentType.Id = i;
                contentType.Name = "documentType" + i;
                contentType.Alias = "Aliastest";
                contentType.SetDefaultTemplate(MockRepository.GenerateStub<ITemplate>());

                contentTypeList.Add(contentType);
            }

            return contentTypeList;
        }
    }
}
