using DataModel;
using DocumentRepositoryService;
using DocumentRepositoryService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.IO;
using Unity;

namespace FrissDMSUnitTests
{
    [TestFixture]
    public class DocumentUnitTests
    {
        private readonly IUnityContainer _uc = new UnityContainer();
        private DocumentRepository<Document> _docRepo;
        private DocumentUnitOfWork<Document> _docHelper;
        private CustomLogger.CustomLogger _logger;

        [SetUp]
        public void Setup()
        {
            _uc.RegisterType(typeof(DbContext), typeof(FRISSDmsContext));
            _uc.RegisterType(typeof(IDocumentRepository<Document>), typeof(DocumentRepository<Document>));
            _uc.RegisterType(typeof(IDocumentUnitOfWork<>), typeof(DocumentUnitOfWork<>));
            _uc.RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>();

            _docRepo = (DocumentRepository<Document>)_uc.Resolve(typeof(DocumentRepository<Document>));
            _docHelper = (DocumentUnitOfWork<Document>)_uc.Resolve(typeof(DocumentUnitOfWork<Document>));

            _logger = new CustomLogger.CustomLogger("DocumentUnitTests-logs.txt");
        }

        [Test]
        public void SaveUpload()
        {
            try
            {
                using (var file = File.OpenRead("c:\\test-source.txt"))
                {
                    using (var stream = new FileStream("c:\\test-dest.txt", FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                var doc = _docHelper.SaveUpload(new FileInfo("c:\\test-dest.txt"),
                    "523e13c6-c8de-4bdd-a045-b08f71ab87a6");

                Assert.IsNotNull(doc);
            }
            catch (AssertionException assertExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp);
            }
        }

        [Test]
        public void SaveDownload() //67 is an existed document ID.
        {
            try
            {
                var doc = _docHelper.SaveDownload(67);
                Assert.IsNotNull(doc);
            }
            catch (AssertionException assertExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp);
            }
        }

    }
}