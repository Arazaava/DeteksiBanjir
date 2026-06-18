using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeteksiBanjir.Services;
using DeteksiBanjir.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DeteksiBanjir.ViewModels;

public partial class AdminViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private string welcomeMessage = string.Empty;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    // Sensor Inputs
    [ObservableProperty]
    private double waterLevel;

    [ObservableProperty]
    private double rainfall;

    [ObservableProperty]
    private double discharge;

    [ObservableProperty]
    private double temperature = 25.0;

    [ObservableProperty]
    private double humidity = 70.0;

    // Thresholds Inputs
    [ObservableProperty]
    private double yellowThreshold;

    [ObservableProperty]
    private double orangeThreshold;

    [ObservableProperty]
    private double redThreshold;

    public ObservableCollection<SensorReading> ReadingsList { get; } = new();
    public ObservableCollection<User> UsersList { get; } = new();

    public AdminViewModel(AuthService authService, DatabaseService databaseService)
    {
        _authService = authService;
        _databaseService = databaseService;

        WelcomeMessage = $"Admin Panel - {_authService.CurrentUser?.Username ?? "Admin"}";
    }

    [RelayCommand]
    public async Task LoadAdminDataAsync()
    {
        // Load Thresholds
        var thresholdsList = await _databaseService.GetAllAsync<Thresholds>();
        if (thresholdsList != null && thresholdsList.Count > 0)
        {
            var thresholds = thresholdsList[0];
            YellowThreshold = thresholds.YellowAlertLevel;
            OrangeThreshold = thresholds.OrangeAlertLevel;
            RedThreshold = thresholds.RedAlertLevel;
        }

        // Load Readings
        var readings = await _databaseService.GetAllAsync<SensorReading>();
        ReadingsList.Clear();
        if (readings != null)
        {
            foreach (var r in readings.OrderByDescending(r => r.Timestamp).Take(10))
            {
                ReadingsList.Add(r);
            }
        }

        // Load Users
        var users = await _databaseService.GetAllAsync<User>();
        UsersList.Clear();
        if (users != null)
        {
            foreach (var u in users)
            {
                UsersList.Add(u);
            }
        }
    }

    [RelayCommand]
    public async Task SubmitSensorDataAsync()
    {
        var newReading = new SensorReading
        {
            Timestamp = DateTime.Now,
            WaterLevel = WaterLevel,
            Rainfall = Rainfall,
            Discharge = Discharge,
            Temperature = Temperature,
            Humidity = Humidity
        };

        int result = await _databaseService.InsertAsync(newReading);
        if (result > 0)
        {
            StatusMessage = "Data sensor berhasil disimpan!";
            
            // Clear inputs
            WaterLevel = 0;
            Rainfall = 0;
            Discharge = 0;
            Temperature = 25.0;
            Humidity = 70.0;

            await LoadAdminDataAsync();
        }
        else
        {
            StatusMessage = "Gagal menyimpan data sensor.";
        }
    }

    [RelayCommand]
    public async Task UpdateThresholdsAsync()
    {
        var thresholdsList = await _databaseService.GetAllAsync<Thresholds>();
        Thresholds thresholds;
        if (thresholdsList != null && thresholdsList.Count > 0)
        {
            thresholds = thresholdsList[0];
            thresholds.YellowAlertLevel = YellowThreshold;
            thresholds.OrangeAlertLevel = OrangeThreshold;
            thresholds.RedAlertLevel = RedThreshold;
            await _databaseService.UpdateAsync(thresholds);
        }
        else
        {
            thresholds = new Thresholds
            {
                YellowAlertLevel = YellowThreshold,
                OrangeAlertLevel = OrangeThreshold,
                RedAlertLevel = RedThreshold
            };
            await _databaseService.InsertAsync(thresholds);
        }

        StatusMessage = "Thresholds berhasil diperbarui!";
        await LoadAdminDataAsync();
    }

    [RelayCommand]
    public async Task LogoutAsync()
    {
        _authService.Logout();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
