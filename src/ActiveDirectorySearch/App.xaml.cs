﻿using AndreasReitberger.Shared.Core.Theme;
using AndreasReitberger.Shared.Enums;
using CommunityToolkit.Mvvm.Messaging;
using AndreasReitberger.ActiveDirectorySearch.Models.Dispatch;
using AndreasReitberger.ActiveDirectorySearch.Models.EventLogger;
using AndreasReitberger.ActiveDirectorySearch.Models.Settings;
using AndreasReitberger.ActiveDirectorySearch.Utilities;

namespace AndreasReitberger.ActiveDirectorySearch;

public partial class App : Application
{

    #region Properties

    ThemeColorInfo _themeColor;
    public ThemeColorInfo ThemeColor
    {
        get => _themeColor;
        set
        {
            if (_themeColor == value) return;
            _themeColor = value;
        }
    }
    #endregion

    public App()
	{
		InitializeComponent();
        SettingsApp.LoadSettings();
        if (Current != null)
        {
            Console.WriteLine($"Current AppTheme: {Current.RequestedTheme}");
            string mainThemeColor = string.Empty; // SettingsApp.Theme_PrimaryThemeColor;
            ThemeColor = ThemeManager.Instance.AppDefaultTheme;
            if (!string.IsNullOrEmpty(mainThemeColor))
            {
                ThemeColor = ThemeManager.Instance.FindThemeOrDefault(mainThemeColor);
            }
            // Statusbar is done in the `OnCreate` event now
            ThemeManager.Instance.UpdatePrimaryThemeColor(ThemeColor, Current);
            Current.RequestedThemeChanged += (s, a) => {
                Console.WriteLine($"Current AppTheme: {Current.RequestedTheme}");
                if (s is App application)
                {
                    //string mainThemeColor = SettingsApp.Theme_PrimaryThemeColor;
                    application.ThemeColor = ThemeManager.Instance.AppDefaultTheme;
                    if (!string.IsNullOrEmpty(mainThemeColor))
                    {
                        application.ThemeColor = ThemeManager.Instance.FindThemeOrDefault(mainThemeColor);
                    }
                    application.ThemeColor = application.ThemeColor;
                    ThemeManager.Instance.UpdatePrimaryThemeColor(application.ThemeColor, Current);
                    ThemeManager.Instance.UpdatePlatformThemeColor(application.ThemeColor);
                }
            };
        }
        MainPage = new AppShell();
	}

    #region Overrides
    protected override void OnSleep()
    {
        base.OnSleep();
        if (SettingsApp.SettingsChanged)
        {
            DispatchManager.Dispatch(Dispatcher, SettingsApp.SaveSettingsAsync);
        }
        //WeakReferenceMessenger.Default.Send(new AppStateChangedMessage(State));
    }
    #endregion
}