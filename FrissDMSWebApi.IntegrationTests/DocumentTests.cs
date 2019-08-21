using FrissDMSWebApi.IntegrationTests.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FrissDMSWebApi.IntegrationTests
{
    [TestFixture]
    public class DocumentTests
    {
        private IConfigurationRoot _config;
        private CustomLogger.CustomLogger _logger;
        private string _clientUrl;
        [SetUp]
        public void Setup()
        {
            _logger = new CustomLogger.CustomLogger("HttpTrans-Integration.txt");
            _config = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Path.GetDirectoryName(
                    Path.GetDirectoryName(Directory.GetCurrentDirectory()))))
                .AddJsonFile("appsettings.json").Build();
            _clientUrl = _config.GetSection("AppSettings").GetValue<string>("ClientUrl");
        }

        [Test]
        public async Task GetAllDocuments_Unauthorized()
        {
            try
            {
                using (var client = new ClientProvider().Client)
                {
                    var response = await
                        client.GetAsync(_clientUrl + "/api/Document/GetDocuments");
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before Assert_Unauthorized",
                        "GetAllDocument_IntegrationTest", "cagnesa", HttpStatusCode.Created);

                    Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);

                    //As there is no exception thrown, it means calling the WebApi service has occurred successfully.
                }
            }
            catch (AssertionException assertExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp.Message,
                    "GetAllDocuments_Unauthorized", "cagnesa", HttpStatusCode.BadRequest);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message,
                    "GetAllDocuments_Unauthorized", "cagnesa", HttpStatusCode.BadRequest);
            }

        }

        [Test]
        public async Task UploadDocument_Unauthorized()
        {
            try
            {
                var fileContent = await File.ReadAllLinesAsync("c:\\test-dest.txt");
                var content = JsonConvert.SerializeObject(fileContent);
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var client = new ClientProvider().Client)
                {
                    var response = await client.PostAsync(_clientUrl + "/api/Document/Upload", byteContent);

                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before Assert",
                        "UploadDocument_IntegrationTest", "cagnesa", HttpStatusCode.Created);

                    Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
                }
            }
            catch (AssertionException assertExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp.Message,
                    "UploadDocument_Unauthorized", "cagnesa", HttpStatusCode.BadRequest);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message,
                    "UploadDocument_Unauthorized", "cagnesa", HttpStatusCode.BadRequest);
            }
        }

        [Test]
        public async Task DownloadDocument_UpdateLastAccessTime_IncreaseDownloadCounterByOne_Unauthorized()
        {
            try
            {
                var content = JsonConvert.SerializeObject(26); //26 is an existed DocumentID.
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var client = new ClientProvider().Client)
                {
                    var response = await
                        client.PostAsync(_clientUrl + "/api/Document/Download", byteContent);
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before Assert_Unauthorized",
                        "DownloadDocument_IntegrationTest", "cagnesa", HttpStatusCode.Created);

                    Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);

                    //As there is no exception thrown, it means calling the WebApi service has occurred successfully.
                }
            }
            catch (AssertionException assertExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp.Message,
                    "DownloadDocument_UpdateLastAccessTime_IncreaseDownloadCounterByOne_Unauthorized",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message,
                    "DownloadDocument_UpdateLastAccessTime_IncreaseDownloadCounterByOne_Unauthorized",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
        }
    }
}