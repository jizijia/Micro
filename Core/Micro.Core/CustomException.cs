using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Micro.Core
{
    public class CustomException : Exception
    { /// <summary>
      /// Creates a new <see cref="AbpException"/> object.
      /// </summary>
        public CustomException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        public CustomException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public CustomException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
