using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient.Misc
{
    class QueuedFunction
    {
        public List<object> Parameters { get; set; }
        public Action<List<Object>> Function { get; set; }
    }
}
