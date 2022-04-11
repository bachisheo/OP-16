using Microsoft.Toolkit.Mvvm.Input;
using OP_17.ViewModels;

namespace OP_17.Views
{
    /// <summary>
    /// Interaction logic for SignatureWindow.xaml
    /// </summary>
    public partial class SignatureWindow
    {
        private SignatureViewModel _viewModel;
        public SignatureViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value ?? new SignatureViewModel(
                    new RelayCommand(() => this.DialogResult = true),
                    new RelayCommand(() => this.DialogResult = false));
                DataContext = _viewModel;
            }
        }

        public SignatureWindow()
        {
            InitializeComponent();
        }
    }
}
