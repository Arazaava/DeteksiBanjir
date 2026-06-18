using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeteksiBanjir.Services;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DeteksiBanjir.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthService _authService;

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string errorMessage;

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    public async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Username dan password tidak boleh kosong.";
            return;
        }

        bool success = await _authService.LoginAsync(Username, Password);
        if (success)
        {
            ErrorMessage = string.Empty;
            if (_authService.IsAdmin())
            {
                await Shell.Current.GoToAsync("//AdminPage");
            }
            else
            {
                await Shell.Current.GoToAsync("//DashboardPage");
            }
        }
        else
        {
            ErrorMessage = "Username atau password salah.";
        }
    }
}
