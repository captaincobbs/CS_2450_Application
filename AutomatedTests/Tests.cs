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
            memory.WriteWord(0, -4);
            memory.WriteWord(1, -3);
            processor.Load(0);
            processor.Multiply(1);
            processor.Store(3);
            Assert.AreEqual(12, memory.Read(3));
        }

        [TestMethod]
        public void Test10()
        {
        }

        [TestMethod]
        public void Test11()
        {
        }

        [TestMethod]
        public void Test12()
        {
        }

        [TestMethod]
        public void Test13()
        {
        }

        [TestMethod]
        public void Test14()
        {
        }

        [TestMethod]
        public void Test15()
        {
        }
    }
}