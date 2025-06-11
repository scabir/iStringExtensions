using System;
using System.Collections.Generic;
using System.Text;

namespace iStringExtensions
{
    internal static class Helpers
    {
        
    }

    internal static class Guard
    {
        internal static void AgainstNull(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException();
            }
        }
    }
}
