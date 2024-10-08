﻿using System;

namespace UVSim
{
    public class Processor
    {
        private readonly Register accumulator = new();
        private readonly Memory mainMemory;
        private int currentLocation = 0;

        public Processor(Memory mainMemory)
        {
            this.mainMemory = mainMemory;
        }

        /// <summary>
        /// Used mainly for the tests, will return the location to ensure Halt has worked.
        /// </summary>
        /// <returns></returns>
        public int GetCurrentLocation()
        {
            return currentLocation;
        }

        public void SetAccumulator(int value)
        {
            accumulator.Data = value;
        }

        public int GetAccumulator()
        {
            return accumulator.Data;
        }

        /// <summary>
        /// Resets accumulator and begins executing from the specified location.
        /// Executes until end of memory or Halt is called.
        /// </summary>
        /// <param name="location">Location to begin execution</param>
        /// <returns>Executes and returns true if beginning location was valid, otherwise does not execute and returns false</returns>
        public bool Execute(int location)
        {
            accumulator.Data = 0;
            bool last_command = false;
            if(location < 0 || location > 99)
            {
                return false;
            }
            else
            {
                currentLocation = location;
            }
            while(currentLocation < mainMemory.capacity)
            {
                last_command = Interpret();
            }
            return last_command;
        }

        #region Methods
        /// <summary>
        /// Executes the instruction at the currentLocation and updates the currentLocation
        /// </summary>
        public bool Interpret()
        {
            int word = mainMemory.Read(currentLocation);
            currentLocation++;
            //parse instruction
            int location = word % 100;
            int instruction = word / 100;

            //switch function autocompleted by AI (chatgpt)
            switch (instruction)
            {
                case (int)BasicML.READ:
                    Read(location);
                    break;
                case (int)BasicML.WRITE:
                    Write(location);
                    break;
                case (int)BasicML.LOAD:
                    Load(location);
                    break;
                case (int)BasicML.STORE:
                    Store(location);
                    break;
                case (int)BasicML.ADD:
                    Add(location);
                    break;
                case (int)BasicML.SUBTRACT:
                    Subtract(location);
                    break;
                case (int)BasicML.DIVIDE:
                    Divide(location);
                    break;
                case (int)BasicML.MULTIPLY:
                    Multiply(location);
                    break;
                case (int)BasicML.BRANCH:
                case (int)BasicML.BRANCHNEG:
                case (int)BasicML.BRANCHZERO:
                    Branch(instruction, location);
                    break;
                case (int)BasicML.HALT:
                    Halt();
                    Console.WriteLine("program HALT -- exit: success");
                    break;
                default:
                    // Halts on invalid instruction
                    Console.WriteLine($"error -- invalid instruction at location {location}\nprocess halted");
                    Halt();
                    return false;
                    break;
            }
            return true;
        }

        /// <summary>
        /// Reads a word from the keyboard into the specified memory location. Loops until a valid word can be stored.
        /// </summary>
        /// <param name="location"></param>
        public void Read(int location)
        {
            while (true)
            {
                Console.Write("input: ");
                string? in_word = Console.ReadLine();
                if (!int.TryParse(in_word, out int word))
                {
                    Console.WriteLine("error - please enter a 4 digit decimal number");
                    continue;
                }

                if (!mainMemory.WriteWord(location, word))
                {
                    Console.WriteLine($"error - max word is {mainMemory.max_word}");
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Writes the word at the specified location in memory to the console.
        /// </summary>
        /// <param name="location"></param>
        public void Write(int location)
        {
            Console.WriteLine($"{mainMemory.Read(location)}");
        }

        /// <summary>
        /// Loads a word from the location in mainMemory into accumulator
        /// </summary>
        /// <param name="location"></param>
        public void Load(int location)
        {
            accumulator.Data = mainMemory.Read(location);
        }

        /// <summary>
        /// Stores the value in the accumulator into the location in mainMemory
        /// </summary>
        /// <param name="location"></param>
        public void Store(int location)
        {
            mainMemory.WriteWord(location, accumulator.Data);
        }
        /// <summary>
        /// Adds the word at the location in mainMemory to the value in the accumulator
        /// </summary>
        /// <param name="location"></param>
        public void Add(int location)
        {
            int result = accumulator.Data + mainMemory.Read(location);
            accumulator.Data = result;
        }
        /// <summary>
        /// Subtracts the word at the location in mainMemory from the value in the accumulator
        /// </summary>
        /// <param name="location"></param>
        public void Subtract(int location)
        {
            int result = accumulator.Data - mainMemory.Read(location);
            accumulator.Data = result;
        }
        /// <summary>
        /// Divides the value in the accumulator by the word at the location in mainMemory
        /// </summary>
        /// <param name="location"></param>
        public void Divide(int location)
        {
            int result = accumulator.Data / mainMemory.Read(location);
            accumulator.Data = result;
        }
        /// <summary>
        /// Multiplies the value in the accumulator by the word at the location in mainMemory
        /// </summary>
        /// <param name="location"></param>
        public void Multiply(int location)
        {
            int result = accumulator.Data * mainMemory.Read(location);
            accumulator.Data = result;
        }
        /// <summary>
        /// Branches to the location based on the condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="location"></param>
        public void Branch(int condition, int location)
        {
            switch (condition) {
                case (int) BasicML.BRANCH:
                    currentLocation = location;
                    break;
                case (int) BasicML.BRANCHNEG:
                    if(accumulator.Data < 0)
                    {
                        currentLocation = location;
                    }
                    break;
                case (int) BasicML.BRANCHZERO:
                    if(accumulator.Data == 0)
                    {
                        currentLocation = location;
                    }
                    break;
            }
        }
        /// <summary>
        /// Halts execution by setting current location beyond possible locations
        /// </summary>
        public void Halt()
        {
            currentLocation = mainMemory.capacity;
        }
    }
    #endregion
}
