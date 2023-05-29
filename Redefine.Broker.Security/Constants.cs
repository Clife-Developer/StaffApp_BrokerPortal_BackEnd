using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Security
{
    public class Constants
    {
		public static class ClaimIdentifiers
		{
			public const string Role = System.Security.Claims.ClaimsIdentity.DefaultRoleClaimType;
			public const string Name = System.Security.Claims.ClaimsIdentity.DefaultNameClaimType;
			public const string Id = nameof(Id);
			public const string Verified = nameof(Verified);
		}

		public static class Claims
		{
			public const string WebApiUser = nameof(WebApiUser);
			public const string Viewer = nameof(Viewer);
			public const string Admin = nameof(Admin);
			public const string Verified = nameof(Verified);
		}
	}
}
