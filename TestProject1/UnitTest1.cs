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
        public void TestConstructor()
        {
            var wms = new FoxitWatermarkingStrategy();
            Assert.Pass();
        }

        [Test]
        public void TestFilePath()
        {
            var wms = new ITextWatermarkingStrategy();
            Assert.IsNotNull(wms.GetFilePathForSample(Seismic.WatermarkingPoc.SampleFileEnum.Simple));
        }
    }
}
