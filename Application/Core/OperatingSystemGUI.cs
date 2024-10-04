namespace UVSim
{
    public class OperatingSystemGUI
    {
        public readonly Processor CPU;
        public readonly Memory MainMemory;

        public OperatingSystemGUI()
        {
            MainMemory = new Memory();
            CPU = new Processor(MainMemory);
        }

        /// <summary>
        /// Prompts a user for a memory location and begins instruction execution at that location.
        /// </summary>
        public void Execute()
        {
            CPU.Execute(0);
        }
    }
}
