using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Redefine.Broker.Data.Models.Security;

namespace Redefine.Broker.Data.Identity
{
	public class UserStore : IUserStore<User>, IUserPasswordStore<User>
	{
		private readonly RedefineBrokerContext _context;
		public UserStore(RedefineBrokerContext context)
		{
			_context = context;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_context?.Dispose();
			}
		}

		public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
		{
			try
			{
				//	Verify user does not exist
				var existingUser = await (from u in _context.Users where u.Email == user.Email select u).FirstOrDefaultAsync();
				if (existingUser != null)
					return await Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "-1", Description = "Email address already registered.", }));

				_context.Add(user);

				var count = await _context.SaveChangesAsync(cancellationToken);
				return await Task.FromResult((count > 0) ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Code = "-1", Description = "User not saved" }));
			}
			catch (Exception ex)
			{
				return await Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "-1", Description = ex.Message, }));
			}
		}

		public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
		{
			user.IsDeleted = true;
			_context.Update(user);

			await _context.SaveChangesAsync(cancellationToken);
			return await Task.FromResult(IdentityResult.Success);
		}

		public async Task<User> FindByIdAsync(string email, CancellationToken cancellationToken)
		{
			
				return await _context.Users
							  .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
		}

		public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
		{
			return await _context.Users
							.SingleOrDefaultAsync(p => p.Email == normalizedUserName, cancellationToken);
		}

		public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
		{
			return await Task.FromResult(user.Email.ToLower());
		}

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(user.Password))
                return await Task.FromResult(user.Password);

            return await Task.FromResult((await _context.Users.SingleOrDefaultAsync(u => u.UserId == user.UserId || u.Email == user.Email))?.Password);
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
		{
			return await Task.FromResult(user.UserId.ToString());
		}

		public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
		{
			return await Task.FromResult(user.Email);
		}

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(string.IsNullOrWhiteSpace(user.Password));
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            _context.Update(user);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
		{
			_context.Update(user);

			await _context.SaveChangesAsync(cancellationToken);

			return await Task.FromResult(IdentityResult.Success);
		}

		//public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
		//{
		//	User dbUser = await FindUserAsync(user, cancellationToken);

		//	if (dbUser != null) //	throw?
		//	{
		//		var dbRole = await (from r in _context.Role where r.RoleName == roleName select r).FirstOrDefaultAsync();
		//		if (dbRole != null)
		//		{
		//			if (dbUser.UserRole.FirstOrDefault(ur => ur.RoleId == dbRole.RoleId) == null)
		//			{
		//				dbUser.UserRole.Add(new UserRole { RoleId = dbRole.RoleId, UserId = dbUser.UserId, CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow });
		//				await _context.SaveChangesAsync();
		//			}
		//		}
		//	}
		//}

		//public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
		//{
		//	User dbUser = await FindUserAsync(user, cancellationToken);

		//	if (dbUser != null)
		//	{
		//		var ur = dbUser.UserRole.FirstOrDefault(ur => ur.Role.RoleName == roleName);
		//		if (ur != null)
		//		{
		//			_context.Remove(ur);
		//			await _context.SaveChangesAsync();
		//		}
		//	}
		//}

		public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		//private async Task<User> FindUserAsync(User user, CancellationToken cancellationToken)
		//{
		//	return await _context.Users
		//		.Include(u => u.UserRole)
		//		.ThenInclude(ur => ur.Role)
		//		.Include(u => u.PerformerUser)
		//		.SingleOrDefaultAsync(u => u.UserId == user.UserId
		//			|| u.Email == user.Email, cancellationToken);
		//}
	}
}