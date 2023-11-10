using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace GPTQuiz_ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Beispieltextdatei zur Nutzung im Quiz liegt in "..\Implementierung\GPTQuiz_ConsoleApp\beispieltext.txt"

            //Aufgabe 6
            //1. Funktionsweise erklären
            Console.WriteLine("--\tChatGPT-Quiz\t--\n\n" +
                "Im folgenden Programm wird die Chat-GPT API genutzt um Multiple Choice-Fragen zu generieren.\n" +
                "Der Nutzer gibt dazu entweder ein entsprechendes Thema ein oder einen längeren Text, zu dem die KI eine Frage erstellen soll.\n" +
                "Die Fragen besitzen immer vier Antwortmöglichkeiten (A-D), von denen jeweils eine richtig ist (Die Richtigkeit der Antworten wird auch von ChatGPT bestimmt).\n\n" +
                "Zuerst wird der Nutzer gefragt, welche der beiden Optionen er nutzen möchte, woraufhin er seine Texteingabe machen soll.\n" +
                "Im letzten Schritt der Erstellung kann der Nutzer die Schwierigkeit der Quizfrage bestimme\n(1: Einfach; 2: Mittel; 3: Schwer; 0: Keine Angabe).\n\n" +
                "Nachdem die Frage mit seinen Antworten generiert und angezeigt wird, soll der Nutzer seine Antwort, in Form des Buchstabens der erwünschten Antwort, geben.\n" +
                "Die Antwort wird vom Programm bewertet und dem Teilnehmerkonto werden bei einer richtigen Antwort Punkte zugeschrieben.\n");

            List<Teilnehmer> teilnehmer = new List<Teilnehmer>();
            Teilnehmer t1 = new Teilnehmer("Regin");
            Teilnehmer t2 = new Teilnehmer("Schmegin");
            Teilnehmer t3 = new Teilnehmer("Walter");
            teilnehmer.Add(t1);
            teilnehmer.Add(t2);
            teilnehmer.Add(t3);

            //Im Prototyp nur ein Teilnehmer, der zu Beginn der Runde angelegt wird
            Console.Write("-- Geben Sie bitte Ihren Namen ein: ");
            Teilnehmer t0 = new Teilnehmer(Console.ReadLine());
            teilnehmer.Add(t0);

            Quiz quiz = new Quiz(t0);

            quiz.Spielen();

            Console.Clear();

            //RANKING
            Console.WriteLine("--\tTeilnehmerrangliste\t--\n");
            List<Teilnehmer> teilnehmerRangliste = teilnehmer.OrderByDescending(t => t.GetPunkteGesamt()).ToList();

            Console.WriteLine("Name:\tPunkte");
            foreach (Teilnehmer t in teilnehmerRangliste)
            {
                Console.WriteLine(t.GetName() + ": " + t.GetPunkteGesamt() + " Punkte");
            }
            Console.ReadLine();
        }
    }

    public enum Schwierigkeitsgrad
    { 
        //Zur Nutzung im Prompt benannt
        einfache,
        mittelschwere,
        schwere,
        Keine_Angabe
    }
}
