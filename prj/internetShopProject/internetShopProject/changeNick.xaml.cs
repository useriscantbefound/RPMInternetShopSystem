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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Data.Entity.Migrations;

namespace internetShopProject
{
    /// <summary>
    /// Логика взаимодействия для changeNick.xaml
    /// </summary>
    public partial class changeNick : Window
    {
        public changeNick()
        {
            InitializeComponent();
        }

        void newNickBtn_Clicked(object sender, RoutedEventArgs e)
        {
            string nickInSession = (string)App.Current.Properties["nicknameInSession"];

            // Запрос на выборку поиска ника пользователя для его последующего изменения
            var nicknameSearch = App.Context.users.Where(u => u.user_Nickname == newNickTextBox.Text).Select(u => u.user_Nickname).FirstOrDefault();

            if (nicknameSearch != null)
            {
                MessageBox.Show("This nickname already was taken!");
            } else
            {
                // Метод, подсчитывающий соответствующее количество каждого необходимого условия в новом заполненном нике
                string newNick = newNickTextBox.Text;
                int upperLetters = 0;
                int lowerLetters = 0;
                int digitCount = 0;
                int specialLetters = 0;

                foreach (char c in newNick)
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

                // Конкретные проверки заполненного нового ника
                if (newNickTextBox.Text == "")
                {
                    MessageBox.Show("Fill the empty box!");
                }

                else if (newNickTextBox.Text.Length > 20)
                {
                    MessageBox.Show("New nickname must be maximum 20 symbols!");
                }

                else if (newNickTextBox.Text.Length < 8)
                {
                    MessageBox.Show("New nickname must be minimum 8 symbols!");
                }

                else if (newNickTextBox.Text.Contains(" "))
                {
                    MessageBox.Show("New nickname must be wrote without spaces!");
                }

                else
                {
                    // Процедура изменения ника с занесением нового ника в базу данных
                    var foundNickname = App.Context.users.FirstOrDefault(n => n.user_Nickname == nickInSession);

                    if (foundNickname != null)
                    {
                        foundNickname.user_Nickname = newNickTextBox.Text;
                        App.Context.SaveChanges();
                    }

                    // Занесение нового ника в одно из значений глобального списка программы для регулирования данных (сессия с ником)
                    MessageBox.Show("Nick \"" + nickInSession + "\"" + " was successfully changed on \"" + newNickTextBox.Text + "\".");
                    App.Current.Properties["nicknameInSession"] = newNickTextBox.Text;
                    this.Hide();
                }

                // Обнуление подсчитанных в имеющемся методе значений
                upperLetters = 0;
                lowerLetters = 0;
                digitCount = 0;
                specialLetters = 0;
            }
        }
    }
}