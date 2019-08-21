using DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace FrissDMSUnitTests.Helpers
{
    public class MocUserManager<T> : IUserPasswordStore<T>, IUserEmailStore<T> where T : User
    {
        private readonly IUnityContainer _uc = new UnityContainer();
        private readonly FRISSDmsContext _dbContext;

        public MocUserManager()
        {
            _uc.RegisterType(typeof(DbContext), typeof(FRISSDmsContext));
            _uc.RegisterType(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));

            _dbContext = (FRISSDmsContext)_uc.Resolve(typeof(FRISSDmsContext));
        }
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetUserIdAsync(T user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetUserNameAsync(T user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetUserNameAsync(T user, string userName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(T user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(T user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(T user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (_dbContext.Users.AddAsync(user, cancellationToken).Result.Entity.Id == null)
                    return IdentityResult.Failed();

                _dbContext.SaveChangesAsync(cancellationToken);
                return IdentityResult.Success;
            }, cancellationToken);
        }

        public Task<IdentityResult> UpdateAsync(T user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                _dbContext.User.Update(user);
                return _dbContext.SaveChangesAsync(cancellationToken).Result > 0 ?
                    IdentityResult.Success : IdentityResult.Failed();
            }, cancellationToken);
        }

        public Task<IdentityResult> DeleteAsync(T user, CancellationToken cancellationToken)
        {
            var userObj = (User)user;

            return Task.Run(() =>
            {
                _dbContext.User.Remove(userObj);
                return _dbContext.SaveChangesAsync(cancellationToken).Result > 0 ?
                    IdentityResult.Success : IdentityResult.Failed();
            }, cancellationToken);
        }

        public Task<T> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetPasswordHashAsync(T user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(T user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(T user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetEmailAsync(T user, string email, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetEmailAsync(T user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(T user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(T user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                return (T)_dbContext.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken).Result;
            }, cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(T user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(T user, string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }

    public class MocUserManager
    {
        public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;

            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions { Lockout = { AllowedForNewUsers = false } };
            options.Setup(o => o.Value).Returns(idOptions);

            var userValidators = new List<IUserValidator<TUser>>();
            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);

            var pwdValidators = new List<PasswordValidator<TUser>> { new PasswordValidator<TUser>() };

            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);

            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            return userManager;
        }
    }
}
