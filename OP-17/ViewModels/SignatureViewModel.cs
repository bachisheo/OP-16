using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace OP_17.ViewModels;

public class SignatureViewModel:ObservableObject
{
    public string FormerPost { get; set; }
    public string Former { get; set; }
    public string CompanyHead { get; set; }
    public string CompanyHeadPost { get; set; }
    public string ProductionHead { get; set; }

    public RelayCommand SubmitCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }

}