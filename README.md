## Table of Contents
### Introduction
### Installation
- Cloning the Repository
- Building the Application
### Usage
- Writing and Executing BasicML
- BasicML instructions
  - I/O Operations
  - Load/Store Operations
  - Arithmetic Operations
  - Control Operations
### GUI
- Buttons and Design


## Introduction
Welcome to the UVSim Program! This Project was created to have UVU students pursuing a Computer Science degree practice and learn Some Assembly that we created. Since this Introduction aims to describe the main functionality of this project, all information on the operations/instructions of this program is written below. This project allows users to create a project using our BasicML(Basic Machine Learning) to create programs on either one page or multiple pages. These pages act independently from each other and do not interfere with the processing of other pages. An additional feature in this project is allowing the user to save their code into a file on their device and then later loading that saved file back into the program. Lastly, the Program we have created also allows users to personalize their page by using the Edit Theme button. 

### Current plans for the future
Current improvement plans are letting the user store letters and other characters. This will allow users the opportunity to create a larger variety of programs. Currently, there are no ways of allowing users to store or utilize characters other than digits. In essence, the current functionality of our program is a calculator.

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

![image](https://github.com/user-attachments/assets/375cacd5-4664-40ee-b575-fa55485c84dc)

To start using the program, either begin editing the memory, or importing an existing .uvsim (or valid .txt) file, then hitting run to have the operating system iterate through each instruction.

![image](https://github.com/user-attachments/assets/35ea28c9-57e7-435a-bd56-61ef64094666)

The UI also contains a Theme manager, which can be used to determine the colors of various parts of the UI.

![image](https://github.com/user-attachments/assets/fe497396-ea8d-4da9-8468-7a034b87a421)

### BasicML instructions :
![image](https://github.com/user-attachments/assets/287781d7-5779-4bb4-9dbf-c7684e31e4d5)


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


## GUI
![image](https://github.com/user-attachments/assets/ea49874a-4970-4b13-8d09-cfd53b9e1d06)

#### Buttons
There are a few buttons on the main page/screen of the User Interface. These buttons consist of:
**Main page buttons**
- Open Project: lets the user select a pre-written file of valid code to the interface
- New Project: allows users to open a new project. However, the user is asked to specify the maximum number of lines their code will consist of.
- Edit Theme: Allows users to change the entire interface's color theme by selecting a color or inputting a color.
- Exit: Closes the Program

