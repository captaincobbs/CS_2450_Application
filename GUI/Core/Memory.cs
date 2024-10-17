using System.IO;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace UVSim
{
    public partial class Memory
    {
        public ObservableCollection<MemoryLine> Locations { get; set; }
        public readonly int MaxWord;
        public readonly int Capacity;


        public Memory(int capacity = 100, int maxWord = 9999)
        {
            MaxWord = maxWord;
            Capacity = capacity;
            Locations = [];


            for (int i = 0; i < capacity; i++)
            {
                Locations.Add(new MemoryLine()
                {
                    LineNumber = i,
                    Data = 0,
                    Instruction = BasicML.NONE
                });
            }
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
            if (location < 0 || location >= Locations.Count || Math.Abs(data) > MaxWord)
            {
                return false;
            }
            // If within bounds, assign data and instruction
            else
            {
                Locations[location].Data = data % 100;
                int instruct_data = Math.Abs(data / 100);
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
            if (!File.Exists(fileName))
            {
                return false;
            }
            else
            {
                foreach (string line in File.ReadLines(fileName))
                {
                    // Convert line to int
                    _ = int.TryParse(line, out int data);

                    // If value is equal to end signature, stop reading file as file is finished
                    if (data == -99999)
                    {
                        return true;
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

                // Write data, preserve columns by always ensuring 5 total digits
                sw.WriteLine($"{sign}{instruction:D2}{data:D2}");
            }
            // Finish file with end signature
            sw.WriteLine("-99999");
        }
    }
}
