using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
