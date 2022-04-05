using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace OP_17.ViewModels;

public class MainViewModel : ObservableObject
{
    #region DishesPage

    public string DocumentNumber 
    {
        get => _documentNumber;
        set => SetProperty(ref _documentNumber, value);
    }

    public DateTime DocumentDateTime
    {
        get => _documentDateTime;
        set
        {
            SetProperty(ref _documentDateTime, value);
            OnPropertyChanged(nameof(DocumentDate));
        }
    }

    public string DocumentDate => DocumentDateTime.ToString("dd.MM.yyyy");

    public string DocumentOperation { get; set; }

    public DateTime? StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value);
    }

    public DateTime? EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value);
    }

    public string CompanyName { get; set; }
    public string CompanyOKPO { get; set; }
    public string CompanyUnit { get; set; }
    public string CompanyOKDP { get; set; }

    public ObservableCollection<DateTime?> SalesDates
    {
        get
        {
            if (_salesDates.All(d => d == null) && StartDate != null && EndDate != null)
            {
                _salesDates[0] = StartDate;
                for (int i = 1; i < 5; i++)
                    _salesDates[i] = _salesDates[i - 1]?.AddDays(1);
            }

            return _salesDates;
        }
    }

    public ObservableCollection<DishViewModel> Dishes { get; set; }

    public MainViewModel()
    {
        DocumentOperation = string.Empty;
        PropertyChanged += OnPropertyChangedHandler;
        DocumentDateTime = DateTime.Now;
        DocumentNumber = "1";
        Dishes = new ObservableCollection<DishViewModel>();
        Dishes.CollectionChanged += DishesCollectionChanged;
        _salesDates = new ObservableCollection<DateTime?>(new DateTime?[5]);
        _startDate = DateTime.Now.AddDays(-4);
        _endDate = DateTime.Now;
        
    }

    private void OnPropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(StartDate):
            case nameof(EndDate):
                OnPropertyChanged(nameof(SalesDates));
                break;
        }
    }

    private void DishesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            var dish = (DishViewModel)e.NewItems![0]!;
            dish.Card = Dishes.Select(d => d.Card).ToImmutableSortedSet().Last() + 1;
            dish.Code = Dishes.Select(d => d.Code).ToImmutableSortedSet().Last() + 1;
        }
    }

    private DateTime _documentDateTime;
    private DateTime? _startDate;
    private DateTime? _endDate;
    private readonly ObservableCollection<DateTime?> _salesDates;
    private string _documentNumber;

    #endregion

}