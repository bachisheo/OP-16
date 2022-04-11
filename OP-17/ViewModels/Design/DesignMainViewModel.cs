using System;
using System.Collections.ObjectModel;

namespace OP_17.ViewModels.Design;

public class DesignMainViewModel:MainViewModel
{
    public DesignMainViewModel()
    {
        DocumentNumber = "42";
        DocumentDateTime = new DateTime(2022, 2, 1);
        StartDate = DateTime.Now;
        EndDate = DateTime.Now.AddDays(5);
        DocumentOperation = "2";
        CompanyName = "Какая то компания";
        CompanyOKDP = "123";
        CompanyUnit = "Какое то подразделение";
        CompanyOKPO = "321";
        Dishes = new LimitedSizeObservableCollection<DishViewModel>(3)
        {
            new() {Card = 1, Code = 2, Name = "Какое то блюдо", Price = 3, Sales = new ObservableCollection<int?>(new int?[]{1,2,null,4,5}), ProductsCounts = new ObservableCollection<double?>(new double?[]{1,2,null,4,5})},
            new() {Card = 1, Code = 2, Name = "Какое то блюдо", Price = 3, Sales = new ObservableCollection<int?>(new int?[]{1,3,null,null,5}), ProductsCounts = new ObservableCollection<double?>(new double?[]{1,3,null,null,5})},
            new() {Card = 1, Code = 2, Name = "Какое то блюдо", Price = 3, Sales = new ObservableCollection<int?>(new int?[]{1,2,null,4,5}), ProductsCounts = new ObservableCollection<double?>(new double?[]{1,2,null,4,5})}
        };
    }
}