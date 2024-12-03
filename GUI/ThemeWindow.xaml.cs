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

            Button button = new()
            {
                Content = "Reset All to Default"
            };

            button.Click += ResetColors;

            ColorPickerPanel.Children.Add(button);

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

        // I have no clue how this works, ChatGPT made it, if it explodes I am not legally responsible
        private void ResetColors(object sender, RoutedEventArgs e)
        {
            var properties = typeof(Theme).GetProperties();
            foreach (var property in properties)
            {
                var defaultValue = property.GetCustomAttributes(typeof(DefaultValueAttribute), false)
                                           .Cast<DefaultValueAttribute>()
                                           .FirstOrDefault()?.Value;

                if (defaultValue != null)
                {
                    property.SetValue(App.Theme, defaultValue.ToString());
                }
            }

            // Reload the color pickers to reflect the reset values
            ColorPickerPanel.Children.Clear();
            LoadColors();

            foreach (Window window in Application.Current.Windows)
            {
                BindingOperations.GetBindingExpressionBase(window, BackgroundProperty)?.UpdateTarget();
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
