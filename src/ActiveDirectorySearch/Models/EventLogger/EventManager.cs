using AndreasReitberger.Shared.Core.Enums;
using AndreasReitberger.Shared.Core.EventLogger;
using AndreasReitberger.Shared.Core.Interfaces;
using AndreasReitberger.Shared.Core.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using AndreasReitberger.ActiveDirectorySearch.Enums;
using AndreasReitberger.ActiveDirectorySearch.Models.Dispatch;
#if AppCenter
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

namespace AndreasReitberger.ActiveDirectorySearch.Models.EventLogger
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public partial class EventManager : ObservableObject, IEventManager
    {
        #region Instance
        static EventManager _instance = null;
        static readonly object Lock = new();
        /// <summary>
        /// A static instance of the <c>EventManager</c>
        /// </summary>
        public static EventManager Instance
        {
            get
            {
                lock (Lock)
                {
                    _instance ??= new EventManager();
                }
                return _instance;
            }
            set
            {
                if (_instance == value) return;
                lock (Lock)
                {
                    _instance = value;
                }
            }

        }
        #endregion

        #region Properties
        [ObservableProperty]
        IDispatcher? dispatcher = null;

        [ObservableProperty]
        bool allowCrashAnalyticsData = false;

        [ObservableProperty]
        bool allowAnalyticsData = false;

        [ObservableProperty]
        bool hasCriticalError = false;

        [ObservableProperty]
        ObservableCollection<AppEvent> events = [];
        partial void OnEventsChanged(ObservableCollection<AppEvent> value)
        {
            if (value?.Count > 0)
            {
                HasCriticalError = value?.Where(error => error.Level == ErrorLevel.Critical).ToList().Count > 0;
            }
        }

        #endregion

        #region Ctor
        /// <summary>
        /// Default ctor, if now <c>Dispatcher</c> is provided, try to use the <c>Application.Current.Dispatcher</c>
        /// </summary>
        public EventManager()
        {
            Dispatcher = Application.Current.Dispatcher;
        }
        /// <summary>
        /// Ctor to pass a <c>Dispatcher</c>
        /// </summary>
        /// <param name="dispatcher"><c>IDispatcher</c>, to run log action thread safe/param>
        public EventManager(IDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Logs an <c>Exception</c> and reports it further to <c>AppCenter</c>,
        /// if configured.
        /// </summary>
        /// <param name="exception"><c>Exception</c></param>
        /// <param name="forceReport"><c>true</c>, to force report to <c>ApPCenter</c></param>
        /// <param name="dispatcher"><c>IDispatcher</c>, to run log action thread safe/param>
#if DEBUG
        public void LogError(Exception exception, bool forceReport = true, IDispatcher? dispatcher = null)
#else
        public void LogError(Exception exception, bool forceReport = false, IDispatcher? dispatcher = null)
#endif
        {
            try
            {
                if (exception == null) return;
                // Log error
                dispatcher ??= Dispatcher;
                DispatchManager.Dispatch(dispatcher, () =>
                {
                    Events.Add(new AppErrorEvent()
                    {
                        Message = exception.Message,
                        Exceptio = DebugLogger.GetLastInnerException(exception),
                        Level = ErrorLevel.Critical,
                        Type = (int)ErrorType.UnhandledException,
                    });
#if DEBUG
                    MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Shell.Current?.DisplayAlert("Error logged", $"msg: {exception?.Message} at {exception?.StackTrace}", "OK");
                    });
#endif
                    if (AllowCrashAnalyticsData || forceReport)
                    {
#if AppCenter
                        Crashes.TrackError(exception);
#endif
                    }

                });
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Logs an <c>AppErrorEvent</c> and reports it further to <c>AppCenter</c>,
        /// if configured.
        /// </summary>
        /// <param name="error"><c>AppErrorEvent</c></param>
        /// <param name="forceReport"><c>true</c>, to force report to <c>ApPCenter</c></param>
        /// <param name="dispatcher"><c>IDispatcher</c>, to run log action thread safe/param>
#if DEBUG
        public void LogError(AppErrorEvent error, bool forceReport = true, IDispatcher? dispatcher = null)
#else
        public void LogError(AppErrorEvent error, bool forceReport = false, IDispatcher? dispatcher = null)
#endif
        {
            // Log error
            dispatcher ??= Dispatcher;
            DispatchManager.Dispatch(dispatcher, () =>
            {
                Events.Add(error);
#if DEBUG
#if AppCenter
                Crashes.TrackError(error?.Exception);
#endif
                MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current?.DisplayAlert("Error logged", $"msg: {error.SourceName} with {error?.Message}", "OK");
                });
#endif
                if (AllowCrashAnalyticsData || forceReport)
                {
#if AppCenter
                    Crashes.TrackError(error?.Exception);
#endif
                }
            });
        }

        /// <summary>
        /// Logs an <c>AppInfoEvent</c> and reports it further to <c>AppCenter</c>,
        /// if configured.
        /// </summary>
        /// <param name="info"><c>AppInfoEvent</c></param>
        /// <param name="forceReport"><c>true</c>, to force report to <c>ApPCenter</c></param>
        /// <param name="dispatcher"><c>IDispatcher</c>, to run log action thread safe/param>
#if DEBUG
        public void LogInfo(AppInfoEvent info, bool forceReport = true, IDispatcher? dispatcher = null)
#else
        public void LogInfo(AppInfoEvent info, bool forceReport = false, IDispatcher? dispatcher = null)
#endif
        {
            try
            {
                dispatcher ??= Dispatcher;
                DispatchManager.Dispatch(dispatcher, () =>
                {
                    Events.Add(info);
#if DEBUG
                    Console.WriteLine($"Info logged: {info.Message}");
#endif
                    if (AllowAnalyticsData || forceReport)
                    {
#if AppCenter
                        Analytics.TrackEvent($"Info:{info.Message}");
#endif
                    }
                });
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Logs an <c>AppWarningEvent</c> and reports it further to <c>AppCenter</c>,
        /// if configured.
        /// </summary>
        /// <param name="warning"><c>AppWarningEvent</c></param>
        /// <param name="forceReport"><c>true</c>, to force report to <c>ApPCenter</c></param>
        /// <param name="dispatcher"><c>IDispatcher</c>, to run log action thread safe/param>
        public void LogWarning(AppWarningEvent warning, bool forceReport = false, IDispatcher? dispatcher = null)
        {
            try
            {
                dispatcher ??= Dispatcher;
                DispatchManager.Dispatch(dispatcher, () =>
                {
                    Events.Add(warning);
#if DEBUG
                    Console.WriteLine($"Warning logged: {warning.Message}");
#else
                    if (AllowAnalyticsData || forceReport)
                    {
#if AppCenter
                        Analytics.TrackEvent($"Warning:{warning.Message}");
#endif
                    }
#endif
                });
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Logs an <c>AppEvent</c> and reports it further to <c>AppCenter</c>,
        /// if configured.
        /// </summary>
        /// <param name="appEvent"><c>AppEvent</c></param>
        /// <param name="forceReport"><c>true</c>, to force report to <c>ApPCenter</c></param>
        /// <param name="dispatcher"><c>IDispatcher</c>, to run log action thread safe/param>
#if DEBUG
        public void LogEvent(AppEvent appEvent, bool forceReport = true, IDispatcher? dispatcher = null)
#else
        public void LogEvent(AppEvent appEvent, bool forceReport = false, IDispatcher? dispatcher = null)
#endif
        {
            try
            {
                dispatcher ??= Dispatcher;
                DispatchManager.Dispatch(dispatcher, () =>
                {
                    Events.Add(appEvent);
#if DEBUG
                    Console.WriteLine($"AppEvent logged: {appEvent.Message}");
#endif
                    if (AllowAnalyticsData || forceReport)
                    {
#if AppCenter
                        Analytics.TrackEvent($"Event:{appEvent.Message}");
#endif
                    }

                });
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Clears the logs
        /// </summary>
        /// <param name="dispatcher"><c>IDispatcher</c>, to run log action thread safe/param>
        public void Clear(IDispatcher? dispatcher = null)
        {
            dispatcher ??= Dispatcher;
            DispatchManager.Dispatch(dispatcher, () => Events?.Clear());
        }
        #endregion
    }
}
