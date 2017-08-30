using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holy_Bible.Domain
{
    public class Chapter
    {

        public Int32 number { get; set; }
        public IList<Verse> verses { get; set; }

        public string FirstVerse
        {
            get
            {
                string text = verses.FirstOrDefault().text;
                if (text.Length > 38)
                {
                    text = text.Substring(0, 38);
                    text = text.Substring(0, text.LastIndexOf(" "));
                    return text + "...";
                }
                else
                {
                    return text;
                }
            }
        }

        public Verse GetRandomVerse()
        {
            if (verses == null)
                return new Verse();
            else
                return verses.ElementAt((new Random()).Next(verses.Count));
        }
    }
}
