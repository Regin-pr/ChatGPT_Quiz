using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;

using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;


namespace GPTQuiz_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GPTQuiz_DBContext context = new GPTQuiz_DBContext();

        //Schweirigkeitsgrad wird usrprünglich ohne Angabe besetzt, falls keine Auswahl getätigt wird
        Schwierigkeitsgrad schwierigkeitsgrad = Schwierigkeitsgrad.beliebige;
        int rundeCounter = 0;
        int spielerAntwortDran = 0;

        string anfrageErgebnis;
        string[] anfrageSplit;

        //Liste aller Teilnehmer, die in der DB existieren
        List<Teilnehmer> DBTeilnehmer = new List<Teilnehmer>();

        //Liste aller Teilnehmer aus der DB, die am Quiz teilnehmen sollen
        List<Teilnehmer> quizTeilnehmer = new List<Teilnehmer>();

        List<ThemaText> themenListe = new List<ThemaText>();
        List<ThemaText> textListe = new List<ThemaText>();

        public MainWindow()
        {
            InitializeComponent();
            Startseite.Visibility = Visibility.Visible;

            Seitenleiste.Visibility = Visibility.Collapsed;
            PromptErstellungsUndFrageseite.Visibility = Visibility.Collapsed;
            PromptErstellungsseite.Visibility = Visibility.Collapsed;
            Frageseite.Visibility = Visibility.Collapsed;

            IEnumerable<Teilnehmer> teilnehmers = context.Teilnehmers;
            foreach (Teilnehmer t in teilnehmers)
            {
                DBTeilnehmer.Add(t);
            }

            IEnumerable<ThemaText> themen = context.ThemaTexte;
            foreach (ThemaText tt in themen)
            {
                if (tt.IstThema)
                { themenListe.Add(tt); }
                else
                { textListe.Add(tt); }
            }

            DBTeilnehmerListe.ItemsSource = DBTeilnehmer;
            QuizTeilnehmerListe.ItemsSource = quizTeilnehmer;

            ThemenListView.ItemsSource = themenListe;
            TexteListView.ItemsSource = textListe;     
        }

        private void TeilnehmerAdd_Click(object sender, RoutedEventArgs e)
        {
            Teilnehmer selected = (Teilnehmer)DBTeilnehmerListe.SelectedItem;

            if ((Teilnehmer)DBTeilnehmerListe.SelectedItem != null && selected.Name != null && quizTeilnehmer.Count() < 4)
            {
                quizTeilnehmer.Add(selected);
                quizTeilnehmer = quizTeilnehmer.Distinct().ToList();
                StartseiteExceptionThrow.Text = "";

                QuizTeilnehmerListe.ItemsSource = quizTeilnehmer;
            }
            else if ((Teilnehmer)DBTeilnehmerListe.SelectedItem == null || selected.Name == null)
            {
                StartseiteExceptionThrow.Text = " Bitte wähle einen Teilnehmer aus der linken Tabelle zum Hinzufügen aus!";
            }
            else if (quizTeilnehmer.Count() >= 4)
            { StartseiteExceptionThrow.Text = "Es wurden bereits 4 Teilnehmer ausgewählt."; }
        }

        private void TeilnehmerRemove_Click(object sender, RoutedEventArgs e)
        {
            Teilnehmer selected = (Teilnehmer)QuizTeilnehmerListe.SelectedItem;

            if ((Teilnehmer)QuizTeilnehmerListe.SelectedItem != null && selected.Name != null && quizTeilnehmer.Count() > 0)
            {
                quizTeilnehmer.Remove(selected);
                StartseiteExceptionThrow.Text = "";

                QuizTeilnehmerListe.Items.Refresh();
            }
            else if ((Teilnehmer)DBTeilnehmerListe.SelectedItem == null || selected == null)
            {
                StartseiteExceptionThrow.Text = " Bitte wähle einen Teilnehmer aus der rechten Tabelle zum Entfernen aus!";
            }
            else if (quizTeilnehmer.Count() <= 0)
            { StartseiteExceptionThrow.Text = "Die Teilnehmerliste ist leer."; }
        }

        private void TeilnehmerNeu_Click(object sender, RoutedEventArgs e)
        {
            if (TeilnehmerNeuName.Text != "")
            {
                bool NameExistiert = false;
                foreach (Teilnehmer t in DBTeilnehmer)
                {
                    if (t.Name.ToString() == TeilnehmerNeuName.Text.ToString())
                    {
                        NameExistiert = true;
                        StartseiteExceptionThrow.Text = "Nutzer existiert bereits";
                        break;
                    }
                }
                if (!NameExistiert)
                {
                    DBTeilnehmer.Add(new Teilnehmer(TeilnehmerNeuName.Text));
                    context.Teilnehmers.Add(new Teilnehmer(TeilnehmerNeuName.Text));
                    context.SaveChanges();

                    StartseiteExceptionThrow.Text = "Nutzer erfolgreich hinzugefügt";


                    DBTeilnehmerListe.Items.Refresh();
                }
            }
        }

        private void StartseiteWeiter_Click(object sender, RoutedEventArgs e)
        {
            if (quizTeilnehmer.Count() == 0)
            { StartseiteExceptionThrow.Text = "Bitte füge mindestens einen Teilnehmer hinzu"; }
            else
            {
                Startseite.Visibility = Visibility.Collapsed;
                PromptErstellungsUndFrageseite.Visibility = Visibility.Visible;
                Seitenleiste.Visibility = Visibility.Visible;
                PromptErstellungsseite.Visibility = Visibility.Visible;
                GenerierTextBestaetigung.Visibility = Visibility.Collapsed;

                Frageseite.Visibility = Visibility.Collapsed;
                AntwortUndNeueRundePanel.Visibility = Visibility.Collapsed;

                QuizTeilnehmerSeitenleiste.ItemsSource = quizTeilnehmer;

                //Jede Runde darf einer der Teilnehmer den Prompt bestimmen
                //Die Auswähl dafür findet abwechselnd mithilfe der folgenden Modulo-Rechnung statt
                //rundeCounter % quizTeilnehmer.Count() <-vor increment
                PromptErstellungTeilnehmerNameAnzeige.Text = quizTeilnehmer[rundeCounter % quizTeilnehmer.Count()].Name
                    + ", bestimme deine nächste Frage!";

                RundenanzeigeSeitenleiste.Text = "Runde: " + ++rundeCounter;

                //Thema-Knopf vorausgewählt
                ToggleThema.IsChecked = true;
            }
        }

        private void ThemaTextAuswahl_Click(object sender, RoutedEventArgs e)
        {
            if (ThemaTextTab.SelectedItem == ThemaTabItem)
            {
                ThemaText tt = (ThemaText)ThemenListView.SelectedItem;
                ThemaBox.Text = tt.Bezeichnung;
            }
            else if (ThemaTextTab.SelectedItem == TextTabItem)
            {
                ThemaText themaText = (ThemaText)TexteListView.SelectedItem;
                TexteBox.Text = themaText.Bezeichnung;
            }
        }

        private void ToggleThema_Checked(object sender, RoutedEventArgs e)
        {
            ToggleText.IsChecked = false;
        }

        private void ToggleText_Checked(object sender, RoutedEventArgs e)
        {
            ToggleThema.IsChecked = false;
        }

        private void ToggleEinfach_Checked(object sender, RoutedEventArgs e)
        {
            ToggleMittel.IsChecked = false;
            ToggleSchwer.IsChecked = false;
        }

        private void ToggleMittel_Checked(object sender, RoutedEventArgs e)
        {
            ToggleEinfach.IsChecked = false;
            ToggleSchwer.IsChecked = false;
        }

        private void ToggleSchwer_Checked(object sender, RoutedEventArgs e)
        {
            ToggleEinfach.IsChecked = false;
            ToggleMittel.IsChecked = false;
        }

        private async void FrageGenerieren_Click(object sender, RoutedEventArgs e)
        {            
            GenerierTextBestaetigung.Visibility = Visibility.Visible;
            Quiz quiz = new Quiz();
            //Quiz quiz = new Quiz(context.Teilnehmers.Local.ToArray());

            if (ToggleEinfach.IsChecked == true) 
            { schwierigkeitsgrad = Schwierigkeitsgrad.einfache; }
            else if(ToggleMittel.IsChecked == true) 
            { schwierigkeitsgrad = Schwierigkeitsgrad.mittelschwere; }
            else if(ToggleSchwer.IsChecked == true)
            { schwierigkeitsgrad = Schwierigkeitsgrad.schwere; }

            if (ToggleThema.IsChecked == true && ThemaBox.Text != "" || ToggleText.IsChecked == true && TexteBox.Text != "")
            {
                GenerierTextBestaetigung.Text = "-- Frage wird generiert --";
                
                ChatMessage msg = new ChatMessage();

                ChatMessage[] prompts = new ChatMessage[1];

                if (ToggleThema.IsChecked == true)
                {
                    //Request mit Thema-String
                    msg.TextContent = "Formuliere eine " + schwierigkeitsgrad.ToString() + " Allgemeinwissens-Quizfrage mit vier(4) " +
                                "Antwortmöglichkeiten von A - D, wovon nur eine korrekt ist, zu folgendem Thema mit höchstens 350 Zeichen: " + ThemaBox.Text +
                                " | Stelle sicher, dass am Ende der Ausgabe die richtige Antwort in folgender Form steht: \"Richtig:B\"";
                }
                else if (ToggleText.IsChecked == true)
                {
                    //Request mit Text-String
                    msg.TextContent = "Formuliere eine " + schwierigkeitsgrad.ToString() + " Allgemeinwissens-Quizfrage mit vier(4) " +
                                "Antwortmöglichkeiten von A - D, wovon nur eine korrekt ist, zu folgendem Text mit hochstens 350 Zeichen: '" + TexteBox.Text +
                                "' | Stelle sicher, dass am Ende der Ausgabe die richtige Antwort in folgender Form steht: \"Richtig:B\". Die richtige Antwort muss in der Ausgabe enthalten sein! Deine Antwort darf nicht mehr als 350 Zeichen lang sein!";
                }
                
                prompts[0] = msg;

                Task<ChatResult> anfrage = quiz.AnfrageMachen(prompts);
                await anfrage;

                anfrageErgebnis = anfrage.Result.ToString();

                anfrageSplit = AnfrageSplit(anfrageErgebnis);

                //Bei erfolgreicher Generierung das Thema / den Text und die neue Frage mit seinen Antworten der DB zufügen
                if (anfrageSplit != null)
                {
                    //Bei der Überprüfung, ob ein Thema / Text bereits in der DB existiert wird gegebenenfalls
                    //die ID des vorhandenen Themas gespeichert, um die neu generierte Frage diesem hinzuzufügen
                    int thematextID = -1;
                    if (ToggleThema.IsChecked == true)
                    {
                        foreach (ThemaText tt in context.ThemaTexte)
                        {
                            //Das Thema existiert bereits
                            if (tt.Bezeichnung.ToLower() == ThemaBox.Text.ToLower())
                            {
                                thematextID = tt.ThemaId;
                            }
                        }

                        if (thematextID == -1)
                        {
                            //Das Thema existiert noch nicht, also wird es neu angelegt
                            //und die ID des neusten Objekts in der Variablen gespeichert
                            context.ThemaTexte.Add(new ThemaText(ThemaBox.Text, true));
                            context.SaveChanges();

                            thematextID = context.ThemaTexte.Count();

                            themenListe.Add(new ThemaText(ThemaBox.Text, true));

                            context.SaveChanges();

                            ICollectionView ThemenView = CollectionViewSource.GetDefaultView(themenListe);
                            ThemenView.Refresh();
                        }
                    }
                    else if (ToggleText.IsChecked == true)
                    {
                        //Dasselbe auch für Texte
                        foreach (ThemaText tt in context.ThemaTexte)
                        {
                            if (tt.Bezeichnung.ToLower() == TexteBox.Text.ToLower())
                            {
                                thematextID = tt.ThemaId;
                            }
                        }

                        if (thematextID == -1)
                        {
                            context.ThemaTexte.Add(new ThemaText(TexteBox.Text, false));
                            context.SaveChanges();

                            thematextID = context.ThemaTexte.Count();

                            textListe.Add(new ThemaText(TexteBox.Text, false));

                            context.SaveChanges();

                            ICollectionView TexteView = CollectionViewSource.GetDefaultView(textListe);
                            TexteListView.Items.Refresh();
                        }
                    }

                    IEnumerable<ThemaText> themen = context.ThemaTexte;

                    context.Fragen.Add(new Frage(thematextID, anfrageSplit[0], (int)schwierigkeitsgrad + 1));

                    context.SaveChanges();

                    IEnumerable<Frage> fragen = context.Fragen;

                    for (int i = 0; i < 4; i++)
                    {
                        string Buchstabe = anfrageSplit[i + 1].Remove(1);
                        string Text = anfrageSplit[i + 1].Substring(2);
                        bool Korrektheit = false;
                        if (anfrageSplit[5] == anfrageSplit[i + 1].Remove(1))
                        { Korrektheit = true; }
                        Text.TrimStart();

                        context.Antworten.Add(new Antwort(fragen.Last().FrageId, Buchstabe, Text, Korrektheit));
                    }
                    
                    context.SaveChanges();
                }

                try
                {
                    Fragetext.Text = anfrageSplit[0];
                    AnswerButtonA.Content = anfrageSplit[1];
                    AnswerButtonB.Content = anfrageSplit[2];
                    AnswerButtonC.Content = anfrageSplit[3];
                    AnswerButtonD.Content = anfrageSplit[4];
                }
                catch
                {
                    GenerierTextBestaetigung.Text = "Die Frage konnte nicht generiert werden. Bitte überprüfe deinen eingegebenen Text";
                    return;
                }

                    SpielerFrage.Content = quizTeilnehmer[spielerAntwortDran].Name + ", wähle deine Antwort:";
                    SpielerFrage.Visibility = Visibility.Visible;

                    AntwortUndNeueRundeText.Text = "Die richtige Antwort lautet: " + anfrageSplit[5] + "\nDie erspieleten Punkte wurden den Teilnehmern gutgeschrieben" +
                        "\nEine weitere Runde mit denselben Teilnehmern oder zurück zur Startseite?";

                    Frageseite.Visibility = Visibility.Visible;
                    PromptErstellungsseite.Visibility = Visibility.Collapsed;  
            }
            else
            {
                GenerierTextBestaetigung.Text = "Bitte füge dein erwünschtes Thema / deinen erwünschten Text ein!";
            }
        }

        private string[] AnfrageSplit(string anfrageErgebnis)
        {
            string[] anfrageErgebnisSplit = new string[6];

            //damit der Teilnehmer das Ergebnis nicht komplett angezeigt bekommt
            if (anfrageErgebnis.Contains("Richtig"))
            {
                //Die Position bei der das richtige Ergebnis im String beginnt wird markiert und ein seperater Antwortstring erstellt
                int antwortIndex = anfrageErgebnis.IndexOf("Richtig", 0);
                string antwortString = anfrageErgebnis.Substring(antwortIndex + 8);
                antwortString = antwortString.TrimStart();
                antwortString = antwortString.Substring(0, 1);

                //Der Antwortstring wird von der Frage getrennt
                anfrageErgebnis = anfrageErgebnis.Remove(antwortIndex);

                //Frage von den Antwortmöglichkeiten trennen (wird mit Liste implementiert um zusätzliche Einträge durch unregelmäßige Formatierung zu vermieden)
                List<string> frageSplit = new List<string>();

                string[] tempArray = anfrageErgebnis.Split("\n");
                //tempArray = tempArray[1].Split(",");
                foreach (string s in tempArray)
                { frageSplit.Add(s); }

                anfrageErgebnisSplit[0] = frageSplit[0];

                int offset = 0;
                for (int i = 1; i <= 4; i++)
                {
                    //Vorbeugen einer häufig auftretenden Formatierungsunregelmäßigkeit
                    while(frageSplit[i + offset] == "")
                    { offset++; }
                    
                    anfrageErgebnisSplit[i] = frageSplit[i + offset];
                }

                anfrageErgebnisSplit[5] = antwortString;
            }
            //falls Richtig nicht enthalten ist:
            else
            {
                rundeCounter -= 1;
                GenerierTextBestaetigung.FontSize += 5;
                GenerierTextBestaetigung.Text = "-- Unübliche Formatierung! --\n" +
                    "Das Ergebnis konnte nicht ordnungsgemäß ausgelesen werden und die Frageerstellung wurde somit abgebrochen!";

                return null;
            }

            return anfrageErgebnisSplit;
        }

        private void AnswerButtonA_Click(object sender, RoutedEventArgs e)
        {
            //Überprüfen, ob nicht schon jeder geantwortet hat um Punktezuwachs zu vermeiden
            if (spielerAntwortDran != quizTeilnehmer.Count)
            {
                if (anfrageSplit[5].Substring(anfrageSplit[5].Length - 1, 1).ToUpper() == "A")
                { quizTeilnehmer[spielerAntwortDran].AddPunkteRunde(schwierigkeitsgrad); }

                TeilnehmerGeantwortetCheck();
            }
        }

        private void AnswerButtonB_Click(object sender, RoutedEventArgs e)
        {
            //Überprüfen, ob nicht schon jeder geantwortet hat um Punktezuwachs zu vermeiden
            if (spielerAntwortDran != quizTeilnehmer.Count)
            {
                if (anfrageSplit[5].Substring(anfrageSplit[5].Length - 1, 1).ToUpper() == "B")
                { quizTeilnehmer[spielerAntwortDran].AddPunkteRunde(schwierigkeitsgrad); }

                TeilnehmerGeantwortetCheck();
            }
        }

        private void AnswerButtonC_Click(object sender, RoutedEventArgs e)
        {
            //Überprüfen, ob nicht schon jeder geantwortet hat um Punktezuwachs zu vermeiden
            if (spielerAntwortDran != quizTeilnehmer.Count)
            {
                if (anfrageSplit[5].Substring(anfrageSplit[5].Length - 1, 1).ToUpper() == "C")
                { quizTeilnehmer[spielerAntwortDran].AddPunkteRunde(schwierigkeitsgrad); }

                TeilnehmerGeantwortetCheck();
            }
        }

        private void AnswerButtonD_Click(object sender, RoutedEventArgs e)
        {
            //Überprüfen, ob nicht schon jeder geantwortet hat um Punktezuwachs zu vermeiden
            if (spielerAntwortDran != quizTeilnehmer.Count)
            {
                if (anfrageSplit[5].Substring(anfrageSplit[5].Length - 1, 1).ToUpper() == "D")
                { quizTeilnehmer[spielerAntwortDran].AddPunkteRunde(schwierigkeitsgrad); }

                TeilnehmerGeantwortetCheck();
            }
        }

        //Überprüfen, ob alle Teilnehmer Ihre Antwort abgegeben haben
        private void TeilnehmerGeantwortetCheck()
        {
            if (spielerAntwortDran == quizTeilnehmer.Count - 1)
            {
                SpielerFrage.Visibility = Visibility.Collapsed;

                AntwortUndNeueRundePanel.Visibility = Visibility.Visible;

                foreach (Teilnehmer t in quizTeilnehmer)
                {
                    t.AddPunkteGesamt();
                    context.SaveChanges();
                }
                QuizTeilnehmerSeitenleiste.Items.Refresh();
            }
            else
            {
                spielerAntwortDran++;
                SpielerFrage.Content = quizTeilnehmer[spielerAntwortDran].Name + ", wähle deine Antwort:";
            }
        }

        private void NeueRundeButton_Click(object sender, RoutedEventArgs e)
        {
            ThemaBox.Text = "";
            TexteBox.Text = "";

            spielerAntwortDran = 0;

            StartseiteWeiter_Click(sender, e);
        }

        private void StartseiteBackButton_Click(object sender, RoutedEventArgs e)
        {
            QuizTeilnehmerListe.Items.Refresh();
            ThemaBox.Text = "";
            TexteBox.Text = "";

            spielerAntwortDran = 0;

            Startseite.Visibility = Visibility.Visible;

            Seitenleiste.Visibility = Visibility.Collapsed;
            PromptErstellungsUndFrageseite.Visibility = Visibility.Collapsed;
            PromptErstellungsseite.Visibility = Visibility.Collapsed;
            GenerierTextBestaetigung.Visibility = Visibility.Collapsed;
            
            Frageseite.Visibility = Visibility.Collapsed;

            AntwortUndNeueRundePanel.Visibility = Visibility.Collapsed;
        }
    }
}