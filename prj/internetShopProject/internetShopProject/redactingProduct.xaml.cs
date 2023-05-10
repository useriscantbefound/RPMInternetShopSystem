using internetShopProject.mainEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для redactingProduct.xaml
    /// </summary>
    public partial class redactingProduct : Window
    {
        public redactingProduct()
        {
            InitializeComponent();
        }

        void textInputPrevented(object sender, TextCompositionEventArgs e)
        {
            // Проверка на написание символов (необходимый символ - цифра, если иной - не осуществляет процедуру введения желаемого символа)
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        void wrongInputPrevented(object sender, TextCompositionEventArgs e)
        {
            // Проверка на написание символов (необходимый символ - буква, если иной - не осуществляет процедуру введения желаемого символа)
            if (!char.IsLetter(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        void redactBtnClicked(object sender, RoutedEventArgs e)
        {
            // Иницилизация коллекции продуктов для последующих манипуляций
            ObservableCollection<product> prdCollection = App.Current.Properties["mainProductsCollection"] as ObservableCollection<product>;

            // Иницилизация переменных, принимающих введённые администратором значения
            string oldPrdName = oldProductNameTextBox.Text,
                   newPrdName = newRedactProductNameTextBox.Text,
                   newPrdDesc = newRedactProductDescriptionTextBox.Text,
                   newPrdCount = newRedactProductCountTextBox.Text,
                   newPrdPrice = newRedactProductPriceTextBox.Text;

            // Проверка на отсутствие значения в TextBoxes, проверка на существование пробелов в несанкционированных для этого TextBoxes
            if (oldPrdName == "" || newPrdName == "" || newPrdDesc == "" || newPrdCount == "" || newPrdPrice == "")
            {
                MessageBox.Show("Please fill all boxes of this form!");
            }
            else if (oldPrdName.Contains(" ") || newPrdName.Contains(" ") || newPrdCount.Contains(" ") || newPrdPrice.Contains(" "))
            {
                MessageBox.Show("There should be no spaces in this form beside description!");
            }
            else if (newPrdName.Length > 50)
            {
                MessageBox.Show("Too big size of product name!");
            }
            else
            {
                // Парсинг значения из типа string в тип int для изменения существующего продукта в коллекции и базе данных
                try
                {
                    // Значения для проверки на исключение FormatException
                    int checkCountNumber = int.Parse(newPrdCount);
                    int checkPriceNumber = int.Parse(newPrdPrice);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Unacceptable types of data in count or price forms!");
                    return;
                }

                int newPrdCountNumber = int.Parse(newPrdCount);
                int newPrdPriceNumber = int.Parse(newPrdPrice);

                // Процедура поиска старого продукта по базе данных и изменение данных в базе данных
                var foundProduct = App.Context.products.FirstOrDefault(pn => pn.product_Name == oldPrdName);

                if (foundProduct != null)
                {
                    foundProduct.product_Name = newPrdName;
                    foundProduct.product_Description = newPrdDesc;
                    foundProduct.product_Count = newPrdCountNumber;
                    foundProduct.product_Price = newPrdPriceNumber;

                    App.Context.SaveChanges();

                    Window productsWin = App.Current.Windows.OfType<Products>().FirstOrDefault();
                    productsWin.Close();

                    Products newPrdWin = new Products();
                    newPrdWin.Show();

                    Close();

                    MessageBox.Show("Product was successfully redacted!");
                } else
                {
                    MessageBox.Show("There is no such product with name: " + oldPrdName);
                }
            }
        }
    }
}
