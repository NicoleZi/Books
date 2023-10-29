using Books.Data;

namespace Books
{
    public partial class App : Application
    {
        static BookDatabase database;

        // Create the database connection as a singleton.
        public static BookDatabase Database
        {
            get
            {
                if (database != null)
                    return database;

                database = new BookDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}