using System;
using BenchmarkDotNet.Running;
using Seismic.WatermarkingPoc.Strategies;

namespace Seismic.WatermarkingPoc
{
    class Program
    {
        static void Main(string[] args)
        {
#if (Debug || DEBUG)
            // Run in DEBUG mode to have watermarked PDFs created in the results folder (dotnet run)
            //RunAsposeTests();
            RunFoxitTests();
            RunITextTests();
#else
            // Run in RELEASE mode to have BenchmarkDotNet gather performance metrics for a strategy (dotnet run -c Release)
            //BenchmarkRunner.Run<AsposeWatermarkingStrategy>();
            //BenchmarkRunner.Run<FoxitWatermarkingStrategy>();
            BenchmarkRunner.Run<ITextWatermarkingStrategy>();
#endif
        }

        private static void RunAsposeTests()
        {
            var aspose = new AsposeWatermarkingStrategy();
            aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);
            // aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Tiled);
            // aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Tiled);

            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Tiled);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Tiled);

            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Tiled);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Tiled);

            // // Aspose doesn't put a watermark on this file
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Center);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Footer);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Tiled);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Center);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Footer);
            // // aspose.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Tiled);

            // // Test Edge Cases
            // aspose.TestWatermarking(SampleFileEnum.PasswordProtected, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // aspose.TestWatermarking(SampleFileEnum.ReadOnly, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // aspose.TestWatermarking(SampleFileEnum.Watermarked, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
        }

        private static void RunFoxitTests()
        {
            var foxit = new FoxitWatermarkingStrategy();
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Tiled);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Tiled);

            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Tiled);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Tiled);

            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Tiled);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Tiled);

            foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled_With_Rotation);
            foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);
            foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled_With_Rotation);
            // foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextIncremental, PositionTypeEnum.Tiled);
            // foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageIncremental, PositionTypeEnum.Tiled);

            // // Test Edge Cases
            // foxit.TestWatermarking(SampleFileEnum.PasswordProtected, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // foxit.TestWatermarking(SampleFileEnum.ReadOnly, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // foxit.TestWatermarking(SampleFileEnum.Watermarked, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
        }

        private static void RunITextTests()
        {
            var iText = new ITextWatermarkingStrategy();
            //iText.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            //iText.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            //iText.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            //iText.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);
            // iText.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // iText.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // iText.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // iText.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            // iText.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            // iText.TestWatermarking(SampleFileEnum.Simple, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);

            // iText.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // iText.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // iText.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // iText.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            // iText.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            // iText.TestWatermarking(SampleFileEnum.Form, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);

            // iText.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // iText.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // iText.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // iText.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            // iText.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            // iText.TestWatermarking(SampleFileEnum.Scientific, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);

            iText.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            iText.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            iText.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            iText.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            iText.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            iText.TestWatermarking(SampleFileEnum.Large, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);

            // // Test Edge Cases
            // iText.TestWatermarking(SampleFileEnum.PasswordProtected, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // iText.TestWatermarking(SampleFileEnum.PasswordProtected, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // iText.TestWatermarking(SampleFileEnum.PasswordProtected, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // iText.TestWatermarking(SampleFileEnum.PasswordProtected, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Center);
            // iText.TestWatermarking(SampleFileEnum.PasswordProtected, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Footer);
            // iText.TestWatermarking(SampleFileEnum.PasswordProtected, WatermarkTypeEnum.ImageNewFile, PositionTypeEnum.Tiled);
            // iText.TestWatermarking(SampleFileEnum.ReadOnly, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // iText.TestWatermarking(SampleFileEnum.ReadOnly, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // iText.TestWatermarking(SampleFileEnum.ReadOnly, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
            // iText.TestWatermarking(SampleFileEnum.Watermarked, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Center);
            // iText.TestWatermarking(SampleFileEnum.Watermarked, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Footer);
            // iText.TestWatermarking(SampleFileEnum.Watermarked, WatermarkTypeEnum.TextNewFile, PositionTypeEnum.Tiled);
        }
    }
}
