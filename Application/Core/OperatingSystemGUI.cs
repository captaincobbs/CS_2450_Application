namespace UVSim
{
    public class OperatingSystemGUI
    {
        public readonly Processor CPU;
        public readonly Memory MainMemory;

        /// <summary>
        /// Creates and links a CPU and MainMemory
        /// </summary>
        public OperatingSystemGUI()
        {
            MainMemory = new Memory();
            CPU = new Processor(MainMemory);
        }

        /// <summary>
        /// Begins instruction execution at location 0.
        /// </summary>
        public void Execute()
        {
            CPU.Execute(0);
        }
    }
}
