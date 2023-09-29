namespace Books
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.BookPage), typeof(Views.BookPage));
        }
    }
}