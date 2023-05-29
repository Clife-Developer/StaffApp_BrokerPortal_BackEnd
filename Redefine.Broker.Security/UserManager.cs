using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Security
{
    public class UserManager<TUser, TUserStore, TUserPasswordStore>
        : IUserManager<TUser, TUserStore, TUserPasswordStore>
        where TUser : class
        where TUserStore : IUserStore<TUser>
        where TUserPasswordStore : IUserPasswordStore<TUser>
    {
        protected readonly IUserStore<TUser> _userStore;
        protected readonly IUserPasswordStore<TUser> _passwordStore;
        protected readonly ILogger<UserManager<TUser, TUserStore, TUserPasswordStore>> _logger;

        public UserManager(IUserStore<TUser> userStore, IUserPasswordStore<TUser> passwordStore, ILogger<UserManager<TUser, TUserStore, TUserPasswordStore>> logger)
        {
            _userStore = userStore;
            _passwordStore = passwordStore;
            _logger = logger;
        }

        public async Task<TUser> CreateAsync(TUser user, string password)
        {
            var result = await _userStore.CreateAsync(user, CancellationToken.None);

            if (result.Succeeded)
            {
                await UpdatePasswordAsync(user, password);
                return user;
            }
            else
                return null;
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            return await _userStore.FindByNameAsync(userName, CancellationToken.None);
        }

        public async Task<TUser> FindByIdAsync(string email)
        {
            return await _userStore.FindByIdAsync(email, CancellationToken.None);
        }

        public async Task<bool> ValidateAsync(string userName, string password)
        {
            try
            {
                var user = await _userStore.FindByNameAsync(userName, CancellationToken.None);
                if (user != null)
                {
                    var dbPasswordHash = await _passwordStore.GetPasswordHashAsync(user, CancellationToken.None);
                    var result = await Task.FromResult((await CreatePasswordHasher()).VerifyHashedPassword(user, dbPasswordHash, password));

                    if (result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        //	TODO: stuff	
                    }

                    return result == PasswordVerificationResult.Success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            //	If not found
            return false;
        }

        public async Task<bool> UpdatePasswordAsync(TUser user, string password)
        {
            await _passwordStore.SetPasswordHashAsync(user, await HashPassword(user, password), CancellationToken.None);
            return true;
        }

        private async Task<string> HashPassword(TUser user, string password)
        {
            return await Task.FromResult((await CreatePasswordHasher()).HashPassword(user, password));
        }

        private async Task<PasswordHasher<TUser>> CreatePasswordHasher()
        {
            return await Task.FromResult(new PasswordHasher<TUser>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2,
                    })
                ));
        }
    }
}