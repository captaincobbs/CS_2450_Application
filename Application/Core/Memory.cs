using System.Collections.Generic;

namespace UVSim
{
    public class Memory
    {
        private readonly Dictionary<int, int> locations;

        public Memory()
        {
            locations = [];
            for (int i = 0; i <= 100; i++)
            {
                locations.Add(i, 0);
            }
        }

        public int Read(int location)
        {
            return locations[location];
        }

        public int WriteWord(int location, int data)
        {
            return default;
        }

        public void WriteFile(int location, string fileName)
        {
            
        }
    }
}
