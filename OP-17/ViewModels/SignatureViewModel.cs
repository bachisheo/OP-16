using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace OP_17.ViewModels;

public class SignatureViewModel:ObservableObject
{
    public SignatureViewModel(RelayCommand submitCommand, RelayCommand cancelCommand)
    {
        SubmitCommand = submitCommand;
        CancelCommand = cancelCommand;
    }
    public SignatureViewModel()
    {
    }

    public string FormerPost { get; set; } = string.Empty;
    public string Former { get; set; }= string.Empty;
    public string CompanyHead { get; set; }= string.Empty;
    public string CompanyHeadPost { get; set; }= string.Empty;
    public string ProductionHead { get; set; }= string.Empty;

    public RelayCommand SubmitCommand { get; set; } 
    public RelayCommand CancelCommand { get; set; }

}