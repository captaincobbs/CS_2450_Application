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
    }
}
