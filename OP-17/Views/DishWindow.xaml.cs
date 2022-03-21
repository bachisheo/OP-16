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
using OP_17.ViewModels;

namespace OP_17.Views
{
    /// <summary>
    /// Interaction logic for DishWindow.xaml
    /// </summary>
    public partial class DishWindow : Window
    {
        public DishWindow(DishViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
