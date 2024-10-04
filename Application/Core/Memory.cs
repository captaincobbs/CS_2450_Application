using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace UVSim
{
    public class Memory
    {
        private readonly Dictionary<int, int> locations;
        public readonly int max_word;
        public readonly int capacity;

        public Memory(int capacity = 100, int max_word=9999)
        {
            this.max_word = max_word;
            this.capacity = capacity;
            locations = [];
            for (int i = 0; i < capacity; i++)
            {
                locations.Add(i, 0);
            }
        }

        /// <summary>
        /// Returns a word stored at the specififed memory location.
        /// </summary>
        /// <param name="location">An integer representing a memory location</param>
        /// <returns>The word at the memory location. If location is invalid it returns 0</returns>
        public int Read(int location)
        {
            if (location < 0 || location >= locations.Count)
            {
                return 0;
            }
            else
            {
                return locations[location];
            }
        }

        /// <summary>
        /// Writes a word of data to the specified memory location
        /// </summary>
        /// <param name="location">int</param>
        /// <param name="data">int</param>
        /// <returns>Writes and returns true if location is valid and data is valid, Does not write and returns false otherwise</returns>
        public bool WriteWord(int location, int data)
        {
            if (location < 0 || location >= locations.Count || Math.Abs(data) > max_word)
            {
                return false;
            }
            else
            {
                locations[location] = data;
                return true;
            }
        }

        /// <summary>
        /// Writes a file to memory line by line, starting at the specified location
        /// </summary>
        /// <param name="location">Location in memory to begin writing</param>
        /// <param name="fileName">File with contents to write to memory</param>
        /// <returns>True if successful, fale otherwise</returns>
        /// consider returning WriteStatus obj with a bool and a message
        /// could handle failure of an individual line better
        public bool WriteFile(int location, string fileName)
        {
            if (!File.Exists(fileName))
            {
                return false;
            }
            else
            {
                foreach (string line in File.ReadLines(fileName))
                {
                    if (Int32.TryParse(line, out int data) && WriteWord(location, data))
                    {
                        location++;
                    }
                }
                return true;
            }
        }
    }
}
