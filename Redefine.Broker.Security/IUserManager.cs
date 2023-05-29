using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Security
{
    public interface IUserManager<TUser, TUserStore, TUserPasswordStore>
        where TUser : class
        where TUserStore : IUserStore<TUser>
    {
        Task<TUser> CreateAsync(TUser user, string password);
        Task<TUser> FindByNameAsync(string userName);
        //Task<TUser> FindByIdAsync(int userId);
        Task<bool> ValidateAsync(string userName, string password);
        Task<bool> UpdatePasswordAsync(TUser user, string password);
    }
}