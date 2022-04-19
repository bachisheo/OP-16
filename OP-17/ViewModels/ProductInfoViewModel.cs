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
        set
        {
            this.SetProperty(ref _comboBoxText, value);
            OnPropertyChanged(nameof(ProductsSource));
        }
    }

    private List<Product> _productDirectory;
    public bool isReadOnly => Product == null;
    public Product Product
    {
        get => _product;
        set
        {
            this.SetProperty(ref _product, value);
            OnPropertyChanged(nameof(isReadOnly));
            OnPropertyChanged(nameof(CountsSums));
        }
    }

    public List<Product> ProductsSource => _productDirectory.Where(x => x.Name.Contains(ComboBoxText, StringComparison.InvariantCultureIgnoreCase)).ToList();


    public int RowNumber { get; set; }
    public ObservableCollection<int?> Counts { get; set; }

    public List<double?> CountsSums => Counts.Select(c => c * Product?.Price).Select(db => db.HasValue ? Math.Round(db.Value, 2) : (double?)null).ToList();

    public ProductInfoViewModel()
    {

        _productDirectory = GenerateProducts();
        Counts = new ObservableCollection<int?>(new int?[5]);
        Counts.CollectionChanged += (_, _) => OnPropertyChanged(nameof(Counts));
        Counts.CollectionChanged += (_, _) => OnPropertyChanged(nameof(CountsSums));
    }

    private Product _product;
    private string _comboBoxText = string.Empty;

    public List<Product> GenerateProducts()
    {
        return new List<Product>
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