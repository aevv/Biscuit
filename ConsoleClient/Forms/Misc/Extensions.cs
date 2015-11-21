using System;
using System.Windows.Forms;
using Logging;

namespace ConsoleClient.Forms.Misc
{
    static class Extensions
    {
        /// <summary>
        /// Easy cross thread invocation for controls.
        /// </summary>
        public static void Call(this Control control, MethodInvoker action)
        {
            try
            {
                control.Invoke(action);
            }
            catch (Exception ex)
            {
                // TODO: Fix or hide? Check for the disposed state?
                Out.Red(ex.Message);
            }
        }
    }
}
