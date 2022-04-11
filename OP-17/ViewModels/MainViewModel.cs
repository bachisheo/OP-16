using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using OP_17.Services;
using OP_17.Views;

namespace OP_17.ViewModels;

public class MainViewModel : ObservableObject
{
    #region Header

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

    private DateTime _documentDateTime;
    private DateTime? _startDate;
    private DateTime? _endDate;
    private string _documentNumber;

    #endregion

    #region DishesPage

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

    private readonly ObservableCollection<DateTime?> _salesDates;

    #endregion

    #region ProductsPage

    public ObservableCollection<string> Products { get; }

    #endregion

    #region Signature

    public SignatureViewModel SignatureVM { get; set; }

    #endregion


    public LimitedSizeObservableCollection<DishViewModel> Dishes { get; set; }

    public List<int?> SummarySales => Dishes
        .Select(d => d.Sales)
        .Aggregate(new ObservableCollection<int?>(new int?[5]),
            (sum, list) =>
                new ObservableCollection<int?>(sum.Zip(list,
                    (x, y) => x == null ? y : y == null ? x : x + y)))
        .ToList();

    public int? SummaryAllSales => Dishes.Sum(d => d.AllSales);

    public double? SummaryAllPrice => Dishes.Sum(d => d.AllPrice);


    public List<double?> SummaryAllProductCounts => Dishes
        .Select(d => d.AllProductCounts)
        .Aggregate(new ObservableCollection<double?>(new double?[5]),
            (sum, list) =>
                new ObservableCollection<double?>(sum.Zip(list,
                    (x, y) => x == null ? y : y == null ? x : x + y)))
        .ToList();

    public RelayCommand DebugCommand => new(Debug);

    private void Debug()
    {

    }

    public RelayCommand SignCommand { get; set; }
    public RelayCommand GenerateReportCommand { get; set; }

    public MainViewModel()
    {
        CompanyName = string.Empty;
        CompanyOKPO = string.Empty;
        CompanyUnit = string.Empty;
        CompanyOKDP = string.Empty;
        DocumentOperation = string.Empty;
        PropertyChanged += ThisOnPropertyChanged;
        DocumentDateTime = DateTime.Now;
        _documentNumber = "1";
        Dishes = new LimitedSizeObservableCollection<DishViewModel>(5);
        Dishes.CollectionChanged += DishesCollectionChanged;
        _salesDates = new ObservableCollection<DateTime?>(new DateTime?[5]);
        _startDate = DateTime.Now.AddDays(-4);
        _endDate = DateTime.Now;

        Products = new ObservableCollection<string>(new string[5]);

        GenerateReportCommand = new RelayCommand(() =>
        {
            var exporter = new ExcelExporter();
            var file = exporter.Export(this);
           var res = MessageBox.Show($"Сохранено как {file}. Открыть файл?", "", MessageBoxButton.YesNo);
           if (res != MessageBoxResult.Yes) return;

           FileInfo fi = new FileInfo(file);
           ProcessStartInfo proc = new ProcessStartInfo(fi.FullName);
           proc.UseShellExecute = true;
           Process.Start(proc);
        });

        SignCommand = new RelayCommand(OnSign);
    }

    private void OnSign()
    {
        SignatureWindow signatureWindow = new SignatureWindow { ViewModel = SignatureVM };
        var result = signatureWindow.ShowDialog();
        if (result == true)
            SignatureVM = signatureWindow.ViewModel;
    }

    private void DishesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            var dish = (DishViewModel)e.NewItems![0]!;
            dish.Card = Dishes.Select(d => d.Card).ToImmutableSortedSet().Last() + 1;
            dish.Code = Dishes.Select(d => d.Code).ToImmutableSortedSet().Last() + 1;
            dish.PropertyChanged += DishOnPropertyChanged;
        }
    }

    private void ThisOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(StartDate):
            case nameof(EndDate):
                OnPropertyChanged(nameof(SalesDates));
                break;

        }
    }

    private void DishOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(DishViewModel.Sales):
                OnPropertyChanged(nameof(SummarySales));
                break;
            case nameof(DishViewModel.AllSales):
                OnPropertyChanged(nameof(SummaryAllSales));
                break;
            case nameof(DishViewModel.AllPrice):
                OnPropertyChanged(nameof(SummaryAllPrice));
                break;
            case nameof(DishViewModel.ProductsCounts):
                OnPropertyChanged(nameof(SummaryAllProductCounts));
                break;
        }
    }

}

public class LimitedSizeObservableCollection<T> : ObservableCollection<T>
{
    public int Capacity { get; }

    public LimitedSizeObservableCollection(int capacity)
    {
        Capacity = capacity;
    }

    protected override void InsertItem(int index, T item)
    {
        if (Count < Capacity)
            base.InsertItem(index, item);
    }
}