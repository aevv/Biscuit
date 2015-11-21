using System;

namespace ConsoleClient.Exceptions
{
    /// <summary>
    /// Thrown when a form has specified to not allow even 1 window.
    /// </summary>
    class TooFewWindowsAllowedException : Exception
    {
        public TooFewWindowsAllowedException() : base("Too few windows allowed open for this form. Minimum is 1.")
        {
            
        }
    }
}
