using CommunityToolkit.Mvvm.Messaging.Messages;
using AndreasReitberger.ActiveDirectorySearch.Models.Settings;

namespace AndreasReitberger.ActiveDirectorySearch.Models.Messages
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
#if WPF
    public partial class SettingsChangedMessage : ValueChangedMessage<object>
    {
        public SettingsChangedMessage(object settings) : base(settings)
        {

        }
    }
#else
    public partial class SettingsChangedMessage : ValueChangedMessage<SettingsApp>
    {
        public SettingsChangedMessage(SettingsApp settings) : base(settings)
        {

        }
    }
#endif
}
