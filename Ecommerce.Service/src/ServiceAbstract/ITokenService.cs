using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Entities;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface ITokenService
    {
        public string GetToken(User user);
        public Guid VerifyToken(string token);
    }
}