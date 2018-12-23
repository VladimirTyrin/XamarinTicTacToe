using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTicTacToe.Engine;

namespace XamarinTicTacToe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public GameConfiguration Configuration { get; set; }
        public MainPage MainPage { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void ApplyButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
            MainPage.Load();
        }
    }
}