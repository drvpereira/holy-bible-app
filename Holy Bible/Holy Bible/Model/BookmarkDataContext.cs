using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Holy_Bible.Domain;
using System.Text;
using System.Collections.Generic;

namespace LocalDatabase.Model
{
    public class BookmarkDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public BookmarkDataContext(string connectionString) : base(connectionString)
        { }

        // Specify a table for the to-do items.
        public Table<Bookmark> Bookmarks;
    }

    [Table]
    public class Bookmark : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public Bookmark()
        {
        }

        public Bookmark(Verse verse)
        {
            BookName = verse.bookName;
            BookAcronym = verse.bookAcronym;
            ChapterNumber = verse.chapterNumber;
            VerseNumber = verse.number;
            VerseText = verse.text;
        }

        // Define ID: private field, public property, and database column.
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("Id");
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        // Define item name: private field, public property, and database column.
        private string _bookName;

        [Column]
        public string BookName
        {
            get { return _bookName; }
            set
            {
                if (_bookName != value)
                {
                    NotifyPropertyChanging("BookName");
                    _bookName = value;
                    NotifyPropertyChanged("BookName");
                }
            }
        }

        private string _bookAcronym;

        [Column]
        public string BookAcronym
        {
            get { return _bookAcronym; }
            set
            {
                if (_bookAcronym != value)
                {
                    NotifyPropertyChanging("BookAcronym");
                    _bookAcronym = value;
                    NotifyPropertyChanged("BookAcronym");
                }
            }
        }

        private int _chapterNumber;

        [Column]
        public int ChapterNumber
        {
            get { return _chapterNumber; }
            set 
            {
                if (_chapterNumber != value)
                {
                    NotifyPropertyChanging("ChapterNumber");
                    _chapterNumber = value;
                    NotifyPropertyChanged("ChapterNumber");
                }
            }
        }

        private int _verseNumber;

        [Column]
        public int VerseNumber
        {
            get { return _verseNumber; }
            set
            {
                if (_verseNumber != value)
                {
                    NotifyPropertyChanging("VerseNumber");
                    _verseNumber = value;
                    NotifyPropertyChanged("VerseNumber");
                }
            }
        }

        private string _verseText;

        [Column]
        public string VerseText
        {
            get { return _verseText; }
            set
            {
                if (_verseText != value)
                {
                    NotifyPropertyChanging("VerseText");
                    _verseText = value;
                    NotifyPropertyChanged("VerseText");
                }
            }
        }

        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(BookAcronym);
                sb.Append(" ");
                sb.Append(ChapterNumber);
                sb.Append(":");
                sb.Append(VerseNumber);

                return sb.ToString();
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}