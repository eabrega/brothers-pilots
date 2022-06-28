using nanoFramework.TestFramework;
using System;
using BrothersPilots.Applications;

namespace BrothersPilots.Tests
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = 100;
            var aa = new Game(a);

            Assert.Equal(aa.Name(), "50");
        }

        [TestMethod]
        public void TestMethod2()
        {
        }
    }
}
