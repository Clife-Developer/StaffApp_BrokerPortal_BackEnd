using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Services.Interfaces
{
    public interface IUserService
    {
        Task<Dto.Models.Security.User> Create(Dto.Models.Security.User userModel);
        Task<Dto.Models.Security.User> Login(Dto.Models.Security.User userModel);
    }
}
