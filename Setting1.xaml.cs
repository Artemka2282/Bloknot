using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp6
{
    /// <summary>
    /// Логика взаимодействия для Setting1.xaml
    /// </summary>
    public partial class Setting1 : Page
    {

        public Color background = (Color)ColorConverter.ConvertFromString("#FFFFFF");
        public Color shrift = (Color)ColorConverter.ConvertFromString("#000000");
        private int text = 12;
        public List<string> list
        {
           get;set;
        }

        public string Selected
        {
            get;set;
        }
        public Setting1()
        {
            InitializeComponent();
            list = new List<string>()
            {
                "Times New Roman",
                "Arial",
                "Courier New",
                "Impact",
            };
            Selected = list[0];
            DataContext = this;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            ColorPalettePopup.IsOpen = !ColorPalettePopup.IsOpen;
        }

        private void ColorButton_Click1(object sender, RoutedEventArgs e)
        {
            ColorPalettePopup1.IsOpen = !ColorPalettePopup1.IsOpen;
        }
        private void ColorButton_Clicked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                Color selectedColor = ((SolidColorBrush)button.Background).Color;

                ColorButton.Background = new SolidColorBrush(selectedColor);
                background = selectedColor;
            }
            ColorPalettePopup.IsOpen = false;
        }

        private void ColorButton_Clicked1(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                Color selectedColor = ((SolidColorBrush)button.Background).Color;

                ColorButton123.Background = new SolidColorBrush(selectedColor);
                shrift = selectedColor;
            }
            ColorPalettePopup1.IsOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ColorChanged?.Invoke(this,background);
            TextColorChanged?.Invoke(this, shrift);
            RazmerChanged?.Invoke(this, text);
            ShriftChanged?.Invoke(this, Selected);
            File.WriteAllText(System.IO.Path.GetFullPath($"settings.txt"), $"{background.R},{background.G},{background.B};{shrift.R},{shrift.G},{shrift.B};{text};{Selected}");
        }

        private void FontSizeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
                       e.Handled = !IsTextAllowed(e.Text);
        }

        private void FontSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(FontSizeTextBox.Text, out  text))
            {
                if (text > 50)
                {
                    text = 50;
                    FontSizeTextBox.Text = "50";
                    FontSizeTextBox.CaretIndex = FontSizeTextBox.Text.Length; // Устанавливаем курсор в конец
                }
                else
                {
                    
                }
            }

        }
        private static bool IsTextAllowed(string text)
        {
            return int.TryParse(text, out _);
        }

        public static event EventHandler<Color> ColorChanged;
        public static event EventHandler<string> ShriftChanged;
        public static event EventHandler<int> RazmerChanged;
        public static event EventHandler<Color> TextColorChanged; // Новое событие для изменения цвета текста

    }
}
