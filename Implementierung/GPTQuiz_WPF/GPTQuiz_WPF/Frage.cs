using System;
using System.Collections.Generic;

namespace GPTQuiz_WPF
{
    public partial class Frage
    {
        public Frage()
        {
            Antworts = new HashSet<Antwort>();
        }

        public int FrageId { get; set; }
        public int? ThemaId { get; set; }
        public string? Text { get; set; }
        public int? Schwierigkeit { get; set; }

        public virtual ThemaText? Thema { get; set; }
        public virtual ICollection<Antwort> Antworts { get; set; }

        public Frage(int ThemaId, string Text, int Schwierigkeit)
        { 
            this.ThemaId = ThemaId;
            this.Text = Text;
            this.Schwierigkeit = Schwierigkeit;
        }
    }
}
