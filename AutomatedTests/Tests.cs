using Newtonsoft.Json.Serialization;
using UVSim;

namespace AutomatedTests
{
    [TestClass]
    public class Tests
    {
        Processor processor;
        Memory memory;
        public void TestSetup()
        {
            memory = new();
            processor = new(memory);
        }

        [TestMethod]
        public void TestWritePos()
        {
            TestSetup();
            memory.WriteWord(0, 10);
            Assert.AreEqual(10, memory.Read(0));
        }
        
        [TestMethod]
        public void TestWriteNeg()
        {
            TestSetup();
            memory.WriteWord(0, -10);
            Assert.AreEqual(-10, memory.Read(0));
        }

        [TestMethod]
        public void TestReadPos()
        {
            TestSetup();
            memory.WriteWord(1, 9999);
            Assert.AreEqual(9999, memory.Read(1));
        }

        [TestMethod]
        public void TestReadNeg()
        {
            TestSetup();
            memory.WriteWord(1, -9999);
            Assert.AreEqual(-9999, memory.Read(1));
        }
        [TestMethod]
        public void TestLoadPos()
        {
            TestSetup();
            memory.WriteWord(1, 33);
            processor.Load(1);
            Assert.AreEqual(33, processor.GetAccumulator());
        }

        [TestMethod]
        public void TestLoadNeg()
        {
            TestSetup();
            memory.WriteWord(1, -33);
            processor.Load(1);
            Assert.AreEqual(-33, processor.GetAccumulator());
        }
        [TestMethod]
        public void TestStorePos()
        {
            TestSetup();
            processor.SetAccumulator(1000);
            processor.Store(1);
            Assert.AreEqual(1000, memory.Read(1));
        }

        [TestMethod]
        public void TestStoreNeg()
        {
            TestSetup();
            processor.SetAccumulator(-1000);
            processor.Store(1);
            Assert.AreEqual(-1000, memory.Read(1));
        }
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

        [TestMethod]
        public void TestBranch()
        {
            TestSetup();

            int branchLocation = 25;
            int initialLocation = 3;
            
            processor.Load(initialLocation);
            processor.Branch((int)BasicML.BRANCH, branchLocation);
            Assert.AreEqual(branchLocation, processor.GetCurrentLocation());
        }
        
        [TestMethod]
        public void TestBranchNeg()
        {
            TestSetup();

            int branchLocation = 34;
            int initialLocation = 0;
            
            processor.Load(initialLocation);
            processor.SetAccumulator(-20);
            processor.Branch((int)BasicML.BRANCHNEG, branchLocation);
            Assert.AreEqual(branchLocation, processor.GetCurrentLocation());
        }
        
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
        
        [TestMethod]
        public void TestBranchZero()
        {
            TestSetup();

            int branchLocation = 21;
            int initialLocation = 0;
            
            processor.Load(initialLocation);
            processor.SetAccumulator(0);
            processor.Branch((int)BasicML.BRANCHZERO, branchLocation);
            Assert.AreEqual(branchLocation, processor.GetCurrentLocation());
        }
        

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
            Assert.AreEqual(memory.capacity, processor.GetCurrentLocation());
        }

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
        [TestMethod]
        public void TestWriteFile()
        {
            TestSetup();
            using (StreamWriter mockFileFS = File.AppendText("mockFile.txt")) 
            {
                mockFileFS.Write("+0150\n+0150\n+0050\n+2000\n+3001\n+2102\n+4300\n");
            }
            memory.WriteFile(0, "mockFile.txt");
            Assert.AreEqual(4300, memory.Read(6));
            File.Delete("mockFile.txt");
        }
        [TestMethod]
        public void TestWriteFileNegValues()
        {
            TestSetup();
            using (StreamWriter mockFileFS = File.AppendText("mockFile.txt"))
            {
                mockFileFS.Write("-0150\n-0150\n-0050\n+2000\n+3001\n+2102\n+4300\n");
            }
            memory.WriteFile(0, "mockFile.txt");
            Assert.AreEqual(-50, memory.Read(2));
            File.Delete("mockFile.txt");
        }

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
        [TestMethod]
        public void TestExecuteBadCmd()
        {
            TestSetup();
            //program adds 0 and 1 and stores in 2, with bad instruction  at location 5
            //should fail
            memory.WriteWord(0, 150);
            memory.WriteWord(1, 150);
            memory.WriteWord(2, 50);
            memory.WriteWord(3, 2000);
            memory.WriteWord(4, 3001);
            memory.WriteWord(5, 7200);
            memory.WriteWord(6, 2102);
            memory.WriteWord(7, 4300);
            Assert.AreEqual(false, processor.Execute(3));
        }
    }
}