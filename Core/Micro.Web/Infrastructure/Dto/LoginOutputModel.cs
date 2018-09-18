using Micro.Web.Infrastructure.Domain;
using System.Collections.Generic;

namespace Micro.Web.Infrastructure.Dto
{
    public class LoginOutputModel
    {
        public string ReturnUrl { get; set; }
        public string Token { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
