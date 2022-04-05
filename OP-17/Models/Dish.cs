using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OP_17.Models;

public class Dish
{
    public string Name { get; set; } 
    public int Code { get; set; }
    public int Card { get; set; }
    public double Price { get; set; }
    public ICollection<DishSale> Sales { get; set; }
    public ICollection<DishProduct> Products { get; set; }

    public Dish()
    {
        Name = "";
        Code = 1;
        Products = new HashSet<DishProduct>();
        Sales = new HashSet<DishSale>();    
    }
}