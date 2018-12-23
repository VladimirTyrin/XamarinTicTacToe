using System;
using System.Diagnostics.CodeAnalysis;
using ITCC.Logging.Core;
using ITCC.Logging.Core.Loggers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace XamarinTicTacToe
{
	[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
	public partial class App : Application
	{
	    public static void RunOnUiThread(Action action) => Device.BeginInvokeOnMainThread(action);

        public App()
	    {
	        InitializeComponent();

	        Logger.Level = LogLevel.Trace;
	        Logger.RegisterReceiver(new DebugLogger(LogLevel.Trace));
	        MainPage = new MainPage();
	    }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
