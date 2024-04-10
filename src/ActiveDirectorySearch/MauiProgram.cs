using AndreasReitberger.Shared.Core.Services;
using AndreasReitberger.Shared.Hosting;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using AndreasReitberger.ActiveDirectorySearch.Hosting;

namespace AndreasReitberger.ActiveDirectorySearch;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialDesignIcons" );
			})
            .InitializeSharedMauiStyles()
            .ConfigureApp()
            .ConfigureDispatching() //https://github.com/dotnet/maui/blob/main/src/Core/src/Hosting/Dispatching/AppHostBuilderExtensions.cs
            .ConfigureLifecycleEvents(events =>
            {
                /*
                events.AddWindows(win => win.OnWindowCreated((window) =>
                {
                    string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    var iniFile = new IniFile(Path.Combine(currentDirectory, @"Resources\config.ini"));
                }));
                */
#if WINDOWS
                events.AddWindows(win => win.OnLaunched((window, args) =>
                {
                    //string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    //config = new IniFile(Path.Combine(currentDirectory, @"Resources\config.ini"));
                }));
#endif
#if ANDROID && false

                events.AddAndroid(android => android
                    // The statusbar color needs to be updated after the activity has been created
                    .OnCreate((activity, bundle) => UpdateThemeColor(activity, bundle))
                    );
                static void UpdateThemeColor(Activity activity, Bundle bundle)
                {
                    new PlatformThemeService().SetStatusBarColor(App.Instance.ThemeColor?.PrimaryColor);
                }
#endif
            })
            //.Services.AddSingleton(config);
            ;

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
