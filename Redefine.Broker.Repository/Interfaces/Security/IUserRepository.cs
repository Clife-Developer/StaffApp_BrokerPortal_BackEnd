using Redefine.Broker.Data.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Repository.Interface.Security
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task<User> FindByEmailAddressAsync(string email, CancellationToken cancellationToken = default);
    }
}
