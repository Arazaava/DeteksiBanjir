using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DeteksiBanjir.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeteksiBanjir.Services;

public class GeminiService
{
    private readonly HttpClient _httpClient;

    public GeminiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GenerateChatResponseAsync(string prompt, string context)
    {
        string apiKey = Constants.GEMINI_API_KEY;
        
        if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains("<<<"))
        {
            return "Maaf, API Key Gemini belum diisi. Silakan hubungi admin untuk melakukan konfigurasi sistem agar chatbot dapat berfungsi.";
        }

        string requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}";

        string fullPrompt = $"Konteks Data Sensor dan Status:\n{context}\n\nPertanyaan User: {prompt}\n\nJawablah dengan bahasa awam dan mudah dipahami, hindari istilah terlalu teknis jika memungkinkan.";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[] { new { text = fullPrompt } }
                }
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(requestUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseString);
                var textResponse = json["candidates"]?[0]?["content"]?[0]?["parts"]?[0]?["text"]?.ToString();
                return textResponse ?? "Tidak ada respons dari Gemini.";
            }
            else
            {
                return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
            }
        }
        catch (Exception ex)
        {
            return $"Error memanggil Gemini API: {ex.Message}";
        }
    }
}
