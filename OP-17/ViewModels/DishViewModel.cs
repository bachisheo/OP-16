using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using OP_17.Models;

namespace OP_17.ViewModels;

public class DishViewModel : ViewModelBase
{
    public string? Name { get; set; }
    public string Price { get; set; }
    public string Code { get; set; }
    public ObservableCollection<DishSale> Sales { get; set; }
    public ObservableCollection<DishProduct> DishProducts { get; set; }

    public DishViewModel(Dish dish, ObservableCollection<Product> products)
    {
        Name = dish.Name;
        Price = dish.Price.ToString();
        Code = dish.Code;
        Sales = new ObservableCollection<DishSale>(dish.Sales);
        DishProducts = new ObservableCollection<DishProduct>(dish.Products);
        foreach (var product in products)
        {
        DishProducts.Add(new DishProduct{Count = 0, Dish = dish, Product = product});

        }
        Sales.Add(new DishSale { Count = 2, Date = new DateTime(2022, 3, 19) });
        Sales.Add(new DishSale { Count = 5, Date = new DateTime(2022, 3, 20) });
    }
}