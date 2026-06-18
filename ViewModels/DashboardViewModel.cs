using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeteksiBanjir.Services;
using DeteksiBanjir.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DeteksiBanjir.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly DatabaseService _databaseService;
    private readonly FuzzyService _fuzzyService;
    private readonly NeuralNetworkService _nnService;

    [ObservableProperty]
    private string welcomeMessage = string.Empty;

    [ObservableProperty]
    private double waterLevel;

    [ObservableProperty]
    private double rainfall;

    [ObservableProperty]
    private double discharge;

    [ObservableProperty]
    private double temperature;

    [ObservableProperty]
    private double humidity;

    [ObservableProperty]
    private double waterLevelRatio;

    [ObservableProperty]
    private double rainfallRatio;

    [ObservableProperty]
    private double dischargeRatio;

    [ObservableProperty]
    private double fuzzyRiskScore;

    [ObservableProperty]
    private string fuzzyCategory = "No Alert";

    [ObservableProperty]
    private string fuzzyColor = "#4CAF50";

    [ObservableProperty]
    private double nnRiskScore;

    [ObservableProperty]
    private string nnCategory = "No Alert";

    [ObservableProperty]
    private string nnColor = "#4CAF50";

    [ObservableProperty]
    private string lastUpdated = "Never";

    public DashboardViewModel(
        AuthService authService,
        DatabaseService databaseService,
        FuzzyService fuzzyService,
        NeuralNetworkService nnService)
    {
        _authService = authService;
        _databaseService = databaseService;
        _fuzzyService = fuzzyService;
        _nnService = nnService;
        
        WelcomeMessage = $"Selamat Datang, {_authService.CurrentUser?.Username ?? "User"}";
    }

    [RelayCommand]
    public async Task LoadLatestDataAsync()
    {
        var readings = await _databaseService.GetAllAsync<SensorReading>();
        if (readings != null && readings.Count > 0)
        {
            var latest = readings.OrderByDescending(r => r.Timestamp).First();
            WaterLevel = latest.WaterLevel;
            Rainfall = latest.Rainfall;
            Discharge = latest.Discharge;
            Temperature = latest.Temperature;
            Humidity = latest.Humidity;
            LastUpdated = latest.Timestamp.ToString("dd MMM yyyy HH:mm:ss");
        }
        else
        {
            WaterLevel = 0.0;
            Rainfall = 0.0;
            Discharge = 0.0;
            Temperature = 0.0;
            Humidity = 0.0;
            LastUpdated = "Tidak ada data sensor";
        }

        WaterLevelRatio = Math.Clamp(WaterLevel / 10.0, 0.0, 1.0);
        RainfallRatio = Math.Clamp(Rainfall / 100.0, 0.0, 1.0);
        DischargeRatio = Math.Clamp(Discharge / 150.0, 0.0, 1.0);

        // Fuzzy Logic prediction
        var fuzzyResult = _fuzzyService.CalculateRisk(WaterLevel, Rainfall, Discharge);
        FuzzyRiskScore = fuzzyResult.RiskScore;
        FuzzyCategory = fuzzyResult.Category;
        FuzzyColor = GetColorForCategory(fuzzyResult.Category);

        // Neural Network prediction
        var nnResult = _nnService.Predict(WaterLevel, Rainfall, Discharge);
        NnRiskScore = nnResult.RiskScore;
        NnCategory = nnResult.Category;
        NnColor = GetColorForCategory(nnResult.Category);
    }

    private string GetColorForCategory(string category)
    {
        return category switch
        {
            "No Alert" => "#4CAF50",
            "Yellow Alert" => "#FFEB3B",
            "Orange Alert" => "#FF9800",
            "Red Alert" => "#F44336",
            _ => "#CCCCCC"
        };
    }

    [RelayCommand]
    public async Task OpenChatbotAsync()
    {
        await Shell.Current.GoToAsync("ChatbotPage");
    }

    [RelayCommand]
    public async Task LogoutAsync()
    {
        _authService.Logout();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
