#if !WPF
using AndreasReitberger.Shared.Core.Theme;
using AndreasReitberger.Shared.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using AndreasReitberger.ActiveDirectorySearch.Models.EventLogger;

namespace AndreasReitberger.ActiveDirectorySearch.Utilities
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public partial class ThemeManager : ObservableObject, IThemeManager
    {
        #region Instance
        static ThemeManager _instance = null;
        static readonly object Lock = new();
        public static ThemeManager Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                        _instance = new ThemeManager();
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
        public AppTheme Theme { get; } = AppTheme.Light;

        [ObservableProperty]
        List<ThemeColorInfo> availableThemes = new()
        {
            //new ThemeColorInfo() { ThemeName = ".NET MAUI", PrimaryColor = Color.FromArgb("#512BD4"), IsAppDefault = true },
            new ThemeColorInfo() { ThemeName = "Default", PrimaryColor = Color.FromArgb("#005b6e"), IsAppDefault = true },
            new ThemeColorInfo() { ThemeName = Colors.Gray.ToHex(), PrimaryColor = Colors.Gray },
            new ThemeColorInfo() { ThemeName = Colors.Brown.ToHex(), PrimaryColor = Colors.Brown },
            new ThemeColorInfo() { ThemeName = Colors.Green.ToHex(), PrimaryColor = Colors.Green },
            new ThemeColorInfo() { ThemeName = Colors.Red.ToHex(), PrimaryColor = Colors.Red },
            new ThemeColorInfo() { ThemeName = Colors.Orange.ToHex(), PrimaryColor = Colors.Orange },
            new ThemeColorInfo() { ThemeName = Colors.Blue.ToHex(), PrimaryColor = Colors.Blue },
            new ThemeColorInfo() { ThemeName = Colors.LightGray.ToHex(), PrimaryColor = Colors.LightGray },
            new ThemeColorInfo() { ThemeName = Colors.Teal.ToHex(), PrimaryColor = Colors.Teal },
            new ThemeColorInfo() { ThemeName = Colors.Purple.ToHex(), PrimaryColor = Colors.Purple },
            new ThemeColorInfo() { ThemeName = Colors.Pink.ToHex(), PrimaryColor = Colors.Pink },
            new ThemeColorInfo() { ThemeName = Colors.Beige.ToHex(), PrimaryColor = Colors.Beige },
            new ThemeColorInfo() { ThemeName = Colors.Violet.ToHex(), PrimaryColor = Colors.Violet },
            new ThemeColorInfo() { ThemeName = Colors.Silver.ToHex(), PrimaryColor = Colors.Silver },
            new ThemeColorInfo() { ThemeName = Colors.Gold.ToHex(), PrimaryColor = Colors.Gold },
        };

        [ObservableProperty]
        public ThemeColorInfo selectedTheme;

        public ThemeColorInfo AppDefaultTheme => AvailableThemes?.FirstOrDefault(themeInfo => themeInfo.IsAppDefault);
        #endregion

        public void ApplyTheme(AppTheme theme, Application app)
        {
            try
            {
                app.UserAppTheme = theme;
            }
            catch (Exception exc)
            {
                EventManager.Instance.LogError(exc);
            }

        }

        /// <summary>
        /// Updates the `PrimaryColor` and its dependencies in the App.Resources
        /// </summary>
        /// <param name="theme">The ThemeColorInfo to be changed to</param>
        /// <param name="app">The current app</param>
        public void UpdatePrimaryThemeColor(ThemeColorInfo theme, Application app)
        {
            try
            {
                SelectedTheme = theme;
                app.Resources["PrimaryColor"] = theme.PrimaryColor;
                app.Resources["GradientStart"] = theme.PrimaryColor;

                app.Resources["PrimaryDarkenColor"] = theme.PrimaryDarkerColor;
                app.Resources["GradientEnd"] = theme.PrimaryDarkerColor;

                app.Resources["PrimaryLighterColor"] = theme.PrimaryLigtherColor;

                if (theme.SecondaryColor != null)
                {
                    app.Resources["Secondary"] = theme.PrimaryDarkerColor;
                    app.Resources["SecondaryGradient"] = theme.PrimaryDarkerColor;
                }

#if DEBUG
                Console.WriteLine("Colors updated:\n" +
                    $"primary:      {theme.PrimaryColor?.ToHex()}\n" +
                    $"primary_darker: {theme.PrimaryDarkerColor?.ToHex()}\n" +
                    $"primary_ligther: {theme.PrimaryLigtherColor?.ToHex()}\n" +
                    $"secondary: {theme.SecondaryColor?.ToHex()}"
                    );
#endif
            }
            catch (Exception exc)
            {
                EventManager.Instance.LogError(exc);
            }
        }

        /// <summary>
        /// Updates the `PrimaryColor` for platform specific resources, as the StatusBar
        /// </summary>
        /// <param name="theme">The ThemeColorInfo to be changed to</param>
        /// <param name="app">The current app</param>
        public void UpdatePlatformThemeColor(ThemeColorInfo theme)
        {
            try
            {
                //new PlatformThemeService()?.SetStatusBarColor(theme?.PrimaryColor);
            }
            catch (Exception exc)
            {
                EventManager.Instance.LogError(exc);
            }
        }

        public ThemeColorInfo FindTheme(string primaryColorHexCode)
        {
            return AvailableThemes?.FirstOrDefault(theme => theme.PrimaryColor?.ToArgbHex() == primaryColorHexCode);// ?? AppDefaultTheme;
        }
        public ThemeColorInfo FindThemeOrDefault(string primaryColorHexCode)
        {
            return FindTheme(primaryColorHexCode) ?? AppDefaultTheme;
        }

        [Obsolete("Use the new function with `ThemeColorInfo`")]
        public void UpdatePrimaryThemeColor(string argbColorCode, Application app)
        {
            try
            {
                Color primary = Color.FromArgb(argbColorCode.StartsWith("#") ? argbColorCode : $"#{argbColorCode}");
                app.Resources["PrimaryColor"] = primary;
                app.Resources["GradientStart"] = primary;

                //Color primaryDark = primary.WithLuminosity(primary.GetLuminosity() - (primary.GetLuminosity() * .2));
                //app.Resources["PrimaryDarkColor"] = primaryDark;

                //Color gradientEnd = primary.WithLuminosity(primary.GetLuminosity() + (primary.GetLuminosity() * .2));
                //app.Resources["GradientEnd"] = gradientEnd;
#if DEBUG
                Console.WriteLine("Colors updated:\n" +
                    $"primary:      {primary.ToHex()}\n"// +
                                                        //$"primary_dark: {primaryDark.ToHex()}\n" +
                                                        //$"gradient_end: {gradientEnd.ToHex()}"
                    );
#endif
            }
            catch (Exception exc)
            {
                EventManager.Instance.LogError(exc);
            }
        }

        public Color GetThemeColorFromResource(string resourceName, Application app = null)
        {
            try
            {
                Application instance = app ?? Application.Current;
                instance.Resources.TryGetValue(resourceName, out object resource);
                if (resource is Color color)
                {
                    return color;
                }
            }
            catch (Exception exc)
            {
                EventManager.Instance.LogError(exc);
            }
            return null;
        }
    }
}
#endif