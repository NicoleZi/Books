using Books.Models;
using Microsoft.Maui;
using System.Collections.ObjectModel;

namespace Books.Views;

public partial class TBR : ContentPage
{
    public ObservableCollection<Book> tbrList;

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

        tbrList = new ObservableCollection<Book>();
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

            SetFirstLastIndex(book);

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

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var filteredList = tbrList.Where(a => a.Title.Contains(e.NewTextValue) || a.Autor.Contains(e.NewTextValue));
        collectionView.ItemsSource = filteredList;
    }   

    private void UpButton_Clicked(object sender, EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        var grid = btn.Parent;
        string automationId = grid.AutomationId;
        try
        {
            int index = int.Parse(automationId);
            Console.WriteLine("ID ----------- " + automationId);
            index -= 1;
            Console.WriteLine("INDEX -1 ----------- " + index);
            SetNewIndex(index, index - 1);
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse '{automationId}'");
        }
    }
    private void DownButton_Clicked(object sender, EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        var grid = btn.Parent;
        string automationId = grid.AutomationId;
        try
        {
            int index = int.Parse(automationId);
            Console.WriteLine("ID ----------- " + automationId);
            index -= 1;
            Console.WriteLine("INDEX -1 ----------- " + index);
            SetNewIndex(index, index + 1);
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse '{automationId}'");
        }
    }

    public void SetNewIndex(int a, int b)
    {
        Book temp = tbrList[a];
        tbrList[a] = tbrList[b];
        tbrList[b] = temp;       

        //Set and Save a
        tbrList[a].TBRIndex = tbrList.IndexOf(tbrList[a]) + 1;
        SetFirstLastIndex(tbrList[a]);
        App.Database.SaveNoteAsync(tbrList[a]);

        //Set and Save b
        tbrList[b].TBRIndex = tbrList.IndexOf(tbrList[b]) + 1;
        SetFirstLastIndex(tbrList[b]);
        App.Database.SaveNoteAsync(tbrList[b]);

        collectionView.ItemsSource = tbrList;       
    }

    public void SetFirstLastIndex(Book book)
    {
        if (tbrList.IndexOf(book) == 0)
            book.NotFirstIndex = false;
        else
            book.NotFirstIndex = true;

        if (tbrList.IndexOf(book) == tbrList.Count - 1)
            book.NotLastIndex = false;
        else
            book.NotLastIndex = true;
    }
}