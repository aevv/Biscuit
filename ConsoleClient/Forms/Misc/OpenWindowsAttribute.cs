using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleClient.Exceptions;
using OpenTK.Graphics.OpenGL;

namespace ConsoleClient.Forms.Misc
{
    /// <summary>
    /// Number of open forms allowed for the given form. Used by FormManager to stop forms being opened if they are single instance.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    class OpenWindowsAttribute : Attribute
    {
        private int _amount;

        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (value < 1)
                    throw new TooFewWindowsAllowedException();
                _amount = value;
            }
        }

        public OpenWindowsAttribute(int amount)
        {
            Amount = amount;
        }
    }
}
