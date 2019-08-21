using DataModel;
using DocumentRepositoryService.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace FrissDMS.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly IDocumentRepository<Document> _docRepo;
        private readonly IDocumentUnitOfWork<Document> _docHelper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly CustomLogger.CustomLogger _logger;

        public DocumentController(IDocumentRepository<Document> docRepo, IHostingEnvironment hostingEnvironment,
            IDocumentUnitOfWork<Document> docHelper)
        {
            _docRepo = docRepo;
            _hostingEnvironment = hostingEnvironment;
            _docHelper = docHelper;
            _logger = new CustomLogger.CustomLogger("DocumentWebApi-logs.txt");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Response.Headers.Add("Access-Control-Allow-Headers", "content-type");
            base.OnActionExecuting(context);
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Document>> GetDocuments()
        {
            try
            {
                _logger.Log(LogLevel.Information, "Call GetDocuments.", "DocumentController_GetDocuments",
                    User.FindFirst("Username").Value, HttpStatusCode.Created);
                var docs = _docRepo.GetAll().OrderByDescending(x => x.LastAccessDate);
                _logger.Log(LogLevel.Information, "End calling GetDocuments.", "DocumentController_GetDocuments",
                    User.FindFirst("Username").Value, HttpStatusCode.OK);

                return Ok(docs);
            }
            catch (Exception exp)
            {
                _logger.Log(LogLevel.Error, exp.Message, exp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = exp.Message });
            }
        }

        [HttpPost("[action]"), DisableRequestSizeLimit]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult Upload()
        {
            try
            {
                _logger.Log(LogLevel.Information, "Upload starts.", "DocumentController_Upload",
                    User.FindFirst("Username").Value, HttpStatusCode.Created);
                var fullPath = string.Empty;
                var file = Request.Form.Files[0];
                var folderName = "Uploads";
                var webRootPath = _hostingEnvironment.WebRootPath;
                var newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                var userId = User.FindFirst("UserId");
                var newDoc = _docHelper.SaveUpload(new FileInfo(fullPath), userId.Value);
                _logger.Log(LogLevel.Information, "Upload ends.", "DocumentController_Upload",
                    User.FindFirst("Username").Value, HttpStatusCode.OK);

                return Ok(new { id = newDoc.Id, name = newDoc.Name, message = "Upload Successful." });
            }
            catch (ArgumentNullException argExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, argExp.Message, argExp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = argExp.Message });
            }
            catch (IOException ioExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, ioExp.Message, ioExp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = ioExp.Message });
            }
            catch (Exception exp)
            {
                _logger.Log(LogLevel.Error, exp.Message, exp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = exp.Message });
            }
        }

        [HttpGet("[action]")]
        public ActionResult Download(string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, "Download starts.", "DocumentController_Download",
                    User.FindFirst("Username").Value, HttpStatusCode.Created);
                var doc = _docHelper.SaveDownload(int.Parse(id));
                _logger.Log(LogLevel.Information, "Download ends.", "DocumentController_Download",
                    User.FindFirst("Username").Value, HttpStatusCode.OK);

                return Ok(new { docName = doc.Name });
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, exp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = exp.Message });
            }
        }

        [HttpDelete("[action]"), HttpOptions]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        public int DeleteDocument(string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, "Delete Document starts.", "DocumentController_DeleteDocument",
                    User.FindFirst("Username").Value, HttpStatusCode.Created);
                var doc = _docRepo.Find(int.Parse(id));
                if (doc == null) return -1;

                System.IO.File.Delete(doc.Path);
                _docRepo.Delete(doc);
                _logger.Log(LogLevel.Information, "Delete Document ends.", "DocumentController_DeleteDocument",
                    User.FindFirst("Username").Value, HttpStatusCode.OK);

                return _docRepo.SaveChanges();
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, exp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return 0;
            }
        }


    }
}