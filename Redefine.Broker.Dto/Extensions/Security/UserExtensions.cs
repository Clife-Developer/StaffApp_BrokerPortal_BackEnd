using Redefine.Broker.Dto.Models.Security;
using Redefine.Broker.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Dto.Extensions.Security
{
    public static class UserExtensions
    {
        public static User ToApiModel(this Data.Models.Security.User user, User manager = null)
        {
            var apiUser = new User
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsDeleted = user.IsDeleted,
                DateCreated = user.DateCreated,
                MobileNumber = user.MobileNumber,
            };

            return apiUser;
        }

        public static Data.Models.Security.User ToDataModel(this Dto.Models.Security.User user)
        {
            return new Data.Models.Security.User
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsDeleted = user.IsDeleted,
                DateCreated = user.DateCreated,
                MobileNumber = user.MobileNumber,
                Password = user.Password,
            };
        }

        public async static Task<User> GenerateTokenResponseAsync(this Data.Models.Security.User user, IJwtFactory jwtFactory, JwtIssuerOptions jwtOptions)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User must not be null");

            var identity = jwtFactory.GenerateClaimsIdentity(user.Email, user.UserId, new List<string>());
            var response = user.ToApiModel();

            response.Token = new Dto.Security.JwtToken
            {
                //Id = user.UserId,
                Token = await jwtFactory.GenerateEncodedToken(user.Email, identity),
                ExpiresIn = (int)jwtOptions.Transient.ValidFor.TotalMilliseconds,
            };

            return response;
        }

        public static string ToFullName(this Data.Models.Security.User user)
        {
            return $"{user.FirstName} {user.LastName}";
        }
    }
}