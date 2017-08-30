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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LocalDatabase.Model;
using LocalDatabase.ViewModel;

namespace Holy_Bible
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        // The static ViewModel, to be used across the application.
        private static BookmarkViewModel viewModel;
        public static BookmarkViewModel ViewModel
        {
            get { return viewModel; }
        }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            // Specify the local database connection string.
            string DBConnectionString = "Data Source=isostore:/Bookmark.sdf";

            // Create the database if it does not exist.
            using (BookmarkDataContext db = new BookmarkDataContext(DBConnectionString))
            {
                if (db.DatabaseExists() == false)
                {
                    // Create the local database.
                    db.CreateDatabase();

                    // Save categories to the database.
                    db.SubmitChanges();
                }
            }

            // Create the ViewModel object.
            viewModel = new BookmarkViewModel(DBConnectionString);

            // Query the local database and load observable collections.
            viewModel.LoadCollectionsFromDatabase();
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        #region Cultures
        //private static string _chapterString = "Chapter";
        //private static string _chapterString = "Kapitel";
        //private static string _chapterString = "Capítulo";
        //private static string _chapterString = "Capitolo";
        //private static string _chapterString = "Chapitre";
        private static string _chapterString = "章";
        public static string CHAPTER_STRING
        {
            get
            {
                return _chapterString;
            }
        }

        //private static string _chaptersString = "Chapters";
        //private static string _chaptersString = "Kapiteln";
        //private static string _chaptersString = "Capítulos";
        //private static string _chaptersString = "Capitoli";
        //private static string _chaptersString = "Chapitres";
        private static string _chaptersString = "章節";
        public static string CHAPTERS_STRING
        {
            get
            {
                return _chaptersString;
            }
        }

        //private static string _oldTestamentString = "Old testament";
        //private static string _oldTestamentString = "Alt  Testamentes";
        //private static string _oldTestamentString = "Velho Testamento";
        //private static string _oldTestamentString = "Vecchio Testamento";
        //private static string _oldTestamentString = "Antiguo Testamento";
        //private static string _oldTestamentString = "Ancien Testament";
        private static string _oldTestamentString = "舊約全書";
        public static string OLD_TESTAMENT_STRING
        {
            get
            {
                return _oldTestamentString;
            }
        }

        //private static string _newTestamentString = "New testament";
        //private static string _newTestamentString = "Neu Testamentes";
        //private static string _newTestamentString = "Novo Testamento";
        //private static string _newTestamentString = "Nuovo Testamento";
        //private static string _newTestamentString = "Nuovo Testamento";
        //private static string _newTestamentString = "Nouveau Testament";
        private static string _newTestamentString = "新約全書";
        public static string NEW_TESTAMENT_STRING
        {
            get
            {
                return _newTestamentString;
            }
        }

        //private static string _databaseName = "bible_en.xml";
        private static string _databaseName = "bible_ch.xml";
        public static string DATABASE_NAME
        {
            get
            {
                return _databaseName;
            }
        }
        #endregion
    }
}