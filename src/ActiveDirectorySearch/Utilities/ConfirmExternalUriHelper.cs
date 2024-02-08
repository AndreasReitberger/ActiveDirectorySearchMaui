#if WPF
using MahApps.Metro.Controls.Dialogs;
using System.Diagnostics;
#endif
using AndreasReitberger.ActiveDirectorySearch.Models.EventLogger;

namespace AndreasReitberger.ActiveDirectorySearch.Utilities
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public static class ConfirmExternalUriHelper
    {
#if WPF
        public static async Task<bool> OpenExternalUriAsync(Uri uri, IDialogCoordinator dialogCoordinator, object context, bool confirm = true)
#else
        public static async Task<bool> OpenExternalUriAsync(Uri uri, bool confirm = true)
#endif
        {
            try
            {
                if (uri != null)
                {
                    bool res = true;
                    if (confirm)
                    {
#if WPF
                        var result = await dialogCoordinator.ShowMessageAsync(context,
                            "Open external url?",
                            string.Format("Do you want to open the external url `{0}`?", uri.ToString()),
                            MessageDialogStyle.AffirmativeAndNegative,
                            new MetroDialogSettings() {
                            AffirmativeButtonText = "OK",
                            NegativeButtonText = "Cancel"
                            }
                        );
#else
                        res = await Shell.Current.DisplayAlert(
                            "Open external url?",
                            string.Format("Do you want to open the external url `{0}`?", uri.ToString()),
                            "OK",
                            "Cancel"
                        );
#endif
                    }
                    if (res)
                    {
#if WPF
                        new Process
                        {
                            StartInfo = new ProcessStartInfo(uri.ToString())
                            {
                                UseShellExecute = true
                            }
                        }.Start();
#else
                        await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
#endif
                        EventManager.Instance.LogEvent(new()
                        {
                            Message = $"External uri opened: '{uri.OriginalString}'",
                        });
                        return true;
                    }
                }
            }
            catch (Exception exc)
            {
                EventManager.Instance.LogError(exc);
            }
            return false;
        }

#if WPF
        public static async Task<bool> OpenExternalUriAsync(string uri, IDialogCoordinator dialogCoordinator, object context, bool confirm = true)
#else
        public static async Task<bool> OpenExternalUriAsync(string uri, bool confirm = true)
#endif
        {
            Uri link = new(uri);
#if WPF
            return await OpenExternalUriAsync(link, dialogCoordinator, context, confirm);
#else
            return await OpenExternalUriAsync(link, confirm);
#endif
        }
    }
}
