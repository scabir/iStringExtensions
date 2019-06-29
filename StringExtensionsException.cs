using System;
using System.Collections.Generic;
using System.Text;

namespace iStringExtensions
{
    public class StringExtensionsException : Exception
    {
        public StringExtensionsException()
            : base()
        {

        }

        public StringExtensionsException(string message)
            : base(message)
        {

        }
    }
}
