using UVSim;

namespace AutomatedTests
{
    [TestClass]
    public class Tests
    {
        Processor processor;
        Memory memory;
        /// <summary>
        /// Recreates the initial state assumed for a given test
        /// </summary>
        public void TestSetup()
        {
            memory = new() { ProgramType = ProgramType.FourDigit };
            for (int i = 0; i < 10; i++)
            {
                memory.Locations.Add(new() { Data = 0, LineNumber = i, Instruction = BasicML.NONE });
            }
            processor = new(memory);
        }

        /// <summary>
        /// Test writing a word with a positive value into memory
        /// </summary>
        [TestMethod]
        public void TestWritePos()
        {
            TestSetup();
            memory.WriteWord(0, 10);
            Assert.AreEqual(10, memory.Read(0));
        }
        
        /// <summary>
        /// Test writing a word with a negative value into memory
        /// </summary>
        [TestMethod]
        public void TestWriteNeg()
        {
            TestSetup();
            memory.WriteWord(0, -10);
            Assert.AreEqual(-10, memory.Read(0));
        }

        /// <summary>
        /// Test reading the maximum word value from memory
        /// </summary>
        [TestMethod]
        public void TestReadPos()
        {
            TestSetup();
            memory.WriteWord(1, 99);
            Assert.AreEqual(99, memory.Read(1));
        }

        /// <summary>
        /// Test reading the minimum word value from memory
        /// </summary>
        [TestMethod]
        public void TestReadNeg()
        {
            TestSetup();
            memory.WriteWord(1, -99);
            Assert.AreEqual(-99, memory.Read(1));
        }

        /// <summary>
        /// Test writing a positive valid value into the accumulator
        /// </summary>
        [TestMethod]
        public void TestLoadPos()
        {
            TestSetup();
            memory.WriteWord(1, 33);
            processor.Load(1);
            Assert.AreEqual(33, processor.GetAccumulator());
        }

        /// <summary>
        /// Test writing a negative valid value into the accumulator
        /// </summary>
        [TestMethod]
        public void TestLoadNeg()
        {
            TestSetup();
            memory.WriteWord(1, -33);
            processor.Load(1);
            Assert.AreEqual(-33, processor.GetAccumulator());
        }

        /// <summary>
        /// Test storing a positive overflowing value into the accumulator, then storing it in memory
        /// </summary>
        [TestMethod]
        public void TestStorePos()
        {
            TestSetup();
            processor.SetAccumulator(1000);
            processor.Store(1);
            Assert.AreEqual(0, memory.Read(1));
        }

        /// <summary>
        /// Test storing a negative overflowing value into the accumulator, then storing it in memory
        /// </summary>
        [TestMethod]
        public void TestStoreNeg()
        {
            TestSetup();
            processor.SetAccumulator(-1000);
            processor.Store(1);
            Assert.AreEqual(0, memory.Read(1));
        }

        /// <summary>
        /// Test adding positive numbers
        /// </summary>
        [TestMethod]
        public void TestAddPos()
        {
            TestSetup();
            memory.WriteWord(0, 10);
            memory.WriteWord(1, 10);
            processor.Load(0);
            processor.Add(1);
            processor.Store(3);
            Assert.AreEqual(20, memory.Read(3));
        }

        /// <summary>
        /// Test adding a negative to a positive
        /// </summary>
        [TestMethod]
        public void TestAddNegtoPos()
        {
            TestSetup();
            memory.WriteWord(0, 10);
            memory.WriteWord(1, -10);
            processor.Load(0);
            processor.Add(1);
            processor.Store(3);
            Assert.AreEqual(0, memory.Read(3));
        }

        /// <summary>
        /// Test subtracting two positive numbers
        /// </summary>
        [TestMethod]
        public void TestSubPos()
        {
            TestSetup();
            memory.WriteWord(0, 10);
            memory.WriteWord(1, 5);
            processor.Load(0);
            processor.Subtract(1);
            processor.Store(3);
            Assert.AreEqual(5, memory.Read(3));
        }

        /// <summary>
        /// Test subtracting a negative number from a positive
        /// </summary>
        [TestMethod]
        public void TestSubNegFromPos()
        {
            TestSetup();
            memory.WriteWord(0, 10);
            memory.WriteWord(1, -5);
            processor.Load(0);
            processor.Subtract(1);
            processor.Store(3);
            Assert.AreEqual(15, memory.Read(3));
        }

        /// <summary>
        /// Test dividing two positive numbers
        /// </summary>
        [TestMethod]
        public void TestDividePos()
        {
            TestSetup();
            memory.WriteWord(0, 10);
            memory.WriteWord(1, 5);
            processor.Load(0);
            processor.Divide(1);
            processor.Store(3);
            Assert.AreEqual(2, memory.Read(3));
        }

        /// <summary>
        /// Test dividing a negative number from a positive
        /// </summary>
        [TestMethod]
        public void TestDivideNegFromPos()
        {
            TestSetup();
            memory.WriteWord(0, 10);
            memory.WriteWord(1, -5);
            processor.Load(0);
            processor.Divide(1);
            processor.Store(3);
            Assert.AreEqual(-2, memory.Read(3));
        }

        /// <summary>
        /// Test multiplying two positive numbers
        /// </summary>
        [TestMethod]
        public void TestMultPos()
        {
            TestSetup();
            memory.WriteWord(0, 10);
            memory.WriteWord(1, 5);
            processor.Load(0);
            processor.Multiply(1);
            processor.Store(3);
            Assert.AreEqual(50, memory.Read(3));
        }

        /// <summary>
        /// Test multiplying two negative numbers
        /// </summary>
        [TestMethod]
        public void TestMultNeg()
        {
            TestSetup();
            memory.WriteWord(0, -5);
            memory.WriteWord(1, -4);
            processor.Load(0);
            processor.Multiply(1);
            processor.Store(3);
            Assert.AreEqual(20, memory.Read(3));
        }

        /// <summary>
        /// Test a simple positive branch condition
        /// </summary>
        [TestMethod]
        public void TestBranch()
        {
            TestSetup();

            int branchLocation = 23;
            int initialLocation = 3;
            
            processor.Load(initialLocation);
            processor.Branch((int)BasicML.BRANCH, branchLocation);
            Assert.AreEqual(branchLocation - 1, processor.GetCurrentLocation());
        }
        
        /// <summary>
        /// Test a simple negative branch condition
        /// </summary>
        [TestMethod]
        public void TestBranchNeg()
        {
            TestSetup();

            int branchLocation = 33;
            int initialLocation = 0;
            
            processor.Load(initialLocation);
            processor.SetAccumulator(-20);
            processor.Branch((int)BasicML.BRANCHNEG, branchLocation);
            Assert.AreEqual(branchLocation - 1, processor.GetCurrentLocation());
        }
        
        /// <summary>
        /// Test if a positive number triggers a negative branch
        /// </summary>
        [TestMethod]
        public void TestBranchNeg2() // Testing to see if a positive number would work on BRANCHNEG.
        {
            TestSetup();

            int branchLocation = 34;
            int initialLocation = 0;
            
            processor.Load(initialLocation);
            processor.SetAccumulator(17);
            processor.Branch((int)BasicML.BRANCHNEG, branchLocation);
            Assert.AreNotEqual(branchLocation, processor.GetCurrentLocation());
        }
        
        /// <summary>
        /// Test a simple equalszero branch condition
        /// </summary>
        [TestMethod]
        public void TestBranchZero()
        {
            TestSetup();

            int branchLocation = 21;
            int initialLocation = 0;
            
            processor.Load(initialLocation);
            processor.SetAccumulator(0);
            processor.Branch((int)BasicML.BRANCHZERO, branchLocation);
            Assert.AreEqual(branchLocation - 1, processor.GetCurrentLocation());
        }
        

        /// <summary>
        /// Tests to see if the correct location is saved when halting
        /// </summary>
        [TestMethod]
        public void TestHalt()
        {
            TestSetup();
            memory.WriteWord(0, -5);
            memory.WriteWord(1, -4);
            memory.WriteWord(2, 2);
            processor.Load(0);
            processor.Multiply(1);
            processor.Store(4);
            Assert.AreEqual(20, memory.Read(4));
            processor.Load(4);
            processor.Divide(2);
            processor.Store(5);
            Assert.AreEqual(10, memory.Read(5));
            processor.Halt();
            Assert.AreEqual(memory.Capacity, processor.GetCurrentLocation());
        }

        /// <summary>
        /// Tests if halting mid-program returns the correct output
        /// </summary>
        [TestMethod]
        public void TestHaltMidProgram()
        {
            TestSetup();
            memory.WriteWord(0, 150);
            memory.WriteWord(1, 150);
            memory.WriteWord(2, 50);
            memory.WriteWord(3, 2000);
            memory.WriteWord(4, 3001);
            memory.WriteWord(5, 4300);
            memory.WriteWord(6, 2102);
            processor.Execute(3);
            Assert.AreEqual(50, memory.Read(2));
        }

        /// <summary>
        /// Tests a known program to ensure it executes correctly
        /// </summary>
        [TestMethod]
        public void TestExecuteSuccess()
        {
            TestSetup();
            //program adds 0 and 1 and stores in 2
            memory.WriteWord(0, 150);
            memory.WriteWord(1, 150);
            memory.WriteWord(2, 50);
            memory.WriteWord(3, 2000);
            memory.WriteWord(4, 3001);
            memory.WriteWord(5, 2102);
            memory.WriteWord(6, 4300);
            Assert.AreEqual(true, processor.Execute(3));
        }

        /// <summary>
        /// Tests an intentionally bad word to see if it throws an exception
        /// </summary>
        [TestMethod]
        public void TestExecuteBadCmd()
        {
            TestSetup();
            //should fail
            bool one = memory.WriteWord(0, 9999999);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => processor.Execute(0));
        }
    }
}