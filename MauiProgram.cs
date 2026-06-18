using Microsoft.Extensions.Logging;
using DeteksiBanjir.Services;
using DeteksiBanjir.ViewModels;
using DeteksiBanjir.Views;

namespace DeteksiBanjir;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<FuzzyService>();
        builder.Services.AddSingleton<NeuralNetworkService>();
        builder.Services.AddSingleton<GeminiService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<AdminViewModel>();
        builder.Services.AddTransient<ChatbotViewModel>();

        // Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<AdminPage>();
        builder.Services.AddTransient<ChatbotPage>();

		return builder.Build();
	}
}
