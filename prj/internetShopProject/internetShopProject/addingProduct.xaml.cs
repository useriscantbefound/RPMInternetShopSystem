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
    /// Логика взаимодействия для addingProduct.xaml
    /// </summary>
    public partial class addingProduct : Window
    {
        public addingProduct()
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

        void createPrdClicked(object sender, RoutedEventArgs e)
        {
            // Иницилизация коллекции продуктов для последующих манипуляций
            ObservableCollection<product> prdCollection = App.Current.Properties["mainProductsCollection"] as ObservableCollection<product>;

            // Иницилизация переменных, принимающих введённые администратором значения
            string newPrdName = newProductTextBox.Text,
                   newPrdDesc = newDescriptionTextBox.Text,
                   newPrdCount = newCountTextBox.Text,
                   newPrdPrice = newPriceTextBox.Text;

            // Проверка на отсутствие значения в TextBoxes, проверка на существование пробелов в несанкционированных для этого TextBoxes
            if (newPrdName == "" || newPrdDesc == "" || newPrdCount == "" || newPrdPrice == "")
            {
                MessageBox.Show("Please fill all boxes of this form!");
            } else if (newPrdName.Contains(" ") || newPrdCount.Contains(" ") || newPrdPrice.Contains(" "))
            {
                MessageBox.Show("There should be no spaces in this form beside description!");
            } else if (newPrdName.Length > 50)
            {
                MessageBox.Show("Too big size of product name!");
            }
            else
            {
                // Парсинг значения из типа string в тип int для добавления нового продукта в коллекцию и базу данных
                try
                {
                    // Значения для проверки на исключение FormatException
                    int checkCountNumber = int.Parse(newPrdCount);
                    int checkPriceNumber = int.Parse(newPrdPrice);
                } catch (FormatException)
                {
                    MessageBox.Show("Unacceptable types of data in count or price forms!");
                    return;
                }

                int newPrdCountNumber = int.Parse(newPrdCount);
                int newPrdPriceNumber = int.Parse(newPrdPrice);

                // Добавление в коллекцию
                prdCollection.Add(new product
                {
                    product_Name = newPrdName,
                    product_Count = newPrdCountNumber,
                    product_Description = newPrdDesc,
                    product_Price = newPrdPriceNumber
                });

                // Добавление в базу данных
                App.Context.products.Add(new product
                {
                    product_Name = newPrdName,
                    product_Count = newPrdCountNumber,
                    product_Description = newPrdDesc,
                    product_Price = newPrdPriceNumber
                });

                // Сохранение изменений в базу данных, переоткрытие окна продукции для обновления данных из базы данных
                App.Context.SaveChanges();

                Window productsWin = App.Current.Windows.OfType<Products>().FirstOrDefault();
                productsWin.Close();

                Products newPrdWin = new Products();
                newPrdWin.Show();

                Close();

                MessageBox.Show("Product was successfully added to list!");
            }
        }
    }
}
