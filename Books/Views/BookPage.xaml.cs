using Books.Models;
using Books.Resources.Languages;

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
            Stars((int)note.Rating);
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
        bool answer = await DisplayAlert(AppResources.DeleteBookTask, AppResources.DeleteBookQuestion, AppResources.AnswerYes, AppResources.AnswerNo);

        if(!answer)
        {
            return;
        }

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
            }

            GetImages();
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

            for(int i = 0; i < files.Length; i++)
            {
                var img = new Image
                {
                    Source = files[i],
                    HeightRequest = 200,
                    Margin = 5
                };
                PhotoGrid.AddColumnDefinition(new ColumnDefinition());
                PhotoGrid.Add(img, i, 0);
            }          
        } else
        {
            //PhotoGrid.AddColumnDefinition(new ColumnDefinition());
            PhotoGrid.Add(new Label 
                { Text = AppResources.EmptyView, 
                FontSize = 15, 
                VerticalOptions=LayoutOptions.Center, 
                HorizontalTextAlignment=TextAlignment.Center,
                FontAttributes = FontAttributes.Italic,
                TextColor = Colors.LightGrey,
                WidthRequest=300});
        }                   
    }

    private void Fav_Clicked(object sender, EventArgs e)
    {
        var note = (Book)BindingContext;
        note.Favorite = !note.Favorite;
        Console.WriteLine("Fav ---- " + note.Favorite);

        if (note.Favorite)
            FavBtn.Source = "heart_fill.png";
        else
            FavBtn.Source = "heart_nofill.png";
    }

    private void Star1_Clicked(object sender, EventArgs e)
    {
        int star = 1;
        var note = (Book)BindingContext;

        if (note.Rating < star || note.Rating > star)
        {
            note.Rating = star;
            Stars(star);
        }
        else
        {
            note.Rating = star - 1;
            Stars(star - 1);
        }
    }

    private void Star2_Clicked(object sender, EventArgs e)
    {
        int star = 2;
        var note = (Book)BindingContext;

        if (note.Rating < star || note.Rating > star)
        {
            note.Rating = star;
            Stars(star);
        }
        else
        {
            note.Rating = star - 1;
            Stars(star - 1);
        }
    }

    private void Star3_Clicked(object sender, EventArgs e)
    {
        int star = 3;
        var note = (Book)BindingContext;

        if (note.Rating < star || note.Rating > star)
        {
            note.Rating = star;
            Stars(star);
        }
        else
        {
            note.Rating = star - 1;
            Stars(star - 1);
        }
    }

    private void Star4_Clicked(object sender, EventArgs e)
    {
        int star = 4;
        var note = (Book)BindingContext;

        if (note.Rating < star || note.Rating > star)
        {
            note.Rating = star;
            Stars(star);
        }
        else
        {
            note.Rating = star - 1;
            Stars(star - 1);
        }
    }

    private void Star5_Clicked(object sender, EventArgs e)
    {
        int star = 5;
        var note = (Book)BindingContext;

        if (note.Rating < star || note.Rating > star)
        {
            note.Rating = star;
            Stars(star);
        }
        else
        {
            note.Rating = star - 1;
            Stars(star - 1);
        }
    }

    private void Stars(int rating)
    {
        string fillstar = "star_fill.png";
        string nofillstar = "star_nofill.png";
        
        switch (rating)
        {
            case 0:
                Console.WriteLine("CASE 0");
                star1.Source = nofillstar;
                star2.Source = nofillstar;
                star3.Source = nofillstar;
                star4.Source = nofillstar;
                star5.Source = nofillstar;               
                break;
            case 1:
                Console.WriteLine("CASE 1");
                star1.Source = fillstar;
                star2.Source = nofillstar;
                star3.Source = nofillstar;
                star4.Source = nofillstar;
                star5.Source = nofillstar;                
                break;
            case 2:
                Console.WriteLine("CASE 2");
                star1.Source = fillstar;
                star2.Source = fillstar;
                star3.Source = nofillstar;
                star4.Source = nofillstar;
                star5.Source = nofillstar;               
                break;               
            case 3:
                Console.WriteLine("CASE 3");
                star1.Source = fillstar;
                star2.Source = fillstar;
                star3.Source = fillstar;
                star4.Source = nofillstar;
                star5.Source = nofillstar;              
                break;
            case 4:
                Console.WriteLine("CASE 4");
                star1.Source = fillstar;
                star2.Source = fillstar;
                star3.Source = fillstar;
                star4.Source = fillstar;
                star5.Source = nofillstar;              
                break;
            case 5:
                Console.WriteLine("CASE 5");
                star1.Source = fillstar;
                star2.Source = fillstar;
                star3.Source = fillstar;
                star4.Source = fillstar;
                star5.Source = fillstar;              
                break;
        }
    }
}