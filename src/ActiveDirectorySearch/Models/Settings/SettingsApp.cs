#if !WPF
using AndreasReitberger.Maui;
using AndreasReitberger.Maui.Attributes;

namespace AndreasReitberger.ActiveDirectorySearch.Models.Settings
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public partial class SettingsApp : MauiSettings<SettingsApp>
    {

        #region Properties
        static bool _settingsChanged = false;
        public static bool SettingsChanged
        {
            get => _settingsChanged;
            set
            {
                if (_settingsChanged == value) return;
                _settingsChanged = value;
            }
        }
        #endregion

        #region DeviceSettings
        public static void OpenDeviceSettings()
        {
            AppInfo.ShowSettingsUI();
        }
        #endregion

        #region Settings

        #region Version
        [MauiSetting(Name = nameof(App_SettingsVersion), DefaultValue = "1.0.0")]
        public static Version App_SettingsVersion { get; set; }

        #endregion

        #region URobo
        [MauiSetting(Name = nameof(UR_ScrewsLeft), DefaultValue = 0)]
        public static int UR_ScrewsLeft { get; set; }

        [MauiSetting(Name = nameof(UR_ScrewChargeNumber), DefaultValue = "")]
        public static string UR_ScrewChargeNumber { get; set; }

        [MauiSetting(Name = nameof(UR_LastOrderBC), DefaultValue = "")]
        public static string UR_LastOrderBC { get; set; }
        #endregion

        #region Theme 
        [MauiSetting(Name = nameof(Theme_UseDeviceDefaultSettings), DefaultValue = SettingsStaticDefault.General_UseDeviceSettings)]
        public static bool Theme_UseDeviceDefaultSettings { get; set; }

        [MauiSetting(Name = nameof(Theme_UseDarkTheme), DefaultValue = SettingsStaticDefault.General_UseDarkTheme)]
        public static bool Theme_UseDarkTheme { get; set; }

        [MauiSetting(Name = nameof(Theme_PrimaryThemeColor), DefaultValue = SettingsStaticDefault.Theme_PrimaryThemeColor)]
        public static string Theme_PrimaryThemeColor { get; set; }

        [MauiSetting(Name = nameof(Theme_ShowPrintProgressInTitleView), DefaultValue = SettingsStaticDefault.General_ShowPrintProgressInTitleView)]
        public static bool Theme_ShowPrintProgressInTitleView { get; set; }

        [MauiSetting(Name = nameof(Theme_ShowRemainingPrintTimeInTitleView), DefaultValue = SettingsStaticDefault.General_ShowRemainingPrintTimeInTitleView)]
        public static bool Theme_ShowRemainingPrintTimeInTitleView { get; set; }

        [MauiSetting(Name = nameof(Theme_AppEntryPoint), DefaultValue = nameof(MainPage))]
        public static string Theme_AppEntryPoint { get; set; }

        #endregion

        #region General
        [MauiSetting(Name = nameof(General_IsFirstStart), DefaultValue = true)]
        public static bool General_IsFirstStart { get; set; }

        [MauiSetting(Name = nameof(General_RefreshOnPageAppearing), DefaultValue = SettingsStaticDefault.General_RefreshOnAppearing)]
        public static bool General_RefreshOnPageAppearing { get; set; }

        [MauiSetting(Name = nameof(General_ShowWikiUris), DefaultValue = SettingsStaticDefault.General_ShowWikiUris)]
        public static bool General_ShowWikiUris { get; set; }

        [MauiSetting(Name = nameof(General_ShowTitlesInBar), DefaultValue = SettingsStaticDefault.General_ShowTitlesInBar)]
        public static bool General_ShowTitlesInBar { get; set; }

        [MauiSetting(Name = nameof(General_ExpandTopView), DefaultValue = SettingsStaticDefault.General_ExpandTopView)]
        public static bool General_ExpandTopView { get; set; }

        [MauiSetting(Name = nameof(General_KeepFlyoutOpen), DefaultValue = SettingsStaticDefault.General_KeepFlyoutOpen)]
        public static bool General_KeepFlyoutOpen { get; set; }

        #endregion

        #region Localization

        [MauiSetting(Name = nameof(Localization_CultureCode), DefaultValue = SettingsStaticDefault.Localization_Default)]
        public static string Localization_CultureCode { get; set; }

        #endregion

        #region Servers

        [MauiSetting(Name = nameof(Servers), DefaultValue = "")]
        public static string Servers { get; set; }

        [MauiSetting(Name = nameof(PrintServer_LastUsedServer), DefaultValue = "00000000-0000-0000-0000-000000000000")]
        public static Guid PrintServer_LastUsedServer { get; set; }

        #endregion

        #endregion

        #region Methods

        public static SettingsApp Copy()
        {
            SettingsApp app = new();
            app.LoadObjectSettings();
            return app;
        }

        #endregion
    }
}
#endif