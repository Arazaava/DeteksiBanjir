using DeteksiBanjir.ViewModels;
using Microsoft.Maui.Controls;

namespace DeteksiBanjir.Views;

public partial class AdminPage : ContentPage
{
    public AdminPage(AdminViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AdminViewModel viewModel)
        {
            await viewModel.LoadAdminDataAsync();
        }
    }
}
