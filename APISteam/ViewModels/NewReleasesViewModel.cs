using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APISteam.ViewModels
{
    public class NewReleasesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Game> _newReleases;
        public ObservableCollection<Game> NewReleases
        {
            get => _newReleases;
            set
            {
                _newReleases = value;
                OnPropertyChanged();
            }
        }

        public NewReleasesViewModel()
        {
            // Exemple de données statiques
            NewReleases = new ObservableCollection<Game>
            {
                new Game { Name = "Game 1", ReleaseDate = "2023-10-01", ImageUrl = "URLToImage1" },
                new Game { Name = "Game 2", ReleaseDate = "2023-10-05", ImageUrl = "URLToImage2" },
                // Ajoutez plus de jeux ici
            };
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Game
    {
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public string ImageUrl { get; set; }
    }
}