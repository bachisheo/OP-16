using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public int Code 
    {
        get => _code;
        set => SetProperty(ref _code, value);
    } 
    public int Card
    {
        get => _card;
        set => SetProperty(ref _card, value);
    } 
    public double Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    } 

    public ObservableCollection<int> Sales { get; set; }

    public int AllSales => Sales.Sum();
    public double AllPrice => AllSales * Price;
   
    public DishViewModel()
    {
        _name = string.Empty;
        Sales = new ObservableCollection<int>(new int[5]);
        Sales.CollectionChanged += (_, _) => OnPropertyChanged(nameof(Sales));
        this.PropertyChanged += OnPropertyChangedHandler;
    }

    private void OnPropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
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
                break;
        }
    }

    private string _name;
    private int _code;
    private int _card;
    private double _price;
}