using System;
using System.Collections.Generic;
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
    // ReSharper disable once RedundantExtendsListEntry
    public partial class CellView : ContentView
	{
        private static readonly Dictionary<string, byte[]> ResourceCache = new Dictionary<string, byte[]>();
        private static readonly Dictionary<string, ImageSource> ImageSourceCache = new Dictionary<string, ImageSource>();

        private int _x;
	    private int _y;
	    private Action<int, int> _clickCallback;

        static CellView()
        {
            var keys = new[]
            {
                "Empty.png",
                "O.png",
                "X.png",
                "OWon.png",
                "XWon.png",
                "OJustMoved.png",
                "XJustMoved.png",
            };

            foreach (var key in keys)
            {
                ResourceCache.Add(key, ReadResource(key));
                ImageSourceCache.Add(key, ImageSource.FromStream(() => new MemoryStream(ResourceCache[key])));
            }
        }

	    public CellView()
	    {
	        InitializeComponent();
	        var recognizer = new TapGestureRecognizer { Command = new Command(() => _clickCallback.Invoke(_x, _y)) };
	        SignImage.GestureRecognizers.Add(recognizer);
	        SignImage.Source = ImageSourceCache["Empty.png"];
	    }

	    public void SetParams(int x, int y, Action<int, int> clickCallback)
	    {
	        _x = x;
	        _y = y;
	        _clickCallback = clickCallback;
	    }

	    public async Task LoadPictureAsync(CellSign sign)
        {
            var justMovedName = sign == CellSign.O ? "OJustMoved.png" : "XJustMoved.png";
            var currentSource = ImageSourceCache[justMovedName];
            SignImage.Source = currentSource;
            await Task.Delay(300);
            // Different image is already loaded (for example, game ended)
            if (SignImage.Source != currentSource)
                return;
            var name = sign == CellSign.O ? "O.png" : "X.png";
	        SignImage.Source = ImageSourceCache[name];
	    }

	    public void LoadWonPicture(CellSign sign)
	    {
	        var name = sign == CellSign.O ? "OWon.png" : "XWon.png";
	        SignImage.Source = ImageSourceCache[name];
	    }

        private static byte[] ReadResource(string filename)
        {
            var name = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .FirstOrDefault(n => n.Contains(filename));

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new Exception($"Resource stream {filename} not found");

                var length = stream.Length;
                var arr = new byte[length];

                var totalRead = 0;
                while (totalRead < length)
                {
                    var toRead = (int)(length - totalRead);
                    var currentRead = totalRead;
                    totalRead += stream.Read(arr, currentRead, toRead);
                }

                return arr;
            }
        }
    }
}