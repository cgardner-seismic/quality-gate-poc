using System;
using System.IO;

namespace Seismic.WatermarkingPoc.Strategies
{
    public abstract class WatermarkingStrategy
    {
        private const string ImageWatermarksDirectoryPath = "./image_watermarks";
        private const string SampleDirectoryPath = "./samples";
        private const string ResultsDirectoryPath = "./results";
        private const string SeismicImageFileName = "seismic_logo.png";
        protected const string PasswordProtectedSamplePassword = "test";
        protected abstract string Name { get; }
        public abstract SampleFileEnum SampleFile { get; set; }
        public abstract WatermarkTypeEnum WatermarkType { get; set; }
        public abstract PositionTypeEnum PositionType { get; set; }

        /// <summary>
        /// Runs benchmarks using BenchmarkDotNet
        /// </summary>
        public abstract void Benchmark();

        /// <summary>
        /// General method for running a watermarking strategy for a given sample file and watermarking type. Only used for local debugging
        /// </summary>
        public abstract void TestWatermarking(SampleFileEnum sampleFile, WatermarkTypeEnum watermarkType, PositionTypeEnum positionType);

        /// <summary>
        /// Helper method for getting the file path of the watermarking result
        /// </summary>
        protected string GetFilePathForResult(SampleFileEnum sampleFile, WatermarkTypeEnum watermarkType, PositionTypeEnum positionType) =>
            $"{GetResultsDirectory()}/{watermarkType.ToString()}_{positionType.ToString()}_{GetSampleFileName(sampleFile)}_{DateTime.Now.ToFileTimeUtc()}.pdf";

        /// <summary>
        /// Helper method for getting the file path of the sample file
        /// </summary>
        protected string GetFilePathForSample(SampleFileEnum sampleFile)
        {
            string filePath = $"{SampleDirectoryPath}/{GetSampleFileName(sampleFile)}.pdf";
            if (!File.Exists(filePath))
            {
                throw new InvalidOperationException($"Sample file cannot be found: {filePath}");
            }

            return filePath;
        }

        /// <summary>
        /// Helper method for getting the file path of the image used for image watermarking
        /// </summary>
        protected string GetImageWatermarkPath()
        {
            var filePath = $"{ImageWatermarksDirectoryPath}/{SeismicImageFileName}";
            if (!File.Exists(filePath))
            {
                throw new InvalidOperationException($"Image watermark cannot be found: {filePath}");
            }

            return filePath;
        }

        /// <summary>
        /// Helper method for getting the results directory
        /// </summary>
        protected string GetResultsDirectory()
        {
            var resultsDirectoryPath = $"{ResultsDirectoryPath}/{Name}";
            if (!Directory.Exists(resultsDirectoryPath))
            {
                Directory.CreateDirectory(resultsDirectoryPath);
            }

            return resultsDirectoryPath;
        }

        protected string GetSampleFileName(SampleFileEnum sampleFile)
        {
            switch (sampleFile)
            {
                case SampleFileEnum.Simple:
                    return "simple";
                case SampleFileEnum.Form:
                    return "form";
                case SampleFileEnum.Large:
                    return "five_hundred_pages";
                case SampleFileEnum.PasswordProtected:
                    return "password_protected";
                case SampleFileEnum.ReadOnly:
                    return "read_only";
                case SampleFileEnum.Scientific:
                    return "scientific";
                case SampleFileEnum.Watermarked:
                    return "watermarked";
                default:
                    throw new InvalidOperationException($"Enum {sampleFile.ToString()} is not supported");
            }
        }
    }
}
