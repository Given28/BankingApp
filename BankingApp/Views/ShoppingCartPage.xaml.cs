using BankingApp.Models;
using BankingApp.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BankingApp.Views
{
    public partial class ShoppingCartPage : ContentPage
    {
        private DatabaseService dbService;

        
        public ObservableCollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCartPage()
        {
            InitializeComponent();

            // Initialize database service
            dbService = new DatabaseService();

            // Load shopping cart items
            LoadShoppingCart();
        }

        
        private void LoadShoppingCart()
        {
            try
            {
                var db = dbService.GetConnection();
                var cartItems = from cart in db.Table<ShoppingCart>()
                                join product in db.Table<Product>() on cart.ProductId equals product.ProductId
                                select new ShoppingCartItem
                                {
                                    ProductName = product.ProductName,
                                    Quantity = cart.Quantity,
                                    Price = product.Price
                                };

                
                ShoppingCartItems = new ObservableCollection<ShoppingCartItem>(cartItems.ToList());

                // Bind the ListView to the ObservableCollection
                ShoppingCartListView.ItemsSource = ShoppingCartItems;
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur while loading the cart
                DisplayAlert("Error", "There was an issue loading your shopping cart: " + ex.Message, "OK");
            }
        }
    }
}
