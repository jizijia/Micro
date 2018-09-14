using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Data
{
    /// <summary>
    /// 视图仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewRepository<T>
        where T : class
    {  
        /// <summary>
        /// Table without tracking.
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}
