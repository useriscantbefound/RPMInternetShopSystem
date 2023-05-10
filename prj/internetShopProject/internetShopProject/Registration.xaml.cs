using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
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
    /// Interaction logic for Registration.xaml
    /// </summary>

    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        void createAccountBtnClicked(object sender, RoutedEventArgs e)
        {
            // Метод, подсчитывающий соответствующее количество каждого необходимого условия в пароле
            string text = passwordBox.Password;
            int upperLetters = 0;
            int lowerLetters = 0;
            int digitCount = 0;
            int specialLetters = 0;

            foreach (char c in text)
            {
                if (Char.IsUpper(c))
                {
                    upperLetters++;
                }

                if (Char.IsLower(c))
                {
                    lowerLetters++;
                }

                if (Char.IsDigit(c))
                {
                    digitCount++;
                }

                if (!Char.IsLetterOrDigit(c))
                {
                    specialLetters++;
                }
            }

            // Запрос на проверку существующего ника при регистрации
            var nicknameSearch = App.Context.users.Where(u => u.user_Nickname == loginBox.Text).Select(u => u.user_Nickname).FirstOrDefault();

            if (nicknameSearch != null)
            {
                MessageBox.Show("This nickname already was taken!");
            }
            else
            {
                // Конкретные проверки заполненных данных при регистрации (логин, пароль, повторный пароль)
                if (loginBox.Text == "" || passwordBox.Password == "" || repeatPasswordBox.Password == "")
                {
                    MessageBox.Show("Fill the empty boxes!");
                }

                else if (passwordBox.Password != repeatPasswordBox.Password)
                {
                    MessageBox.Show("Passwords must be the same!");
                }

                else if (loginBox.Text.Length > 20 || passwordBox.Password.Length > 20)
                {
                    MessageBox.Show("Login and password must be maximum 20 symbols!");
                }

                else if (loginBox.Text.Length < 8 || passwordBox.Password.Length < 8)
                {
                    MessageBox.Show("Login and password must be minimum 8 symbols!");
                }

                else if (loginBox.Text.Contains(" ") || passwordBox.Password.Contains(" "))
                {
                    MessageBox.Show("Login and password must be wrote without spaces!");
                }

                else if (upperLetters < 1)
                {
                    MessageBox.Show("There are must be atleast one \"Upper Letter\" in password!");
                }

                else if (lowerLetters < 1)
                {
                    MessageBox.Show("There are must be atleast one \"Lower Letter\" in password!");
                }

                else if (digitCount < 1)
                {
                    MessageBox.Show("There are must be atleast one \"Digit\" in password!");
                }

                else if (specialLetters < 1)
                {
                    MessageBox.Show("There are must be atleast one \"Special Letter\" in password!");
                }

                else
                {
                    // Процедура хэширования пароля в MD5
                    MD5 md5Hash = MD5.Create();

                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(passwordBox.Password));
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sb.Append(data[i].ToString("x2"));
                    }

                    string hashedPassword = sb.ToString();

                    // Процедура добавления пользователя в базу данных с ролью пользователя
                    var userAddingProcedure = new mainEntities.user {
                        user_Nickname = loginBox.Text,
                        user_Password = hashedPassword,
                        role_Id = 1
                    };

                    // Сохранение осуществлённых изменений для базы данных
                    App.Context.users.Add(userAddingProcedure);
                    App.Context.SaveChanges();

                    MessageBox.Show("Account was successfully registered!");

                    this.Hide();

                    // Создание нового авторизационного окна
                    Authorization authWindow = new Authorization();
                    authWindow.Show();
                }
            }

            // Обнуление подсчитанных в имеющемся методе значений
            upperLetters = 0;
            lowerLetters = 0;
            digitCount = 0;
            specialLetters = 0;
        }
    }
}