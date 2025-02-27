using SQLite;

namespace BankingApp.Models
{
    public class ShoppingCart
    {
        [PrimaryKey, AutoIncrement]
        public int CartId { get; set; }
        public int ProfileId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
    public class ShoppingCartItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}