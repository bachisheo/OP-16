using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using OP_17.Models;
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


    public ObservableCollection<DishViewModel> Dishes { get; set; }

    public RelayCommand SignCommand { get; set; }
    public RelayCommand GenerateReportCommand { get; set; }

    public MainViewModel()
    {
        CompanyName = string.Empty;
        CompanyOKPO = string.Empty;
        CompanyUnit = string.Empty;
        CompanyOKDP = string.Empty;
        DocumentOperation = string.Empty;
        PropertyChanged += OnPropertyChangedHandler;
        DocumentDateTime = DateTime.Now;
        _documentNumber = "1";
        Dishes = new ObservableCollection<DishViewModel>();
        Dishes.CollectionChanged += DishesCollectionChanged;
        _salesDates = new ObservableCollection<DateTime?>(new DateTime?[5]);
        _startDate = DateTime.Now.AddDays(-4);
        _endDate = DateTime.Now;

        Products = new ObservableCollection<string>(new string[5]);

        GenerateReportCommand = new RelayCommand(() =>
        {
            var exporter = new ExcelExporter();
            var file = exporter.Export(this);
            MessageBox.Show($"Сохранено как {file}.");
        });

        SignatureVM = new SignatureViewModel();
        SignCommand = new RelayCommand(OnSign);
    }

    private void OnSign()
    {
        SignatureWindow signatureWindow = new SignatureWindow();
        signatureWindow.ViewModel = SignatureVM;
        var result = signatureWindow.ShowDialog();
        if (result == true)
            SignatureVM = signatureWindow.ViewModel;
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

}