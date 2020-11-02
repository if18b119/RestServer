using NUnit.Framework;
using RestServerLib;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            MyServer server = new MyServer(6543);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}