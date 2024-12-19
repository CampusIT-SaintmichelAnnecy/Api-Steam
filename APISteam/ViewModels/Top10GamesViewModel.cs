using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using APISteam.Models;

namespace APISteam.ViewModels
{
    public class Top10GamesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SteamGame> _mostPlayedGames;

        public event PropertyChangedEventHandler PropertyChanged;

        public Top10GamesViewModel(ObservableCollection<SteamGame> games)
        {
            MostPlayedGames = games;
        }

        public ObservableCollection<SteamGame> MostPlayedGames
        {
            get => _mostPlayedGames;
            set
            {
                _mostPlayedGames = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}