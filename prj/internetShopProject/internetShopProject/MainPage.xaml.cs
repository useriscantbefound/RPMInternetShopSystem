using internetShopProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace internetShopProject
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class MainPage : Window
    {
        public MainPage()
        {
            InitializeComponent();

            // Иницилизация значений глобального списка программы
            App.Current.Properties["productsEnabled"] = 0;
            App.Current.Properties["userRole"] = 0;
            App.Current.Properties["nicknameInSession"] = "None";
        }

        void createAccClicked(object sender, RoutedEventArgs e)
        {
            // Создание нового регистрационного окна
            Registration regWindow = new Registration();
            regWindow.Show();
        }

        void authorizeIntoAccClicked(object sender, RoutedEventArgs e)
        {
            // Проверка на существующую сессию авторизации на данный момент в системе
            int authorizedInSystem = (int)App.Current.Properties["productsEnabled"];

            if (authorizedInSystem > 0)
            {
                MessageBox.Show("You are already authorized in system!");
                return;
            } else
            {
                // Создание нового авторизационного окна
                Authorization authWindow = new Authorization();
                authWindow.Show();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            // При закрытии программы, уничтожается общий процесс программы и все окна в процессе
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        void cartBtnClicked(object sender, RoutedEventArgs e)
        {
            // Получение доступа к кнопке "Cart" посредством проверки значения глобального списка программы
            int productsEnabledValue = (int)App.Current.Properties["productsEnabled"];

            // Получение доступа к окну "Products" для последующих манипуляций
            Window productsWindowIsOpened = App.Current.Windows.OfType<Products>().FirstOrDefault();

            if (productsEnabledValue == 0)
            {
                MessageBox.Show("Please authorize in the system to view shopping cart!");
            } else if (productsWindowIsOpened == null) {
                MessageBox.Show("Please open the \"Products\" window to work with a \"Cart\"!");
            } else
            {
                // Проверка на количество открытых окон корзины
                int cartWindowCounting = 0;

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(ShoppingCart))
                    {
                        cartWindowCounting++;
                    }
                }

                if (cartWindowCounting == 1)
                {
                    cartWindowCounting = 0;
                    return;
                } else
                {
                    // Создание нового окна корзины
                    ShoppingCart cartWindow = new ShoppingCart();
                    cartWindow.Show();
                }
            }
        }

        void productsBtnClicked(object sender, RoutedEventArgs e)
        {
            // Получение доступа к кнопке "Products" посредством проверки значения глобального списка программы
            int productsEnabledValue = (int)App.Current.Properties["productsEnabled"];

            if (productsEnabledValue == 0)
            {
                MessageBox.Show("Please authorize in the system to view products!");
            }
            else
            {
                // Проверка на количество открытых окон продуктов
                int productsWindowCounting = 0;

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Products))
                    {
                        productsWindowCounting++;
                    }
                }

                if (productsWindowCounting == 1)
                {
                    productsWindowCounting = 0;
                    return;
                } else
                {
                    // Создание нового окна продукции
                    Products productsWindow = new Products();
                    productsWindow.Show();
                }
            }
        }
    }
}