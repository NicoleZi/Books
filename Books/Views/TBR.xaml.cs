using Books.Models;
using Microsoft.Maui;

namespace Books.Views;

public partial class TBR : ContentPage
{
    public List<Book> tbrList;


    public TBR()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Retrieve all the books from the database, and set them as the
        // data source for the CollectionView.

        List<Book> booksDB = await App.Database.GetNotesAsync();
        List<KeyValuePair<Book, int>> keyValueList = new List<KeyValuePair<Book, int>>();

        foreach (Book book in booksDB)
        {
            if (!book.Read)
            {
                keyValueList.Add(new KeyValuePair<Book, int>(book, book.TBRIndex));
            }
            else
            {
                book.TBRIndex = 0;
                await App.Database.SaveNoteAsync(book);
            }                
        }

        //Sorting the values  
        for (int i = 0; i <= keyValueList.Count; i++)
        {
            for (int j = 1; j < keyValueList.Count; j++)
            {
                if (keyValueList[j - 1].Value > keyValueList[j].Value)
                {
                    KeyValuePair<Book, int> temp = keyValueList[j - 1];
                    //int temp = keyValueList[j].Value;
                    keyValueList[j - 1] = keyValueList[j];
                    keyValueList[j] = temp;
                }
            }
        }

        tbrList = new List<Book>();
        bool newBook = false;

        for (int i = 0; i < keyValueList.Count; i++)
        {
            if (keyValueList[i].Value == 0)
                newBook = true;               
            else               
                tbrList.Add(keyValueList[i].Key);
        }

        if (newBook)
        {
            for (int i = 0; i < keyValueList.Count; i++)
            {
                if (keyValueList[i].Value == 0)
                    tbrList.Add(keyValueList[i].Key);
            }
        }

        foreach(Book book in tbrList)
        {
            book.TBRIndex = tbrList.IndexOf(book) + 1;
            Console.WriteLine("Index: " + book.Title + "..." + tbrList.IndexOf(book));
            await App.Database.SaveNoteAsync(book);
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

    private void UpButton_Clicked(object sender, EventArgs e)
    {
        //Grid gridParent = (Button)sender).Parent;
        Console.WriteLine("ITEM Visual CHILD ---- " + ((Button)sender).Parent);
        Console.WriteLine("ITEM Visual CHILD 2 ---- " + ((Button)sender).Id);
    }
    private void DownButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("DOWN ---- " + sender.ToString());
    }
}