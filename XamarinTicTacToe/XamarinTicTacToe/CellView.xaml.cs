using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTicTacToe.Engine.Enums;

namespace XamarinTicTacToe
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CellView : ContentView
	{
	    private int _x;
	    private int _y;
	    private Action<int, int> _clickCallback;

	    public CellView()
	    {
	        InitializeComponent();
	        var recognizer = new TapGestureRecognizer { Command = new Command(() => _clickCallback.Invoke(_x, _y)) };
	        SignImage.GestureRecognizers.Add(recognizer);
	        SignImage.Source = BuildImageSource("Empty.png");
	    }

	    public void SetParams(int x, int y, Action<int, int> clickCallback)
	    {
	        _x = x;
	        _y = y;
	        _clickCallback = clickCallback;
	    }

	    public void LoadPicture(CellSign sign)
	    {
	        var name = sign == CellSign.O ? "O.png" : "X.png";
	        SignImage.Source = BuildImageSource(name);
	    }

	    public void LoadWonPicture(CellSign sign)
	    {
	        var name = sign == CellSign.O ? "OWon.png" : "XWon.png";
	        SignImage.Source = BuildImageSource(name);
	    }

	    private static Stream ReadResourceStream(string filename)
	    {
	        var name = Assembly.GetExecutingAssembly().GetManifestResourceNames()
	            .FirstOrDefault(n => n.Contains(filename));

	        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
	        {
	            if (stream == null)
	                throw new Exception($"Resource stream {filename} not found");

	            var memoryStream = new MemoryStream();
	            stream.CopyTo(memoryStream);
	            memoryStream.Seek(0L, SeekOrigin.Begin);
	            return memoryStream;
	        }
	    }

	    private static ImageSource BuildImageSource(string name)
	    {
	        return new StreamImageSource
	        {
	            Stream = token => Task.FromResult(ReadResourceStream(name))
	        };
	    }
    }
}