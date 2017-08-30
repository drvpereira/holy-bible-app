using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Holy_Bible.Domain;
using System.IO.IsolatedStorage;
using System.Resources;
using System.Reflection;

namespace Holy_Bible
{
    public partial class SharePage : PhoneApplicationPage
    {
        private ResourceManager rm;

        public SharePage()
        {
            InitializeComponent();
            StartLoadingCulture();
        }

        private void StartLoadingCulture()
        {
            rm = new ResourceManager("Holy_Bible.Cultures.AppResources", Assembly.GetExecutingAssembly());

            ApplicationTitle.Text = rm.GetString("SharePage_Title");
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            GetVerse();

            base.OnNavigatedTo(e);
        }

        private Verse GetVerse()
        {
            Verse verse = null;

            if (this.DataContext == null)
            {
                Object verseObject = null;

                if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("_hbVerse", out verseObject))
                {
                    verse = verseObject as Verse;

                    this.DataContext = verse;
                }
            }
            else
            {
                verse = this.DataContext as Verse;
            }

            return verse;
        }

        private void ListBoxShare_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem li = ListBoxShare.SelectedItem as ListBoxItem;

            if (li != null)
            {
                Verse verse = GetVerse();

                switch (li.Name.ToString())
                {
                    case "sms":
                        string text = verse.text;
                        string pref = "[" + verse.Description + "]";

                        if (text.Length > 140)
                        {
                            text = text.Substring(0, 140);
                            text = text.Substring(0, text.LastIndexOf(" "));
                            text = text + "...";
                        }

                        Microsoft.Phone.Tasks.SmsComposeTask sms = new Microsoft.Phone.Tasks.SmsComposeTask();
                        sms.Body = pref + " " + text;
                        sms.Show();
                        break;
                    case "email":
                        Microsoft.Phone.Tasks.EmailComposeTask email = new Microsoft.Phone.Tasks.EmailComposeTask();
                        email.Subject = "[Holy Bible] " + verse.Description;
                        email.Body = verse.text + "\n" + verse.Description + "\n\nSent from Holy Bible for Windows Phone";
                        email.Show();
                        break;
                    case "facebook":
                    case "twitter":
                    case "linkedin":
                    case "windowslive":
                        Microsoft.Phone.Tasks.ShareStatusTask statusTask = new Microsoft.Phone.Tasks.ShareStatusTask();
                        statusTask.Status = verse.text + "\n" + verse.Description + "\n\nSent from Holy Bible for Windows Phone";
                        statusTask.Show();
                        break;
                    default:
                        break;
                }
            }

            ListBoxShare.SelectedIndex = -1;
        }
    }
}