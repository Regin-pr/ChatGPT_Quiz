using System;
using System.Collections.Generic;

namespace GPTQuiz_WPF
{
    public partial class Teilnehmer
    {
        public int TeilnehmerId { get; set; }
        public string? Name { get; set; }
        public int? PunkteGesamt { get; set; }
        public int? PunkteRunde { get; set; }

        public Teilnehmer(string Name)
        {
            //TeilnehmerId = IDCounter++;
            this.Name = Name;
            PunkteGesamt = 0;
            PunkteRunde = 0;
        }

        public void AddPunkteRunde(Schwierigkeitsgrad schwierigkeitsgrad)
        {
            //Punkte werden den Konten hinzugefügt
            //Die Rechnung ist so aufgebaut damit der Teilnehmer, für den Fall dass keine Schwierigkeit festgelegt wurde feste 100 Punkte erhält und für die restlichen Grade 100 Punkte * Schwierigkeitsstufe erhält
            if (schwierigkeitsgrad == Schwierigkeitsgrad.beliebige)
                PunkteRunde = 100;
            else
                PunkteRunde = ((int)schwierigkeitsgrad + 1) * 100;                ;
        }
        public void AddPunkteGesamt()
        { PunkteGesamt += PunkteRunde; }
    }
}
