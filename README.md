## Installation
### Cloning the Repository
Begin by cloning the repository. This can be done by running this command in the command line: git clone https://github.com/captaincobbs/CS_2450_Application.git

### Building the Application
Build the application by navigating to the cloned directory and selecting the shortcut to the executable file (Application\bin\Debug\net8.0\CS_2450_Application.exe) in the main directory and run it. 

## Usage
### Writing and Executing BasicML
Once the application has been built, A GUI will pop up, allowing the user to interact with the UVSim Operating System.

The GUI contains a register, which store values, boxes for output and input, buttons to upload/save what is currently in memory, buttons to run the what is in memory, and, most importantly, a panel that displays all the lines of memory. In this panel, there are 3 columns:
- **Location**, which denotes the order the lines of memory will be read (they are read in increasing sequential order when "Run" is pressed)
- **Data**, which represents an input for a line of memory's instruction
- **Instruction**, which determines how a line of memory will be interpreted. A list of all instructions can be found below.

To start using the program, either begin editing the memory, or importing an existing .uvsim (or valid .txt) file, then hitting run to have the operating system iterate through each instruction.

The UI also contains a Theme manager, which can be used to determine the colors of various parts of the UI.

### BasicML instructions :
#### I/O operation:

 - **READ** = 10 - Read a word from the keyboard into a specific location in
   memory.
 - **WRITE** = 11 - Write a word from a specific location in memory to
   screen.
   
#### Load/store operations:

 - **LOAD** = 20 - Load a word from a specific location in memory into the
   accumulator.
 - **STORE** = 21 - Store a word from the accumulator into a
   specific location in memory.

#### Arithmetic operation:

 - **ADD** = 30 - Add a word from a specific location in memory to the word in
   the accumulator (leave the result in the accumulator)
 - **SUBTRACT** = 31 - Subtract a word from a specific location in memory from the word in
   the accumulator (leave the result in the accumulator) 
 - **DIVIDE** = 32 - Divide the word in the accumulator by a word from a specific location
   in memory (leave the result in the accumulator).
 - **MULTIPLY** = 33 - multiply a word from a specific location in memory to the word in the
   accumulator (leave the result in the accumulator).

#### Control operation:

 - **BRANCH** = 40 - Branch to a specific location in memory BRANCHNEG = 41
   Branch to a specific location in memory if the accumulator is
   negative.
 - **BRANCHZERO** = 42 - Branch to a specific location in memory if
   the accumulator is zero.
 - **HALT** = 43 - Pause the program

The last two digits of a BasicML instruction are the operand – the address of the memory location containing the word to which the operation applies.


**Example of a BasicML program:**
10 00 // READ input into memory location 00
20 00 // LOAD value from memory location 00 into accumulator 
30 01 // ADD value from memory location 01 to value in the accumulator 
21 02 // STORE accumulator value into memory location 02
11 02 // WRITE value stored in memory location 02 to screen 
43 // HALT

#### Import/Save
When the application is open and the user has a prewritten file of valid BasicML code, the user has the option of selecting the Import button underneath the codespace and importing the file to the codespace. Another function of the application is the "Save" button. When the user needs the save their code in a file the Save button is all that needs to be clicked.

#### Added Features
- **Expanded Memory:** The application now supports data files containing up to 250 lines, with internal memory registers ranging from 000 to 249.
- **Six-Digit Words:** The new file format supports six-digit words, with function codes updated to three digits (e.g. 010 for READ).
- **File Format Support:** The application supports both old (four-digit) and new (six-digit) file formats. Files are differentiated at load or run time, but mixing formats within a single file is not allowed.
- **Multiple File Handling:** The application allows multiple files to be opened simultaneously within a single instance. Users can switch between, edit, and execute each file independently.

