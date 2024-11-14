using System.Windows;

namespace UVSim
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public int? Capacity { get; private set; } = 0;
        public int? InitialLines { get; private set; } = 0;

        public InputWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Capacity = InputCapacity.Value ?? null;
                InitialLines = InputInitial.Value ?? null;

                if (Capacity == null | InitialLines == null)
                {
                    DialogResult = false;
                }

                InitialLines = Math.Min((int)Capacity, (int)InitialLines);
                DialogResult = true;
            }
            catch
            {
                DialogResult = false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
