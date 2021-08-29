using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ESCPOS_NET
{
    public static class Logging
    {
        public static ILogger Logger { get; set; }
        static Logging()
        {

            TaskScheduler.UnobservedTaskException += (s, e) =>
                {
                    //LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
                    e.SetObserved();
                };
        }
    }
}
