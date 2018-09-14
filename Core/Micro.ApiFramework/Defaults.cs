using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.ApiFramework
{
    public static class Defaults
    {
        public static Action<MvcJsonOptions> JsonOptionAction = x =>
        {
            x.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
            x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            x.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        };
    }
}
