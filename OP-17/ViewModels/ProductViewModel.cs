using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace obshepit_form_16.ViewModels;

public class ProductViewModel:ObservableObject
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
    public string NameEi { get; set; }
    public int? CodeEi { get; set; }
    public int? RowNumber
    {
        get => _rowNumber;
        set => SetProperty(ref _rowNumber, value);
    } 
    public double? Price
    {
        get => _price;
        set => SetProperty(ref _price, value.HasValue ?Math.Round(value.Value, 2) : null);
    } 

    public ObservableCollection<int?> Counts { get; set; }

    public List<double?> CountsSums => Counts.Select(c => c * Price).Select(db => db.HasValue ? Math.Round(db.Value, 2) : (double?)null).ToList();

    public ProductViewModel()
    {
        _name = string.Empty;
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
            case nameof(Price):
                OnPropertyChanged(nameof(CountsSums));
                break;
        }
    }


    private string _name;
    private int? _code;
    private int? _rowNumber;
    private double? _price;
    private string _nameEi;
    private int? _codeEi;
}