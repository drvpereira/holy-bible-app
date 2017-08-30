using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using Holy_Bible.Domain;
using System.Windows;
using LocalDatabase.Model;
using System.Resources;
using System.Reflection;
using Microsoft.Phone.Shell;


namespace Holy_Bible
{
    public partial class ReadPage : PhoneApplicationPage
    {
        private int currentChapterIndex = 0;
        private ResourceManager rm;

        public ReadPage()
        {
            InitializeComponent();
            StartLoadingCulture();
            BuildApplicationBar();
        }

        private void StartLoadingCulture()
        {
            rm = new ResourceManager("Holy_Bible.Cultures.AppResources", Assembly.GetExecutingAssembly());

            PanoramaItemChapters.Header = rm.GetString("ReadPage_Panorama_Index");
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBar.IsVisible = true;


            ApplicationBarIconButton buttonPrevious = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Previous.png", UriKind.Relative));
            buttonPrevious.Text = rm.GetString("ReadPage_ApplicationBar_Previous");
            buttonPrevious.Click += ApplicationBarIconButtonPrevious_Click;

            ApplicationBar.Buttons.Add(buttonPrevious);


            ApplicationBarIconButton buttonShare = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Share.png", UriKind.Relative));
            buttonShare.Text = rm.GetString("ReadPage_ApplicationBar_Share");
            buttonShare.Click += ApplicationBarIconButtonShare_Click;

            ApplicationBar.Buttons.Add(buttonShare);


            ApplicationBarIconButton buttonBookmark = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Favs.Add.png", UriKind.Relative));
            buttonBookmark.Text = rm.GetString("ReadPage_ApplicationBar_Bookmark");
            buttonBookmark.Click += ApplicationBarIconButtonAdd_Click;

            ApplicationBar.Buttons.Add(buttonBookmark);


            ApplicationBarIconButton buttonNext = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Next.png", UriKind.Relative));
            buttonNext.Text = rm.GetString("ReadPage_ApplicationBar_Next");
            buttonNext.Click += ApplicationBarIconButtonNext_Click;

            ApplicationBar.Buttons.Add(buttonNext);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Book book = GetBook();

            if (book != null)
            {
                Chapter firstChapter = book.GetFirstChapter();

                SetChapter(firstChapter);
            }

            base.OnNavigatedTo(e);
        }

        private void SetChapter(Chapter chapter)
        {
            PanoramaItemRead.Header = App.CHAPTER_STRING + " " + chapter.number;
            ListBoxVerses.ItemsSource = chapter.verses;
        }

        private void ListBoxChapters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book book = GetBook();

            int selectedIndex = ListBoxChapters.SelectedIndex;

            Chapter chapter = book.GetChapter(selectedIndex);

            if (chapter != null)
            {
                SetChapter(chapter);
                toItemOnPanoramaRead(PanoramaItemRead);
                currentChapterIndex = selectedIndex;
            }
        }

        private void toItemOnPanoramaRead(PanoramaItem panoramaItem)
        {
            panoramaRead.SetValue(Panorama.SelectedItemProperty, panoramaItem);
            panoramaRead.Measure(new Size());
        }

        private Book GetBook()
        {
            Book book = null;

            if (this.DataContext == null)
            {
                Object bookObject = null;

                if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("_hbBook", out bookObject))
                {
                    book = bookObject as Book;

                    this.DataContext = book;
                }
            }
            else
            {
                book = this.DataContext as Book;
            }

            return book;
        }

        private void ApplicationBarIconButtonNext_Click(object sender, EventArgs e)
        {
            Book book = GetBook();

            int nextIndex = (currentChapterIndex + 1);

            if (nextIndex < book.chapters.Count)
            {
                Chapter nextChapter = book.GetChapter(nextIndex);

                if (nextChapter != null)
                {
                    SetChapter(nextChapter);
                    currentChapterIndex++;

                    if (panoramaRead.SelectedIndex == 0)
                        toItemOnPanoramaRead(PanoramaItemRead);
                }
            }
        }

        private void ApplicationBarIconButtonPrevious_Click(object sender, EventArgs e)
        {
            Book book = GetBook();

            int previousIndex = (currentChapterIndex - 1);

            if (previousIndex >= 0)
            {
                Chapter previousChapter = book.GetChapter(previousIndex);

                if (previousChapter != null)
                {
                    SetChapter(previousChapter);
                    currentChapterIndex--;

                    if (panoramaRead.SelectedIndex == 0)
                        toItemOnPanoramaRead(PanoramaItemRead);
                }
            }
        }

        private void ApplicationBarIconButtonShare_Click(object sender, EventArgs e)
        {
            if (ListBoxVerses.SelectedItems != null && ListBoxVerses.SelectedItems.Count == 1)
            {
                Verse selectedVerse = ListBoxVerses.SelectedItems[0] as Verse;
                
                ListBoxVerses.SelectedIndex = -1;

                IsolatedStorageSettings.ApplicationSettings["_hbVerse"] = selectedVerse;
                NavigationService.Navigate(new Uri("/SharePage.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show(rm.GetString("ReadPage_Panorama_Reader_Share_Alert"), rm.GetString("Global_ShowMessage_Alert"), MessageBoxButton.OK);
            }
        }

        private void ApplicationBarIconButtonAdd_Click(object sender, EventArgs e)
        {
            if (ListBoxVerses.SelectedItems != null && ListBoxVerses.SelectedItems.Count > 0)
            {
                Bookmark bookmark = null, bookmarkInDB = null;

                Boolean added = false;

                foreach(Verse verse in ListBoxVerses.SelectedItems)
                {
                    bookmark = new LocalDatabase.Model.Bookmark(verse);

                    bookmarkInDB = App.ViewModel.GetBookmark(bookmark);

                    if (bookmarkInDB == null)
                    {
                        added = true;

                        App.ViewModel.AddBookmark(bookmark);
                    }
                }

                if (added)
                    MessageBox.Show(rm.GetString("ReadPage_Panorama_Reader_Bookmark_Add_Success"), rm.GetString("Global_ShowMessage_Info"), MessageBoxButton.OK);
                else
                    MessageBox.Show(rm.GetString("ReadPage_Panorama_Reader_Bookmark_Add_Exists"), rm.GetString("Global_ShowMessage_Info"), MessageBoxButton.OK);

                ListBoxVerses.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show(rm.GetString("ReadPage_Panorama_Reader_Bookmark_Select_Alert"), rm.GetString("Global_ShowMessage_Alert"), MessageBoxButton.OK);
            }
        }
    }
}