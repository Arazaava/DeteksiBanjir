using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeteksiBanjir.Services;
using DeteksiBanjir.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DeteksiBanjir.ViewModels;

public partial class ChatbotViewModel : ObservableObject
{
    private readonly GeminiService _geminiService;
    private readonly DatabaseService _databaseService;
    private readonly FuzzyService _fuzzyService;
    private readonly NeuralNetworkService _nnService;

    [ObservableProperty]
    private string userInput = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<string> ChatHistory { get; } = new();

    public ChatbotViewModel(
        GeminiService geminiService,
        DatabaseService databaseService,
        FuzzyService fuzzyService,
        NeuralNetworkService nnService)
    {
        _geminiService = geminiService;
        _databaseService = databaseService;
        _fuzzyService = fuzzyService;
        _nnService = nnService;
        ChatHistory.Add("Bot: Halo! Saya asisten pintar Deteksi Banjir. Ada yang bisa saya bantu terkait status banjir saat ini?");
    }

    [RelayCommand]
    public async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(UserInput)) return;

        string question = UserInput;
        UserInput = string.Empty;
        ChatHistory.Add($"Anda: {question}");

        IsBusy = true;

        // Get actual context from database and AI calculations
        string context = "Tidak ada data sensor terbaru.";
        var readings = await _databaseService.GetAllAsync<SensorReading>();
        if (readings != null && readings.Count > 0)
        {
            var latest = readings.OrderByDescending(r => r.Timestamp).First();
            var fuzzyResult = _fuzzyService.CalculateRisk(latest.WaterLevel, latest.Rainfall, latest.Discharge);
            var nnResult = _nnService.Predict(latest.WaterLevel, latest.Rainfall, latest.Discharge);

            context = $"Data Sensor Terkini ({latest.Timestamp:dd MMM yyyy HH:mm:ss}):\n" +
                      $"- Ketinggian Air: {latest.WaterLevel:F2} m\n" +
                      $"- Curah Hujan: {latest.Rainfall:F2} mm/h\n" +
                      $"- Debit Air: {latest.Discharge:F2} m³/s\n" +
                      $"- Suhu: {latest.Temperature:F1} °C, Kelembaban: {latest.Humidity:F1} %\n\n" +
                      $"Hasil Analisis AI:\n" +
                      $"- Logika Fuzzy: Tingkat Risiko {fuzzyResult.RiskScore:F2} / 10 ({fuzzyResult.Category})\n" +
                      $"- Jaringan Saraf Tiruan (Neural Network): Kategori {nnResult.Category} (Risiko Score: {nnResult.RiskScore:F2})";
        }
        
        string response = await _geminiService.GenerateChatResponseAsync(question, context);
        ChatHistory.Add($"Bot: {response}");

        IsBusy = false;
    }
}
