using System.IO;
using System.Collections.ObjectModel;

namespace UVSim
{
    public partial class Memory
    {
        private ProgramType _programType;
        public ProgramType ProgramType
        {
            get => _programType;
            set
            {
                _programType = value;
            }
        }
        int MaxWord
        {
            get
            {
                switch (ProgramType)
                {
                    // If four digit, use four digit max
                    case ProgramType.FourDigit:
                        return 9999;
                    // Otherwise, use six digit max
                    default:
                        return 999999;
                }
            }
        }
        public ObservableCollection<MemoryLine> Locations { get; set; }
        public readonly int Capacity;


        public Memory(int capacity = 250, ProgramType programType = ProgramType.None)
        {
            ProgramType = programType;
            Capacity = capacity;
            Locations = [];
        }

        /// <summary>
        /// Returns the word stored at the specified memory location.
        /// </summary>
        /// <param name="location">Memory location</param>
        /// <returns>The word at the memory location, or 0 if invalid.</returns>
        public int Read(int location)
        {
            if (location < 0 || location >= Locations.Count)
            {
                return 0;
            }
            else
            {
                return Locations[location].Data;
            }
        }

        /// <summary>
        /// Writes a word of data to the specified memory location.
        /// </summary>
        /// <param name="location">Memory location</param>
        /// <param name="data">Data to write</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool WriteWord(int location, int data)
        {
            // If not within bounds, return
            if (location < 0 || location >= Locations.Count || Math.Abs(data) > MaxWord) // The biggest possible value a number can be
            {
                return false;
            }
            // If within bounds, assign data and instruction
            else
            {
                Locations[location].Data = data % (int)ProgramType; // Get ending digits for data value
                int instruct_data = Math.Abs(data / (int)ProgramType); // Get starting digits for instruction value, ensure instructions are positive
                Locations[location].Instruction = Enum.IsDefined(typeof(BasicML), instruct_data) ? (BasicML)instruct_data : BasicML.NONE;
                return true;
            }
        }

        /// <summary>
        /// Writes a file to memory line by line, starting at the specified location.
        /// </summary>
        /// <param name="location">Location in memory to begin writing</param>
        /// <param name="fileName">File path</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool ReadFile(int location, string fileName)
        {
            ProgramType = ProgramType.None;

            // If the file doesn't exist, don't try loading
            if (!File.Exists(fileName))
            {
                return false;
            }
            else
            {
                string[] lines = File.ReadAllLines(fileName);

                // If there are more lines than the capacity, don't try loading
                if (lines.Length > Capacity)
                {
                    return false;
                }

                foreach (string line in lines)
                {
                    // Convert line to int
                    bool isInt = int.TryParse(line, out int data);

                    // If file is invalid, return
                    if (!isInt)
                    {
                        return false;
                    }

                    // If value is equal to end signature, stop reading file as file is finished loading
                    if (data == -99999 || data == -9999999)
                    {
                        break;
                    }

                    // How many significant figures in each line, excluding the sign
                    switch (line.Length - 1)
                    {
                        // If the loaded number has four digits
                        case 4 :
                            // And the ProgramType hasn't yet been detected, set it to a four digit program
                            if (ProgramType == ProgramType.None)
                            {
                                ProgramType = ProgramType.FourDigit;
                            }
                            // And the ProgramType is Six Digits, return false and stop loading
                            else if (ProgramType == ProgramType.SixDigit)
                            {
                                return false;
                            }
                            break;
                        // If the loaded number has six digits
                        case 6:
                            // And the ProgramType hasn't yet been detected, set it to a six digit program
                            if (ProgramType == ProgramType.None)
                            {
                                ProgramType = ProgramType.SixDigit;
                            }
                            // And the ProgramType is Four Digits, return false and stop loading
                            else if (ProgramType == ProgramType.FourDigit)
                            {
                                return false;
                            }
                            break;
                        // If the loaded number does not have four or six digits, return false and stop loading
                        default:
                            return false;
                    }

                    // Otherwise, attempt to write the word
                    if (WriteWord(location, data))
                    {
                        location++;
                    }
                    else
                    {
                        return false;
                    }
                }

                for (int i = location; i < Capacity; i++)
                {
                    _ = WriteWord(location, 0);
                }
                return true;
            }
        }

        /// <summary>
        /// Saves the contents of all memory locations to a file.
        /// </summary>
        /// <param name="fileName">The filepath of the file to create or overwrite.</param>
        public void SaveFile(string fileName)
        {
            using StreamWriter sw = File.CreateText(fileName);
            for (int i = 0; i < Locations.Count; i++)
            {
                MemoryLine? location = Locations[i];
                int data = location.Data;
                int instruction = (int)location.Instruction;

                // Save sign of data
                string sign = string.Empty;
                if (data >= 0)
                    sign += "+";
                else
                    sign += "-";

                // Write data, preserve columns by always ensuring 7 total digits
                sw.WriteLine($"{sign}{instruction:D3}{data:D3}");
            }
            // Finish file with end signature
            sw.WriteLine("-9999999");
        }
    }
}
