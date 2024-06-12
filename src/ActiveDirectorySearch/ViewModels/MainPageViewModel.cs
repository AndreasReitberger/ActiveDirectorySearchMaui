using AndreasReitberger.ActiveDirectorySearch.Models.Settings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using AndreasReitberger.ActiveDirectorySearch.Models.Dispatch;
using AndreasReitberger.ActiveDirectorySearch.Models.EventLogger;
using AndreasReitberger.ActiveDirectorySearch.Models.Messages;


namespace AndreasReitberger.ActiveDirectorySearch.ViewModels
{
    public partial class MainPageViewModel : AppViewModel
    {

        #region Ctor
        public MainPageViewModel(IDispatcher dispatcher, IServiceProvider provider) : base(dispatcher, provider)
        {
            Dispatcher = dispatcher;
            IsLoading = true;
            LoadSettings();
            IsLoading = false;

            RegisterMessages();
        }

        new void RegisterMessages()
        {
            base.RegisterMessages();
            if (!WeakReferenceMessenger.Default.IsRegistered<SettingsChangedMessage>(this))
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
        new void LoadSettings()
        {
            IsLoading = true;

            RefreshOnAppearing = SettingsApp.General_RefreshOnPageAppearing;
            ShowWikiUris = SettingsApp.General_ShowWikiUris;
            ShowTitleInBar = SettingsApp.General_ShowTitlesInBar;

            IsLoading = false;
        }

        #endregion

        #region Messages
        new void OnSettingsChangedMessageReceived(SettingsApp settings)
        {
            try
            {
                LoadSettings();
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }
        #endregion

        #region LifeCylce
        public void OnAppearing()
        {
            try
            {

                DispatchManager.Dispatch(Dispatcher, () => IsBusy = true);
                LoadSettings();

                IsStartingUp = false;
                IsStartUp = false;
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc, forceReport: true);
            }
            DispatchManager.Dispatch(Dispatcher, () => IsBusy = false);
        }
        #endregion
    }
}
