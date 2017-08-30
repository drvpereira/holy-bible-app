using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Holy_Bible.Domain;
using System.Xml.Linq;
using System.Linq;
using Microsoft.Phone.Controls;

namespace Holy_Bible.Repository
{
    public class XmlBibleRepository : IBibleRepository
    {
        private static IList<Book> books = null;

        //private static IList<PanoramaItem> chapterItems = new List<PanoramaItem>();

        static XmlBibleRepository()
        {
            XDocument doc = XDocument.Load("Data/" + App.DATABASE_NAME);

            var data = from book in doc.Descendants("book")
                       select new Book()
                       {
                           acronym = Convert.ToString(book.Attribute("acr").Value),
                           name = Convert.ToString(book.Attribute("name").Value),
                           chapters = (from chapter in book.Descendants("chapter")
                                       select new Chapter()
                                       {
                                           number = Convert.ToInt32(chapter.Attribute("num").Value),
                                           verses = (from verse in chapter.Descendants("verse")
                                                    select new Verse()
                                                    {
                                                        number = Convert.ToInt32(verse.Attribute("num").Value),
                                                        text = Convert.ToString(verse.Attribute("txt").Value),
                                                        chapterNumber = Convert.ToInt32(chapter.Attribute("num").Value),
                                                        bookName = Convert.ToString(book.Attribute("name").Value),
                                                        bookAcronym = Convert.ToString(book.Attribute("acr").Value)
                                                    }).ToList<Verse>()
                                       }).ToList<Chapter>()
                       };

            books = data.ToList();

            bool currentTestament = false;
            int count = 1;
            foreach (Book b in books)
            {
                if (b.acronym.ToLower() == "马 太 福 音" || count == 40) // substitute for a testament property in books after xml refactoring
                {
                    currentTestament = true;
                }

                b.testament = currentTestament;
                count++;
            }
        }

        private List<BookLine> testaments;

        public IList<BookLine> Testaments
        {
            get
            {
                if (testaments == null)
                {
                   IList<Book> oldTestament = books.Where(item => item.testament == false).ToList();
                   IList<Book> newTestament = books.Where(item => item.testament == true).ToList();
                   testaments = new List<BookLine>();

                   testaments.Add(new BookLine()
                   {
                       OldTestament = new Book()
                       {
                           name = App.OLD_TESTAMENT_STRING
                       },
                       NewTestament = new Book()
                       {
                           name = App.NEW_TESTAMENT_STRING
                       },
                       Style = Application.Current.Resources["TestamentTextStyle"] as Style
                   });

                   int i = 0;

                   foreach(Book b in oldTestament) 
                   {
                       BookLine t = new BookLine();
                       t.OldTestament = b;
                       if (i <= (newTestament.Count - 1))
                       {
                           t.NewTestament = newTestament.ElementAt(i);
                       }
                       else
                       {
                           t.NewTestament = new Book();
                       }
                       
                       i++;

                       t.Style = Application.Current.Resources["TextBlockStyle"] as Style;
                       testaments.Add(t);
                   }
                    
                }

                return testaments;
            }
        }

        public IList<Book> Books
        {
            get
            {
                return books;
            }
        }

        public IList<Book> GetAllBooks()
        {
            return Books;
        }


        public Book RandomBook
        {
            get
            {
                return books.ElementAt((new Random()).Next(books.Count));
            }
        }


        public Book GetRandomBook()
        {
            return RandomBook;
        }

        public Verse RandomVerse
        {
            get
            {
                return GetRandomBook().GetRandomChapter().GetRandomVerse();
            }
        }


        public IList<Verse> findVersesByText(string text)
        {
            IList<Verse> verses = new List<Verse>();

            foreach (Book b in books)
            {
                foreach (Chapter c in b.chapters)
                {
                    foreach (Verse v in c.verses)
                    {
                        if (v.text.Contains(text))
                            verses.Add(v);
                    }
                }
            }

            return verses;
        }
    }
}
