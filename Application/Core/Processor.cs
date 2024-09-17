using System.Diagnostics;

namespace UVSim
{
    public class Processor
    {
        private readonly Register accumulator = new Register();
        private readonly Memory mainMemory;
        private int currentLocation = 0;
        private int rawInstruction;

        public Processor(Memory mainMemory)
        {
            this.mainMemory = mainMemory;
        }
        public int Interpret(int instruction)
        {
            return default;
        }
        public void Load(int location)
        {
            accumulator.Data = mainMemory.Read(location);
        }
        public void Store(int location)
        {
            mainMemory.WriteWord(location, accumulator.Data);
        }
        public void Add(int location)
        {
            int result = accumulator.Data + mainMemory.Read(location);
            accumulator.Data = result;
        }

        public void Subtract(int location)
        {
            int result = accumulator.Data - mainMemory.Read(location);
            accumulator.Data = result;
        }

        public void Divide(int location)
        {
            int result = accumulator.Data / mainMemory.Read(location);
            accumulator.Data = result;
        }

        public void Multiply(int location)
        {
            int result = accumulator.Data * mainMemory.Read(location);
            accumulator.Data = result;
        }

        public void Branch(BranchCondition condition, int location)
        {
            switch (condition) {
                case BranchCondition.BRANCH:
                    currentLocation = location;
                    break;
                case BranchCondition.BRANCHNEG:
                    if(accumulator.Data < 0)
                    {
                        currentLocation = location;
                    }
                    break;
                case BranchCondition.BRANCHZERO:
                    if(accumulator.Data == 0)
                    {
                        currentLocation = location;
                    }
                    break;
            }
        }

        public void Halt()
        {

        }
    }
}
