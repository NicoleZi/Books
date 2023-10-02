using Books.Models;

namespace Books.Views;

public partial class TBR : ContentPage
{
	public TBR()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Retrieve all the books from the database, and set them as the
        // data source for the CollectionView.

        List<Book> books = await App.Database.GetNotesAsync();
        List<Book> tbrList = new List<Book>();
        
       
        for(int i = 0; i < books.Count; i++)
        {
            if (books[i].ToBeRead)
            {
                tbrList.Add(books[i]);
                books[i].Index = tbrList.Count;
            } 
        }

        collectionView.ItemsSource = tbrList;
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
}