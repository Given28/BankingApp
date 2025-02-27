using SQLite;
using System.IO;
using BankingApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankingApp.Services
{
    public class DatabaseService
    {
        private readonly SQLiteConnection _db;

        public DatabaseService()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bankingapp.db3");
            _db = new SQLiteConnection(dbPath);

            CreateTables();
            SeedProducts();
        }

        private void CreateTables()
        {
            _db.CreateTable<Profile>();
            _db.CreateTable<Product>();
            _db.CreateTable<ShoppingCart>();
        }

        public SQLiteConnection GetConnection()
        {
            return _db;
        }

        private void SeedProducts()
        {
            if (!_db.Table<Product>().Any()) // Only seed if table is empty
            {
                var items = new List<Product>
                {
                    new Product { ProductName = "Apples", Quantity = 50, Price = 0.99, ImageUrl = "apples.jpg" },
                    new Product { ProductName = "Banana", Quantity = 30, Price = 1.20, ImageUrl = "banana.jpg" },
                    new Product { ProductName = "Grapes", Quantity = 20, Price = 0.75, ImageUrl = "grapes.jpg" },
                    new Product { ProductName = "Orange", Quantity = 15, Price = 1.50, ImageUrl = "orange.jpg" },
                    new Product { ProductName = "Watermelon", Quantity = 15, Price = 1.50, ImageUrl = "watermelon.jpg" }
                };

                _db.InsertAll(items);
            }
        }

        // ✅ Add method to add items to Shopping Cart
        public void AddToCart(int profileId, int productId, int quantity)
        {
            var existingItem = _db.Table<ShoppingCart>().FirstOrDefault(c => c.ProfileId == profileId && c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _db.Update(existingItem);
            }
            else
            {
                var product = _db.Table<Product>().FirstOrDefault(p => p.ProductId == productId);
                if (product != null && product.Quantity >= quantity) // Check stock
                {
                    var cartItem = new ShoppingCart
                    {
                        ProfileId = profileId,
                        ProductId = productId,
                        Quantity = quantity,
                        Price = product.Price
                    };
                    _db.Insert(cartItem);
                }
            }
        }

        // ✅ Get Shopping Cart Items
        public List<ShoppingCart> GetCartItems(int profileId)
        {
            return _db.Table<ShoppingCart>().Where(c => c.ProfileId == profileId).ToList();
        }

        // ✅ Remove Item from Cart
        public void RemoveFromCart(int cartId)
        {
            _db.Delete<ShoppingCart>(cartId);
        }
    }
}
