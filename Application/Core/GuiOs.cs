using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using System.Reflection.Metadata.Ecma335;

namespace UVSim
{
    public class GuiOs
    {
        public readonly Processor cpu;
        public readonly Memory mainMemory;

        public GuiOs()
        {
            mainMemory = new Memory();
            cpu = new Processor(mainMemory);
        }

        /// <summary>
        /// Prompts a user for a memory location and begins instruction execution at that location.
        /// </summary>
        public void Execute()
        {
            cpu.Execute(0);
        }
    }
}
