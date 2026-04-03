namespace GestionnaireLivresWinForms.Models
{
    public class Livre
    {
        public string Titre { get; set; } = string.Empty;
        public string Auteur { get; set; } = string.Empty;
        public int Annee { get; set; }
        public string Genre { get; set; } = string.Empty;
        public bool Lu { get; set; } = false;

        public override string ToString()
        {
            string statut = Lu ? "[Lu]" : "[Non lu]";
            return $"{Titre} - {Auteur} ({Annee}) - {Genre} {statut}";
        }
    }
}