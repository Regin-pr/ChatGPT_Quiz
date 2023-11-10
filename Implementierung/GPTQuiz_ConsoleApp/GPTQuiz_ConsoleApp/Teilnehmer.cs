using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTQuiz_ConsoleApp
{
    public class Teilnehmer
    {
        string name;
        
        int punkteRunde;
        int punkteGesamt;

        public Teilnehmer(string name) 
        { 
            this.name = name;
            punkteRunde = 0;
            punkteGesamt = 0;
        }

        public string GetName()
        { return name; }
        public bool NameÄndern(string newName)
        {
            if (newName.Length <= 15)
            {
                name = newName;
                return true;
            }
            else
            {
                Console.WriteLine("Ungültiger Name! Ihr Name darf maximal 15 Teichen lang sein!");
                return false;
            }
        }

        public int GetPunkteRunde()
        { return punkteRunde; }
        public void AddPunkteRunde(int punkte)
        { punkteRunde = punkte; }

        public int GetPunkteGesamt()
        { return punkteGesamt; }
        public void AddPunkteGesamt()
        { punkteGesamt += punkteRunde; }
    }
}
