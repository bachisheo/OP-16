using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OP_17.Models;

public class Dish
{
    public string Name { get; set; } 
    public string Code { get; set; }
    public double Price { get; set; }
    public ICollection<DishSale> Sales { get; set; }
    public ICollection<DishProduct> Products { get; set; }

    public Dish()
    {
        Name = "";
        Code = "";
        Products = new HashSet<DishProduct>();
        Sales = new HashSet<DishSale>();    
    }
}