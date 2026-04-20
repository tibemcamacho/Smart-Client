using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Common.Helpers
{
    public static class Threads
    {
        public static void RunInOtherThread(Action[] start, Action finish)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    foreach (var action in start)
                    {
                        action.Invoke();
                    }
                }
                finally
                {
                    if (finish != null)
                        finish.Invoke();
                }
            });
            thread.Start();
        }

        public static void RunParallelInOtherThread(Action[] start, Action finish)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    Parallel.Invoke(start);
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (finish != null)
                        finish.Invoke();
                }
            });
            thread.Start();
        }

        public static Task InvokeAsync(this Control control, Func<Task> action)
        {
            if (control.InvokeRequired)
            {
                var tcs = new TaskCompletionSource<object>();

                control.BeginInvoke(new MethodInvoker(async () =>
                {
                    try
                    {
                        await action();
                        tcs.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }));

                return tcs.Task;
            }

            return action();
        }
    }
}
