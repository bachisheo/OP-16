using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using obshepit_form_16.Models;
using obshepit_form_16.Services;

namespace obshepit_form_16.ViewModels;

public class MainViewModel : ObservableObject
{
    private string _comboBoxText;


    public string DocumentNumber { get; set; }
   
    public DateTime? DocumentDateTime { get; set; }

    public string DocumentOperation { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string CompanyName { get; set; }

    public string CompanyOKPO { get; set; }

    public string CompanyUnit { get; set; }

    public string CompanyOKDP { get; set; }

    public ObservableCollection<DateTime?> SalesDates { get; set; }
 
    public ObservableCollection<ProductInfoViewModel> ProductsInfo { get; set; }

    public List<double?> SummaryCountsSums => ProductsInfo
        .Select(d => d.CountsSums)
        .AggregateList((x, y) => x == null ? y : y == null ? x : x + y)
        .Select(db => db.HasValue ? Math.Round(db.Value, 2) : (double?)null)
        .ToList();


    public RelayCommand GenerateReportCommand { get; set; }
    public RelayCommand OpenSignCommand { get; set; }

    public MainViewModel()
    {
      
        Init();
        ProductsInfo.CollectionChanged += ProductsCollectionChanged;
    }

   


    private void Init()
    {
        CompanyName = string.Empty;
        CompanyOKPO = string.Empty;
        CompanyUnit = string.Empty;
        CompanyOKDP = string.Empty;
        DocumentOperation = string.Empty;

        ProductsInfo = new ObservableCollection<ProductInfoViewModel>();
        SalesDates = new ObservableCollection<DateTime?>(new DateTime?[5]);
        GenerateReportCommand = new RelayCommand(OnGenerateExcel);
    }

    private void OnGenerateExcel()
    {
        var exporter = new ExcelExporter();
        var file = exporter.Export(this);
        var res = MessageBox.Show($"Сохранено как {file}. Открыть файл?", "", MessageBoxButton.YesNo);
        if (res != MessageBoxResult.Yes) return;

        FileInfo fi = new FileInfo(file);
        ProcessStartInfo proc = new ProcessStartInfo(fi.FullName);
        proc.UseShellExecute = true;
        Process.Start(proc);
    }


    private void ProductsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        for (int i = 0; i < ProductsInfo.Count; i++)
        {
            ProductsInfo[i].RowNumber = i + 1;
        }

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            (((ProductInfoViewModel)e.NewItems![0])!).PropertyChanged += ProductOnPropertyChanged;
        }
    }



    private void ProductOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ProductInfoViewModel.CountsSums):
                OnPropertyChanged(nameof(SummaryCountsSums));
                break;
        }
    }

}


static class EnumerableExtensions
{
    public static IEnumerable<T> AggregateList<T>(this IEnumerable<IEnumerable<T>> list, Func<T, T, T> everyElementSelector)
    {
        return list.Aggregate(
            new ObservableCollection<T>(new T[5]),
            (curSum, nextList) =>
                new ObservableCollection<T>(curSum.Zip(nextList, everyElementSelector)));
    }
}
