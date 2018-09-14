using Micro.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Micro.Web.Infrastructure.Domain
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsActivie { get; set; }

    }
}
