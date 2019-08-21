using FrissDMSWebApi.IntegrationTests.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
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
    public class UserTests
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
        public async Task Login_Pass() //John is existed in DB.
        {
            try
            {
                using (var client = new ClientProvider().Client)
                {
                    //John is an actual existed user into DB. 
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before calling Login",
                        "Login_IntegrationTest", "cagnesa", HttpStatusCode.Created);
                    var response = await
                        client.GetAsync(_clientUrl + "/api/Authentication/Login?username=john&password=pass@word1");
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before Assert_Success",
                        "Login_IntegrationTest", "cagnesa", HttpStatusCode.OK);

                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
            }
            catch (AssertionException assertExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp.Message, "Login_Pass",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, "Login_Pass",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
        }

        [Test]
        public async Task Login_Fail() //Jack is not existed in DB.
        {
            try
            {
                using (var client = new ClientProvider().Client)
                {
                    //John is an actual existed user into DB. 
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before calling Login",
                        "Login_IntegrationTest", "cagnesa", HttpStatusCode.Created);
                    var response = await
                        client.GetAsync(_clientUrl + "/api/Authentication/Login?username=jack&password=jack@123#");
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before Assert_Login",
                        "Login_IntegrationTest", "cagnesa", HttpStatusCode.OK);

                    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                }
            }
            catch (AssertionException assertExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp.Message, "Login_Fail",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, "Login_Fail",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
        }

        [Test]
        public async Task Register_Pass()
        {
            try
            {
                var content = JsonConvert.SerializeObject(new
                {
                    username = "ahmadali",
                    password = "pass@word1",
                    email = "ahmad@gmail.com",
                    name = "ahmad Ali"
                });
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var client = new ClientProvider().Client)
                {
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before calling Register",
                        "Register_IntegrationTest", "cagnesa", HttpStatusCode.Created);
                    var response = await client.PostAsync(_clientUrl + "/api/Authentication/Register", byteContent);
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "After Assert_Register",
                        "Register_IntegrationTest", "cagnesa", HttpStatusCode.OK);

                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
            }
            catch (AssertionException assertExp)

            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp.Message, "Register_Pass",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, "Register_Pass",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
        }

        [Test]
        public async Task EditUser_Pass() //John is an existed user and these is the new data.
        {
            try
            {
                using (var client = new ClientProvider().Client)
                {
                    var content = JsonConvert.SerializeObject(new
                    {
                        username = "jmalcom",
                        password = "john@$123",
                        email = "john@gmail.com",
                        name = "John Malcom"
                    });

                    var buffer = Encoding.UTF8.GetBytes(content);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before calling EditUser",
                        "User_IntegrationTest", "cagnesa", HttpStatusCode.Created);
                    var response =
                        await client.PutAsync(_clientUrl + "/api/User/EditUser", byteContent);
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before Assert_EditUser",
                        "EditUser_IntegrationTest", "cagnesa", HttpStatusCode.OK);

                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
            }
            catch (AssertionException assertExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp.Message, "EditUser_Pass",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, "EditUser_Pass",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
        }

        [Test]
        public async Task EditUser_Fail() //Jack is not existed in DB.
        {
            try
            {
                using (var client = new ClientProvider().Client)
                {
                    var content = JsonConvert.SerializeObject(new
                    {
                        username = "jackyouvi",
                        password = "jack#2@",
                        email = "jack@gmail.com",
                        name = "Jack Youvi"
                    });

                    var buffer = Encoding.UTF8.GetBytes(content);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before calling EditUser",
                        "User_IntegrationTest", "cagnesa", HttpStatusCode.Created);
                    var response =
                        await client.PutAsync(_clientUrl + "/api/User/EditUser", byteContent);
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before Assert_EditUser",
                        "EditUser_IntegrationTest", "cagnesa", HttpStatusCode.OK);

                    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                }
            }
            catch (AssertionException assertExp)

            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp.Message, "EditUser_Fail",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, "EditUser_Fail",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
        }

        [Test]
        public async Task DeleteUser_Pass() //UserID "56952596-54e5-4955-82e0-7423b3e24cd0" is existed in DB.
        {
            try
            {
                using (var client = new ClientProvider().Client)
                {
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before calling DeleteUser",
                        "User_IntegrationTest", "cagnesa", HttpStatusCode.Created);
                    var response =
                        await client.DeleteAsync(_clientUrl + "/api/User/DeleteUser?id=56952596-54e5-4955-82e0-7423b3e24cd0");
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "After Assert_DeleteUser",
                        "DeleteUser_IntegrationTest", "cagnesa", HttpStatusCode.OK);

                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
            }
            catch (AssertionException assertExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp.Message, "DeleteUser_Pass",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, "DeleteUser_Pass",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
        }

        [Test]
        public async Task DeleteUser_Fail() //UserID "52552114-36e4-1002-87t8-0141b5s24yr0" is not existed in DB.
        {
            try
            {
                using (var client = new ClientProvider().Client)
                {
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "Before calling DeleteUser",
                        "User_IntegrationTest", "cagnesa", HttpStatusCode.Created);
                    var response =
                        await client.DeleteAsync(_clientUrl + "/api/User/DeleteUser?id=52552114-36e4-1002-87t8-0141b5s24yr0");
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Trace, "After Assert_DeleteUser",
                        "DeleteUser_IntegrationTest", "cagnesa", HttpStatusCode.OK);

                    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                }
            }
            catch (AssertionException assertExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, assertExp.Message, "DeleteUser_Fail",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, "DeleteUser_Fail",
                    "cagnesa", HttpStatusCode.BadRequest);
            }
        }
    }
}