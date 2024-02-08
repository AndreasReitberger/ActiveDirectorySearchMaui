using AndreasReitberger.Shared.Core;

namespace AndreasReitberger.ActiveDirectorySearch.Models.Settings
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public static class SettingsStaticDefault
    {
        #region App
        public const bool App_IsBeta = false;
        public const bool App_IsLightVersion = false;
        public const string App_SupportMail = "andreas_reitberger@zollner.de";
        public const string App_Secret = "URoboControlProgram";
        #endregion

        #region Localization
        public const string Localization_Default = "";  //en-US
        #endregion

        #region General
        public const bool General_UseDarkTheme = true;
        public const string Theme_PrimaryThemeColor = "";
        public const bool Theme_AutoTabletMode = false;
        public const bool Theme_EnableTabletModeManually = true;

        public const bool General_UseDeviceSettings = true;
        public const bool General_ShowPrintProgressInTitleView = true;
        public const bool General_ShowRemainingPrintTimeInTitleView = true;
        public const bool General_ShowOnlyWebCamInLandscape = true;
        public const bool DemoMode_Enabled = false;
        public const bool General_RefreshOnAppearing = false;
        public const bool General_ShowWikiUris = true;
        public const bool General_ShowTitlesInBar = false;
        public const bool General_ExpandTopView = true;
        public const bool General_KeepFlyoutOpen = false;

        public const int General_GaugeHeight = 175;
        public const int General_GaugeHeightPrinting = 175;
        public const int General_MaxGaugeHeigh = 500;

        public const int General_GaugeHeightTablet = 300;
        public const int General_GaugeHeightPrintingTablet = 300;
        public const int General_MaxGaugeHeighTablet = 1000;
        #endregion

#region Theme
#if !WPF
        public static List<ColorPickerElement> DefaultColors = new()
        {
            // Custom
            new ColorPickerElement() { Name = "Klipper", ChipColor = Color.FromArgb("B02F35") },
            new ColorPickerElement() { Name = "Fluidd", ChipColor = Color.FromArgb("2096F3") },
            new ColorPickerElement() { Name = "#E6E2AF", ChipColor = Color.FromArgb("E6E2AF") },
            new ColorPickerElement() { Name = "#225378", ChipColor = Color.FromArgb("225378") },
            new ColorPickerElement() { Name = "#1695A3", ChipColor = Color.FromArgb("1695A3") },
            new ColorPickerElement() { Name = "#468966", ChipColor = Color.FromArgb("468966") },
            new ColorPickerElement() { Name = "#2C3E50", ChipColor = Color.FromArgb("2C3E50") },
            new ColorPickerElement() { Name = "#E74C3C", ChipColor = Color.FromArgb("E74C3C") },
            new ColorPickerElement() { Name = "#FF6138", ChipColor = Color.FromArgb("FF6138") },
            new ColorPickerElement() { Name = "#00A388", ChipColor = Color.FromArgb("00A388") },
            new ColorPickerElement() { Name = "#3E606F", ChipColor = Color.FromArgb("3E606F") },
            new ColorPickerElement() { Name = "#D1DBBD", ChipColor = Color.FromArgb("D1DBBD") },
            // Stock
            new ColorPickerElement() { Name = Colors.Black.ToHex(), ChipColor = Colors.Black },
            new ColorPickerElement() { Name = Colors.White.ToHex(), ChipColor = Colors.White },
            new ColorPickerElement() { Name = Colors.Gray.ToHex(), ChipColor = Colors.Gray },
            new ColorPickerElement() { Name = Colors.Brown.ToHex(), ChipColor = Colors.Brown },
            new ColorPickerElement() { Name = Colors.Green.ToHex(), ChipColor = Colors.Green },
            new ColorPickerElement() { Name = Colors.Red.ToHex(), ChipColor = Colors.Red },
            new ColorPickerElement() { Name = Colors.Orange.ToHex(), ChipColor = Colors.Orange },
            new ColorPickerElement() { Name = Colors.Blue.ToHex(), ChipColor = Colors.Blue },
            new ColorPickerElement() { Name = Colors.LightGray.ToHex(), ChipColor = Colors.LightGray },
            new ColorPickerElement() { Name = Colors.Teal.ToHex(), ChipColor = Colors.Teal },
            new ColorPickerElement() { Name = Colors.Purple.ToHex(), ChipColor = Colors.Purple },
            new ColorPickerElement() { Name = Colors.Pink.ToHex(), ChipColor = Colors.Pink },
            new ColorPickerElement() { Name = Colors.Beige.ToHex(), ChipColor = Colors.Beige },
            new ColorPickerElement() { Name = Colors.Violet.ToHex(), ChipColor = Colors.Violet },
            new ColorPickerElement() { Name = Colors.Silver.ToHex(), ChipColor = Colors.Silver },
            new ColorPickerElement() { Name = Colors.Gold.ToHex(), ChipColor = Colors.Gold },
            /**/
        };
#endif
        #endregion
    }
}
