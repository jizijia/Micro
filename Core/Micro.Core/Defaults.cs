using Micro.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Core
{
    public class Defaults
    {
        /// <summary>
        /// Gets or sets the default file provider
        /// </summary>
        public static IAppFileProvider DefaultFileProvider { get; set; }
    }
}
