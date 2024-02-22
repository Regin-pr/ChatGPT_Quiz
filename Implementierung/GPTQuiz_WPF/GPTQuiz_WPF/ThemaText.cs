using System;
using System.Collections.Generic;

namespace GPTQuiz_WPF
{
    public partial class ThemaText
    {
        public ThemaText()
        {
            Fragen = new HashSet<Frage>();
        }

        public int ThemaId { get; set; }
        public string? Bezeichnung { get; set; }
        public bool IstThema { get; set; }

        public virtual ICollection<Frage> Fragen { get; set; }

        public ThemaText(string Bezeichnung, bool IstThema)
        { 
            this.Bezeichnung = Bezeichnung;
            this.IstThema = IstThema;
        }
    }
}
