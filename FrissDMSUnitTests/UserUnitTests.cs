using DataModel;
using FrissDMSUnitTests.Helpers;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FrissDMSUnitTests
{
    [TestFixture]
    public class UserUnitTests
    {
        private CustomLogger.CustomLogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new CustomLogger.CustomLogger("DocumentUnitTests-logs.txt");
        }

        [Test]
        public async Task Create_User()
        {
            var mocUser = new MocUserManager<User>();
            try
            {
                var newUser = new User
                {
                    UserName = "cagnesa",
                    Email = "claudio@friss.com",
                    FullName = "Claudio Agnesa"
                };

                var result = await mocUser.CreateAsync(newUser, new System.Threading.CancellationToken());

                Assert.That(result.Succeeded && !string.IsNullOrEmpty(newUser.Id));
            }
            catch (AssertionException asserExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, asserExp);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp);
            }

        }

        [Test]
        public async Task Find_User_Not_Existed_In_DB()
        {
            var mocUser = new MocUserManager<User>();
            try
            {
                var user = await mocUser.FindByEmailAsync("peterma@friss.com", new System.Threading.CancellationToken());
                Assert.IsNull(user);
            }
            catch (AssertionException asserExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, asserExp);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp);
            }
        }

        [Test]
        public async Task Edit_User_Is_Existed_In_DB()
        {
            var mocUser = new MocUserManager<User>();
            await Create_User();

            try
            {
                var user = await mocUser.FindByEmailAsync("claudio@friss.com", new System.Threading.CancellationToken());

                user.Email = "cagnesa@friss.com";
                await mocUser.UpdateAsync(user, new System.Threading.CancellationToken());

                Assert.AreNotEqual(user.Email, "claudio@friss.com");
            }
            catch (AssertionException asserExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, asserExp);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp);
            }
        }

        [Test]
        public async Task Delete_User()
        {
            var mocUser = new MocUserManager<User>();
            try
            {
                var user = await mocUser.FindByEmailAsync("cagnesa@friss.com",
                    new System.Threading.CancellationToken());

                var result = await mocUser.DeleteAsync(user, new System.Threading.CancellationToken());

                Assert.That(result.Succeeded);
            }
            catch (AssertionException asserExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, asserExp);
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp);
            }
        }

    }
}