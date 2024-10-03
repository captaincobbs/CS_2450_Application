using Avalonia.Controls;
using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace UVSim
{
    /// <summary>
    /// Interface with the machine components
    /// </summary>
    public class OperatingSystem
    {
        private readonly Processor cpu;
        private readonly Memory mainMemory;

        public OperatingSystem()
        {
            mainMemory = new Memory();
            cpu = new Processor(mainMemory);
            Console.WriteLine("Welcome to UV Sim OS v1.0...\n\n");
        }
        /// <summary>
        /// The text-based user interface driver.
        /// </summary>
        public void BootToTextUI()
        {
            while (true)
            {
                Console.Write("""
                    -----Main Menu-----
                1: Fill Memory Location
                2: Load File
                3: Execute Instructions
                4: View Memory
                5: Restart
                6: Exit




                """);
                int condition;
                bool success;
                while (true)
                {
                    Console.Write("Selection (1-6): ");
                    string? selection = Console.ReadLine();
                    switch (selection)
                    {
                        case "1":
                            ManualFill();
                            condition = 1;
                            success = true;
                            break;
                        case "2":
                            condition = 2;
                            success = LoadFile();
                            break;
                        case "3":
                            Execute();
                            condition = 3;
                            success = true;
                            break;
                        case "4":
                            ViewMemory();
                            condition = 4;
                            success = true;
                            break;
                        case "5":
                            Restart();
                            condition = 5;
                            success = true;
                            break;
                        case "6":
                            Console.Write("Are you sure you want to exit (n to cancel): ");
                            string? exit = Console.ReadLine();
                            if(exit != null && (exit == "n" || exit == "N"))
                            {
                                continue;
                            }
                            else
                            {
                                return;
                            }
                        default:
                            Console.WriteLine("error -- make a selction 1-6");
                            continue;
                    }
                    break;
                }
                if (condition <= 2)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 8);
                    switch (condition)
                    {
                        case 1:
                            Console.Write("Manual Fill Successful");
                            break;
                        case 2:
                            string status = success ? "File Load Successful" : "File Load Failed";
                            Console.Write(status);
                            break;
                    }
                    Console.SetCursorPosition(0, 0);
                }
                else
                {
                    Console.WriteLine("\n");
                }
            }
        }
        /// <summary>
        /// Allows user to input a word into a location in memory.
        /// </summary>
        private void ManualFill()
        {
            int location;
            while (true)
            {
                Console.Write($"Select an address to fill (0-{mainMemory.capacity - 1}): ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out location) || location < 0 || location > mainMemory.capacity - 1)
                {
                    Console.WriteLine($"error -- input a location 0-{mainMemory.capacity - 1}");
                }
                else
                {
                    break;
                }
            }
            cpu.Read(location);
        }
        /// <summary>
        /// Allows user to load a file into memory.
        /// </summary>
        private bool LoadFile()
        {
            const string path_chars = "\\/";
            char[] chars = path_chars.ToCharArray();
            Console.WriteLine("Files:");
            foreach(string file in Directory.GetFiles($"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Files"))
            {
                Console.WriteLine($"\t{file[(file.LastIndexOfAny(chars) + 1)..]}");
            }
            string? filePath;
            while (true)
            {
                Console.Write("Select a file to load from the list: ");
                filePath = Console.ReadLine();
                if(!String.IsNullOrWhiteSpace(filePath))
                {
                    filePath = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Files{Path.DirectorySeparatorChar}{filePath}";
                    if(File.Exists(filePath))
                    {
                        break;
                    }
                }
                Console.WriteLine("error -- select a file from the list");
            }
            int location;
            while (true)
            {
                Console.Write($"Select an address to begin loading file (0-{mainMemory.capacity - 1}): ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out location) || location < 0 || location > mainMemory.capacity - 1)
                {
                    Console.WriteLine($"error -- input a location 0-{mainMemory.capacity - 1}");
                }
                else
                {
                    break;
                }
            }
            return mainMemory.WriteFile(location, filePath);
        }
        /// <summary>
        /// Prompts a user for a memory location and begins instruction execution at that location.
        /// </summary>
        private void Execute()
        {
            int location;
            while (true)
            {
                Console.Write($"Select an address to begin execution (0-{mainMemory.capacity - 1}): ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out location) || location < 0 || location > mainMemory.capacity - 1)
                {
                    Console.WriteLine($"error -- input a location 0-{mainMemory.capacity - 1}");
                }
                else
                {
                    break;
                }
            }
            cpu.Execute(location);
        }
        /// <summary>
        /// Displays the memory locations and the values stored
        /// </summary>
        private void ViewMemory()
        {
            Console.WriteLine("Address:\tData");
            for(int i = 0; i < mainMemory.capacity; i++)
            {
                string data = String.Format("     {0:00}:\t{1:+0000;-0000}", i, mainMemory.Read(i));
                Console.WriteLine(data);
            }
        }
        /// <summary>
        /// Restarts the OS by resetting memory.
        /// </summary>
        private void Restart()
        {
            Console.Clear();
            Console.Write("Restarting");
            for(int i = 0; i < mainMemory.capacity; i++)
            {
                mainMemory.WriteWord(i, 0);
                Console.Write(".");
                if((i+1)%3 == 0)
                {
                    Console.Write("\b\b\b");
                }
            }
            Console.WriteLine("\nWelcome to UV Sim OS v1.0...\n");
        }
    }
}
