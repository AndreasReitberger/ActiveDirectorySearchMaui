#if !WPF
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AndreasReitberger.ActiveDirectorySearch.Models.Messages
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public partial class ConnectivityChangedMessage : ValueChangedMessage<ConnectivityChangedEventArgs>
    {
        public ConnectivityChangedMessage(ConnectivityChangedEventArgs connectionState) : base(connectionState)
        {

        }
    }
}
#endif