using DeteksiBanjir.Views;

namespace DeteksiBanjir;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
        Routing.RegisterRoute(nameof(ChatbotPage), typeof(ChatbotPage));
	}
}
