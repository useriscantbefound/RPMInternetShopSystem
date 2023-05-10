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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;

namespace internetShopProject
{
    /// <summary>
    /// Логика взаимодействия для ShoppingCart.xaml
    /// </summary>
    public partial class ShoppingCart : Window
    {
        public ShoppingCart()
        {
            InitializeComponent();
        }

        void shoppingCartWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Занесение в коллекцию соответствующих продуктов и обновление вида
            ObservableCollection<product> shoppingCartProductList = App.Current.Properties["listOfProductsForCart"] as ObservableCollection<product>;
            cartListViewForProducts.ItemsSource = shoppingCartProductList;

            ICollectionView viewForRefresh = CollectionViewSource.GetDefaultView(shoppingCartProductList);
            viewForRefresh.Refresh();
        }

        void clearButtonClicked(object sender, RoutedEventArgs e)
        {
            // Принудительная очистка коллекции от товаров
            ObservableCollection<product> prdOfShopCartCollection = App.Current.Properties["listOfProductsForCart"] as ObservableCollection<product>;
            ObservableCollection<product> prdCollection = App.Current.Properties["mainProductsCollection"] as ObservableCollection<product>;

            // Проход по коллекциям и обновление данных в основной коллекции
            foreach (product productInProducts in prdCollection)
            {
                foreach (product productInCart in prdOfShopCartCollection)
                {
                    if (productInProducts.product_Name == productInCart.product_Name)
                    {
                        productInProducts.product_Count += productInCart.product_Count;
                        prdOfShopCartCollection.Remove(productInCart);

                        ICollectionView cartProductRefreshView = CollectionViewSource.GetDefaultView(prdCollection);
                        cartProductRefreshView.Refresh();

                        break;
                    }
                }
            }
        }

        void orderButtonClicked(object sender, RoutedEventArgs e)
        {
            ObservableCollection<product> cartProductList = App.Current.Properties["listOfProductsForCart"] as ObservableCollection<product>;

            if (cartProductList.Count == 0)
            {
                MessageBox.Show("You have no products to order in a \"Shopping Cart\"");
            } else
            {
                // Процедура формирования PDF-отчёта (чека) о покупке имеющихся в корзине продуктов пользователем
                // Создание PDF-файла
                PdfWriter pdfWriter = new PdfWriter("..\\..\\mainDocuments\\productCheck.pdf");
                PdfDocument pdfDocument = new PdfDocument(pdfWriter);

                // Создание документа
                Document document = new Document(pdfDocument);

                // Добавление контента внутрь созданного документа
                string nickInSession = (string)App.Current.Properties["nicknameInSession"];
                DateTime currentDate = DateTime.Now;

                // Заголовок контента чека
                Paragraph mainParagraph = new Paragraph("INTERNET SHOP SYSTEM PRODUCT CHECK" + "\n\n");
                mainParagraph.SetBold();

                // Текст в заголовке контента чека
                mainParagraph.Add(new Text("COUNT TYPES OF PRODUCTS BOUGHT: " + cartProductList.Count + "\n"));
                mainParagraph.Add(new Text("DATE OF A PURCHASE: " + currentDate + "\n"));
                mainParagraph.Add(new Text("BUYER NICKNAME: " + nickInSession));
                document.Add(mainParagraph);

                // Данные о продуктах (заголовок)
                Paragraph secondaryParagraph = new Paragraph("PRODUCT DATA");
                secondaryParagraph.SetBold();

                document.Add(secondaryParagraph);

                Paragraph thirdParagraph = new Paragraph();

                List<Paragraph> temporaryListForProducts = new List<Paragraph>();

                // Вставка продуктов из корзины в созданный документ
                foreach (product cartProduct in cartProductList)
                {
                    var mainProductText = new Paragraph("Product: " + cartProduct.product_Name + "\nDescription: " + cartProduct.product_Description + "\nCount: " + cartProduct.product_Count.ToString() + "\nPrice for one: " + cartProduct.product_Price.ToString() + "\n");
                    temporaryListForProducts.Add(mainProductText);
                }

                for (int i = 0; i < temporaryListForProducts.Count; i++)
                {
                    document.Add(temporaryListForProducts[i]);
                }

                // Итоговая сумма всей покупки
                Paragraph fourthParagraph = new Paragraph();

                List<int> temporaryListCounts = new List<int>();

                foreach (product cartProduct in cartProductList)
                {
                    int countedValueForOneProduct = (int)(cartProduct.product_Price * cartProduct.product_Count);
                    temporaryListCounts.Add(countedValueForOneProduct);
                }

                Paragraph resultedSumma = new Paragraph("RESULT PRICE: " + temporaryListCounts.Sum().ToString());
                resultedSumma.SetBold();

                document.Add(resultedSumma);

                // Закрытие документа
                document.Close();

                ObservableCollection<product> prdOfShopCartCollection = App.Current.Properties["listOfProductsForCart"] as ObservableCollection<product>;
                ObservableCollection<product> prdCollection = App.Current.Properties["mainProductsCollection"] as ObservableCollection<product>;

                // Проход по коллекциям и очистка данных из приложения + базы данных (купленного количества)
                foreach (product productInProducts in prdCollection)
                {
                    foreach (product productInCart in prdOfShopCartCollection)
                    {
                        if (productInProducts.product_Name == productInCart.product_Name)
                        {
                            // Процедура изменения количества продукции в базе данных после формирования отчёта (запрос)
                            var foundProduct = App.Context.products.FirstOrDefault(p => p.product_Name == productInCart.product_Name);

                            if (foundProduct != null)
                            {
                                foundProduct.product_Count -= productInCart.product_Count;
                                App.Context.SaveChanges();
                            }

                            prdOfShopCartCollection.Remove(productInCart);

                            break;
                        }
                    }
                }

                MessageBox.Show("Product check was successfully created!");
            }
        }

        void cartDoubleClicked(object sender, RoutedEventArgs e)
        {
            Window detectingProduct = App.Current.Windows.OfType<Products>().FirstOrDefault();

            if (detectingProduct != null)
            {
                // Получение выбранного элемента
                ListBox listBox = sender as ListBox;
                object selectedItem = listBox.SelectedItem;
                product selectedProduct = selectedItem as product;

                // Уменьшение количества имеющихся продуктов из корзины
                if (selectedProduct.product_Count > 1 || selectedProduct.product_Count == 1)
                {
                    // Формирование коллекций данных для корзины и продуктов пользователя
                    ObservableCollection<product> cartProductCollectionForCheckingProducts = App.Current.Properties["listOfProductsForCart"] as ObservableCollection<product>;
                    ObservableCollection<product> prdCollectionForCheckingProducts = App.Current.Properties["mainProductsCollection"] as ObservableCollection<product>;

                    // Процедура удаления записи из коллекции корзины, если осталась 1 штука продукта
                    if (selectedProduct.product_Count == 1)
                    {
                        cartProductCollectionForCheckingProducts.Remove(selectedProduct);

                        foreach (product itemInProducts in prdCollectionForCheckingProducts)
                        {
                            if (itemInProducts.product_Name == selectedProduct.product_Name)
                            {
                                itemInProducts.product_Count += 1;

                                ICollectionView prdRefreshView = CollectionViewSource.GetDefaultView(prdCollectionForCheckingProducts);
                                prdRefreshView.Refresh();

                                return;
                            }
                        }
                    }

                    // Цикл для манипуляции с данными удаления и добавления значений
                    foreach (product itemInProducts in prdCollectionForCheckingProducts)
                    {
                        if (itemInProducts.product_Name == selectedProduct.product_Name)
                        {
                            itemInProducts.product_Count += 1;
                            selectedProduct.product_Count -= 1;

                            ICollectionView cartProductRefreshView = CollectionViewSource.GetDefaultView(cartProductCollectionForCheckingProducts);
                            cartProductRefreshView.Refresh();

                            ICollectionView prdRefreshView = CollectionViewSource.GetDefaultView(prdCollectionForCheckingProducts);
                            prdRefreshView.Refresh();
                        }
                    }
                }
            }
        }
    }
}
