using System;
using System.Collections.Generic;
using System.Text;

namespace Micro.Core
{
    /// <summary>
    /// Ioc 
    /// </summary>
    public class IocResolver
    {
        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
            where T : class
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <returns>Resolved service</returns>
        public static object Resolve(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
