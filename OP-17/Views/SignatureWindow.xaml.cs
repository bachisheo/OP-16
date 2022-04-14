using Microsoft.Toolkit.Mvvm.Input;
using OP_17.ViewModels;

namespace OP_17.Views
{
    /// <summary>
    /// Interaction logic for SignatureWindow.xaml
    /// </summary>
    public partial class SignatureWindow
    {
        public SignatureViewModel ViewModel { get; set; }

        public SignatureWindow(SignatureViewModel signatureViewModel)
        {
            ViewModel = new SignatureViewModel();
            ViewModel.CompanyHead = signatureViewModel.CompanyHead;
            ViewModel.CompanyHeadPost = signatureViewModel.CompanyHeadPost;
            ViewModel.Former = signatureViewModel.Former;
            ViewModel.FormerPost = signatureViewModel.FormerPost;
            ViewModel.ProductionHead = signatureViewModel.ProductionHead;
            ViewModel.SubmitCommand = new RelayCommand(() => this.DialogResult = true);
            ViewModel.CancelCommand = new RelayCommand(() => this.DialogResult = false);
            DataContext = ViewModel;

            InitializeComponent();
        }
    }
}
