using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Toolkit.Mvvm.Input;
using OP_17.ViewModels;

namespace OP_17.Views
{
    /// <summary>
    /// Interaction logic for SignatureWindow.xaml
    /// </summary>
    public partial class SignatureWindow : Window
    {
        private SignatureViewModel _viewModel;
        public SignatureViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                if (_viewModel == null)
                    _viewModel = new SignatureViewModel();
                _viewModel.SubmitCommand = new RelayCommand(() => this.DialogResult = true);
                _viewModel.CancelCommand = new RelayCommand(() => this.DialogResult = false);
                DataContext = _viewModel;
            }
        }

        public SignatureWindow()
        {
            InitializeComponent();
            ViewModel = new SignatureViewModel();
        }
    }
}
