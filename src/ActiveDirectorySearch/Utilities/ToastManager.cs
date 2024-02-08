using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace AndreasReitberger.ActiveDirectorySearch.Utilities
{
    public static class ToastManager
    {
        #region Methods
        public static async Task ShowToastNotificationAsync(string message, ToastDuration duration = ToastDuration.Short, double fontSize = 14, CancellationTokenSource cts = default)
        {
            cts ??= new();
            var toast = Toast.Make(message, duration, fontSize);
            await toast.Show(cts.Token);
        }
        #endregion
    }
}
