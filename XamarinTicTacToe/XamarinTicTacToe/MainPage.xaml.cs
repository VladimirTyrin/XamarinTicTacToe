﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ITCC.Logging.Core;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XamarinTicTacToe.Engine;
using XamarinTicTacToe.Engine.Bots.BrainTvs;
using XamarinTicTacToe.Engine.Bots.BrainTvsV2;
using XamarinTicTacToe.Engine.Enums;
using XamarinTicTacToe.Engine.Interfaces;
using XamarinTicTacToe.Engine.Utils;

namespace XamarinTicTacToe
{
    [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
    public partial class MainPage : ContentPage
	{
        private GameConfiguration _configuration;
        private Game _game;

        private CellView[,] _cellControls;

        private bool _waitingForTurn;
        private readonly object _stateLock = new object();
        private IPlayer _currentPlayer;
        private CellSign _currentSign;
        private CellSign[,] _fieldState;

        private readonly AutoResetEvent _turnEvent = new AutoResetEvent(false);
        private bool _botTesting;

        public MainPage()
        {
            InitializeComponent();
            LoadDefault();
        }

        private static string PlayerDescription(IPlayer player) => $"{player.Name} ({player.Type})";

        private void LoadDefault()
        {
            App.RunOnUiThread(() => WinnerLabel.Text = string.Empty);
            LoadDefaultConfiguration();
            if (_configuration.FirstPlayer.Type == PlayerType.Bot && _configuration.SecondPlayer.Type == PlayerType.Bot)
            {
                _botTesting = true;
            }
            else
            {
                // Do not leave extra space
                NextButton.IsVisible = false;
                NextButton.HeightRequest = 0;
                NextButton.WidthRequest = 0;
            }
            LoadGame();
        }

        public void Load()
        {
            App.RunOnUiThread(() => WinnerLabel.Text = string.Empty);
            // Do not reload config
            LoadGame();
        }

        private void LoadDefaultConfiguration()
        {
            _configuration = new GameConfiguration
            {
                BotTurnLength = 1_000_000,
                FirstPlayer = new BrainTvsBot(),
                SecondPlayer = new BrainTvsBotV2(),
                Height = 10,
                Width = 10
            };
        }

        private void LoadGame()
        {
            _game = Game.CreateNewGame(_configuration);
            if (_game == null)
            {
                return;
            }

            _currentPlayer = _configuration.FirstPlayer;
            _currentSign = CellSign.X;
            FirstPlayerNameLabel.Text = PlayerDescription(_configuration.FirstPlayer);
            SecondPlayerNameLabel.Text = PlayerDescription(_configuration.SecondPlayer);

            _game.GameStateChanged += ProcessGameStateChanged;
            _game.GameEnded += ProcessGameEnded;
            ReloadGrid();
            LoadCellControls();

            _game.Start();
            if (!IsHumansTurn())
                return;
            lock (_stateLock)
            {
                _waitingForTurn = true;
            }
        }

        private void ReloadGrid()
        {
            FieldGrid.IsEnabled = true;
            FieldGrid.Children.Clear();
            FieldGrid.RowDefinitions.Clear();
            FieldGrid.ColumnDefinitions.Clear();

            for (var i = 0; i < _configuration.Height; ++i)
            {
                FieldGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (var i = 0; i < _configuration.Width; ++i)
            {
                FieldGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void LoadCellControls()
        {
            var height = _configuration.Height;
            var width = _configuration.Width;

            _fieldState = new CellSign[height, width];
            _cellControls = new CellView[height, width];
            for (var i = 0; i < height; ++i)
            {
                for (var j = 0; j < width; ++j)
                {
                    _cellControls[i, j] = new CellView();
                    _cellControls[i, j].SetParams(i, j, ProcessCellClick);
                    FieldGrid.Children.Add(_cellControls[i, j]);
                    Grid.SetRow(_cellControls[i, j], i);
                    Grid.SetColumn(_cellControls[i, j], j);
                }
            }
        }

        private void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer == _configuration.FirstPlayer
                ? _configuration.SecondPlayer
                : _configuration.FirstPlayer;
            _currentSign = _currentSign == CellSign.O ? CellSign.X : CellSign.O;
        }

        private bool IsHumansTurn() => _currentPlayer.Type == PlayerType.Human;

        private void ProcessCellClick(int row, int column)
        {
            if (_fieldState[row, column] != CellSign.Empty)
            {
                return;
            }

            lock (_stateLock)
            {
                if (!_waitingForTurn)
                    return;
                _waitingForTurn = false;
            }

            var human = _currentPlayer as HumanPlayer;

            human?.SetNextMove(new Engine.Utils.Cell(row, column));
        }

        private void ProcessGameEnded(object sender, GameEndedEventArgs gameEndedEventArgs)
        {
            var winnerMessage = gameEndedEventArgs.Winner == null ? "Draw" : $"{gameEndedEventArgs.Winner.Name} won";
            var lastMove = gameEndedEventArgs.History.Last();
            App.RunOnUiThread(async () => await _cellControls[lastMove.X, lastMove.Y].LoadPictureAsync(_currentSign));

            if (gameEndedEventArgs.WinningSet != null)
            {
                App.RunOnUiThread(() =>
                {
                    foreach (var cell in gameEndedEventArgs.WinningSet)
                    {
                        _cellControls[cell.X, cell.Y].LoadWonPicture(_currentSign);
                    }
                });
            }

            App.RunOnUiThread(() =>
            {
                WinnerLabel.Text = winnerMessage;
                FieldGrid.IsEnabled = false;
            });
        }

        private void ProcessGameStateChanged(object sender, GameStateChangedEventArgs gameStateChangedEventArgs)
        {
            for (var i = 0; i < gameStateChangedEventArgs.CurrentState.Height; ++i)
            {
                for (var j = 0; j < gameStateChangedEventArgs.CurrentState.Width; ++j)
                {
                    _fieldState[i, j] = gameStateChangedEventArgs.CurrentState.Field[i, j];
                }
            }
            var lastMove = gameStateChangedEventArgs.CurrentState.LastMove;
            var currentSign = gameStateChangedEventArgs.CurrentState.PlayerSign;
            App.RunOnUiThread(async () => await _cellControls[lastMove.X, lastMove.Y].LoadPictureAsync(currentSign));

            SwitchPlayer();
            if (IsHumansTurn())
            {
                lock (_stateLock)
                {
                    _waitingForTurn = true;
                }
            }
            Thread.Sleep(50);

            if (_botTesting)
            {
                _turnEvent.WaitOne();
                _turnEvent.Reset();
            }

            _game.ReportStepProcessed();
        }

        private void RestartButton_OnClick(object sender, EventArgs e) => Load();

        private async void SettingsButton_OnClicked(object sender, EventArgs e)
        {
            var page = new SettingsPage {Configuration = _configuration, MainPage = this};
            await Navigation.PushModalAsync(page);
        }

        private void NextButton_OnClicked(object sender, EventArgs e)
        {
            _turnEvent.Set();
        }
    }
}
