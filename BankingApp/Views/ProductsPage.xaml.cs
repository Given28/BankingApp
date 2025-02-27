using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace BankingApp.Views;

public partial class ProductsPage : ContentPage
{
    public ProductsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
    private void OnGoToCartClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ShoppingCartPage());

    }
    private void AddToCart(string productName)
    {

        Debug.WriteLine($"Product added to cart: {productName}");
    }
}