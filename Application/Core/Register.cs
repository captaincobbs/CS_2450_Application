namespace UVSim
{
    public class Register
    {
        private int data;

        public int Load => data;
        public void Store(int data)
        {
            this.data = data;
        }
    }
}
