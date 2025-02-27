using BankingApp.Models;
using BankingApp.Services;
using Microsoft.Maui.Controls;
using System.Linq;
using System.Windows.Input;

namespace BankingApp.ViewModels
{
    public class ProfileViewModel : BindableObject
    {
        private readonly DatabaseService dbService;

        private Profile _profile;
        public Profile Profile
        {
            get => _profile;
            set
            {
                if (_profile != value)
                {
                    _profile = value;
                    OnPropertyChanged();  // Notify UI when Profile is updated
                }
            }
        }

        public ICommand SaveCommand { get; }

        public ProfileViewModel()
        {
            dbService = new DatabaseService();
            LoadProfile();
            SaveCommand = new Command(SaveProfile, CanSaveProfile);
        }

        private void LoadProfile()
        {
            var db = dbService.GetConnection();
            // Load the first profile from the database, or create a new one if not found
            Profile = db.Table<Profile>().FirstOrDefault() ?? new Profile();
        }

        // Method to validate if the Save button can be enabled
        private bool CanSaveProfile()
        {
            // Enable the Save button only if all fields are filled in
            return !string.IsNullOrEmpty(Profile.Name) &&
                   !string.IsNullOrEmpty(Profile.Surname) &&
                   !string.IsNullOrEmpty(Profile.EmailAddress) &&
                   !string.IsNullOrEmpty(Profile.Bio);
        }

        private async void SaveProfile()
        {
           
            if (string.IsNullOrEmpty(Profile.Name) || string.IsNullOrEmpty(Profile.Surname) ||
                string.IsNullOrEmpty(Profile.EmailAddress) || string.IsNullOrEmpty(Profile.Bio))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "All fields are required. Please fill in all the fields.", "OK");
                return;
            }

            var db = dbService.GetConnection();

            // Insert or replace the Profile in the database
            try
            {
                db.InsertOrReplace(Profile);
                await Application.Current.MainPage.DisplayAlert("Success", "Profile saved successfully!", "OK");
            }
            catch (Exception ex)
            {
                // Catch any potential errors and show an error message
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
