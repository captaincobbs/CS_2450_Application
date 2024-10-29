using System.ComponentModel;

namespace UVSim
{
    /// <summary>
    /// Retains data for the Processor
    /// </summary>
    public class Register
    {
        private int _data;
        public int Data
        {
            get => _data;
            set
            {
                if (_data != value)
                {
                    _data = value;
                    PropertyChanged(value); // Notify the UI of changes
                }
            }
        }

        public Register()
        {
            Data = 0;
        }

        public event Action<int>? OnPropertyChanged;

        private void PropertyChanged(int data)
        {
            OnPropertyChanged?.Invoke(data);
        }
    }
}
