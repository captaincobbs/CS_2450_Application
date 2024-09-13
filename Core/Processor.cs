namespace UVSim
{
    public class Processor
    {
        private Register accumulator;
        private int location;
        private int rawInstruction;

        public int Interpret(int instruction)
        {
            return default;
        }

        public int Add(int instruction)
        {
            return default;
        }

        public int Subtract(int instruction)
        {
            return default;
        }

        public int Divide(int instruction)
        {
            return default;
        }

        public int Multiply(int instruction)
        {
            return default;
        }

        public void Branch(BranchCondition condition, int instruction)
        {

        }

        public void Halt()
        {

        }
    }
}
