using System;
using System.Collections.Generic;

namespace GPTQuiz_WPF
{
    public partial class Antwort
    {
        public int FrageId { get; set; }
        public string Buchstabe { get; set; } = null!;
        public string? Text { get; set; }
        public bool? Korrektheit { get; set; }

        public virtual Frage Frage { get; set; } = null!;

        public Antwort() { }

        public Antwort(int FrageId ,string Buchstabe, string Text, bool Korrektheit)
        {
            this.FrageId = FrageId;
            this.Buchstabe = Buchstabe;
            this.Text = Text;
            this.Korrektheit = Korrektheit;
        }
    }
}
