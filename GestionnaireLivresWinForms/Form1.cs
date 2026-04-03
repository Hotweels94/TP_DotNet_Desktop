using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GestionnaireLivresWinForms.Models;

namespace GestionnaireLivresWinForms
{
    public partial class Form1 : Form
    {
        private TextBox Titre = null!;
        private TextBox Auteur = null!;
        private TextBox Annee = null!;
        private ComboBox Genre = null!;
        private CheckBox Lu = null!;
        private Button Ajouter = null!;
        private Button Modifier = null!;
        private Button Supprimer = null!;
        private ListBox Livres = null!;

        private readonly List<Livre> bibliotheque = new List<Livre>();

        private static readonly Color Fond = Color.FromArgb(15, 17, 26);
        private static readonly Color Surface = Color.FromArgb(24, 27, 42);
        private static readonly Color SurfaceHaute = Color.FromArgb(32, 36, 56);
        private static readonly Color Bordure = Color.FromArgb(48, 53, 80);
        private static readonly Color Accent = Color.FromArgb(99, 179, 237);
        private static readonly Color AccentVert = Color.FromArgb(72, 199, 142);
        private static readonly Color AccentRouge = Color.FromArgb(252, 110, 81);
        private static readonly Color TextePrinc = Color.FromArgb(225, 230, 255);
        private static readonly Color TexteSecond = Color.FromArgb(120, 130, 170);

        private static readonly Font FonteTitrePage = new Font("Palatino Linotype", 17F, FontStyle.Bold);
        private static readonly Font FonteLabel = new Font("Palatino Linotype", 11F, FontStyle.Regular);
        private static readonly Font FonteChamp = new Font("Consolas", 12F, FontStyle.Regular);
        private static readonly Font FonteBouton = new Font("Palatino Linotype", 11F, FontStyle.Bold);
        private static readonly Font FonteAide = new Font("Palatino Linotype", 10F, FontStyle.Italic);
        private static readonly Font FonteListe = new Font("Consolas", 12F, FontStyle.Regular);

        public Form1()
        {
            Text = "Gestionnaire de Livres";
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Palatino Linotype", 11F, FontStyle.Regular, GraphicsUnit.Point);
            MinimumSize = new Size(1400, 900);
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            BackColor = Fond;

            InitialiserInterface();
            LierEvenements();
        }

        private void InitialiserInterface()
        {
            Controls.Clear();

            var principal = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(28),
                BackColor = Fond
            };

            principal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 520F));
            principal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var panelFormulaire = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                Padding = new Padding(24),
                BackColor = Surface,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            panelFormulaire.Margin = new Padding(0, 0, 14, 0);

            panelFormulaire.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelFormulaire.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            panelFormulaire.RowStyles.Add(new RowStyle(SizeType.Absolute, 115F));
            panelFormulaire.RowStyles.Add(new RowStyle(SizeType.Absolute, 115F));
            panelFormulaire.RowStyles.Add(new RowStyle(SizeType.Absolute, 115F));
            panelFormulaire.RowStyles.Add(new RowStyle(SizeType.Absolute, 115F));
            panelFormulaire.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            panelFormulaire.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
            panelFormulaire.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var lblTitreBloc = new Label
            {
                Text = "Gestion du livre",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = FonteTitrePage,
                ForeColor = Accent,
                BackColor = Color.Transparent
            };

            panelFormulaire.Controls.Add(lblTitreBloc, 0, 0);
            panelFormulaire.Controls.Add(CreerLigneTexte("Titre :", out Titre), 0, 1);
            panelFormulaire.Controls.Add(CreerLigneTexte("Auteur :", out Auteur), 0, 2);
            panelFormulaire.Controls.Add(CreerLigneTexte("Année :", out Annee), 0, 3);
            panelFormulaire.Controls.Add(CreerLigneGenre(), 0, 4);

            var ligneLu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = false,
                Padding = new Padding(8, 14, 8, 8),
                Margin = new Padding(0),
                BackColor = Color.Transparent
            };

            Lu = new CheckBox
            {
                Text = "Lu ?",
                AutoSize = true,
                Font = FonteLabel,
                ForeColor = TextePrinc,
                BackColor = Color.Transparent
            };

            Lu.CheckedChanged += (s, e) =>
                Lu.ForeColor = Lu.Checked ? AccentVert : TextePrinc;

            ligneLu.Controls.Add(Lu);
            panelFormulaire.Controls.Add(ligneLu, 0, 5);

            var blocBoutons = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0),
                Padding = new Padding(6),
                BackColor = Color.Transparent
            };

            blocBoutons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            blocBoutons.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            blocBoutons.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

            var ligneBoutonsHaut = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0),
                BackColor = Color.Transparent
            };

            ligneBoutonsHaut.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            ligneBoutonsHaut.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            ligneBoutonsHaut.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Ajouter = new Button
            {
                Text = "＋  Ajouter",
                Dock = DockStyle.Fill,
                Margin = new Padding(6, 6, 4, 6),
                BackColor = Accent,
                ForeColor = Fond,
                FlatStyle = FlatStyle.Flat,
                Font = FonteBouton,
                Cursor = Cursors.Hand
            };
            Ajouter.FlatAppearance.BorderSize = 0;
            Ajouter.FlatAppearance.MouseOverBackColor = Color.FromArgb(130, 200, 255);
            Ajouter.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 150, 210);

            Modifier = new Button
            {
                Text = "✎  Modifier",
                Dock = DockStyle.Fill,
                Margin = new Padding(4, 6, 6, 6),
                BackColor = AccentVert,
                ForeColor = Fond,
                FlatStyle = FlatStyle.Flat,
                Font = FonteBouton,
                Cursor = Cursors.Hand
            };
            Modifier.FlatAppearance.BorderSize = 0;
            Modifier.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 220, 165);
            Modifier.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 170, 115);

            Supprimer = new Button
            {
                Text = "✕  Supprimer",
                Dock = DockStyle.Fill,
                Margin = new Padding(6, 0, 6, 6),
                Enabled = false,
                BackColor = Color.FromArgb(50, 35, 35),
                ForeColor = Color.FromArgb(150, 100, 90),
                FlatStyle = FlatStyle.Flat,
                Font = FonteBouton,
                Cursor = Cursors.Hand
            };
            Supprimer.FlatAppearance.BorderSize = 0;
            Supprimer.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 45, 40);
            Supprimer.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 30, 20);

            Supprimer.EnabledChanged += (s, e) =>
            {
                if (Supprimer.Enabled)
                {
                    Supprimer.BackColor = AccentRouge;
                    Supprimer.ForeColor = Fond;
                }
                else
                {
                    Supprimer.BackColor = Color.FromArgb(50, 35, 35);
                    Supprimer.ForeColor = Color.FromArgb(150, 100, 90);
                }
            };

            ligneBoutonsHaut.Controls.Add(Ajouter, 0, 0);
            ligneBoutonsHaut.Controls.Add(Modifier, 1, 0);

            blocBoutons.Controls.Add(ligneBoutonsHaut, 0, 0);
            blocBoutons.Controls.Add(Supprimer, 0, 1);

            panelFormulaire.Controls.Add(blocBoutons, 0, 6);

            var lblAide = new Label
            {
                Text = "◈  Double-clique sur un livre pour le modifier",
                Dock = DockStyle.Top,
                AutoSize = true,
                ForeColor = TexteSecond,
                Font = FonteAide,
                Margin = new Padding(8, 12, 8, 0),
                BackColor = Color.Transparent
            };

            panelFormulaire.Controls.Add(lblAide, 0, 7);

            var panelListe = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(24),
                BackColor = Surface,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            panelListe.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelListe.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            panelListe.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var lblListe = new Label
            {
                Text = "Liste des livres",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = FonteTitrePage,
                ForeColor = AccentVert,
                BackColor = Color.Transparent
            };

            Livres = new ListBox
            {
                Dock = DockStyle.Fill,
                IntegralHeight = false,
                Font = FonteListe,
                HorizontalScrollbar = true,
                BackColor = SurfaceHaute,
                ForeColor = TextePrinc,
                BorderStyle = BorderStyle.None,
                SelectionMode = SelectionMode.One
            };

            Livres.DrawMode = DrawMode.OwnerDrawFixed;
            Livres.ItemHeight = 34;
            Livres.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                bool selectionne = (e.State & DrawItemState.Selected) != 0;

                Color bgItem = selectionne
                    ? Color.FromArgb(40, 99, 170)
                    : (e.Index % 2 == 0 ? SurfaceHaute : Color.FromArgb(28, 32, 50));

                e.Graphics.FillRectangle(new SolidBrush(bgItem), e.Bounds);

                if (selectionne)
                    e.Graphics.FillRectangle(new SolidBrush(Accent),
                        new Rectangle(e.Bounds.X, e.Bounds.Y, 4, e.Bounds.Height));

                string texte = Livres.Items[e.Index]?.ToString() ?? "";
                Color couleurTexte = selectionne ? Color.White : TextePrinc;

                TextRenderer.DrawText(e.Graphics, texte, FonteListe,
                    new Rectangle(e.Bounds.X + 14, e.Bounds.Y,
                                  e.Bounds.Width - 14, e.Bounds.Height),
                    couleurTexte,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            };

            panelListe.Controls.Add(lblListe, 0, 0);
            panelListe.Controls.Add(Livres, 0, 1);

            principal.Controls.Add(panelFormulaire, 0, 0);
            principal.Controls.Add(panelListe, 1, 0);

            Controls.Add(principal);
        }

        private Control CreerLigneTexte(string texteLabel, out TextBox textBox)
        {
            var ligne = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(6),
                BackColor = Color.Transparent
            };

            ligne.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 150F));
            ligne.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            ligne.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));

            var label = new Label
            {
                Text = texteLabel,
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = FonteLabel,
                ForeColor = TexteSecond,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 0, 6)
            };

            textBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = FonteChamp,
                BackColor = SurfaceHaute,
                ForeColor = TextePrinc,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 2, 0, 0)
            };

            ligne.Controls.Add(label, 0, 0);
            ligne.Controls.Add(textBox, 0, 1);

            return ligne;
        }

        private Control CreerLigneGenre()
        {
            var ligne = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(6),
                BackColor = Color.Transparent
            };

            ligne.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 150F));
            ligne.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            ligne.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));

            var label = new Label
            {
                Text = "Genre :",
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = FonteLabel,
                ForeColor = TexteSecond,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 0, 6)
            };

            Genre = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = FonteChamp,
                BackColor = SurfaceHaute,
                ForeColor = TextePrinc,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 2, 0, 0)
            };

            Genre.Items.AddRange(new string[] { "Roman", "SF", "Fantasy", "Policier", "Autre" });

            ligne.Controls.Add(label, 0, 0);
            ligne.Controls.Add(Genre, 0, 1);

            return ligne;
        }

        private void LierEvenements()
        {
            Ajouter.Click += Ajouter_Click;
            Modifier.Click += Modifier_Click;
            Supprimer.Click += Supprimer_Click;
            Livres.SelectedIndexChanged += Livres_SelectedIndexChanged;
            Livres.DoubleClick += Livres_DoubleClick;
        }

        private string ValiderFormulaire()
        {
            string erreurs = "";

            if (string.IsNullOrWhiteSpace(Titre.Text) || Titre.Text.Trim().Length < 2)
                erreurs += "- Le titre doit contenir au moins 2 caractères.\n";

            if (string.IsNullOrWhiteSpace(Auteur.Text) || Auteur.Text.Trim().Length < 2)
                erreurs += "- L'auteur doit contenir au moins 2 caractères.\n";

            if (!int.TryParse(Annee.Text.Trim(), out int annee) || annee < 1800 || annee > DateTime.Now.Year)
                erreurs += $"- L'année doit être un nombre valide entre 1800 et {DateTime.Now.Year}.\n";

            if (Genre.SelectedItem == null)
                erreurs += "- Vous devez sélectionner un genre.\n";

            return erreurs;
        }

        private void ViderFormulaire()
        {
            Titre.Clear();
            Auteur.Clear();
            Annee.Clear();
            Genre.SelectedIndex = -1;
            Lu.Checked = false;
            Livres.ClearSelected();
        }

        private void ActualiserListe()
        {
            Livres.BeginUpdate();
            Livres.Items.Clear();

            foreach (var livre in bibliotheque)
                Livres.Items.Add(livre);

            Livres.EndUpdate();
        }

        private void Ajouter_Click(object? sender, EventArgs e)
        {
            string erreurs = ValiderFormulaire();

            if (!string.IsNullOrEmpty(erreurs))
            {
                MessageBox.Show(
                    "Veuillez corriger les erreurs suivantes :\n\n" + erreurs,
                    "Erreur de validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            Livre nouveauLivre = new Livre
            {
                Titre = Titre.Text.Trim(),
                Auteur = Auteur.Text.Trim(),
                Annee = int.Parse(Annee.Text.Trim()),
                Genre = Genre.SelectedItem?.ToString() ?? "Autre",
                Lu = Lu.Checked
            };

            bibliotheque.Add(nouveauLivre);
            ActualiserListe();
            ViderFormulaire();
        }

        private void Livres_SelectedIndexChanged(object? sender, EventArgs e)
        {
            Supprimer.Enabled = Livres.SelectedItem != null;
        }

        private void Livres_DoubleClick(object? sender, EventArgs e)
        {
            if (Livres.SelectedItem is Livre livreSelectionne)
            {
                Titre.Text = livreSelectionne.Titre;
                Auteur.Text = livreSelectionne.Auteur;
                Annee.Text = livreSelectionne.Annee.ToString();
                Genre.SelectedItem = livreSelectionne.Genre;
                Lu.Checked = livreSelectionne.Lu;
            }
        }

        private void Modifier_Click(object? sender, EventArgs e)
        {
            if (Livres.SelectedItem is Livre livreSelectionne)
            {
                string erreurs = ValiderFormulaire();

                if (!string.IsNullOrEmpty(erreurs))
                {
                    MessageBox.Show(
                        "Veuillez corriger les erreurs suivantes :\n\n" + erreurs,
                        "Erreur de validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                livreSelectionne.Titre = Titre.Text.Trim();
                livreSelectionne.Auteur = Auteur.Text.Trim();
                livreSelectionne.Annee = int.Parse(Annee.Text.Trim());
                livreSelectionne.Genre = Genre.SelectedItem?.ToString() ?? "Autre";
                livreSelectionne.Lu = Lu.Checked;

                ActualiserListe();
                ViderFormulaire();
            }
        }

        private void Supprimer_Click(object? sender, EventArgs e)
        {
            if (Livres.SelectedItem is Livre livreSelectionne)
            {
                var confirmation = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer '{livreSelectionne.Titre}' ?",
                    "Confirmation de suppression",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmation == DialogResult.Yes)
                {
                    bibliotheque.Remove(livreSelectionne);
                    ActualiserListe();
                    ViderFormulaire();
                }
            }
        }
    }
}