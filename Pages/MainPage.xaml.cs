using DeteksiBanjir.Models;
using DeteksiBanjir.PageModels;

namespace DeteksiBanjir.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}