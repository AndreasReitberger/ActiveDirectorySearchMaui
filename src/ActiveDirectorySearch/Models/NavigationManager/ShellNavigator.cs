using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Shared.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using AndreasReitberger.ActiveDirectorySearch.Enums;
using AndreasReitberger.ActiveDirectorySearch.Models.Dispatch;
using AndreasReitberger.ActiveDirectorySearch.Models.EventLogger;
using AndreasReitberger.ActiveDirectorySearch.Resources.Localization;

namespace AndreasReitberger.ActiveDirectorySearch.Models.NavigationManager
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public partial class ShellNavigator : BaseModel, IShellNavigator
    {
        #region Instance
        static ShellNavigator _instance = null;
        static readonly object Lock = new();
        public static ShellNavigator Instance
        {
            get
            {
                lock (Lock)
                {
                    _instance ??= new ShellNavigator();
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

        public string CurrentRoute => GetCurrentRoute();

        [ObservableProperty]
        string previousRoute = string.Empty;

        [ObservableProperty]
        string rootPage = string.Empty;

        [ObservableProperty]
        List<string> availableEntryPages = new()
        {
            nameof(MainPage),
        };

        #endregion

        #region Constructor
        public ShellNavigator()
        {
            RootPage = nameof(MainPage);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Performs a navigation to the provided target.
        /// </summary>
        /// <param name="target">The name of the target route</param>
        /// <param name="flyoutIsPresented">Whether the flyout is kept open</param>
        /// <param name="delay">A delay in ms for the navigation</param>
        /// <param name="animate">Whether to animate the navigation</param>
        /// <returns><c>Task</c></returns>
        public Task<bool> GoToAsync(IDispatcher dispatcher, string target, bool flyoutIsPresented = false, int delay = -1, bool animate = true)
            => GoToAsync(dispatcher, target: target, parameters: null, flyoutIsPresented: flyoutIsPresented, delay: delay, animate: animate);

        /// <summary>
        /// Performs a navigation to the provided target.
        /// </summary>
        /// <param name="target">The name of the target route</param>
        /// <param name="flyoutIsPresented">Whether the flyout is kept open</param>
        /// <param name="delay">A delay in ms for the navigation</param>
        /// <param name="animate">Whether to animate the navigation</param>
        /// <returns><c>Task</c></returns>
        public Task<bool> GoToAsync(IDispatcher dispatcher, ShellRoute target, bool flyoutIsPresented = false, int delay = -1, bool animate = true)
            => GoToAsync(dispatcher, target.ToString(), null, flyoutIsPresented, delay, animate);

        /// <summary>
        /// Performs a navigation to the provided target.
        /// </summary>
        /// <param name="target">The name of the target route</param>
        /// <param name="parameters">Query parameters passed to the navigated route</param>
        /// <param name="flyoutIsPresented">Whether the flyout is kept open</param>
        /// <param name="delay">A delay in ms for the navigation</param>
        /// <param name="animate">Whether to animate the navigation</param>
        /// <returns><c>Task</c></returns>
        public Task<bool> GoToAsync(IDispatcher dispatcher, ShellRoute target, Dictionary<string, object> parameters, bool flyoutIsPresented = false, int delay = -1, bool animate = true)
            => GoToAsync(dispatcher, target.ToString(), parameters, flyoutIsPresented, delay, animate);

        /// <summary>
        /// Performs a navigation to the provided target.
        /// </summary>
        /// <param name="target">The name of the target route</param>
        /// <param name="parameters">Query parameters passed to the navigated route</param>
        /// <param name="flyoutIsPresented">Whether the flyout is kept open</param>
        /// <param name="delay">A delay in ms for the navigation</param>
        /// <param name="animate">Whether to animate the navigation</param>
        /// <returns><c>Task</c></returns>
        public async Task<bool> GoToAsync(IDispatcher dispatcher, string target, Dictionary<string, object> parameters, bool flyoutIsPresented = false, int delay = -1, bool animate = true)
        {
            try
            {
                if (Shell.Current.FlyoutBehavior == FlyoutBehavior.Flyout)
                {
                    Shell.Current.FlyoutIsPresented = flyoutIsPresented;
                }
                if (delay != -1)
                {
                    await Task.Delay(delay);
                }
                // Workaround for #13510 - https://github.com/xamarin/Xamarin.Forms/issues/13510
                else if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    await Task.Delay(50);
                }

                //await DispatchManager.UpdateUIThreadSaveAsync(async () =>
                await DispatchManager.DispatchAsync(dispatcher, async () =>
                {
                    try
                    {
                        PreviousRoute = GetCurrentRoute();
                        if (parameters == null)
                            await Shell.Current.GoToAsync(state: target, animate: animate);
                        else
                            await Shell.Current.GoToAsync(state: target, parameters: parameters, animate: animate); ;
                    }
                    catch (Exception exc)
                    {
                        EventManager.Instance.LogError(exc);
                    }
                });
                return true;
            }
            catch (Exception exc)
            {
                // Log error
                EventManager.Instance.LogError(exc);
                return false;
            }
        }

        /// <summary>
        /// Navigates back one route from the current navigation stack.
        /// </summary>
        /// <param name="flyoutIsPresented">Whether the flyout is kept open</param>
        /// <param name="delay">A delay in ms for the navigation</param>
        /// <param name="animate">Whether to animate the navigation</param>
        /// <param name="confirm">Whether to confirm the navigation</param>
        /// <returns><c>Task</c></returns>
        public Task GoBackAsync(IDispatcher dispatcher, bool flyoutIsPresented = false, int delay = -1, bool animate = true, bool confirm = false)
            => GoBackAsync(dispatcher, null, flyoutIsPresented: flyoutIsPresented, delay: delay, animate: animate, confirm: confirm);

        /// <summary>
        /// Navigates back one route from the current navigation stack.
        /// </summary>
        /// <param name="parameters">Query parameters passed to the navigated route</param>
        /// <param name="flyoutIsPresented">Whether the flyout is kept open</param>
        /// <param name="delay">A delay in ms for the navigation</param>
        /// <param name="animate">Whether to animate the navigation</param>
        /// <param name="confirm">Whether to confirm the navigation</param>
        /// <returns></returns>
        public async Task GoBackAsync(IDispatcher dispatcher, Dictionary<string, object> parameters, bool flyoutIsPresented = false, int delay = -1, bool animate = true, bool confirm = false)
        {
            if (confirm)
            {
                bool result = await Shell.Current.DisplayAlert(
                        Strings.DialogConfirmGoBackHeadline,
                        Strings.DialogConfirmGoBackContent,
                        Strings.GoBack,
                        Strings.Close
                        );

                if (result)
                {
                    _ = await GoToAsync(dispatcher, "..", parameters, flyoutIsPresented, delay, animate);
                }
            }
            else
            {
                _ = await GoToAsync(dispatcher, "..", parameters, flyoutIsPresented, delay, animate);
            }
        }

        /// <summary>
        /// Returns true if the current path (route) is the root page
        /// </summary>
        /// <returns><c>true</c> if current path is root</returns>
        public bool IsCurrentPathRoot()
        {
            try
            {
                string[] parts = Shell.Current.CurrentState.Location.OriginalString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                return parts.Length <= 1;
            }
            catch (Exception exc)
            {
                EventManager.Instance.LogError(exc);
                return false;
            }
        }

        /// <summary>
        /// Returns the current route as <c>string</c>
        /// </summary>
        /// <returns>Current route as <c>string</c></returns>
        string GetCurrentRoute()
        {
            try
            {
                ShellNavigationState state = Shell.Current.CurrentState;
                if (state == null) return string.Empty;

                string lastPart = state.Location.ToString().Split('/').Where(x => !string.IsNullOrWhiteSpace(x)).LastOrDefault();
                return lastPart;
            }
            catch (Exception exc)
            {
                EventManager.Instance.LogError(exc);
                return string.Empty;
            }
        }

        #endregion


        #region RegisterRoutes

        public void RegisterRoutes()
        {

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));           
        }

        #endregion
    }
}
