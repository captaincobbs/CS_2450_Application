namespace UVSim
{
    public class OperatingSystemGui
    {
        public readonly Processor CPU;
        public readonly Memory MainMemory;

        /// <summary>
        /// Creates and links a CPU and MainMemory
        /// </summary>
        public OperatingSystemGui(int capacity)
        {
            MainMemory = new Memory(capacity, ProgramType.SixDigit);
            CPU = new Processor(MainMemory);
        }

        /// <summary>
        /// Begins instruction execution at location 0.
        /// </summary>
        public bool Execute(int location)
        {
            return CPU.Execute(location);
        }
    }
}
