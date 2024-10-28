using System.ComponentModel;

namespace UVSim
{
    /// <summary>
    /// where the data for the table in the UI is stored
    /// </summary>
    public class MemoryLine : INotifyPropertyChanged
    {
        private int _lineNumber;
        private int _data;
        private BasicML _instruction;
        public int LineNumber
        {
            get { return _lineNumber; }
            set {
                _lineNumber = value;
                OnPropertyChanged(nameof(LineNumber));
            }
        }

        public int Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }
        public BasicML Instruction
        {
            get { return _instruction; }
            set {
                _instruction = value;
                OnPropertyChanged(nameof(Instruction));
            }
        }

        public int Word { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}