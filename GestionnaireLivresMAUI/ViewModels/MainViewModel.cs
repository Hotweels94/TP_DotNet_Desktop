using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GestionnaireLivresMAUI.Models;

namespace GestionnaireLivresMAUI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _afficherLusSeulement;

        public ObservableCollection<Livre> Livres { get; } = new ObservableCollection<Livre>();

        public ObservableCollection<Livre> LivresFiltres { get; } = new ObservableCollection<Livre>();

        public bool AfficherLusSeulement
        {
            get => _afficherLusSeulement;
            set
            {
                if (_afficherLusSeulement != value)
                {
                    _afficherLusSeulement = value;
                    OnPropertyChanged();
                    AppliquerFiltre();
                }
            }
        }

        public ICommand LivreSelectionneCommand { get; }

        public MainViewModel()
        {
            Livres.Add(new Livre
            {
                Titre = "Le Rouge et le Noir",
                Auteur = "Stendhal",
                Annee = 1830,
                Genre = "Roman",
                Lu = true
            });

            Livres.Add(new Livre
            {
                Titre = "1984",
                Auteur = "George Orwell",
                Annee = 1949,
                Genre = "SF",
                Lu = false
            });

            Livres.Add(new Livre
            {
                Titre = "Le Seigneur des Anneaux",
                Auteur = "J.R.R. Tolkien",
                Annee = 1954,
                Genre = "Fantasy",
                Lu = true
            });

            Livres.Add(new Livre
            {
                Titre = "Shutter Island",
                Auteur = "Dennis Lehane",
                Annee = 2003,
                Genre = "Policier",
                Lu = false
            });

            Livres.Add(new Livre
            {
                Titre = "Notre Dame de Paris",
                Auteur = "Victor Hugo",
                Annee = 1831,
                Genre = "Autre",
                Lu = false
            });

            LivreSelectionneCommand = new Command<Livre>(async (livre) => await AfficherDetails(livre));

            AppliquerFiltre();
        }

        private void AppliquerFiltre()
        {
            LivresFiltres.Clear();

            var livresAAfficher = AfficherLusSeulement
                ? Livres.Where(l => l.Lu)
                : Livres;

            foreach (var livre in livresAAfficher)
                LivresFiltres.Add(livre);
        }

        private async Task AfficherDetails(Livre? livre)
        {
            if (livre == null)
                return;

            await Shell.Current.DisplayAlert(
                livre.Titre,
                $"Auteur : {livre.Auteur}\nAnnée : {livre.Annee}\nGenre : {livre.Genre}\nLu : {(livre.Lu ? "Oui" : "Non")}",
                "OK"
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}