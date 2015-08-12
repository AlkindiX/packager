using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Packager.VersioControl
{
    [Serializable]
    internal class BadStatmentException : Exception
    {
        public BadStatmentException()
        {
        }

        public BadStatmentException(string message)
            : base(message)
        {
        }

        public BadStatmentException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BadStatmentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}