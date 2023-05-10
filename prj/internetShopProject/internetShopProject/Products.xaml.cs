using internetShopProject.mainEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
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
using System.Xml.Linq;

namespace internetShopProject
{
    /// <summary>
    /// Логика взаимодействия для Products.xaml
    /// </summary>
    public partial class Products : Window
    {
        public Products()
        {
            InitializeComponent();

            // Иницилизация коллекции для продуктов корзины
            ObservableCollection<product> cartProductList = new ObservableCollection<product>();
            App.Current.Properties["listOfProductsForCart"] = cartProductList;

            // Проверка роли пользователя на соответствие
            int userRoleValue = (int)App.Current.Properties["userRole"];

            if (userRoleValue == 1)
            {
                addProductButton.Visibility = Visibility.Collapsed;
                removeProductButton.Visibility = Visibility.Collapsed;
                changeProductButton.Visibility = Visibility.Collapsed;
            }

            // Иницилизация коллекции для продуктов
            ObservableCollection<product> productsCollection = new ObservableCollection<product>();
            var productsList = App.Context.products.ToList();

            App.Current.Properties["mainProductsCollection"] = productsCollection;

            // Занесение полученных данных из базы данных в коллекцию
            foreach (var product in productsList)
            {
                productsCollection.Add(new product
                {
                    product_Name = product.product_Name,
                    product_Count = product.product_Count,
                    product_Description = product.product_Description,
                    product_Price = product.product_Price
                });
            }

            listViewForProducts.ItemsSource = productsCollection;
        }

        void productDoubleClicked(object sender, RoutedEventArgs e)
        {
            Window detectingCart = App.Current.Windows.OfType<ShoppingCart>().FirstOrDefault();

            if (detectingCart != null)
            {
                // Получение выбранного элемента
                ListBox listBox = sender as ListBox;
                object selectedItem = listBox.SelectedItem;
                product selectedProduct = selectedItem as product;

                // Уменьшение количества имеющихся продуктов в приложении
                if (selectedProduct.product_Count != 0)
                {
                    // Формирование коллекций данных для корзины и продуктов пользователя
                    ObservableCollection<product> cartProductCollectionForCheckingProducts = App.Current.Properties["listOfProductsForCart"] as ObservableCollection<product>;
                    ObservableCollection<product> prdCollectionForCheckingProducts = App.Current.Properties["mainProductsCollection"] as ObservableCollection<product>;

                    // Цикл для манипуляции с данными удаления и добавления значений
                    foreach (product itemInCart in cartProductCollectionForCheckingProducts)
                    {
                        if (itemInCart.product_Name == selectedProduct.product_Name)
                        {
                            selectedProduct.product_Count -= 1;
                            itemInCart.product_Count += 1;

                            ICollectionView cartProductRefreshView = CollectionViewSource.GetDefaultView(cartProductCollectionForCheckingProducts);
                            cartProductRefreshView.Refresh();

                            ICollectionView prdRefreshView = CollectionViewSource.GetDefaultView(prdCollectionForCheckingProducts);
                            prdRefreshView.Refresh();

                            return;
                        }
                    }
                }
                else
                {
                    // Если продукт закончился, конец выполнения дальнейшего скрипта
                    MessageBox.Show("This product is over!");
                    return;
                }

                // Иницилизация коллекции для её последующего обновления
                ObservableCollection<product> productToRefreshCollection = App.Current.Properties["mainProductsCollection"] as ObservableCollection<product>;
                ICollectionView viewForRefresh = CollectionViewSource.GetDefaultView(productToRefreshCollection);
                viewForRefresh.Refresh();

                // Создание словаря для последующих манипуляций с данными
                Dictionary<string, string> productInfoDictionary = new Dictionary<string, string>
                {
                { "productName", selectedProduct.product_Name },
                { "productCount", selectedProduct.product_Count.ToString() },
                { "productDescription", selectedProduct.product_Description },
                { "productPrice", selectedProduct.product_Price.ToString() }
                };

                // Формирование коллекции данных для корзины пользователя
                ObservableCollection<product> cartProductList = App.Current.Properties["listOfProductsForCart"] as ObservableCollection<product>;

                cartProductList.Add(new product
                {
                    product_Name = productInfoDictionary["productName"],
                    product_Count = 1,
                    product_Description = productInfoDictionary["productDescription"],
                    product_Price = selectedProduct.product_Price
                });

                selectedProduct.product_Count -= 1;

                // Вставка коллекции данных в корзину пользователя
                Window cartWindow = App.Current.Windows.OfType<ShoppingCart>().FirstOrDefault();

                if (cartWindow != null)
                {
                    ListBox listBoxOfCartWindow = cartWindow.FindName("cartListViewForProducts") as ListBox;
                    listBoxOfCartWindow.ItemsSource = cartProductList;
                }
                else
                {
                    MessageBox.Show("Open the \"Shopping Cart\" window to buy products!");
                }
            }
        }

        void refreshProductClicked(object sender, RoutedEventArgs e)
        {
            // Иницилизация коллекции для её последующего обновления
            ObservableCollection<product> productToRefreshCollection = App.Current.Properties["mainProductsCollection"] as ObservableCollection<product>;

            // Проход по коллекции и проверка на закончившиеся продукты в коллекции (если закончились, запись удаляется из коллекции)
            foreach (product productInProducts in productToRefreshCollection)
            {
                if (productInProducts.product_Count == 0)
                {
                    productToRefreshCollection.Remove(productInProducts);
                    break;
                }
            }

            // Обновление view коллекции продуктов
            ICollectionView viewForRefresh = CollectionViewSource.GetDefaultView(productToRefreshCollection);
            viewForRefresh.Refresh();
        }

        void removePrdClicked(object sender, RoutedEventArgs e)
        {
            double top = Top;
            double left = Left;

            // Создание нового окна удаления продукта
            removingProduct deleteProductWindow = new removingProduct();
            deleteProductWindow.Top = top;
            deleteProductWindow.Left = left - 250;

            deleteProductWindow.Show();
        }

        void addPrdClicked(object sender, RoutedEventArgs e)
        {
            double top = Top;
            double left = Left;

            // Создание нового окна добавления продукта
            addingProduct addProductWindow = new addingProduct();
            addProductWindow.Top = top;
            addProductWindow.Left = left - 250;

            addProductWindow.Show();
        }

        void changePrdClicked(object sender, RoutedEventArgs e)
        {
            double top = Top;
            double left = Left;

            // Создание нового окна изменения продукта
            redactingProduct changeProductWindow = new redactingProduct();
            changeProductWindow.Top = top;
            changeProductWindow.Left = left - 250;

            changeProductWindow.Show();
        }
    }
}