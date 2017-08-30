using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Holy_Bible.Repository;
using Holy_Bible.Domain;
using System.IO.IsolatedStorage;
using LocalDatabase.Model;
using System.Windows;
using System.Resources;
using System.Reflection;
using Microsoft.Phone.Tasks;

namespace Holy_Bible
{
    public partial class MainPage : PhoneApplicationPage
    {
        private IBibleRepository repository;
        private ResourceManager rm;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            StartLoadingCulture();
            StartLoadingData();
        }

        private void StartLoadingCulture()
        {
            rm = new ResourceManager("Holy_Bible.Cultures.AppResources", Assembly.GetExecutingAssembly());

            PanoramaMain.Title = rm.GetString("MainPage_Title");

            PanoramaItemBooks.Header = rm.GetString("MainPage_Panorama_Books");
            PanoramaItemSearch.Header = rm.GetString("MainPage_Panorama_Search");
            PanoramaItemBookmarks.Header = rm.GetString("MainPage_Panorama_Bookmarks");
            PanoramaItemAbout.Header = rm.GetString("MainPage_Panorama_About");

            //ButtonSearch.Content = rm.GetString("MainPage_Panorama_Search_Button");

            TextBlockAboutTitle.Text = rm.GetString("MainPage_Panorama_About_App_Title");
            TextBlockAboutTranslation.Text = rm.GetString("MainPage_Panorama_About_Book_Translation");
            TextBlockAboutLanguageDB.Text = rm.GetString("MainPage_Panorama_About_Book_Language_Database");
            TextBlockAboutVersion.Text = rm.GetString("MainPage_Panorama_About_App_Version");
            TextBlockAboutResume.Text = rm.GetString("MainPage_Panorama_About_App_Resume");
        }

        private void StartLoadingData()
        {
            repository = new XmlBibleRepository();
            this.DataContext = repository;
            ListBoxBooks.UpdateLayout();

            ListBoxBookmarks.ItemsSource = App.ViewModel.AllBookmarks;
        }

        private void GestureListener_Tap(object sender, GestureEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            string bookName = (sp.Children[0] as TextBlock).Text;

            Book selected = repository.GetAllBooks().Where(item => item.name == bookName).FirstOrDefault();
            if (selected != null)
            {
                IsolatedStorageSettings.ApplicationSettings["_hbBook"] = selected;
                NavigationService.Navigate(new Uri("/ReadPage.xaml", UriKind.Relative));
            }
        }

        private void ButtonDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                Bookmark bookmark = button.DataContext as Bookmark;

                MessageBoxResult result = MessageBox.Show(rm.GetString("MainPage_Panorama_Bookmark_Delete_Confirmation"), rm.GetString("Global_ShowMessage_Alert"), MessageBoxButton.OKCancel);

                if (result.Equals(MessageBoxResult.OK))
                {
                    App.ViewModel.DeleteBookmark(bookmark);
                    //MessageBox.Show("Bookmark successfully deleted!");
                }
            }
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TextBoxSearch.Text))
            {
                ListBoxSearch.ItemsSource = repository.findVersesByText(TextBoxSearch.Text.Trim());
            }
        }

        private void HyperlinkButtonAboutURL_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.byraphaelmedeiros.com/", UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void HyperlinkButtonAboutRef_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.byraphaelmedeiros.com/p/holy-bible", UriKind.Absolute);
            webBrowserTask.Show();
        }
    }
}