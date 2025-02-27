using BankingApp.Models;
using BankingApp.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BankingApp.ViewModels
{
    public class ProductsViewModel : BindableObject
    {
        private readonly DatabaseService dbService;

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(); // Notifies when the collection changes
            }
        }

        public ICommand AddToCartCommand { get; }

        public ProductsViewModel()
        {
            dbService = new DatabaseService();
            Products = new ObservableCollection<Product>();

            LoadProducts();

            
            AddToCartCommand = new Command<Product>(AddToCart);
        }

        // Load products from the database or insert default values
        private void LoadProducts()
        {
            var db = dbService.GetConnection();
            var dbProducts = db.Table<Product>().ToList();

            if (dbProducts.Any())
            {
                Products = new ObservableCollection<Product>(dbProducts); // Fetch from DB
            }
            else
            {
                // Fallback to default products if DB is empty
                Products = new ObservableCollection<Product>
                {
                    new Product { ProductId = 1, ProductName = "Smart TV", Price = 10000.00, ImageUrl = "smarttv.png" },
                    new Product { ProductId = 2, ProductName = "Smartwatch", Price = 2500.00, ImageUrl = "smartwatch.png" },
                    new Product { ProductId = 3, ProductName = "Tablet", Price = 5000.00, ImageUrl = "tablet.png" },
                    new Product { ProductId = 4, ProductName = "Phone", Price = 8000.00, ImageUrl = "phone.png" },
                    new Product { ProductId = 5, ProductName = "Laptop", Price = 15000.00, ImageUrl = "laptop.png" }
                };

                db.InsertAll(Products); // Insert default products into the database
            }
        }

        // Handle adding product to the shopping cart
        private void AddToCart(Product product)
        {
            if (product == null) return;

            var db = dbService.GetConnection();
            var cartItem = db.Table<ShoppingCart>().FirstOrDefault(c => c.ProductId == product.ProductId);

            if (cartItem == null)
            {
                // Product not in cart, add a new cart item
                cartItem = new ShoppingCart
                {
                    ProfileId = 1,  // Assuming ProfileId is hardcoded for now
                    ProductId = product.ProductId,
                    Quantity = 1
                };
                db.Insert(cartItem); // Insert the new item into the cart
            }
            else
            {
                
                cartItem.Quantity += 1;
                db.Update(cartItem); // Update the existing item in the cart
            }

            // Display success message to the user
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            mainPage?.DisplayAlert("Success", $"{product.ProductName} added to cart!", "OK");
        }
    }
}
