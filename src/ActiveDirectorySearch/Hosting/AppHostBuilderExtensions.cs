using AndreasReitberger.ActiveDirectorySearch.Models.NavigationManager;
using AndreasReitberger.ActiveDirectorySearch.ViewModels;
using AndreasReitberger.Shared.Core;

namespace AndreasReitberger.ActiveDirectorySearch.Hosting
{
    public static class AppHostBuilderExtensions
    {
        /*
         * The view models do not inherit from an interface, so they only need their concrete type provided to the AddSingleton<T> and AddTransient<T> methods.
         */

        public static MauiAppBuilder ConfigureApp(this MauiAppBuilder builder)
        {
            builder
                .RegisterDispatcher()
                .RegisterMainViewModels()
                .RegisterPageViewModels()
                .RegisterModalViewModels()
                .RegisterSettingsViewModels()
                .RegisterMainViews()
                .RegisterPageViews()
                .RegisterModalViews()
                .RegisterSettingsViews()
                .RegisterNavigationRoots()
                ;
            return builder;
        }

        /// <summary>
        /// Registers the main view models as Singleton.
        /// https://learn.microsoft.com/en-us/dotnet/architecture/maui/dependency-injection
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>MauiAppBuilder</returns>
        public static MauiAppBuilder RegisterMainViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<ViewModelBase>();
            builder.Services.AddSingleton<BaseViewModel>();
            builder.Services.AddSingleton<AppViewModel>();
            
            // Main view models
            builder.Services.AddSingleton<MainPageViewModel>();
            return builder;
        }
        public static MauiAppBuilder RegisterMainViews(this MauiAppBuilder builder)
        {
            // Example: https://github.com/microsoft/dotnet-podcasts/blob/main/src/Mobile/Pages/PagesExtensions.cs
            // Loading
            //builder.Services.AddSingleton<LoadingPage>();
            // Main view models
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<MainPage>();
            return builder;
        }

        public static MauiAppBuilder RegisterPageViewModels(this MauiAppBuilder builder)
        {
            // Page view models
            //builder.Services.AddSingleton<AboutProVersionPageViewModel>();
            return builder;
        }
        public static MauiAppBuilder RegisterPageViews(this MauiAppBuilder builder)
        {
            // Page view models
            //builder.Services.AddSingleton<AboutProVersionPage>();
            return builder;
        }

        public static MauiAppBuilder RegisterModalViewModels(this MauiAppBuilder builder)
        {
            // Modal view models
            //builder.Services.AddTransient<AdjustCloudSyncModalPageViewModel>();
            return builder;
        }
        public static MauiAppBuilder RegisterModalViews(this MauiAppBuilder builder)
        {
            // Modal view models
            //builder.Services.AddTransient<AdjustCloudSyncModalPage>();
            return builder;
        }

        public static MauiAppBuilder RegisterSettingsViewModels(this MauiAppBuilder builder)
        {
            // Settings view models
            //builder.Services.AddTransient<SettingsAdsSetupPageViewModel>();
            return builder;
        }
        public static MauiAppBuilder RegisterSettingsViews(this MauiAppBuilder builder)
        {
            // Settings view models
            //builder.Services.AddTransient<SettingsAdsSetupPage>();
            return builder;
        }

        public static MauiAppBuilder RegisterNavigationRoots(this MauiAppBuilder builder)
        {
            // Settings view models
            ShellNavigator.Instance.RegisterRoutes();
            return builder;
        }

        public static MauiAppBuilder RegisterDispatcher(this MauiAppBuilder builder)
        {
            // Example: https://github.com/jamesmontemagno/ToolkitMessenger/blob/master/MauiApp2/MauiProgram.cs
            //builder.Services.AddSingleton<IDispatcherProvider>(DispatcherProvider.Current);
            builder.Services.AddSingleton<IDispatcher>(Dispatcher.GetForCurrentThread());
            return builder;
        }
    }
}
