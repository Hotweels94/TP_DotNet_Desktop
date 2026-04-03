using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GestionnaireLivresWPF.Data;
using GestionnaireLivresWPF.Models;

namespace GestionnaireLivresWPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly LivreRepository _repository = new LivreRepository();
        private Livre? _livreSelectionne;
        private string _titre = "";
        private string _auteur = "";
        private string _genre = "Autre";
        private string _annee = DateTime.Now.Year.ToString();
        private bool _lu;
        private string _termeRecherche = "";
        private int _totalLivres;
        private int _livresLus;
        private double _pourcentageLus;

        public ObservableCollection<Livre> Livres { get; } = new ObservableCollection<Livre>();

        public ObservableCollection<string> Genres { get; } = new ObservableCollection<string>
        {
            "Roman",
            "SF",
            "Fantasy",
            "Policier",
            "Autre"
        };

        public Livre? LivreSelectionne
        {
            get => _livreSelectionne;
            set
            {
                if (SetProperty(ref _livreSelectionne, value) && value != null)
                {
                    Titre = value.Titre;
                    Auteur = value.Auteur;
                    Annee = value.Annee.ToString();
                    Genre = value.Genre;
                    Lu = value.Lu;
                    ActualiserEtatCommandes();
                }
            }
        }

        public string Titre
        {
            get => _titre;
            set => SetProperty(ref _titre, value);
        }

        public string Auteur
        {
            get => _auteur;
            set => SetProperty(ref _auteur, value);
        }

        public string Annee
        {
            get => _annee;
            set => SetProperty(ref _annee, value);
        }

        public string Genre
        {
            get => _genre;
            set => SetProperty(ref _genre, value);
        }

        public bool Lu
        {
            get => _lu;
            set => SetProperty(ref _lu, value);
        }

        public string TermeRecherche
        {
            get => _termeRecherche;
            set
            {
                if (SetProperty(ref _termeRecherche, value))
                    ChargerLivres();
            }
        }

        public int TotalLivres
        {
            get => _totalLivres;
            set => SetProperty(ref _totalLivres, value);
        }

        public int LivresLus
        {
            get => _livresLus;
            set => SetProperty(ref _livresLus, value);
        }

        public double PourcentageLus
        {
            get => _pourcentageLus;
            set => SetProperty(ref _pourcentageLus, value);
        }

        public ICommand AjouterCommand { get; }
        public ICommand ModifierCommand { get; }
        public ICommand SupprimerCommand { get; }

        public MainViewModel()
        {
            _repository.InitialiserBase();

            AjouterCommand = new RelayCommand(_ => AjouterLivre());
            ModifierCommand = new RelayCommand(_ => ModifierLivre(), _ => LivreSelectionne != null);
            SupprimerCommand = new RelayCommand(_ => SupprimerLivre(), _ => LivreSelectionne != null);

            ChargerLivres();
        }

        private void ChargerLivres()
        {
            Livres.Clear();

            var resultats = string.IsNullOrWhiteSpace(TermeRecherche)
                ? _repository.GetAll()
                : _repository.GetByRecherche(TermeRecherche);

            foreach (var livre in resultats)
                Livres.Add(livre);

            RecalculerStatistiques();
            ActualiserEtatCommandes();
        }

        private string ValiderFormulaire()
        {
            string erreurs = "";

            if (string.IsNullOrWhiteSpace(Titre) || Titre.Trim().Length < 2)
                erreurs += "- Le titre doit contenir au minimum 2 caractères.\n";

            if (string.IsNullOrWhiteSpace(Auteur) || Auteur.Trim().Length < 2)
                erreurs += "- L'auteur doit contenir au minimum 2 caractères.\n";

            if (!int.TryParse(Annee, out int annee) || annee < 1800 || annee > DateTime.Now.Year)
                erreurs += $"- L'année doit être entre 1800 et {DateTime.Now.Year}.\n";

            if (string.IsNullOrWhiteSpace(Genre))
                erreurs += "- Besoin d'un genre.\n";

            return erreurs;
        }

        private void AjouterLivre()
        {
            var erreurs = ValiderFormulaire();

            if (!string.IsNullOrEmpty(erreurs))
            {
                MessageBox.Show(erreurs, "Erreurs de validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _repository.Add(new Livre
            {
                Titre = Titre.Trim(),
                Auteur = Auteur.Trim(),
                Annee = int.Parse(Annee),
                Genre = Genre,
                Lu = Lu
            });

            ViderFormulaire();
            ChargerLivres();
        }

        private void ModifierLivre()
        {
            if (LivreSelectionne == null)
                return;

            var erreurs = ValiderFormulaire();

            if (!string.IsNullOrEmpty(erreurs))
            {
                MessageBox.Show(erreurs, "Erreurs de validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LivreSelectionne.Titre = Titre.Trim();
            LivreSelectionne.Auteur = Auteur.Trim();
            LivreSelectionne.Annee = int.Parse(Annee);
            LivreSelectionne.Genre = Genre;
            LivreSelectionne.Lu = Lu;

            _repository.Update(LivreSelectionne);
            ChargerLivres();
        }

        private void SupprimerLivre()
        {
            if (LivreSelectionne == null)
                return;

            var reponse = MessageBox.Show(
                $"Etes-vous supprimer \"{LivreSelectionne.Titre}\" ?",
                "Supprimer",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (reponse == MessageBoxResult.Yes)
            {
                _repository.Delete(LivreSelectionne.Id);
                ViderFormulaire();
                ChargerLivres();
            }
        }

        private void ViderFormulaire()
        {
            LivreSelectionne = null;
            Titre = "";
            Auteur = "";
            Annee = DateTime.Now.Year.ToString();
            Genre = "Autre";
            Lu = false;
        }

        private void RecalculerStatistiques()
        {
            TotalLivres = Livres.Count;
            LivresLus = Livres.Count(l => l.Lu);
            PourcentageLus = TotalLivres == 0 ? 0 : Math.Round((double)LivresLus / TotalLivres * 100, 2);
        }

        private void ActualiserEtatCommandes()
        {
            if (ModifierCommand is RelayCommand modifier)
                modifier.RaiseCanExecuteChanged();

            if (SupprimerCommand is RelayCommand supprimer)
                supprimer.RaiseCanExecuteChanged();
        }
    }
}