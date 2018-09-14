using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Core.Data
{
    /// <summary>
    /// 数据库连接字符串配置
    /// </summary>
    public class DataSettings
    {
        /// <summary>
        /// 主库连接字符串
        /// </summary>
        public string MainConnectionString { get; set; }

        /// <summary>
        /// 从库连接字符串
        /// </summary>
        public string SlaveConnectionString { get; set; }

        /// <summary>
        /// PDM 接口地址
        /// </summary>
        public string PDMWebApiUrl { get; set; }
    }
}
