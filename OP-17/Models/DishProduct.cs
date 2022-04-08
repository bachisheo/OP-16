namespace OP_17.Models;

public class DishProduct
{
    public DishProduct(Dish dish, Product product)
    {
        Dish = dish;
        Product = product;
    }

    public Dish Dish { get; set; }
    public Product Product { get; set; }
    public double? Count { get; set; }
}