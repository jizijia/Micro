using JetBrains.Annotations;
using Micro.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Micro.Core
{
    [DebuggerStepThrough]
    public static class Guard
    {
        [ContractAnnotation("value:null => halt")]
        public static T ArgumentIsNotNull<T>(T value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string ArgumentNotNullOrEmpty(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string ArgumentNotNullOrWhiteSpace(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static ICollection<T> ArgumentNotNullOrEmpty<T>(ICollection<T> value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
            }

            return value;
        }
    }
}
