Cloning the Repository
Begin by cloning the repository. This can be done by running this command in the command line: git clone https://github.com/captaincobbs/CS_2450_Application.git

Building the Application
Build the application by navigating to the cloned directory and selecting the shortcut to the executable file (.exe) in the main directory and run it. 

Writing and Executing BasicML
Once the application has been built, you should be able to start executing BasicML files by following the input prompts displayed in the terminal.

BasicML instructions (from Group Project Milestone 2 description) :
I/O operation:
READ = 10 Read a word from the keyboard into a specific location in memory.
WRITE = 11 Write a word from a specific location in memory to screen.
Load/store operations:
LOAD = 20 Load a word from a specific location in memory into the accumulator.
STORE = 21 Store a word from the accumulator into a specific location in memory.
Arithmetic operation:
ADD = 30 Add a word from a specific location in memory to the word in the accumulator (leave the result in the accumulator)
SUBTRACT = 31 Subtract a word from a specific location in memory from the word in the accumulator (leave the result in the accumulator)
DIVIDE = 32 Divide the word in the accumulator by a word from a specific location in memory (leave the result in the accumulator).
MULTIPLY = 33 multiply a word from a specific location in memory to the word in the accumulator (leave the result in the accumulator).
Control operation:
BRANCH = 40 Branch to a specific location in memory
BRANCHNEG = 41 Branch to a specific location in memory if the accumulator is negative.
BRANCHZERO = 42 Branch to a specific location in memory if the accumulator is zero.
HALT = 43 Pause the program
The last two digits of a BasicML instruction are the operand – the address of the memory location containing the word to which the operation applies.


Example of a BasicML program:
10 00 // READ input into memory location 00
20 00 // LOAD value from memory location 00 into accumulator 
30 01 // ADD value from memory location 01 to value in the accumulator 
21 02 // STORE accumulator value into memory location 02
11 02 // WRITE value stored in memory location 02 to screen 
43 // HALT


