using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Holy_Bible.Domain
{
    public class BookmarkOld
    {
        int index { get; set; } // index na lista
        string book { get; set; } // nome completo do livro
        Int32 chapter { get; set; } // numero do capitulo
        Int32[] verseNumber { get; set; } // numero do versiculo
        string verseText { get; set; } // texto do versiculo

        public BookmarkOld(Book book, Chapter chapter, IList<Verse> selectedVerses)
        {
            Verse v1 = selectedVerses.First();
            this.book = book.acronym;
            this.chapter = chapter.number;

            StringBuilder sb = new StringBuilder();
            this.verseNumber = new Int32[selectedVerses.Count];
            int i = 0;
            foreach (Verse v in selectedVerses.OrderBy(item => item.number))
            {
                this.verseNumber[i++] = v.number;
                sb.Append(v.text + " ");
            }

            this.verseText = sb.ToString();
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public string Book
        {
            get { return book; }
            set { book = value; }
        }

        public Int32 Chapter
        {
            get { return chapter; }
            set { chapter = value; }
        }

        public Int32[] VerseNumber
        {
            get { return verseNumber; }
            set { verseNumber = value; }
        }

        public string VerseText
        {
            get { return verseText; }
            set { verseText = value; }
        }

        [XmlIgnore]
        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(book);
                sb.Append(" ");
                sb.Append(chapter.ToString());
                sb.Append(", ");
                sb.Append(verseNumber[0]);
                sb.Append("-");
                sb.Append(verseNumber[verseNumber.Length - 1]);

                return sb.ToString();
            }
        }
    }

    /*
    [DataContract]
    public class Bookmark
    {

        public Bookmark(Book book, Chapter chapter, IList<Verse> selectedVerses)
        {
            Verse v1 = selectedVerses.First();
            this.book = book.acronym;
            this.chapter = chapter.number;

            StringBuilder sb = new StringBuilder();
            verseNumber = new Int32[selectedVerses.Count];
            int i = 0;
            foreach (Verse v in selectedVerses.OrderBy(item => item.number))
            {
                verseNumber[i++] = v.number;
                sb.Append(v.text + " ");
            }

            verseText = sb.ToString();
        }

        [DataMember]
        public int index { get; set; } // index na lista
        [DataMember]
        public string book { get; set; } // nome completo do livro
        [DataMember]
        public Int32 chapter { get; set; } // numero do capitulo
        [DataMember]
        public Int32[] verseNumber { get; set; } // numero do versiculo
        [DataMember]
        public string verseText { get; set; } // texto do versiculo

        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(book);
                sb.Append(" ");
                sb.Append(chapter.ToString());
                sb.Append(", ");
                sb.Append(verseNumber[0]);
                sb.Append("-");
                sb.Append(verseNumber[verseNumber.Length-1]);

                return sb.ToString();
            }
        }
    }
    */
}
