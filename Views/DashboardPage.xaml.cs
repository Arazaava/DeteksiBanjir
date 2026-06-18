using DeteksiBanjir.ViewModels;
using Microsoft.Maui.Controls;

namespace DeteksiBanjir.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DashboardViewModel viewModel)
        {
            await viewModel.LoadLatestDataAsync();
        }
    }
}
