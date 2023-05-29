using Redefine.Broker.Repository.Interface.Security;
using Redefine.Broker.Services.Interfaces;
using Redefine.Broker.Security;
using Redefine.Broker.Data.Identity;
using Microsoft.Extensions.Options;
using Redefine.Broker.Dto.Extensions.Security;

namespace Redefine.Broker.Services.Services.Security
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<Data.Models.Security.User, UserStore, UserStore> _userManager;
        public UserService(IUserRepository userRepository, UserManager<Data.Models.Security.User, UserStore, UserStore> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
            _userManager = userManager;
            _jwtOptions = jwtOptions?.Value;
        }

        public async Task<Dto.Models.Security.User> Login(Dto.Models.Security.User userModel)
        {
            var user = userModel.ToDataModel();
            if (await _userManager.ValidateAsync(user.Email, user.Password))
            {
                var userToVerify = await _userManager.FindByNameAsync(user.Email);

                var response = await userToVerify.GenerateTokenResponseAsync(_jwtFactory, _jwtOptions);

                return response;
            }

            return null;
        }

        public async Task<Dto.Models.Security.User> Create(Dto.Models.Security.User userModel)
        {
            //	Check user exists
            var exists = await _userRepository.FindByEmailAddressAsync(userModel.Email);
            if (exists != null)
            {
                //TO DO return a proper message
                return null;
            }
            var userDataModel = await _userRepository.CreateAsync(userModel.ToDataModel());

            return userDataModel.ToApiModel();
        }
    }
}