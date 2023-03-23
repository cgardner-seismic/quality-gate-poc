using System.Collections.Generic;
using System.IO;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using BenchmarkDotNet.Attributes;

namespace Seismic.WatermarkingPoc.Strategies
{
    [MemoryDiagnoser]
    [BenchmarkCategory("Aspose")]
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 20)]
    public class AsposeWatermarkingStrategy : WatermarkingStrategy
    {
        [Params(SampleFileEnum.Simple, SampleFileEnum.Form, SampleFileEnum.Scientific)] // SampleFileEnum.Large doesn't actually apply a watermark, skipping
        public override SampleFileEnum SampleFile { get; set; }

        [Params(WatermarkTypeEnum.TextNewFile, WatermarkTypeEnum.ImageNewFile)]
        public override WatermarkTypeEnum WatermarkType { get; set; }

        [Params(PositionTypeEnum.Center, PositionTypeEnum.Footer, PositionTypeEnum.Tiled)]
        public override PositionTypeEnum PositionType { get; set; }

        protected override string Name { get => "Apose"; }

        private FileStream CopiedSampleFile { get; set; }

        public AsposeWatermarkingStrategy()
        {
            var license = new Aspose.Pdf.License();
            license.SetLicense("resources/Aspose.Pdf.NET.lic");
        }

        [Benchmark]
        public override void Benchmark()
        {
            switch (WatermarkType)
            {
                case WatermarkTypeEnum.TextNewFile:
                    ApplyTextWatermarksNewFile();
                    break;

                case WatermarkTypeEnum.ImageNewFile:
                    ApplyImageWatermarksNewFile();
                    break;

                case WatermarkTypeEnum.TextIncremental:
                    ApplyTextWatermarksIncremental();
                    break;

                case WatermarkTypeEnum.ImageIncremental:
                    ApplyImageWatermarksIncremental();
                    break;

            }
        }

        /// <summary>
        /// For incremental updates, we do not want to overwrite our sample file. Instead, we create a copy of the sample file and use
        /// that for our watermarking tests
        /// </summary>
        [IterationSetup]
        public void CreateCopyOfSampleFile()
        {
            if (WatermarkType == WatermarkTypeEnum.TextIncremental || WatermarkType == WatermarkTypeEnum.ImageIncremental)
            {
                var sourceFilePath = GetFilePathForSample(SampleFile);
                var targetFilePath = GetFilePathForResult(SampleFile, WatermarkType, PositionType);
                File.Copy(sourceFilePath, targetFilePath);
                CopiedSampleFile = new FileStream(targetFilePath, FileMode.Open, FileAccess.ReadWrite);
            }
        }

        public override void TestWatermarking(SampleFileEnum sampleFile, WatermarkTypeEnum watermarkType, PositionTypeEnum positionType)
        {
            WatermarkType = watermarkType;
            SampleFile = sampleFile;
            PositionType = positionType;

            switch (watermarkType)
            {
                case WatermarkTypeEnum.TextNewFile:
                    ApplyTextWatermarksNewFile();
                    break;

                case WatermarkTypeEnum.ImageNewFile:
                    ApplyImageWatermarksNewFile();
                    break;

                case WatermarkTypeEnum.TextIncremental:
                    CreateCopyOfSampleFile();
                    ApplyTextWatermarksIncremental();
                    break;

                case WatermarkTypeEnum.ImageIncremental:
                    CreateCopyOfSampleFile();
                    ApplyImageWatermarksIncremental();
                    break;
            }
        }

        private void ApplyTextWatermarksNewFile()
        {
            var document = new Document(GetFilePathForSample(SampleFile), SampleFile == SampleFileEnum.PasswordProtected ? PasswordProtectedSamplePassword : string.Empty);
            foreach (var page in document.Pages)
            {
                var artifacts = GetTextArtifacts();
                foreach (var artifact in artifacts)
                {
                    page.Artifacts.Add(artifact);
                }
            }

            document.Save(GetFilePathForResult(SampleFile, WatermarkType, PositionType));
        }

        private void ApplyImageWatermarksNewFile()
        {
            var document = new Document(GetFilePathForSample(SampleFile), SampleFile == SampleFileEnum.PasswordProtected ? PasswordProtectedSamplePassword : string.Empty);
            foreach (var page in document.Pages)
            {
                var artifacts = GetImageArtifacts();
                foreach (var artifact in artifacts)
                {
                    page.Artifacts.Add(artifact);
                }
            }

            document.Save(GetFilePathForResult(SampleFile, WatermarkType, PositionType));
        }

        private void ApplyTextWatermarksIncremental()
        {
            var document = new Document(CopiedSampleFile, SampleFile == SampleFileEnum.PasswordProtected ? PasswordProtectedSamplePassword : string.Empty);
            foreach (var page in document.Pages)
            {
                var artifacts = GetTextArtifacts();
                foreach (var artifact in artifacts)
                {
                    page.Artifacts.Add(artifact);
                }
            }

            document.Save();
        }

        private void ApplyImageWatermarksIncremental()
        {
            var document = new Document(CopiedSampleFile, SampleFile == SampleFileEnum.PasswordProtected ? PasswordProtectedSamplePassword : string.Empty);
            foreach (var page in document.Pages)
            {
                var artifacts = GetImageArtifacts();
                foreach (var artifact in artifacts)
                {
                    page.Artifacts.Add(artifact);
                }
            }

            document.Save();
        }

        private List<WatermarkArtifact> GetTextArtifacts()
        {
            var artifacts = new List<WatermarkArtifact>();
            switch (PositionType)
            {
                case PositionTypeEnum.Center:
                    var centerArtifact = new WatermarkArtifact();
                    centerArtifact.SetTextAndState(
                        "Seismic Software\nTim Cardwell",
                        new TextState()
                        {
                            FontSize = 60,
                            ForegroundColor = Color.FromArgb(241, 89, 42),
                            Font = FontRepository.FindFont("Courier"),
                            FontStyle = FontStyles.Bold
                        });
                    centerArtifact.ArtifactHorizontalAlignment = HorizontalAlignment.Center;
                    centerArtifact.ArtifactVerticalAlignment = VerticalAlignment.Center;
                    centerArtifact.Opacity = 0.4;
                    //centerArtifact.Rotation = 180f; This breaks stuff
                    artifacts.Add(centerArtifact);
                    break;
                case PositionTypeEnum.Footer:
                    var footerArtifact = new WatermarkArtifact();
                    footerArtifact.SetTextAndState(
                        "Seismic Software\nTim Cardwell",
                        new TextState()
                        {
                            FontSize = 32,
                            ForegroundColor = Color.FromArgb(241, 89, 42),
                            Font = FontRepository.FindFont("Courier"),
                            FontStyle = FontStyles.Italic
                        });
                    footerArtifact.ArtifactHorizontalAlignment = HorizontalAlignment.Right;
                    footerArtifact.ArtifactVerticalAlignment = VerticalAlignment.Bottom;
                    footerArtifact.Opacity = 0.4;
                    artifacts.Add(footerArtifact);
                    break;
                case PositionTypeEnum.Tiled:
                    var horizontalAlignments = new List<HorizontalAlignment> { HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Right };
                    var verticalAlignments = new List<VerticalAlignment> { VerticalAlignment.Top, VerticalAlignment.Center, VerticalAlignment.Bottom };

                    foreach (var horizontalAlignment in horizontalAlignments)
                    {
                        foreach (var verticalAlignment in verticalAlignments)
                        {
                            var artifact = new WatermarkArtifact();
                            artifact.SetTextAndState(
                                "Seismic Software\nTim Cardwell",
                                new TextState()
                                {
                                    FontSize = 20,
                                    ForegroundColor = Color.FromArgb(241, 89, 42),
                                    Font = FontRepository.FindFont("Courier")
                                });
                            artifact.ArtifactHorizontalAlignment = horizontalAlignment;
                            artifact.ArtifactVerticalAlignment = verticalAlignment;
                            artifact.Opacity = 0.4;
                            artifacts.Add(artifact);
                        }
                    }
                    break;
            }

            return artifacts;
        }

        private List<WatermarkArtifact> GetImageArtifacts()
        {
            var artifacts = new List<WatermarkArtifact>();
            switch (PositionType)
            {
                case PositionTypeEnum.Center:
                    var centerArtifact = new WatermarkArtifact();
                    centerArtifact.SetImage(GetImageWatermarkPath());
                    centerArtifact.ArtifactHorizontalAlignment = HorizontalAlignment.Center;
                    centerArtifact.ArtifactVerticalAlignment = VerticalAlignment.Center;
                    centerArtifact.Rotation = 45;
                    centerArtifact.Opacity = 0.5;
                    artifacts.Add(centerArtifact);
                    break;
                case PositionTypeEnum.Footer:
                    var footerArtifact = new WatermarkArtifact();
                    footerArtifact.SetImage(GetImageWatermarkPath());
                    footerArtifact.ArtifactHorizontalAlignment = HorizontalAlignment.Right;
                    footerArtifact.ArtifactVerticalAlignment = VerticalAlignment.Bottom;
                    footerArtifact.Opacity = 0.5;
                    artifacts.Add(footerArtifact);
                    break;
                case PositionTypeEnum.Tiled:
                    var horizontalAlignments = new List<HorizontalAlignment> { HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Right };
                    var verticalAlignments = new List<VerticalAlignment> { VerticalAlignment.Top, VerticalAlignment.Center, VerticalAlignment.Bottom };

                    foreach (var horizontalAlignment in horizontalAlignments)
                    {
                        foreach (var verticalAlignment in verticalAlignments)
                        {
                            var artifact = new WatermarkArtifact();
                            artifact.SetImage(GetImageWatermarkPath());
                            artifact.ArtifactHorizontalAlignment = horizontalAlignment;
                            artifact.ArtifactVerticalAlignment = verticalAlignment;
                            artifact.Opacity = 0.5;
                            artifacts.Add(artifact);
                        }
                    }
                    break;
            }

            return artifacts;
        }
    }
}
