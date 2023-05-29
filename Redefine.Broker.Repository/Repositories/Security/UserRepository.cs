using Redefine.Broker.Data;
using Redefine.Broker.Data.Models.Security;
using Redefine.Broker.Repository.Interface.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Repository.Repository.Security
{
    public class UserRepository : RepositoryBase<User, RedefineBrokerContext>, IUserRepository
    {
        public UserRepository(RedefineBrokerContext dbContext) : base(dbContext) { }

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            Add(user);
            await SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(user);

            await SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<User> FindByEmailAddressAsync(string email, CancellationToken cancellationToken = default)
        {
            return await FindAsync(e => e.Email == email, cancellationToken);
        }
    }
}