using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace UVSim
{
    /// <summary>
    /// Interaction logic for ThemeWindow.xaml
    /// </summary>
    public partial class ThemeWindow : Window
    {
        public ThemeWindow()
        {
            InitializeComponent();
            Closing += OnWindowClosing;
            LoadColors();
        }

        #region Functions
        /// <summary>
        /// Finds all properties in <see cref="Theme"/>, and adds an associated bound <see cref="ColorPicker"/> and <see cref="Label"/> to the stackpanel
        /// </summary>
        private void LoadColors()
        {
            var properties = typeof(Theme).GetProperties();

            foreach (var property in properties)
            {
                ColorPicker picker = new()
                {
                    Margin = new Thickness(5),
                    SelectedColor = (Color)(ColorConverter.ConvertFromString(property.GetValue(App.Theme)?.ToString()) ?? "#FFFFFF")
                };

                Binding binding = new()
                {
                    Source = App.Theme,
                    Path = new PropertyPath(property.Name),
                    Converter = new ColorStringConverter(),
                    Mode = BindingMode.TwoWay,
                };

                picker.SetBinding(ColorPicker.SelectedColorProperty, binding);

                Label label = new() { Content = property.Name };
                ColorPickerPanel.Children.Add(label);
                ColorPickerPanel.Children.Add(picker);
            }
        }
        #endregion

        #region Events
        private void OnWindowClosing(object? sender, CancelEventArgs e)
        {
            DialogResult = true;
            App.SaveColorScheme();
        }
        #endregion
    }
}
