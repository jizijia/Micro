using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Core
{
    public interface IClientContext
    {
        dynamic User { get; set; }
        string CreateToken(dynamic user);
        string UserAgent { get; }
        string ClientIp { get; }
        string RequestPath { get; }
    }
}
