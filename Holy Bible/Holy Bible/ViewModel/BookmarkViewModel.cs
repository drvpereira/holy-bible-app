using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using LocalDatabase.Model;

namespace LocalDatabase.ViewModel
{
    public class BookmarkViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private BookmarkDataContext bookmarkDB;

        // Class constructor, create the data context object.
        public BookmarkViewModel(string bookmarkDBConnectionString)
        {
            bookmarkDB = new BookmarkDataContext(bookmarkDBConnectionString);
        }

        // All to-do items.
        private ObservableCollection<Bookmark> _allBookmarks;
        public ObservableCollection<Bookmark> AllBookmarks
        {
            get { return _allBookmarks; }
            set
            {
                _allBookmarks = value;
                NotifyPropertyChanged("AllBookmarks");
            }
        }

        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            bookmarkDB.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        // Query database and load the collections
        public void LoadCollectionsFromDatabase()
        {

            // Specify the query for all bookmarks in the database.
            var BookmarksInDB = from Bookmark bookmark in bookmarkDB.Bookmarks
                                select bookmark;

            // Query the database and load all bookmarks.
            AllBookmarks = new ObservableCollection<Bookmark>(BookmarksInDB);
        }

        // Add a to-do item to the database and collections.
        public void AddBookmark(Bookmark bookmark)
        {
            // Add a to-do item to the data context.
            bookmarkDB.Bookmarks.InsertOnSubmit(bookmark);

            // Save changes to the database.
            bookmarkDB.SubmitChanges();

            // Add a to-do item to the "all" observable collection.
            AllBookmarks.Add(bookmark);
        }

        // Remove a to-do task item from the database and collections.
        public void DeleteBookmark(Bookmark bookmark)
        {
            // Remove the to-do item from the "all" observable collection.
            AllBookmarks.Remove(bookmark);

            // Remove the to-do item from the data context.
            bookmarkDB.Bookmarks.DeleteOnSubmit(bookmark);

            // Save changes to the database.
            bookmarkDB.SubmitChanges();
        }

        public Bookmark GetBookmark(Bookmark bookmark)
        {
            try
            {
                var BookmarkInDB = (from Bookmark b in bookmarkDB.Bookmarks
                                    where b.BookAcronym == bookmark.BookAcronym
                                       && b.ChapterNumber == bookmark.ChapterNumber
                                       && b.VerseNumber == bookmark.VerseNumber
                                    select b).Single<Bookmark>();

                if (BookmarkInDB != null)
                {
                    return BookmarkInDB;
                }
            }
            catch { }

            return null;
        }
    }
}