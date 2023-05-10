using internetShopProject.mainEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace internetShopProject
{
    /// <summary>
    /// Логика взаимодействия для removingProduct.xaml
    /// </summary>
    public partial class removingProduct : Window
    {
        public removingProduct()
        {
            InitializeComponent();
        }

        void deleteProductClicked(object sender, RoutedEventArgs e)
        {
            // Формирование коллекции продуктов
            ObservableCollection<product> prdCollectionProducts = App.Current.Properties["mainProductsCollection"] as ObservableCollection<product>;
            string writtenProductName = mainProductNameTextBox.Text;

            // Запрос на происк продукта и процедура его удаления из базы данных
            var foundProductName = App.Context.products.FirstOrDefault(pn => pn.product_Name == writtenProductName);

            if (foundProductName != null)
            {
                App.Context.products.Remove(foundProductName);
                App.Context.SaveChanges();

                Window productsWin = App.Current.Windows.OfType<Products>().FirstOrDefault();
                productsWin.Close();

                Products newPrdWin = new Products();
                newPrdWin.Show();

                Close();

                MessageBox.Show("Product was successfully removed from database!");
            } else
            {
                MessageBox.Show("Product was not found!");
            }
        }
    }
}