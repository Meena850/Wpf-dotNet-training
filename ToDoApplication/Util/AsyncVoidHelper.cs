using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Windows;

namespace ToDoApplication.Util
{
    static class AsyncVoidHelper
    {
        public static async void TryThrowOnDispatcher(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch(Exception ex)
            {
                var info = ExceptionDispatchInfo.Capture(ex);
                Application.Current.Dispatcher.Invoke(() => info.Throw());
            }
        }
    }
}
