using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using System.Web.Http;

using Mirabeau.uTransporter.DependencyResolution;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;
using Mirabeau.uTransporter.Models;

using StructureMap;

using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace $rootnamespace$.Controllers
{
    [PluginController("Mirabeau")]
    public class UTransporterController : UmbracoAuthorizedApiController
    {
        private readonly IUTransporter _uTransporter;

        public UTransporterController()
        {
             IoC.Bootstrap();
            _uTransporter = ObjectFactory.GetInstance<IUTransporter>();
        }

        [HttpPost]
        public HttpResponseMessage StartSync()
        {
            SyncResult syncResult = _uTransporter.SynchronizeDocumentTypes();

            // Compose response
            HttpStatusCode statusCode = syncResult.Successful ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            return Request.CreateResponse(statusCode, syncResult);
        }

        [HttpPost]
        public HttpResponseMessage StartUpGeneration()
        {
            GenerateOptions options = new GenerateOptions
                                      {
                                          RemoveOutDatedDocumentTypes = true
                                      };

            GenerateResult generateResult = _uTransporter.GenerateDocumentTypes(options);

            // Compose response
            HttpStatusCode statusCode = generateResult.Successful ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            return Request.CreateResponse(statusCode, generateResult);
        }

        [HttpPost]
        public HttpResponseMessage StartRemoveDocumentTypes()
        {
            RemoveResult removeResult = _uTransporter.RemoveDocumentTypes();

            // Compose response
            HttpStatusCode statusCode = removeResult.Successful ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            return Request.CreateResponse(statusCode, removeResult);
        }

        [HttpGet]
        public HttpResponseMessage GetLog(string path)
        {
            LogFileDataService service = new LogFileDataService();
            List<LogFileDataItem> logFileDataItems = service.GetLogData(path);

            // Compose response
            HttpStatusCode statusCode = (logFileDataItems != null && logFileDataItems.Any()) ? HttpStatusCode.OK : HttpStatusCode.NoContent;
            return Request.CreateResponse(statusCode, logFileDataItems);
        }

        [HttpGet]
        public HttpResponseMessage DownloadLog()
        {
            LogFileService logFileService = new LogFileService();
            LogFile logFile = logFileService.GetLogFile();

            // Compose response
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StreamContent(new FileStream(logFile.Path, FileMode.Open));
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = string.Format("UmbracoSyncLog_{0:yyyyMMdd}.txt", DateTime.Now) };
            return result;
        }

        [HttpPost]
        public HttpResponseMessage DryRun()
        {
            SyncResult syncResult = _uTransporter.SynchronizeDocumentTypesDryRun();

            // Compose response
            HttpStatusCode statusCode = syncResult.Successful ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            return Request.CreateResponse(statusCode, syncResult);
        }

        [HttpGet]
        public HttpResponseMessage GetVersion()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _uTransporter.GetVersion());
        }
    }
}