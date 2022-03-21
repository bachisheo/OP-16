using System.Collections.ObjectModel;
using OP_17.Models;

namespace OP_17.ViewModels;

public class MainViewModel
{
    public ObservableCollection<Product> Products { get; set; } = new();
    public ObservableCollection<Dish> Dishes { get; set; } = new();

    public MainViewModel()
    {
        Products.Add(new Product{Name = "Капуста белокочанная"});
        Products.Add(new Product{Name = "Молоко"});
        Products.Add(new Product{Name = "Гречневая крупа"});
        Dishes.Add(new Dish{Name = "Пирожок с капустой", Price = 1});
        Dishes.Add(new Dish{Name = "Борщ", Price = 10});
    }
}