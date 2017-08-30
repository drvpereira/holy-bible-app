using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Media;

namespace Holy_Bible.Domain
{
    public class Book {

        public Boolean testament { get; set; } // true = new, false = old
        public string acronym { get; set; }
        public string name { get; set; }
        public IList<Chapter> chapters { get; set; }

        public string info
        {
            get
            {
                if (acronym != null)
                {
                    string chapterCountDescription = App.CHAPTER_STRING.ToLower();

                    if (chapters.Count > 1)
                        chapterCountDescription = App.CHAPTERS_STRING.ToLower();

                    string[] parts = acronym.Split(' ');
                    if (parts.Length > 1)
                    {
                        string firstLetter = parts[1].Substring(0, 1);
                        string remainingString = parts[1].Substring(1);
                        return parts[0] + " " + firstLetter.ToUpper() + remainingString + " - " + chapters.Count + " " + chapterCountDescription;
                    }
                    else
                    {
                        string firstLetter = parts[0].Substring(0, 1);
                        string remainingString = parts[0].Substring(1);
                        return firstLetter.ToUpper() + remainingString + " - " + chapters.Count + " " + chapterCountDescription;
                    }
                }

                return null;
            }
        }

        public Chapter GetRandomChapter()
        {
            if (chapters == null)
                return new Chapter();
            else
                return chapters.ElementAt((new Random()).Next(chapters.Count));
        }

        public Chapter GetFirstChapter()
        {
            if (chapters != null && chapters.Count > 0)
                return chapters.ElementAt(0);
            else
                return new Chapter();
        }

        public Chapter GetChapter(int index)
        {
            if (chapters != null && chapters.Count > index)
                return chapters.ElementAt(index);
            else
                return null;
        }
    }
}
