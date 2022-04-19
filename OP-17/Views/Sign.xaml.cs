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
using obshepit_form_16.ViewModels;

namespace obshepit_form_16.Views
{
    /// <summary>
    /// Логика взаимодействия для Sign.xaml
    /// </summary>
    public partial class Sign : Window
    {
        public Sign(SignViewModel signVM)
        {
            DataContext = signVM;
            InitializeComponent();
        }
    }
}
