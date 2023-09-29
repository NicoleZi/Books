using Books.Models;

namespace Books.Views;

public partial class AllBooksPage : ContentPage
{
	public AllBooksPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Retrieve all the notes from the database, and set them as the
        // data source for the CollectionView.
        collectionView.ItemsSource = await App.Database.GetNotesAsync();
    }

    async void OnAddClicked(object sender, EventArgs e)
    {
        // Navigate to the NoteEntryPage.
        await Shell.Current.GoToAsync(nameof(BookPage));
    }

    async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            // Navigate to the NoteEntryPage, passing the ID as a query parameter.
            Book note = (Book)e.CurrentSelection.FirstOrDefault();
            await Shell.Current.GoToAsync($"{nameof(BookPage)}?{nameof(BookPage.ItemId)}={note.ID.ToString()}");
        }
    }
}