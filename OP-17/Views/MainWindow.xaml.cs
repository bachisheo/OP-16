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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OP_17.Models;
using OP_17.ViewModels;
using OP_17.Views;

namespace OP_17
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel = new();
            DataContext = MainViewModel;
        }

        private MainViewModel MainViewModel { get; set; }

        private void CreateDishOnClick(object sender, RoutedEventArgs e)
        {
            var dishWindow = new DishWindow(new DishViewModel(new Dish(),  MainViewModel.Products))
            {
                Owner = this,
                Title = "Создание рецепта"
            };
            dishWindow.ShowDialog();
        }

        private void DataGridOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DishesDataGrid.SelectedItem == null) return;
            var product = (Dish) DishesDataGrid.SelectedItem;
            var dishWindow = new DishWindow(new DishViewModel(product,  MainViewModel.Products))
            {
                Owner = this,
                Title = "Свойства рецепта"
            };
            dishWindow.ShowDialog();
        }
    }
}
