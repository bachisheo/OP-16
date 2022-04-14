using System;
using System.Collections.ObjectModel;

namespace OP_17.ViewModels.Design;

public class DesignMainViewModel : MainViewModel
{
    public DesignMainViewModel()
    {
        DocumentNumber = "42";
        DocumentOperation = "2";
        CompanyName = "ООО \"Пирожковый рай\"";
        CompanyOKPO = "1234";
        CompanyUnit = "Пирожковый отдел";
        CompanyOKDP = "32.12.23";

        Products.Clear();
        Products.Add("Мука пшеничная");
        Products.Add("Фарш говяжий");
        Products.Add("Капуста белокочанная");
        Products.Add("Яблоки");
        Products.Add("Сливочное масло");
        Dishes.Clear();
        Dishes.Add(new() { Card = 1, Code = 2, Name = "Пирожок с капустой", Price = 50, Sales = new ObservableCollection<int?>(new int?[] { 31, 32, 31, 34, 35 }), ProductsCounts = new ObservableCollection<double?>(new double?[] { 0.1, null, 0.1, null, null }) });
        Dishes.Add(new() { Card = 2, Code = 3, Name = "Пирожок с мясом", Price = 40, Sales = new ObservableCollection<int?>(new int?[] { 21, 23, 22, 23, 25 }), ProductsCounts = new ObservableCollection<double?>(new double?[] { 0.1, 0.1, null, null, null }) });
        Dishes.Add(new() { Card = 3, Code = 4, Name = "Пирожок с яблоком", Price = 45, Sales = new ObservableCollection<int?>(new int?[] { 11, 12, 14, 14, 15 }), ProductsCounts = new ObservableCollection<double?>(new double?[] { 0.1, null, null, 0.1, 0.01 }) });

        SignatureVM.CompanyHead = "Иванова И.И.";
        SignatureVM.CompanyHeadPost = "Директор";
        SignatureVM.Former = "Петров П.П.";
        SignatureVM.FormerPost = "Бухгалтер";
        SignatureVM.ProductionHead = "Сидорова С.С.";
    }
}