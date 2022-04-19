using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using obshepit_form_16.Models;

namespace obshepit_form_16.ViewModels;

public class ProductInfoViewModel:ObservableObject
{
    public string ComboBoxText
    {
        get => _comboBoxText;
        set => this.SetProperty(ref _comboBoxText, value);
    }

    public bool isReadOnly
    {
        get;
        set;
    }
    public Product Product
    {
        get => _product;
        set => this.SetProperty(ref _product, value);
    }

    public ObservableCollection<Product> ProductsList
    {
        get => _productsList;
        set =>this.SetProperty(ref _productsList, value);
    }

    public int RowNumber { get; set; }
    public ObservableCollection<int?> Counts { get; set; }

    public List<double?> CountsSums => Counts.Select(c => c * Product?.Price).Select(db => db.HasValue ? Math.Round(db.Value, 2) : (double?)null).ToList();

    public ProductInfoViewModel()
    {

        ProductsList = GenerateProducts();
        Counts = new ObservableCollection<int?>(new int?[5]);
        this.PropertyChanged += ThisOnPropertyChanged;
        Counts.CollectionChanged += (_, _) => OnPropertyChanged(nameof(Counts));
    }

    private void ThisOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Counts):
                OnPropertyChanged(nameof(CountsSums));
                break;
            case nameof(Product):
                OnPropertyChanged(nameof(CountsSums));
                break;
            case nameof(ComboBoxText):
                ProductsList = GenerateProducts();
                var rightProducts = new ObservableCollection<Product>();
                foreach (var prod in ProductsList)
                {
                    if (prod.Name.Contains(ComboBoxText))
                    {
                        rightProducts.Add(prod);
                    }
                }
                ProductsList = rightProducts;
                break;
        }
    }
    private Product _product;
    private string _comboBoxText;
    private ObservableCollection<Product> _productsList;

    public ObservableCollection<Product> GenerateProducts()
    {
        return new ObservableCollection<Product>
        {
            new("Вишня замороженная", 12, "кг", 166, 150),
            new("Смесь овощей для супа", 34, "кг", 166, 90),
            new("Масло сливочное крестьянское", 56, "кг", 166, 390),
            new("Масло сливочное алтайское", 58, "кг", 166, 590),
            new("Напиток фруктовый", 12, "л", 123, 500),
            new("Молоко", 12, "л", 123, 100)
        };
    }
}