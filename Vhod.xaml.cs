using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp6
{
    /// <summary>
    /// Логика взаимодействия для Vhod.xaml
    /// </summary>
    public partial class Vhod : Window
    {
        public Vhod()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordBox.Password;
            if (!string.IsNullOrEmpty(password)) {
                if(File.Exists("namefile"))
                {
                    string hesh = File.ReadAllText("namefile");
                    if(SecurePasswordHasher.Verify(password, hesh))
                    {
                        MainWindow window = new MainWindow();
                        window.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Неправильный пароль!");
                    }
                }
                else
                {
                    File.WriteAllText("namefile", SecurePasswordHasher.Hash(password));
                    MainWindow window = new MainWindow();
                    window.Show();
                    Close();
                }
            
            }
            else
            {
                MessageBox.Show("Строка пустая!");
            }
        }
    }
}
