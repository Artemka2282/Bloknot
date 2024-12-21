using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using WpfApp6.Properties;

namespace WpfApp6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public RelayCommand Delete
        {
            get => new RelayCommand(nazvkl =>
            {
                File.Delete(Path.GetFullPath($"Save/{nazvkl}.txt"));
                Spisok = new ObservableCollection<EditableString>(Spisok.Where(el=>el.Str != nazvkl.ToString()));
            });
        }
        public class EditableString : INotifyPropertyChanged
        {
            private string _str;
            public string Str
            {
                get => _str;
                set
                {
                    if(_str != null)
                    {
                        _str1 = _str;
                    }
                    _str = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Str"));
                }
            }
            private string _str1;
            public string Str1
            {
                get => _str1;
                set
                {
                    _str1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Str1"));
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
        private bool isDarkMode = false;

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<EditableString> _spisok;

        public ObservableCollection<EditableString> Spisok {  get =>_spisok; 
            set { 
                _spisok = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("Spisok"));
            }
        }
        private EditableString vibor;
        public EditableString Vibor
        {
            get => vibor;
            set
            {
                vibor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vibor"));
            }
        }

        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            if (!Directory.Exists(Path.GetFullPath("Save")))
            {
                Directory.CreateDirectory(Path.GetFullPath("Save"));
            }

            // возрващаем только имена файлов
            Spisok = new ObservableCollection<EditableString>
                (Directory.EnumerateFiles(Path.GetFullPath("Save"))
                .Where(el => Path.GetExtension(el) == ".txt")
                .Select(el => new EditableString() { Str = Path.GetFileNameWithoutExtension(el)}));
                foreach(EditableString s in Spisok)
            {
                s.PropertyChanged += Obnova;
            }

                PropertyChanged += Obnova;
            Setting1.ColorChanged += Fon123;
            Setting1.TextColorChanged += Text123;
            Setting1.RazmerChanged += Razmer123;
            Setting1.ShriftChanged += Shrift123;
            Closing += Zakritie;
            try
            {
                string[] Data = File.ReadAllText(Path.GetFullPath($"settings.txt")).Split(';');
                string[] rgb1 = Data[0].Split(',');

                string[] rgb2 = Data[1].Split(',');
                Fon123(this,Color.FromRgb(byte.Parse(rgb1[0]), byte.Parse(rgb1[1]), byte.Parse(rgb1[2])));
                Text123(this,Color.FromRgb(byte.Parse(rgb2[0]), byte.Parse(rgb2[1]), byte.Parse(rgb2[2])));
                Razmer123(this,int.Parse(Data[2]));
                Shrift123(this,Data[3]);
            }
            catch
            {

            }
        }

        private void Zakritie(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Сохранить?", "Сохранить последнюю вкладку", MessageBoxButton.YesNo)==MessageBoxResult.Yes) 
            {
                CommandBinding_Executed(this, null);
            }

          
        }

        private void Text123(object sender, Color e)
        {
            InputTextBox.Foreground = new SolidColorBrush(e);
        }
        private void Shrift123(object sender, string e)
        {
            InputTextBox.FontFamily = new FontFamily(e);
        }
        private void Razmer123(object sender, int e)
        {
            InputTextBox.FontSize = e;
        }

        private void Fon123(object sender, Color e)
        {
           Background = new SolidColorBrush(e);
            Set.Background = Background;
            Set1.Background = Background;
            Set3.Background = Background;
            InputTextBox.Background = Background;
        }

        private void Obnova(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Vibor")
            {
                Text = File.ReadAllText(Path.GetFullPath($"Save/{Vibor.Str}.txt"));
            }
            else if(e.PropertyName == "Str")
            {
                foreach (EditableString s in Spisok)
                {
                    try
                    {
                        File.Move(Path.GetFullPath($"Save/{s.Str1}.txt"), Path.GetFullPath($"Save/{s.Str}.txt"));
                    }
                    catch
                    {
                        if (s.Str1 != s.Str)
                        {
                            File.Delete(Path.GetFullPath($"Save/{s.Str1}.txt"));
                        }
                    }
                    
                }
                    
            }
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            // Переключение между светлой и темной темой
            if (isDarkMode)
            {
                // Установить светлую тему
                this.Background = Brushes.White; // Фон окна
                Border.BorderBrush = Brushes.Black; // Цвет бордера
                ColorButton.Background = Brushes.Black; // Цвет кнопки
                ColorButton.ToolTip = "Цвет темы"; // Текст подсказки
                Set.Background = Brushes.White;
                Set.BorderBrush = Brushes.White;
                Set1.Background = Brushes.White;
                Set1.BorderBrush = Brushes.White;
                Set3.Background = Brushes.White;
                Set3.BorderBrush = Brushes.White;
                cvet.Source = new BitmapImage(new Uri("settings.png", UriKind.Relative));
                cvet1.Source = new BitmapImage(new Uri("blackhesh.png", UriKind.Relative));

                // Изменение цвета текста в TextBlock   
                text1.Foreground = Brushes.Black; // Цвет текста
                // Изменение цвета текста в TextBox
                InputTextBox.Foreground = Brushes.Black; // Цвет текста в TextBox
                InputTextBox.Background = Brushes.White; // Цвет фона в TextBox

            }
            else
            {
                // Установить темную тему
                this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c")); // Фон окна
                Border.BorderBrush = Brushes.White; // Цвет бордера
                ColorButton.Background = Brushes.White; // Цвет кнопки
                ColorButton.ToolTip = "Цвет темы"; // Текст подсказки
                Set.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c"));
                Set1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c"));
                Set3.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c"));

                cvet.Source = new BitmapImage(new Uri("settingswh.png",UriKind.Relative));
                cvet1.Source = new BitmapImage(new Uri("whitehesh.png", UriKind.Relative));
                Set.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c"));
                Set1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c"));
                Set3.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c"));
                text1.Foreground = Brushes.White; // Цвет текста
                // Изменение цвета текста в TextBox
                InputTextBox.Foreground = Brushes.White; // Цвет текста в TextBox
                InputTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c")); // Цвет фона в TextBox
            }

            // Переключение состояния
            isDarkMode = !isDarkMode;
        }

        private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (Vibor != null)
            {
                try
                {
                    File.WriteAllText(Path.GetFullPath($"Save/{Vibor.Str}.txt"), Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Не выбран файл для сохранения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CommandBinding_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_1(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            File.WriteAllText("Save/Новая вкладка.txt", "");
            foreach (EditableString s in Spisok)
            {
                s.PropertyChanged -= Obnova;
            }
            Spisok = new ObservableCollection<EditableString>
               (Directory.EnumerateFiles(Path.GetFullPath("Save"))
               .Where(el => Path.GetExtension(el) == ".txt")
               .Select(el => new EditableString() { Str = Path.GetFileNameWithoutExtension(el) }));
            foreach (EditableString s in Spisok)
            {
                s.PropertyChanged += Obnova;
            }
        }

        private void CommandBinding_CanExecute_1(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            if(Spisok != null)
            {
                if (Spisok.Count < 9)
                {
                    e.CanExecute = true;
                }
                else
                {
                    e.CanExecute = false;
                }
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
          
        }

        public void Use_Nastr()
        {
            Setting1 settings = new Setting1();
            Fon.Background = settings.Background;
        }

    }

}