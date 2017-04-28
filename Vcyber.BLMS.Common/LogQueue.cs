using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    public static class LogQueue
    {
        private static readonly Queue<string> _logQueue = new Queue<string>();
        static LogQueue()
        {
            Task.Run(() =>
             {
                 while (true)
                 {
                     if (_logQueue.Count > 0)
                     {
                         lock (_logQueue)
                         {
                             if (_logQueue.Count > 0)
                             {
                                 Vcyber.BLMS.Common.LogService.Instance.Info(_logQueue.Dequeue());
                             }
                         }
                     }
                     else
                     {
                         Thread.Sleep(500);
                     }
                 }
             });
        }

        public static void AddLogQueue(string info)
        {
            lock (_logQueue)
            {
                _logQueue.Enqueue(info);
            }

        }
    }
}
