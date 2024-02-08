using AndreasReitberger.ActiveDirectorySearch.ViewModels;

namespace AndreasReitberger.ActiveDirectorySearch;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        // https://learn.microsoft.com/en-us/dotnet/architecture/maui/dependency-injection#resolution
        BindingContext = viewModel;

        Loaded += ((MainPageViewModel)BindingContext).Pages_Loaded;
    }
    ~MainPage()
    {
        Loaded -= ((MainPageViewModel)BindingContext).Pages_Loaded;
    }

    #region Methods
    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            ((MainPageViewModel)BindingContext).OnAppearing();
            //await ((AboutServerPageViewModel)BindingContext).OnAppearing();
        }
        catch (Exception) { }
    }
    #endregion
}

