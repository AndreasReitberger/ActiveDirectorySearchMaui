using AndreasReitberger.ActiveDirectorySearch.Enums;
using AndreasReitberger.ActiveDirectorySearch.Models.ActiveDirectory;
using AndreasReitberger.ActiveDirectorySearch.Models.Dispatch;
using AndreasReitberger.ActiveDirectorySearch.Models.Documentation;
using AndreasReitberger.ActiveDirectorySearch.Models.EventLogger;
using AndreasReitberger.ActiveDirectorySearch.Resources.Localization;
using AndreasReitberger.ActiveDirectorySearch.Utilities;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace AndreasReitberger.ActiveDirectorySearch.ViewModels
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public partial class AppViewModel : BaseViewModel
    {

        #region Properties

        #region General

        #region States

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RefreshCommand))]
        bool isRefreshing = false;

        [ObservableProperty]
        bool isLoadingAll = false;

        [ObservableProperty]
        bool isLoadingFiles = false;

        [ObservableProperty]
        bool dataChanged = true;

        #endregion

        #region Tutorial

        [ObservableProperty]
        bool tutorialShown = false;

        #endregion

        #endregion

        #region Active Directory

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoadUsersFromAdCommand))]
        string searchText = string.Empty;
        partial void OnSearchTextChanged(string value) => Task.Run(() => LoadUsersFromAd(value));

        #region Collections

        [ObservableProperty]
        ObservableCollection<ActiveDirectoryUser> users = [];
        #endregion

        #endregion

        #region Actions
        /// <summary>
        /// This action will be invoked if data has changed.
        /// </summary>
        protected Func<Task> OnDataChanged;
        #endregion

        #endregion

        #region Constructor

        public AppViewModel(IDispatcher dispatcher, IServiceProvider provider) : base(dispatcher)
        {
            Dispatcher = dispatcher;
            RegisterMessages();

            OnDataChanged = Refresh;
        }
        #endregion

        #region Commands

        #region Logbook
        bool LoadUsersFromAdCommand_CanExcecute() => SearchText?.Length > 2;
        [RelayCommand(CanExecute = nameof(LoadUsersFromAdCommand_CanExcecute))]
        protected void LoadUsersFromAd(object parameter)
        {
            try
            {
                string searchPattern = string.Empty;
                if (parameter is string wildcard && wildcard.Length > 2)
                    searchPattern = wildcard;
                else return;

                IsBusyCounter++;
                List<ActiveDirectoryUser> users = ActiveDirectoryManager.GetUsersFromActiveDirectory(searchPattern);
                DispatchManager.Dispatch(Dispatcher, () =>
                {
                    Users = [.. users?
                        .Where(user => (!string.IsNullOrEmpty(user?.Email) || !string.IsNullOrEmpty(user?.Phone))
                        // Filter for specific user names
                        //&& (user?.UserName?.ToLower()?.StartsWith("q") is true || user?.UserName?.ToLower()?.StartsWith("s") is true)
                        )];
                });
                IsBusyCounter--;
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }

        #endregion

        #region Refresh
        bool RefreshCommand_CanExcecute() => !IsRefreshing;
        [RelayCommand(CanExecute = nameof(RefreshCommand_CanExcecute))]
        async Task Refresh()
        {
            try
            {
                DispatchManager.Dispatch(Dispatcher, () =>
                {
                    IsRefreshing = true;
                });
                await Task.Run(() => LoadUsersFromAd(SearchText));
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
            DispatchManager.Dispatch(Dispatcher, () => IsRefreshing = false);
        }
        #endregion

        #region Templates
        [RelayCommand]
        async Task CopyPhone(object parameter)
        {
            try
            {
                bool copied = false;
                if (parameter is string phone)
                {
                    await Clipboard.SetTextAsync(phone);
                    copied = true;
                }
                else if (parameter is ActiveDirectoryUser user)
                {
                    await Clipboard.SetTextAsync(user.Phone);
                    copied = true;
                }
                if (copied)
                {
                    await ToastManager.ShowToastNotificationAsync(Strings.CopiedToClipboard);
                }
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }

        [RelayCommand]
        async Task CopyMail(object parameter)
        {
            try
            {
                bool copied = false;
                if(parameter is string mail)
                {
                    await Clipboard.SetTextAsync(mail);
                    copied = true;
                }
                else if (parameter is ActiveDirectoryUser user)
                {
                    await Clipboard.SetTextAsync(user.Email);
                    copied = true;
                }
                if (copied)
                {
                    await ToastManager.ShowToastNotificationAsync(Strings.CopiedToClipboard);
                }
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }

        [RelayCommand]
        async Task OpenInTeams(object parameter)
        {
            try
            {
                if(parameter is string mail)
                {
                    Process.Start($"msteams:l/chat/0/0?users={mail}&message=Hello");
                }
                else if (parameter is ActiveDirectoryUser user)
                {
                    bool chat = await Shell.Current.DisplayAlert(
                        Strings.DialogOpenChatOrCallTeamsHeadline,
                        string.Format(Strings.DialogOpenChatOrCallTeamsFormatContent, $"{user.Lastname}, {user.Firstname}"),
                        Strings.Chat,
                        Strings.Call
                        );
                    var processStartInfo = new ProcessStartInfo
                    {
                        FileName = chat ? $"msteams:l/chat/0/0?users={user.Email}" : $"msteams:l/call/0/0?users={user.Email}",
                        UseShellExecute = true
                    };
                    Process.Start(processStartInfo);
                }
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }
        #endregion

        #region Wiki

        [RelayCommand]
        protected async Task OpenWikiUri(object paramter)
        {
            try
            {
                if (paramter is DocumentationIdentifier documentationIdentifier)
                {
                    string str = DocumentationManager.CreateUrl(documentationIdentifier);
#if WPF
                    _ = await ConfirmExternalUriHelper.OpenExternalUriAsync(str, DialogCoordinator, this);
#else
                    _ = await ConfirmExternalUriHelper.OpenExternalUriAsync(str);
#endif

                    EventManager.Instance.LogInfo(new()
                    {
#if WPF
                        Message = $"{Application.ResourceAssembly.GetName().Name}: Wiki URL opened: '{documentationIdentifier}'",
#else
                        Message = $"{AppInfo.Current.Name}: Wiki URL opened: '{documentationIdentifier}'",
#endif
                        SourceName = nameof(OpenWikiUri),
                    });
                }
            }
            catch (Exception exc)
            {
                // Log Error
                EventManager.Instance.LogError(exc);
            }
        }
        #endregion

        #endregion

        #region Methods

        #region LoadSettings
        protected new void LoadSettings()
        {
            base.LoadSettings();
            IsLoading = true;

            IsLoading = false;
        }
#endregion

        #region DataLoading
        /// <summary>
        /// This is the default `Pages_Loaded` event. Nothing happens here. If needed, overwrite it in the 
        /// inherited view model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Pages_Loaded(object sender, EventArgs e)
        {
            //OnDefault, do noting.
            Task.Run(OnLoadedAsync);
        }

        /// <summary>
        /// Loads the data needed for this input form modal.
        /// Attention! This code runs not on the UI Thread!
        /// </summary>
        /// <returns></returns>
        protected async Task OnLoadDataAsync()
        {
            try
            {
                await Task.Delay(100);
                LoadUsersFromAd(SearchText);
#if WPF
                await DispatchManager.UpdateUIThreadSaveAsync(() =>
#else
                await DispatchManager.DispatchAsync(Dispatcher, () =>
#endif
                {
                    UpdateFromInstanceData();
                    EventManager.Instance.LogInfo(new()
                    {
#if WPF
                        Message = $"{Application.ResourceAssembly.GetName().Name}: Client data refreshed from server at {DateTime.Now}.",
#else
                        Message = $"{AppInfo.Current.Name}: Client data refreshed from server at {DateTime.Now}.",
#endif
                        SourceName = nameof(OnLoadDataAsync),
                    });
                    IsLoadingAll = false;
                });
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }

        /// <summary>
        /// Updates the needed information from the RepetierClient.Instance.
        /// Important: This must run on the UIThread!
        /// </summary>
        protected void UpdateFromInstanceData()
        {
            try
            {

            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
            }
        }
        #endregion

        #endregion

        #region LifeCycle

        public async Task OnLoadedAsync()
        {
            try
            {
                if (DataChanged)
                {
                    //await RepetierClient.Instance.RefreshAllAsync();
                    await Task.Delay(100);
                    LoadUsersFromAd(SearchText);
                    EventManager.Instance.LogInfo(new()
                    {
#if WPF
                        Message = $"{Application.ResourceAssembly.GetName().Name}: {nameof(OnLoadedAsync)} was triggered at {nameof(AppViewModel)} and a refresh was necessary.",
#else
                        Message = $"{AppInfo.Current.Name}: {nameof(OnLoadedAsync)} was triggered at {nameof(AppViewModel)} and a refresh was necessary.",
#endif
                        SourceName = nameof(OnLoadedAsync),
                    });
                }
                else
                {
#if WPF
                    DispatchManager.UpdateUIThreadSave(() =>
#else
                    DispatchManager.Dispatch(Dispatcher, () =>
#endif
                    {
                        UpdateFromInstanceData();
                        EventManager.Instance.LogInfo(new()
                        {
#if WPF
                            Message = $"{Application.ResourceAssembly.GetName().Name}: {nameof(OnLoadedAsync)} was triggered at {nameof(AppViewModel)}, however the data was still valid.",
#else
                            Message = $"{AppInfo.Current.Name}: {nameof(OnLoadedAsync)} was triggered at {nameof(AppViewModel)}, however the data was still valid.",
#endif
                            SourceName = nameof(OnLoadedAsync),
                        });
                    });
                }
                EventManager.Instance.LogInfo(new()
                {
#if WPF
                    Message = $"{Application.ResourceAssembly.GetName().Name}: {nameof(OnLoadedAsync)} called from {nameof(AppViewModel)}.",
#else
                    Message = $"{AppInfo.Current.Name}: {nameof(OnLoadedAsync)} called from {nameof(AppViewModel)}.",
#endif
                    SourceName = nameof(OnLoadedAsync),
                });
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
