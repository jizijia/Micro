using Micro.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Micro.Web.Infrastructure.Domain
{
    public class Permission:BaseEntity
    {
        public string Name { get; set; }

        public string ApiPath { get; set; }

        public string Icon { get; set; } 
    }
}
