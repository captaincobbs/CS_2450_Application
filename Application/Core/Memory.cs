﻿using System;
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
        /// <summary>
        /// A class which represents each memory location formatted for the GUI.
        /// </summary>
        public class MemoryLine : ObservableObject
        {
            private int _lineNumber;
            private int _data;
            private BasicML _instruction;
            public int LineNumber
            {
                get { return _lineNumber; }
                set { SetProperty(ref _lineNumber, value); }
            }

            public int Data
            {
                get { return _data; }
                set 
                { 
                    SetProperty(ref _data, value);
                    int instruct_data = _data / 100;
                    Instruction = Enum.IsDefined(typeof(BasicML), instruct_data) ? (BasicML)instruct_data : BasicML.NONE;
                }
            }
            public BasicML Instruction
            {
                get { return _instruction; }
                set { SetProperty(ref _instruction, value); }
            }
        }


        public readonly ObservableCollection<MemoryLine> locations;
        public readonly int MaxWord;
        public readonly int Capacity;

        public Memory(int capacity = 100, int maxWord=9999)
        {
            MaxWord = maxWord;
            Capacity = capacity;
            locations = [];
            for (int i = 0; i < capacity; i++)
            {
                locations.Add(new MemoryLine()
                {
                    LineNumber = i,
                    Data = 0,
                    Instruction = BasicML.NONE
                });

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
                return locations[location].Data;
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
            if (location < 0 || location >= locations.Count || Math.Abs(data) > MaxWord)
            {
                return false;
            }
            else
            {
                locations[location].Data = data;
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
                    if (int.TryParse(line, out int data) && WriteWord(location, data))
                    {
                        location++;
                    }
                }
                return true;
            }
        }
        /// <summary>
        /// Saves the contents of all memory locations to a file.
        /// </summary>
        /// <param name="fileName">The filepath of the file to create or overwrite</param>
        public void SaveToDisk(string fileName)
        {
            using (StreamWriter sw = File.CreateText(fileName))
            {
                foreach(var location in locations)
                {
                    int data = location.Data;
                    if (data > 0)
                    {
                        sw.Write("+");
                    }
                    sw.WriteLine(data);
                }
            }
        }
    }
}
