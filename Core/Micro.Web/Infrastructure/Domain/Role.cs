using Micro.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Micro.Web.Infrastructure.Domain
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
