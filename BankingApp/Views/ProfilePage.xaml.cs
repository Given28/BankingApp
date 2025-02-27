using BankingApp.Models;
using SQLite;
using System;
using System.IO;
using Microsoft.Maui.Controls;

namespace BankingApp.Views
{
    public partial class ProfilePage : ContentPage
    {
        private readonly string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "profile.db3");

        public ProfilePage()
        {
            InitializeComponent();
            LoadProfile();
        }

        private async void OnUploadClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Profile Picture",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    ProfileImage.Source = ImageSource.FromFile(result.FullPath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Upload Failed", $"Error: {ex.Message}", "OK");
            }
        }

        private void LoadProfile()
        {
            try
            {
                using var db = new SQLiteConnection(dbPath);
                db.CreateTable<Profile>();
                var profile = db.Table<Profile>().FirstOrDefault();

                if (profile != null)
                {
                    NameEntry.Text = profile.Name;
                    SurnameEntry.Text = profile.Surname;
                    EmailEntry.Text = profile.EmailAddress;
                    BioEditor.Text = profile.Bio;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load profile: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(SurnameEntry.Text) ||
                string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(BioEditor.Text))
            {
                await DisplayAlert("Error", "All fields are required. Please fill in all the fields.", "OK");
                return;
            }

            try
            {
                using var db = new SQLiteConnection(dbPath);
                db.CreateTable<Profile>();

                var profile = new Profile
                {
                    Name = NameEntry.Text,
                    Surname = SurnameEntry.Text,
                    EmailAddress = EmailEntry.Text,
                    Bio = BioEditor.Text
                };

                db.InsertOrReplace(profile);
                await DisplayAlert("Success", "Profile saved successfully!", "OK");
                await Navigation.PushAsync(new ProductsPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Save Failed", $"Error: {ex.Message}", "OK");
            }
        }
    }
}
