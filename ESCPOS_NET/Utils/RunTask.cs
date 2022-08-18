using System;
using System.Threading;
using System.Threading.Tasks;

namespace ESCPOS_NET.Utils
{
    public static class CancelableTaskRunnerWithTimeout
    {
        public static async Task<T> RunTask<T>(Task<T> task, int timeout = 0, CancellationToken cancellationToken = default)
        {
            await RunTask((Task)task, timeout, cancellationToken);
            return await task;
        }

        public static async Task RunTask(Task task, int timeout = 0, CancellationToken cancellationToken = default)
        {
            if (timeout == 0) timeout = -1;

            var timeoutTask = Task.Delay(timeout, cancellationToken);
            await Task.WhenAny(task, timeoutTask);

            cancellationToken.ThrowIfCancellationRequested();
            if (timeoutTask.IsCompleted)
            {
                throw new TimeoutException();
            }
            await task;
        }
    }
}

