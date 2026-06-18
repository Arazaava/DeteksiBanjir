using DeteksiBanjir.ViewModels;
using Microsoft.Maui.Controls;

namespace DeteksiBanjir.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
