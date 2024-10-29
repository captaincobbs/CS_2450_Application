using System;

namespace UVSim
{
    /// <summary>
    /// Models the processor of the virtual machine
    /// </summary>
    public class Processor(Memory mainMemory)
    {
        public event Action<bool, int>? AwaitingInput;
        public event Action<string>? OnOutput;
        public bool IsAwaitingInput { get; set; } = false;

        public readonly Register Accumulator = new();
        private readonly Memory mainMemory = mainMemory;
        private int currentLocation = 0;
        private int savedLocation = 0;
        private bool unbroken = true;

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
            if(location < 0 || location > 99)
            {
                return false;
            }
            else
            {
                currentLocation = location;
            }

            bool operationSuccess = false;
            while(currentLocation < mainMemory.Capacity && unbroken)
            {
                operationSuccess = Interpret();
                currentLocation++;

                if (!unbroken)
                {
                    return true; // Returns false if it expects you to be coming back
                }
            }
            return operationSuccess;
        }

        #region Methods
        /// <summary>
        /// Executes the instruction at the currentLocation and updates the currentLocation
        /// </summary>
        public bool Interpret()
        {
            int location = mainMemory.Locations[currentLocation].Data;
            int instruction = mainMemory.Locations[currentLocation].Word;

            //switch function autocompleted by AI (chatgpt)
            switch (instruction)
            {
                case 0: // Continue if it is just a stored number
                    break;
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
            AwaitingInput?.Invoke(true, location);
            IsAwaitingInput = true;
            savedLocation = location;
            unbroken = false;
        }

        /// <summary>
        /// Writes the word at the specified location in memory to the console.
        /// </summary>
        /// <param name="location"></param>
        public void Write(int location)
        {
            string output = $"{mainMemory.Read(location)}";
            OnOutput?.Invoke(output);
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
            // Offset by -1 because it is immediately incremented after this
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

        /// <summary>
        /// Send input to awaiting READ command so processor may continue
        /// </summary>
        public void ReceiveInput(int input)
        {
            mainMemory.WriteWord(savedLocation, input);
            OnOutput?.Invoke($"> {input}");

            IsAwaitingInput = false;
            AwaitingInput?.Invoke(false, currentLocation);
            unbroken = true;
            Execute(-1);
        }
    }
    #endregion
}
