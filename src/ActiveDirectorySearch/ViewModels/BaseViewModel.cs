#if WPF
using System.Reflection;
using MahApps.Metro.Controls.Dialogs;
#else
using AndreasReitberger.ActiveDirectorySearch.Models.NavigationManager;
using AndreasReitberger.ActiveDirectorySearch.Models.Settings;
#endif
using AndreasReitberger.Shared.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using AndreasReitberger.ActiveDirectorySearch.Models.EventLogger;
using AndreasReitberger.ActiveDirectorySearch.Models.Messages;
using AndreasReitberger.ActiveDirectorySearch.Utilities;

namespace AndreasReitberger.ActiveDirectorySearch.ViewModels
{
    //[QueryProperty(nameof(Filter), "filter")]
    public partial class BaseViewModel : ViewModelBase
    {
#if WPF
        #region Mahapps
        [ObservableProperty]
        IDialogCoordinator dialogCoordinator;
        #endregion
#endif

        #region Properties

        #region App
        [ObservableProperty]
        string version = string.Empty;

        [ObservableProperty]
        string build = string.Empty;

        #endregion

        #region Shell
        [ObservableProperty]
        bool keepFlyoutOpen = false;
#if !WPF
#if WINDOWS
        public FlyoutBehavior FlyoutBehavior => FlyoutBehavior.Locked;
#else
        public FlyoutBehavior FlyoutBehavior => KeepFlyoutOpen ? FlyoutBehavior.Locked : FlyoutBehavior.Flyout;
#endif
#endif
        #endregion

        #region Navigation

        [ObservableProperty]
        bool isViewShown = false;
        #endregion

        #region Connection

        [ObservableProperty]
        bool isConnecting = false;
        #endregion

        #region WebSocket

        [ObservableProperty]
        bool isListening = false;

        [ObservableProperty]
        bool isListeningToWebsocket = false;

        [ObservableProperty]
        bool closeWebSocketWhenLeaving = false;
        #endregion

        #region Module

        [ObservableProperty]
        bool timerActive = false;

        [ObservableProperty]
        bool isAppStartupCompleted = false;

        [ObservableProperty]
        bool hasRestApiError = false;

        [ObservableProperty]
        bool refreshOnAppearing = true;

        [ObservableProperty]
        bool showTitleInBar = false;

        [ObservableProperty]
        bool showWikiUris = true;

        #endregion

        #endregion

        #region Constructor
#if WPF
        public BaseViewModel(IDialogCoordinator dialogCoordinator)
        {
            DialogCoordinator = dialogCoordinator;
#else
            public BaseViewModel(IDispatcher dispatcher) : base(dispatcher)
        {
            Dispatcher = dispatcher;
#endif
            UpdateVersionBuild();
            RegisterMessages();
        }

        protected void RegisterMessages()
        {
            if (!WeakReferenceMessenger.Default.IsRegistered<SettingsChangedMessage>(this))
            {
                WeakReferenceMessenger.Default.Register<SettingsChangedMessage>(this, (r, m) =>
                {
                    try
                    {
                        EventManager.Instance.LogEvent(new()
                        {
                            Message = $"WeakReferenceMessenger received from {r}: (message =>  {m})",
                            SourceName = nameof(SettingsChangedMessage),
                        });
                        OnSettingsChangedMessageReceived(m.Value);
                    }
                    catch (Exception exc)
                    {
                        // Log error
                        EventManager.Instance.LogError(exc);
                    }
                });
            }
#if !WPF
            if (!WeakReferenceMessenger.Default.IsRegistered<ConnectivityChangedMessage>(this))
            {
                WeakReferenceMessenger.Default.Register<ConnectivityChangedMessage>(this, (r, m) =>
                {
                    try
                    {
                        EventManager.Instance.LogEvent(new()
                        {
                            Message = $"WeakReferenceMessenger received from {r}: (message =>  {m})",
                            SourceName = nameof(ConnectivityChangedMessage),
                        });
                        OnConnectivityChangedMessageReceived(m.Value);
                    }
                    catch (Exception exc)
                    {
                        // Log error
                        EventManager.Instance.LogError(exc);
                    }
                });
            }
#endif
        }

        protected void LoadSettings()
        {
            IsLoading = true;
#if !WPF
            ShowWikiUris = SettingsApp.General_ShowWikiUris;
            ShowTitleInBar = SettingsApp.General_ShowTitlesInBar;
#endif
            IsLoading = false;
        }
        #endregion

        #region Destructor
        ~BaseViewModel()
        {
            // Unregisters all when the ViewModel is GC.
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
        #endregion

        #region Messages
#if WPF
        protected void OnSettingsChangedMessageReceived(object settings)
#else
        protected void OnSettingsChangedMessageReceived(SettingsApp settings)
#endif
        {
            try
            {
                _ = settings;
                LoadSettings();
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }
#if !WPF
        protected void OnConnectivityChangedMessageReceived(ConnectivityChangedEventArgs args)
        {
            try
            {
                if (args != null)
                {
                    /*
                    WifiOn = args.ConnectionProfiles.Contains(ConnectionProfile.WiFi);
                    EventManager.Instance.LogEvent(new AppEvent()
                    {
                        SourceName = nameof(BaseViewModel),
                        Message = $"App: Connectivity has changed: wifi available: {WifiOn}",
                    });
                    */
                }
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }
#endif
        #endregion

        #region Commands

        #region Navigation
#if !WPF
        [RelayCommand]
        protected Task Close() => Back();

        [RelayCommand]
        protected Task Back() => ShellNavigator.Instance.GoBackAsync(Dispatcher, false);

        [RelayCommand]
        protected void GoToDeviceSettings() => SettingsApp.OpenDeviceSettings();
#endif
        #endregion

        [RelayCommand]
        protected async Task OpenUri(object parameter)
        {
            try
            {
                string str = string.Empty;
                if (parameter is Uri uri)
                {
                    str = uri.ToString();
                }
                else if (parameter is string uriString)
                {
                    str = uriString;
                }
                if (!string.IsNullOrEmpty(str))
                {
#if WPF
                    _ = await ConfirmExternalUriHelper.OpenExternalUriAsync(str, DialogCoordinator, this);
#else
                    _ = await ConfirmExternalUriHelper.OpenExternalUriAsync(str);
#endif
                }
            }
            catch (Exception exc)
            {
                //Log error
                EventManager.Instance.LogError(exc);
            }
        }
#endregion

        #region Methods

        protected void UpdateVersionBuild()
        {
            try
            {
#if WPF
                Version = Build = Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? string.Empty;
#else
                Version = AppInfo.VersionString;
                Build = AppInfo.BuildString;
#endif
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }
#endregion

        #region LiveCycle
        /*
        public async Task OnAppearingAsync()
        {

        }
        */

        public void OnDisappearing()
        {
            try
            {
#if !WPF
                if (SettingsApp.SettingsChanged)
                {
                    // Notify other modules
                    WeakReferenceMessenger.Default.Send(new SettingsChangedMessage(SettingsApp.SettingsObject));
                    SettingsApp.SaveSettings();
                    SettingsApp.SettingsChanged = false;
                }
#endif
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }
#endregion
    }
}
