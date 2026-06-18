using DeteksiBanjir.ViewModels;
using Microsoft.Maui.Controls;

namespace DeteksiBanjir.Views;

public partial class ChatbotPage : ContentPage
{
    public ChatbotPage(ChatbotViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
