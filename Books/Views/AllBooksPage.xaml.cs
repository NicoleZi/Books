using Books.Models;

namespace Books.Views;

public partial class AllBooksPage : ContentPage
{
    List<Book> bookCollection;

	public AllBooksPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Retrieve all the notes from the database, and set them as the
        // data source for the CollectionView.
        bookCollection = await App.Database.GetNotesAsync();
        collectionView.ItemsSource = bookCollection;
    }

    async void OnAddClicked(object sender, EventArgs e)
    {
        // Navigate to the BookPage.
        await Shell.Current.GoToAsync(nameof(BookPage));
    }

    async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            // Navigate to the BookPage, passing the ID as a query parameter.
            Book note = (Book)e.CurrentSelection.FirstOrDefault();
            await Shell.Current.GoToAsync($"{nameof(BookPage)}?{nameof(BookPage.ItemId)}={note.ID}");
        }
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e) 
    {
        var filteredList = bookCollection.Where(a => a.Title.Contains(e.NewTextValue) || a.Autor.Contains(e.NewTextValue));
        collectionView.ItemsSource = filteredList;
    }
}