using NUnit.Framework;
using Seismic.WatermarkingPoc.Strategies;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestFilePath()
        {
            var wms = new FoxitWatermarkingStrategy();
            Assert.Pass("Success");
        }
    }
}
