using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using System.IO;
using internetShopProject.mainEntities;
using iText.IO.Image;

namespace internetShopProject
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>

    public partial class Profile : Window
    {
        public Profile()
        {
            InitializeComponent();

            // Вывод ника в форму профиля при входе в профильное окно
            string nickInSession = (string)App.Current.Properties["nicknameInSession"];
            TextBlock welcomingText = (TextBlock)FindName("welcomingText");
            string resultMessage = "Welcome, " + nickInSession + "!";
            welcomingText.Text = resultMessage;

            // Запрос на выборку роли пользователя с последующим выводом на форму профиля 
            var role = App.Context.users.Where(u => u.user_Nickname == nickInSession).Select(r => r.role_Id).FirstOrDefault();

            if (role != 0)
            {
                if (role == 1)
                {
                    TextBlock welcomingRole = (TextBlock)FindName("roleTextBlock");
                    string resultMessageOfRole = "Role: User";
                    welcomingRole.Text = resultMessageOfRole;

                    App.Current.Properties["userRole"] = 1;
                }
                else
                {
                    TextBlock welcomingRole = (TextBlock)FindName("roleTextBlock");
                    string resultMessageOfRole = "Role: Admin";
                    welcomingRole.Text = resultMessageOfRole;

                    App.Current.Properties["userRole"] = 2;
                }
            }
            else
            {
                MessageBox.Show("User role wasn't found!");
            }

            // Запрос на проверку имеющейся картинки в базе данных (присутствует, либо отсутствует)
            var gottenImg = App.Context.users.Where(u => u.user_Nickname == nickInSession).Select(i => i.user_Image).FirstOrDefault();

            if (gottenImg != null)
            {
                // Если картинка имеется
                addImgBtn.Visibility = Visibility.Collapsed;
                
                // Считываем двоичные данные картинки из базы данных
                MemoryStream ms = new MemoryStream(gottenImg);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                mainProfileImage.Source = image;
            } else
            {
                // Если картинка отсутствует
                changeImgBtn.Visibility = Visibility.Collapsed;
            }
        }

        void changeNicknameBtnClicked(object sender, RoutedEventArgs e)
        {
            double top = Top;
            double left = Left;

            // Создание нового окна изменения никнейма
            changeNick chNickWindow = new changeNick();
            chNickWindow.Top = top + 300;
            chNickWindow.Left = left;

            chNickWindow.Show();
        }

        void addImageBtnClicked(object sender, RoutedEventArgs e)
        {
            // Функция добавления картинки в профиль (соответственно, при отсутствии картинки в профиле)
            string nickInSession = (string)App.Current.Properties["nicknameInSession"];

            // Открытие файлового диалога для выбора желаемой картинки, перенос на директорию "Рабочего стола"
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            openFileDialog.InitialDirectory = @"C:\Desktop\";
            Nullable<bool> result = openFileDialog.ShowDialog();

            // Условный оператор успешного выбора картинки
            if (result == true)
            {
                // Получение пути к картинке и её иницилизация в программу
                string selectedFileName = openFileDialog.FileName;

                // Записываем двоичные данные картинки для базы данных
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(selectedFileName);
                image.EndInit();

                mainProfileImage.Source = image;

                // Добавление выбранной картинки в базу данных
                byte[] imageData = File.ReadAllBytes(selectedFileName);

                var foundNickname = App.Context.users.FirstOrDefault(n => n.user_Nickname == nickInSession);

                if (foundNickname != null)
                {
                    foundNickname.user_Image = imageData;
                    App.Context.SaveChanges();
                }

                // Смена видимости с кнопки добавления картинки на кнопку изменения картинки
                addImgBtn.Visibility = Visibility.Collapsed;
                changeImgBtn.Visibility = Visibility.Visible;

                MessageBox.Show("An image was successfully added to your profile!");
            }
        }

        void changeImageBtnClicked(object sender, RoutedEventArgs e)
        {
            // Функция изменения картинки в профиле (соответственно, при присутствии картинки в профиле)
            string nickInSession = (string)App.Current.Properties["nicknameInSession"];

            // Открытие файлового диалога для выбора желаемой картинки, перенос на директорию "Рабочего стола"
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            openFileDialog.InitialDirectory = @"C:\Desktop\";
            Nullable<bool> result = openFileDialog.ShowDialog();

            // Условный оператор успешного выбора картинки
            if (result == true)
            {
                // Получение пути к картинке и её иницилизация в программу
                string selectedFileName = openFileDialog.FileName;

                // Записываем двоичные данные картинки для базы данных
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(selectedFileName);
                image.EndInit();

                mainProfileImage.Source = image;

                // Изменение существующей картинки в базе данных на новую
                byte[] imageData = File.ReadAllBytes(selectedFileName);

                var foundNickname = App.Context.users.FirstOrDefault(n => n.user_Nickname == nickInSession);

                if (foundNickname != null)
                {
                    foundNickname.user_Image = imageData;
                    App.Context.SaveChanges();
                }

                MessageBox.Show("An image was successfully changed in your profile!");
            }
        }

        void refreshClicked(object sender, RoutedEventArgs e)
        {
            // Обновление данных (ника) пользователя при нажатии на кнопку "Refresh"
            string nickInSession = (string)App.Current.Properties["nicknameInSession"];

            TextBlock welcomingText = (TextBlock)FindName("welcomingText");
            string resultMessage = "Welcome, " + nickInSession + "!";
            welcomingText.Text = resultMessage;

            var foundRole = App.Context.users.Where(u => u.user_Nickname == nickInSession).Select(ur => ur.role_Id).FirstOrDefault();

            if (foundRole == 1)
            {
                TextBlock welcomingRole = (TextBlock)FindName("roleTextBlock");
                string resultMessageOfRole = "Role: User";
                welcomingRole.Text = resultMessageOfRole;

                App.Current.Properties["userRole"] = 1;
            } else
            {
                TextBlock welcomingRole = (TextBlock)FindName("roleTextBlock");
                string resultMessageOfRole = "Role: Admin";
                welcomingRole.Text = resultMessageOfRole;

                App.Current.Properties["userRole"] = 2;
            }
            
        }

        void signOutClicked(object sender, RoutedEventArgs e)
        {
            // Установление запрета на доступ к кнопке и вкладке "Products" для гостя
            App.Current.Properties["productsEnabled"] = 0;
            double top = Top;
            double left = Left;

            // Цикл очистки окон при санкционированном закрытии профиля с помощью кнопки "Sign Out"
            var windowsToClose = new List<Window>();

            windowsToClose.AddRange(App.Current.Windows.OfType<Products>());
            windowsToClose.AddRange(App.Current.Windows.OfType<changeNick>());
            windowsToClose.AddRange(App.Current.Windows.OfType<ShoppingCart>());
            windowsToClose.AddRange(App.Current.Windows.OfType<addingProduct>());
            windowsToClose.AddRange(App.Current.Windows.OfType<removingProduct>());
            windowsToClose.AddRange(App.Current.Windows.OfType<redactingProduct>());

            foreach (var window in windowsToClose)
            {
                window.Close();
            }

            windowsToClose = null;

            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            // Установление запрета на доступ к кнопке и вкладке "Products" для гостя
            App.Current.Properties["productsEnabled"] = 0;
            double top = Top;
            double left = Left;

            // Цикл очистки окон при принудительном закрытии профиля
            var windowsToClose = new List<Window>();

            windowsToClose.AddRange(App.Current.Windows.OfType<Products>());
            windowsToClose.AddRange(App.Current.Windows.OfType<changeNick>());
            windowsToClose.AddRange(App.Current.Windows.OfType<ShoppingCart>());
            windowsToClose.AddRange(App.Current.Windows.OfType<addingProduct>());
            windowsToClose.AddRange(App.Current.Windows.OfType<removingProduct>());
            windowsToClose.AddRange(App.Current.Windows.OfType<redactingProduct>());

            foreach (var window in windowsToClose)
            {
                window.Close();
            }

            windowsToClose = null;

            base.OnClosed(e);
            Close();
        }
    }
}