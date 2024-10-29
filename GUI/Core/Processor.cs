using System;

namespace UVSim
{
    /// <summary>
    /// Models the processor of the virtual machine
    /// </summary>
    public class Processor(Memory mainMemory)
    {
        public readonly Register Accumulator = new();
        private readonly Memory mainMemory = mainMemory;
        private int currentLocation = 0;
        public event Action<bool>? AwaitingInput;

        /// <summary>
        /// Used mainly for the tests, will return the location to ensure Halt has worked.
        /// </summary>
        /// <returns>Current location in memory being read</returns>
        public int GetCurrentLocation()
        {
            return currentLocation;
        }

        /// <summary>
        /// Sets the value retained in the accumulator to the argument
        /// </summary>
        /// <param name="value">The value to store in the accumulator</param>
        public void SetAccumulator(int value)
        {
            Accumulator.Data = value;
        }

        /// <summary>
        /// Returns the current value retained in the accumulator
        /// </summary>
        /// <returns>The value stored in the accumulator</returns>
        public int GetAccumulator()
        {
            return Accumulator.Data;
        }

        /// <summary>
        /// Resets accumulator and begins executing from the specified location.
        /// Executes until end of memory or Halt is called.
        /// </summary>
        /// <param name="location">Location to begin execution</param>
        /// <returns>Executes and returns true if beginning location was valid, otherwise does not execute and returns false</returns>
        public bool Execute(int location = -1)
        {
            if (location < 0)
            {
                location = currentLocation;
            }

            Accumulator.Data = 0;
            bool last_command = false;
            if(location < 0 || location > 99)
            {
                return false;
            }
            else
            {
                currentLocation = location;
            }
            while(currentLocation < mainMemory.Capacity)
            {
                last_command = Interpret();
                currentLocation++;
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
            int location = mainMemory.Locations[currentLocation].Data;
            int instruction = mainMemory.Locations[currentLocation].Word;

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
                    break;
                default:
                    // Halts on invalid instruction
                    Halt();
                    return false;
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

                try
                {
                    mainMemory.WriteWord(location, word);
                }
                catch
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Writes the word at the specified location in memory to the console.
        /// </summary>
        /// <param name="location"></param>
        public string Write(int location)
        {
            string output = $"{mainMemory.Read(location)}";
            Console.WriteLine(output);
            return output;
        }

        /// <summary>
        /// Loads a word from the location in mainMemory into accumulator
        /// </summary>
        /// <param name="location"></param>
        public void Load(int location)
        {
            Accumulator.Data = mainMemory.Read(location);
        }

        /// <summary>
        /// Stores the value in the accumulator into the location in mainMemory
        /// </summary>
        /// <param name="location"></param>
        public void Store(int location)
        {
            mainMemory.WriteWord(location, Accumulator.Data);
        }
        /// <summary>
        /// Adds the word at the location in mainMemory to the value in the accumulator
        /// </summary>
        /// <param name="location"></param>
        public void Add(int location)
        {
            int result = Accumulator.Data + mainMemory.Read(location);
            Accumulator.Data = result;
        }
        /// <summary>
        /// Subtracts the word at the location in mainMemory from the value in the accumulator
        /// </summary>
        /// <param name="location"></param>
        public void Subtract(int location)
        {
            int result = Accumulator.Data - mainMemory.Read(location);
            Accumulator.Data = result;
        }
        /// <summary>
        /// Divides the value in the accumulator by the word at the location in mainMemory
        /// </summary>
        /// <param name="location"></param>
        public void Divide(int location)
        {
            int result = Accumulator.Data / mainMemory.Read(location);
            Accumulator.Data = result;
        }
        /// <summary>
        /// Multiplies the value in the accumulator by the word at the location in mainMemory
        /// </summary>
        /// <param name="location"></param>
        public void Multiply(int location)
        {
            int result = Accumulator.Data * mainMemory.Read(location);
            Accumulator.Data = result;
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
                    if(Accumulator.Data < 0)
                    {
                        currentLocation = location;
                    }
                    break;
                case (int) BasicML.BRANCHZERO:
                    if(Accumulator.Data == 0)
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
            currentLocation = mainMemory.Capacity;
        }
    }
    #endregion
}
