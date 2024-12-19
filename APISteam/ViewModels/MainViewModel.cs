using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using APISteam.Models;
using APISteam.Services;
using APISteam.Views;

namespace APISteam.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly SteamApiService _steamApiService;
        private bool _isLoading;
        private string? _errorMessage;
        private object _currentViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowTop10GamesCommand { get; }
        public ICommand ShowNewReleasesCommand { get; }

        public MainViewModel()
        {
            string apiKey = ConfigurationManager.AppSettings["SteamApiKey"];
            _steamApiService = new SteamApiService(apiKey);

            ShowTop10GamesCommand = new RelayCommand(async () => await ShowTop10Games());
            ShowNewReleasesCommand = new RelayCommand(async () => await ShowNewReleases());

            // Default view
            ShowTop10Games().Wait();
        }

        private async Task ShowTop10Games()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            try
            {
                var games = await _steamApiService.GetTopGamesAsync();
                var viewModel = new Top10GamesViewModel( new ObservableCollection<SteamGame>(games));
                CurrentViewModel = viewModel;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load games: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ShowNewReleases()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var viewModel = new NewReleasesViewModel();
                CurrentViewModel = viewModel;
            });
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

#pragma warning disable 0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

        public async void Execute(object? parameter) => await _execute();
    }
}