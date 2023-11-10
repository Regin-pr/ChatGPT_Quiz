using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenAI_API;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.IO;

namespace GPTQuiz_ConsoleApp
{
    internal class Quiz
    {
        //KEY ENTFERNEN VOR DEM HOCHLADEN!!!
        OpenAIAPI api = new OpenAIAPI("API-KEY-HIER");

        //In der Konsolenanwendung des Prototypen gibt es nur einen Nutzer, der Eingaben macht, aber eigentlich sollten mehrere Spieler teilnehmen können
        Teilnehmer teilnehmer;

        int rundeCounter = 0;
        string eingabeStorage;
        Schwierigkeitsgrad schwierigkeitsgrad;

        bool istThema;
        string thema_text;
        List<string> themen_texte = new List<string>();

        public Quiz(Teilnehmer t)
        {
            teilnehmer = t;
        }

        public void Spielen()
        {
            Console.WriteLine("-----------\n| Runde " + ++rundeCounter + " |\n-----------\n");

            Console.WriteLine("\nMöchten Sie ein Thema oder einen längeren Text nutzen?\n");

            //2. Eingaben einlesen
            while (true)
            {
                Console.Write("Bitte geben Sie entweder 'Thema' oder 'Text' ein: ");
                eingabeStorage = Console.ReadLine();

                if (eingabeStorage.ToLower() == "thema")
                {
                    Console.Write("Was soll das Thema für die Frage sein?: ");
                    thema_text = Console.ReadLine();

                    istThema = true;
                    themen_texte.Add(thema_text);
                    Console.WriteLine("\n--\tThema gespeichert\t--\n");
                    break;
                }
                else if (eingabeStorage.ToLower() == "text")
                {
                    Console.WriteLine("Schreiben Sie den Text, den Sie benutzen wollen in eine Textdatei und fügen Sie den Dateienpfad hier ein:\n" +
                        "(Notiz:\tJe länger der Text, desto besser kann die KI eine Frage dazu erstellen)\n");

                    thema_text = TextEinlesen(Console.ReadLine());

                    istThema = false;
                    themen_texte.Add(thema_text);
                    Console.WriteLine("\n--\tText gespeichert\t--\n");
                    break;
                }
                else
                {
                    Console.WriteLine("____________________________________\n" +
                    "--\tInkorrekte Eingabe\t--\nBitte tätigen Sie eine neue Eingabe!\n" +
                    "____________________________________\n");
                }
            }

            Console.WriteLine("Sie können nun zusätzlich den Schwierigkeitsgrad Ihrer Frage angeben.\n" +
                "- '1' für eine einfache Frage\n" +
                "- '2' für eine mittelschwere Frage\n" +
                "- '3' für eine schwere Frage\n" +
                "- Jede beliebige andere Eingabe um keine Angabe zur Schwierigkeit zu machen\n" +
                "  (Frage wird punktemäßig wie eine einfache Frage bewertet!)\n");
            Console.Write("Eingabe: ");

            eingabeStorage = Console.ReadLine();

            if (eingabeStorage == "1")
            {
                schwierigkeitsgrad = Schwierigkeitsgrad.einfache;
                Console.WriteLine("'Einfach' wurde als Schwierigkeitsgrad festgelegt.");
            }
            else if (eingabeStorage == "2")
            {
                schwierigkeitsgrad = Schwierigkeitsgrad.mittelschwere;
                Console.WriteLine("'Mittel' wurde als Schwierigkeitsgrad festgelegt.");
            }
            else if (eingabeStorage == "3")
            {
                schwierigkeitsgrad = Schwierigkeitsgrad.schwere;
                Console.WriteLine("'Schwer' wurde als Schwierigkeitsgrad festgelegt.");
            }
            else
            {
                schwierigkeitsgrad = Schwierigkeitsgrad.Keine_Angabe;
                Console.WriteLine("Es wurde kein Schwierigkeitsgrad festgelegt.");
            }

            ChatMessage msg = new ChatMessage();

            ChatMessage[] prompts = new ChatMessage[1];

            //3. Prompt anpassen

            //THEMA
            if (istThema)
            {
                //Anpassung des Prompts mithilfe der Angaben des Themas und des Schwierigkeitsgrads und Abweichung falls der Nutzer keine Angabe zum Schwierigkeitsgrad gemacht hat
                if (schwierigkeitsgrad == Schwierigkeitsgrad.Keine_Angabe)
                {
                    msg.Content = "Formuliere eine Allgemeinwissens-Quizfrage mit vier(4) " +
                        "Antwortmöglichkeiten von A - D, wovon nur eine korrekt ist, zu folgendem Thema mit höchstens 350 Zeichen: " + thema_text +
                        " | Stelle sicher, dass am Ende der Ausgabe die richtige Antwort in folgender Form steht: \"Richtig:B\"(oder der jeweils korrekte Buchstabe). Deine Antwort darf nicht länger als 350 Zeichen sein!";
                }
                else
                {
                    msg.Content = "Formuliere eine " + schwierigkeitsgrad.ToString() + " Allgemeinwissens-Quizfrage mit vier(4) " +
                            "Antwortmöglichkeiten von A - D, wovon nur eine korrekt ist, zu folgendem Thema mit höchstens 350 Zeichen: " + thema_text +
                            " | Stelle sicher, dass am Ende der Ausgabe die richtige Antwort in folgender Form steht: \"Richtig:B\"(oder der jeweils korrekte Buchstabe). Deine Antwort darf nicht länger als 350 Zeichen sein!";
                }
            }
            //TEXT
            else
            {
                //Anpassung des Prompts mithilfe der Angaben des Themas und des Schwierigkeitsgrads und Abweichung falls der Nutzer keine Angabe zum Schwierigkeitsgrad gemacht hat
                if (schwierigkeitsgrad == Schwierigkeitsgrad.Keine_Angabe)
                {
                    msg.Content = "Formuliere eine Allgemeinwissens-Quizfrage mit vier(4) " +
                        "Antwortmöglichkeiten von A - D, wovon nur eine korrekt ist, zu folgendem Text mit höchstens 350 Zeichen: " + thema_text +
                        " | Stelle sicher, dass am Ende der Ausgabe die richtige Antwort in folgender Form steht: \"Richtig:B\"(oder der jeweils korrekte Buchstabe). Deine Antwort darf nicht länger als 350 Zeichen sein!";
                }
                else
                {
                    msg.Content = "Formuliere eine " + schwierigkeitsgrad.ToString() + " Allgemeinwissens-Quizfrage mit vier(4) " +
                            "Antwortmöglichkeiten von A - D, wovon nur eine korrekt ist, zu folgendem Text mit hochstens 350 Zeichen: " + thema_text +
                            " | Stelle sicher, dass am Ende der Ausgabe die richtige Antwort in folgender Form steht: \"Richtig:B\"(oder der jeweils korrekte Buchstabe). Deine Antwort darf nicht länger als 350 Zeichen sein!";
                }
            }

            //4. Prompt an API über Methode schicken und als String zurück erhalten
            prompts[0] = msg;

            ////HAT NACH VIELEM PROBIEREN NICHT REGELMÄßIG FUNKTIONIERT!
            //Zweite Anfrage, die die vorherige Ausgabe formatierten lässt
            //prompts = new ChatMessage[2];
            //ChatMessage msg2 = new ChatMessage();
            //msg2.Content = "Formatierte bitte deine letzte Ausgabe, sodass sie exakt dem folgendem Beispiel folgt inklusive aller Leerzeichen und Kommata und ohne Zeilenumbrüche!: \"Wo steht der Eiffelturm?A) Rom,B) Paris,C) Bordeaux,D) Tokio,Richtig:B\"";
            //prompts[1] = msg2;

            string ergebnis = AnfrageMachen(prompts).Result.ToString();

            //5. Antwort auslesen und dem Nutzer anzeigen

            if (ergebnis.Contains("Richtig"))
            {
                //Die übliche Formatierung der ChatGPT-Ausgabe wurde eingehalten
                //und das Ergebnis kann in Frage + Antworten und Richtige Antwort unterteilt werden,
                //damit der Teilnehmer das Ergebnis nicht komplett angezeigt bekommt
                int antwortIndex = ergebnis.IndexOf("Richtig", 0);
                string antwortString = ergebnis.Substring(antwortIndex, ergebnis.Length - antwortIndex);
                ergebnis = ergebnis.Remove(antwortIndex);

                Console.WriteLine("--\tFrage\t--\n" + ergebnis);
                Console.WriteLine("> Geben Sie bitte den Buchstaben der Antwort, die Sie für korrekt befinden ein!\n[A, B, C, D]: ");
                eingabeStorage = Console.ReadLine();

                if (eingabeStorage.ToUpper() == antwortString.Substring(antwortString.Length - 1, 1).ToUpper())
                {
                    Console.WriteLine("RICHTIG! Die korrekte Antwort war " + antwortString.Substring(antwortString.Length - 1, 1));
                    //Punkte werden den Konten hinzugefügt
                    //Die Rechnung ist so aufgebaut damit der Teilnehmer, für den Fall dass keine Schwierigkeit festgelegt wurde feste 100 Punkte erhält und für die restlichen Grade 100 Punkte * Schwierigkeitsstufe erhält
                    if (schwierigkeitsgrad == Schwierigkeitsgrad.Keine_Angabe)
                        teilnehmer.AddPunkteRunde(100);
                    else
                        teilnehmer.AddPunkteRunde( ((int)schwierigkeitsgrad + 1) * 100);

                    teilnehmer.AddPunkteGesamt();
                }
                else
                    Console.WriteLine("FALSCH! Die korrekte Antwort war " + antwortString.Substring(antwortString.Length - 1, 1));

                Console.WriteLine("Sie haben nun " + teilnehmer.GetPunkteGesamt() + " Punkte!");
            }
            else
            {
                //Die Formatierung der GPT-Ausgabe scheint das Wort 'Richtig' zu enthalten, wodurch ein Aufsplitten nicht erfolgen kann
                Console.WriteLine("Unübliche Formatierung! Das Ergebnis konnte nicht ordnungsgemäß ausgelesen werden und die Runde wird somit abgebrochen!");
                Console.WriteLine("\nAusgabe:\n" + ergebnis + "\n____________________\n");
                rundeCounter -= 1;
            }

            Console.WriteLine("Eine weitere Runde spielen?");
            Console.WriteLine("'j' / 'n' : ");

            if (Console.ReadLine().ToLower() == "j" || Console.ReadLine().ToLower() == "ja" || Console.ReadLine().ToLower() == "y" || Console.ReadLine().ToLower() == "yes")
            {
                Console.Clear();
                Spielen();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Gespielte Runden: " + rundeCounter);
                Console.WriteLine("Dein Endpunktestand: " + teilnehmer.GetPunkteGesamt());
                Console.WriteLine("Danke fürs Spielen!");
                Console.ReadLine();
            }

            ////HAT NACH VIELEM AUSPROBIEREN NICHT REGELMÄßIG FUNKTIONIERT!
            ////Erst Ausplitten in Frage und Antwort, da die Frage Kommata enthalten kann, welches als Trennzeichen benutzt werden soll
            //string[] ergSplit = ergebnis.Split('?');
            //string frage = ergSplit[0];

            //Dann erneut Splitten in die einzelnen Antwort-Komponenten
            //ergSplit = ergSplit[1].Split(',');

            //Ausgabe
            //if (ergSplit.Length == 5)
            //{
            //    Console.WriteLine("\nFrage: " + frage);
            //    Console.WriteLine(ergSplit[0] + "  |  " + ergSplit[1]);
            //    Console.WriteLine(ergSplit[2] + "  |  " + ergSplit[3]);

            //    Console.WriteLine("\nGeben Sie bitte den Buchstaben der Antwort, die Sie für korrekt befinden ein!\n[A, B, C, D]: ");
            //    eingabeStorage = Console.ReadLine();

            //    if (eingabeStorage.ToUpper() == ergSplit[4].Substring(ergSplit[4].Length - 1, 1).ToUpper())
            //        Console.WriteLine("RICHTIG! Die korrekte Antwort war " + ergSplit[4].Substring(ergSplit[4].Length - 1, 1));
            //    else
            //        Console.WriteLine("FALSCH! Die korrekte Antwort war " + ergSplit[4].Substring(ergSplit[4].Length - 1, 1));
            //}
            //else
            //{ 
            //    Console.WriteLine("Formatierungsanfrage hat scheinbar nicht geklappt!");
            //    Console.WriteLine(ergebnis);
            //}
        }

        //6. Funktionen zum Auslagern nutzen
        public string TextEinlesen(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                String text = sr.ReadToEnd();
                return text;
            }
        }

        public Task<ChatResult> AnfrageMachen(ChatMessage[] messages)
        {
            ChatRequest request = new ChatRequest();

            request.Model = Model.ChatGPTTurbo;
            request.Temperature = 0.1;
            request.MaxTokens = 100;
            request.Messages = messages;

            Console.WriteLine("--\tFrage wird generiert\t--");

            Task<ChatResult> result = api.Chat.CreateChatCompletionAsync(request);
            
            result.Wait();

            Console.Clear();
            return result;
        }
    }
}
