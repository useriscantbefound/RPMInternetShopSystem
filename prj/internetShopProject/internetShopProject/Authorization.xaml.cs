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
using System.Xml;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace internetShopProject
{
    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>

    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        void authorizeBtnClicked(object sender, RoutedEventArgs e)
        {
            // Запрос на проверку существующего ника при авторизации
            var nicknameSearch = App.Context.users.Where(u => u.user_Nickname == loginBox2.Text).Select(u => u.user_Nickname).FirstOrDefault();

            if (nicknameSearch != null)
            {
                // Процедура хэширования пароля в MD5 для сравнения паролей
                MD5 md5Hash = MD5.Create();

                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(passwordBox2.Password));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }

                string hashedPassword = sb.ToString();

                // Конкретный запрос на проверку соответствия введёных пользователем данных при авторизации (логин и пароль)
                var passwordSearch = App.Context.users.Where(u => u.user_Nickname == loginBox2.Text && u.user_Password == hashedPassword).Select(u => u.user_Password).FirstOrDefault();

                if (passwordSearch != null)
                {
                    // Разрешение пользования кнопкой "Products" и соответствующим окном продукции после авторизации
                    // Занесение логина в сессию никнейма в программу
                    App.Current.Properties["productsEnabled"] = 1;
                    App.Current.Properties["nicknameInSession"] = loginBox2.Text;

                    MessageBox.Show("Valid data!");
                    this.Hide();

                    // Создание нового профильного окна
                    Profile profileWindow = new Profile();
                    profileWindow.Show();
                }
                else
                {
                    MessageBox.Show("Invalid password!");
                }
            }
            else
            {
                MessageBox.Show("Invalid login!");
            }
        }
    }
}