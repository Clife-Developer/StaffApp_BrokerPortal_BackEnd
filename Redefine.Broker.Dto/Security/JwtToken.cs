using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Dto.Security
{
    public class JwtToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}