using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Security
{
    public class JwtFactory : IJwtFactory
	{
		private readonly JwtIssuerOptions _jwtOptions;

		public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
		{
			_jwtOptions = jwtOptions.Value;
			
			ThrowIfInvalidOptions(_jwtOptions);
		}

		public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, userName),
				new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
				new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.Transient.IssuedAt).ToString(), ClaimValueTypes.Integer64),
				identity.FindFirst(Constants.ClaimIdentifiers.Id),
			};

			claims.AddRange(identity.Claims);

			// Create the JWT security token and encode it.
			var jwt = new JwtSecurityToken(
				issuer: _jwtOptions.Issuer,
				audience: _jwtOptions.Audience,
				claims: claims,
				notBefore: _jwtOptions.Transient.NotBefore.UtcDateTime,
				expires: _jwtOptions.Transient.Expiration.UtcDateTime,
				signingCredentials: _jwtOptions.SigningCredentials);

			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}

		public ClaimsIdentity GenerateClaimsIdentity(string userName, Guid id, List<string> roles)
		{
			var claims = new List<Claim>
			{
				new Claim(Constants.ClaimIdentifiers.Id, id.ToString()),
				new Claim(Constants.ClaimIdentifiers.Name, userName),
				new Claim(Constants.ClaimIdentifiers.Role, Constants.Claims.WebApiUser),
			};

			//claims.AddRange(roles.ConvertAll(r => new Claim(Constants.ClaimIdentifiers.Role, r)));

			return new ClaimsIdentity(new GenericIdentity(userName, "Token"), claims.ToArray());
		}

		/// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
		private static long ToUnixEpochDate(DateTimeOffset date)
		  => (long)Math.Round((date.UtcDateTime -
							   new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
							  .TotalSeconds);

		private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));

			if (options.Transient.ValidFor <= TimeSpan.Zero)
			{
				throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptionsTransient.ValidFor));
			}

			if (options.SigningCredentials == null)
			{
				throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
			}

			if (options.JtiGenerator == null)
			{
				throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
			}
		}
	}
}