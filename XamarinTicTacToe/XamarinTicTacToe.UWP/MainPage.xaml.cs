namespace XamarinTicTacToe.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new XamarinTicTacToe.App());
        }
    }
}
