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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp6
{


    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {


        List<Page> pages = new List<Page>()
        {
            new Setting1(),
            new Author(),
        };
        int str = 0;
        public Settings()
        {
            InitializeComponent();
            MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (str < 1)
            {
                str++;
                MainFrame.Navigate(pages[str]);
            }
            else
            {

            }
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (str > 0)
            {
                str--;
                MainFrame.GoBack();
            }
            else
            {

            }
        }
    }
}
