using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebApp.Tests
{
    [TestFixture]
    public class Test1
    {
        [TestCase]
        public void CanRunTest()
        {
            Assert.AreEqual(2 + 2, 4);
        }
    }
}
