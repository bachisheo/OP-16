using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace OP_17.ViewModels;

public class DishViewModel:ObservableObject
{

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    } 
    public int? Code 
    {
        get => _code;
        set => SetProperty(ref _code, value);
    } 
    public int? Card
    {
        get => _card;
        set => SetProperty(ref _card, value);
    } 
    public double? Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    } 

    public ObservableCollection<int?> Sales { get; set; }

    public int? AllSales => Sales.Sum();

    public double? AllPrice => AllSales * Price;

    public ObservableCollection<double?> ProductsCounts { get; set; }

    public List<double?> AllProductCounts => ProductsCounts.Select(c => c * AllSales).ToList();

    public DishViewModel()
    {

        _name = string.Empty;
        Sales = new ObservableCollection<int?>(new int?[5]);
        ProductsCounts = new ObservableCollection<double?>(new double?[5]);

        this.PropertyChanged += ThisOnPropertyChanged;
        Sales.CollectionChanged += (_, _) => OnPropertyChanged(nameof(Sales));
        ProductsCounts.CollectionChanged += (_, _) => OnPropertyChanged(nameof(ProductsCounts));
    }


    private void ThisOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Sales):
                OnPropertyChanged(nameof(AllSales));
                break;
            case nameof(Price):
                OnPropertyChanged(nameof(AllPrice));
                break;
            case nameof(AllSales):
                OnPropertyChanged(nameof(AllPrice));
                OnPropertyChanged(nameof(AllProductCounts));
                break;
            case nameof(ProductsCounts):
                OnPropertyChanged(nameof(AllProductCounts));
                break;
        }
    }


    private string _name;
    private int? _code;
    private int? _card;
    private double? _price;
}