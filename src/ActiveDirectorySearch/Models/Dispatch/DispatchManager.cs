#if WPF
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EventManager = AndreasReitberger.ActiveDirectorySearch.Models.EventLogger.EventManager;
#else
using AndreasReitberger.ActiveDirectorySearch.Models.EventLogger;
#endif

namespace AndreasReitberger.ActiveDirectorySearch.Models.Dispatch
{    
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public static class DispatchManager
    {
        //Help: https://stackoverflow.com/questions/62154462/xamarin-difference-between-device-begininvokeonmainthread-and-mainthread-begini
        #region Properties
        public static void UpdateUIThreadSave(Action action, bool forceUiThread = false)
        {
#if WPF
            if (Application.Current.Dispatcher.Thread != Thread.CurrentThread || forceUiThread)
#else
            if (!MainThread.IsMainThread || forceUiThread)
#endif
            {
#if WPF
                Application.Current.Dispatcher.Invoke(() =>
#else
                MainThread.InvokeOnMainThreadAsync(() =>
#endif
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception exc)
                    {
                        EventManager.Instance.LogError(exc);
                    }
                });
            }
            else
            {
                action?.Invoke();
            }
        }
        public static async Task UpdateUIThreadSaveAsync(Action action, bool forceUiThread = false)
        {
#if WPF
            if (Application.Current.Dispatcher.Thread != Thread.CurrentThread || forceUiThread)
#else
            if (!MainThread.IsMainThread || forceUiThread)
#endif
            {
#if WPF
                await Application.Current.Dispatcher.InvokeAsync(() =>
#else
                await MainThread.InvokeOnMainThreadAsync(() =>
#endif
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception exc)
                    {
                        EventManager.Instance.LogError(exc);
                    }
                });
            }
            else
            {
                action?.Invoke();
            }
        }
        public static async Task UpdateInNewTaskAsync(Action action)
        {
            await Task.Run(() =>
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception exc)
                {
                    EventManager.Instance.LogError(exc);
                }
            });
        }

        public static void UpdateUIThreadSave(Func<Task> action, bool forceUiThread = false)
        {
#if WPF
            if (Application.Current.Dispatcher.Thread != Thread.CurrentThread || forceUiThread)
#else
            if (!MainThread.IsMainThread || forceUiThread)
#endif
            {
#if WPF
                Application.Current.Dispatcher.Invoke(() =>
#else
                MainThread.InvokeOnMainThreadAsync(() =>
#endif
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception exc)
                    {
                        EventManager.Instance.LogError(exc);
                    }
                });
            }
            else
            {
                action?.Invoke();
            }
        }
        public static async Task UpdateUIThreadSaveAsync(Func<Task> action, bool forceUiThread = false)
        {
#if WPF
            if (Application.Current.Dispatcher.Thread != Thread.CurrentThread || forceUiThread)
#else
            if (!MainThread.IsMainThread || forceUiThread)
#endif
            {
#if WPF
                await Application.Current.Dispatcher.InvokeAsync(() =>
#else
                await MainThread.InvokeOnMainThreadAsync(async () =>
#endif
                {
                    try
                    {
#if WPF
                        action.Invoke();
#else
                        await action.Invoke();
#endif
                    }
                    catch (Exception exc)
                    {
                        EventManager.Instance.LogError(exc);
                    }
                });
            }
            else
            {
                await action.Invoke();
            }
        }
        public static async Task UpdateInNewTaskAsync(Func<Task> action)
        {
            await Task.Run(async () =>
            {
                try
                {
                    await action.Invoke();
                }
                catch (Exception exc)
                {
                    EventManager.Instance.LogError(exc);
                }
            });
        }
#if !WPF
        public static void Dispatch(IDispatcher? dispatcher, Action action, bool forceUiThread = false)
        {
            if (dispatcher?.IsDispatchRequired is true || forceUiThread)
            {
                ArgumentNullException.ThrowIfNull(dispatcher);
                dispatcher.Dispatch(() =>
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception exc)
                    {
                        EventManager.Instance.LogError(exc);
                    }
                });
            }
            else
            {
                action?.Invoke();
            }
        }
        public static void Dispatch(IDispatcher? dispatcher, Func<Task> action, bool forceUiThread = false)
        {
            if (dispatcher?.IsDispatchRequired is true || forceUiThread)
            {
                ArgumentNullException.ThrowIfNull(dispatcher);
                dispatcher.Dispatch(() =>
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception exc)
                    {
                        EventManager.Instance.LogError(exc);
                    }
                });
            }
            else
            {
                action?.Invoke();
            }
        }
        public static async Task DispatchAsync(IDispatcher? dispatcher, Action action, bool forceUiThread = false)
        {
            if (dispatcher?.IsDispatchRequired is true|| forceUiThread)
            {
                ArgumentNullException.ThrowIfNull(dispatcher);
                await dispatcher.DispatchAsync(() =>
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception exc)
                    {
                        EventManager.Instance.LogError(exc);
                    }
                });
            }
            else
            {
                action?.Invoke();
            }
        }
        public static async Task DispatchAsync(IDispatcher? dispatcher, Func<Task> action, bool forceUiThread = false)
        {
            if (dispatcher?.IsDispatchRequired is true || forceUiThread)
            {
                ArgumentNullException.ThrowIfNull(dispatcher);
                await dispatcher.DispatchAsync(async () =>
                {
                    try
                    {
                        await action.Invoke();
                    }
                    catch (Exception exc)
                    {
                        EventManager.Instance.LogError(exc);
                    }
                });
            }
            else
            {
                await action.Invoke();
            }
        }
#endif
#endregion
    }
}
