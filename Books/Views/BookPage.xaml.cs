using Books.Models;

namespace Books.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class BookPage : ContentPage
{
    public string ItemId
    {
        set
        {
            LoadNote(value);
        }
    }

    public BookPage()
    {
        InitializeComponent();

        // Set the BindingContext of the page to a new Book.
        BindingContext = new Book();
    }

    async void LoadNote(string itemId)
    {
        try
        {
            int id = Convert.ToInt32(itemId);
            // Retrieve the book and set it as the BindingContext of the page.
            Book note = await App.Database.GetNoteAsync(id);
            BindingContext = note;
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to load book.");
        }
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var note = (Book)BindingContext;

        //note.Date = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(note.Title))
        {
            await App.Database.SaveNoteAsync(note);
        }

        // Navigate backwards
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var note = (Book)BindingContext;
        await App.Database.DeleteNoteAsync(note);

        // Navigate backwards
        await Shell.Current.GoToAsync("..");
    }
}