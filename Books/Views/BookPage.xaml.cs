using Books.Models;
using Microsoft.Maui.Controls;
using System.Diagnostics;

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

            GetImages();
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to load book.");
        }
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var note = (Book)BindingContext;

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

    void OnVolumeStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        var note = (Book)BindingContext;
        note.Volume = (int)e.NewValue;

        ((Stepper)sender).Value = note.Volume;

        VolumeLabel.Text = note.Volume.ToString();
    }

    void OnRatingStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        var note = (Book)BindingContext;
        note.Rating = (float)e.NewValue;

        ((Stepper)sender).Value = note.Rating;

        RatingLabel.Text = note.Rating.ToString();
    }

    private async void PhotoButton_Clicked(object sender, EventArgs e)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            var foldername = (Book)BindingContext;

            string dir = Path.Combine(FileSystem.CacheDirectory, "Books");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            FileResult photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo != null)
            {
                dir = Path.Combine(dir, foldername.Title);
                if(!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string localFilePath = Path.Combine(dir, photo.FileName);
                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);
                await sourceStream.CopyToAsync(localFileStream);

                foldername.GalleryFolder = dir;
                await App.Database.SaveNoteAsync(foldername);
            }           
        }
        else
            await Shell.Current.DisplayAlert("Oops", "You device isn't supported", "Ok");
    }

    public void GetImages()
    {
        var note = (Book)BindingContext;

        if (!string.IsNullOrWhiteSpace(note.GalleryFolder))
        {
            string[] files = Directory.GetFiles(note.GalleryFolder);
            

            Console.WriteLine("................." + files.Length);
            for(int i = 0; i < files.Length; i++)
            {
                var img = new Image();
                ColumnDefinition columnDefinition = new ColumnDefinition();
                img.Source = files[i];                
                img.HeightRequest = 200;
                img.Margin = 5;
                PhotoGrid.AddColumnDefinition(columnDefinition);
                PhotoGrid.Add(img, i, 0);
            }          
        } else
        {
            Shell.Current.DisplayAlert("Nooo", "Leer", "Ok");
        }                   
    }
}