using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System;
using iText.Kernel.Colors;

namespace Seismic.WatermarkingPoc.Strategies
{
    [MemoryDiagnoser]
    [BenchmarkCategory("IText")]
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 20, id: "IText")]
    public class ITextWatermarkingStrategy : WatermarkingStrategy
    {
        [Params(SampleFileEnum.Simple, SampleFileEnum.Large, SampleFileEnum.Form, SampleFileEnum.Scientific)]
        public override SampleFileEnum SampleFile { get; set; }

        [Params(WatermarkTypeEnum.TextNewFile, WatermarkTypeEnum.ImageNewFile)]
        public override WatermarkTypeEnum WatermarkType { get; set; }

        [Params(PositionTypeEnum.Center, PositionTypeEnum.Tiled, PositionTypeEnum.Footer)]
        public override PositionTypeEnum PositionType { get; set; }

        protected override string Name { get => "IText"; }

        public ITextWatermarkingStrategy()
        {
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
            }
        }

        private void ApplyTextWatermarksNewFile()
        {
            switch (PositionType)
            {
                case PositionTypeEnum.Center:
                    ApplyTextWatermarkCenter(GetFilePathForSample(SampleFile), GetFilePathForResult(SampleFile, WatermarkTypeEnum.TextNewFile, PositionType), PositionType);
                    break;

                case PositionTypeEnum.Tiled:
                    ApplyTextWatermarkTiled(GetFilePathForSample(SampleFile), GetFilePathForResult(SampleFile, WatermarkTypeEnum.TextNewFile, PositionType), PositionType);
                    break;

                case PositionTypeEnum.Footer:
                    ApplyTextWatermarkFooter(GetFilePathForSample(SampleFile), GetFilePathForResult(SampleFile, WatermarkTypeEnum.TextNewFile, PositionType));
                    break;
            }
        }

        private void ApplyImageWatermarksNewFile()
        {
            switch (PositionType)
            {
                case PositionTypeEnum.Center:
                    ApplyImageWatermarkCenter(GetFilePathForSample(SampleFile), GetFilePathForResult(SampleFile, WatermarkTypeEnum.ImageNewFile, PositionType));
                    break;
                case PositionTypeEnum.Tiled:
                    ApplyImageWatermarkTiled(GetFilePathForSample(SampleFile), GetFilePathForResult(SampleFile, WatermarkTypeEnum.ImageNewFile, PositionType));
                    break;
                case PositionTypeEnum.Footer:
                    ApplyImageWatermarkFooter(GetFilePathForSample(SampleFile), GetFilePathForResult(SampleFile, WatermarkTypeEnum.ImageNewFile, PositionType));
                    break;
            }
        }

        private void ApplyTextWatermarkCenter(string srcFilePath, string targetFilePath, PositionTypeEnum pos)
        {
            PdfDocument pdfDoc = null;

            if (SampleFile == SampleFileEnum.PasswordProtected)
            {
                var passwordBytes = new System.Text.ASCIIEncoding().GetBytes(PasswordProtectedSamplePassword);
                pdfDoc = new PdfDocument(
                    new PdfReader(srcFilePath, new ReaderProperties().SetPassword(passwordBytes)),
                    new PdfWriter(targetFilePath),
                    new StampingProperties().PreserveEncryption());
            }
            else
            {
                pdfDoc = new PdfDocument(new PdfReader(srcFilePath), new PdfWriter(targetFilePath));
            }
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {

                var pdfPage = pdfDoc.GetPage(i);
                var pageSize = pdfPage.GetPageSizeWithRotation();
                var company = new Text("Seismic Software\n");
                var statement = new Text("Tim Cardwell");
                var paragraph = new Paragraph()
                    .Add(company)
                    .Add(statement)
                    .SetFont(font)
                    .SetFontSize(60f)
                    .SetFontColor(WebColors.GetRGBColor("#F1592A"))
                    .SetOpacity(.4f)
                    .SetBold()
                    .SetRotationAngle(0.786f); // 45 degrees
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);

                var canvas = new Canvas(new PdfCanvas(pdfPage), pageSize);
                canvas.ShowTextAligned(paragraph, pageSize.GetWidth() / 2, pageSize.GetHeight() / 2, TextAlignment.CENTER, VerticalAlignment.MIDDLE);
                canvas.Close();
            }
            pdfDoc.Close();
        }

        private void ApplyTextWatermarkFooter(string srcFilePath, string targetFilePath)
        {
            PdfDocument pdfDoc = null;
            if (SampleFile == SampleFileEnum.PasswordProtected)
            {
                var passwordBytes = new System.Text.ASCIIEncoding().GetBytes(PasswordProtectedSamplePassword);
                pdfDoc = new PdfDocument(
                    new PdfReader(srcFilePath, new ReaderProperties().SetPassword(passwordBytes)),
                    new PdfWriter(targetFilePath),
                    new StampingProperties().PreserveEncryption());
            }
            else
            {
                pdfDoc = new PdfDocument(new PdfReader(srcFilePath), new PdfWriter(targetFilePath));
            }
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.COURIER);

            // Implement transformation matrix usage in order to scale image
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var pdfPage = pdfDoc.GetPage(i);
                var pageSize = pdfPage.GetPageSizeWithRotation();
                var company = new Text("Seismic Software\n");
                var statement = new Text("Tim Cardwell");
                Paragraph paragraph = new Paragraph()
                    .Add(company)
                    .Add(statement)
                    .SetFont(font)
                    .SetFontSize(32f)
                    .SetFontColor(WebColors.GetRGBColor("#F1592A"))
                    .SetOpacity(.4f)
                    .SetItalic();
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);
                var canvas = new Canvas(pdfCanvas, pageSize);
                canvas.ShowTextAligned(paragraph, pageSize.GetWidth(), 0, TextAlignment.RIGHT, VerticalAlignment.BOTTOM);
                canvas.Close();
            }
            pdfDoc.Close();
        }

        private void ApplyTextWatermarkTiled(string srcFilePath, string targetFilePath, PositionTypeEnum pos)
        {
            PdfDocument pdfDoc = null;

            if (SampleFile == SampleFileEnum.PasswordProtected)
            {
                var passwordBytes = new System.Text.ASCIIEncoding().GetBytes(PasswordProtectedSamplePassword);
                pdfDoc = new PdfDocument(
                    new PdfReader(srcFilePath, new ReaderProperties().SetPassword(passwordBytes)),
                    new PdfWriter(targetFilePath),
                    new StampingProperties().PreserveEncryption());
            }
            else
            {
                pdfDoc = new PdfDocument(new PdfReader(srcFilePath), new PdfWriter(targetFilePath));
            }
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.COURIER);

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {

                var pdfPage = pdfDoc.GetPage(i);
                var pageSize = pdfPage.GetPageSizeWithRotation();
                var company = new Text("Seismic Software\n");
                var statement = new Text("Tim Cardwell");
                var paragraph = new Paragraph()
                    .Add(company)
                    .Add(statement)
                    .SetFont(font)
                    .SetFontColor(ColorConstants.RED)
                    .SetOpacity(.4f)
                    .SetFontSize(20f);
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);

                var pdfCanvas = new PdfCanvas(pdfPage);
                var canvas = new Canvas(pdfCanvas, pageSize);
                canvas.ShowTextAligned(paragraph, 0, 0, TextAlignment.LEFT, VerticalAlignment.BOTTOM);
                canvas.ShowTextAligned(paragraph, 0, pageSize.GetHeight() / 2, TextAlignment.LEFT, VerticalAlignment.MIDDLE);
                canvas.ShowTextAligned(paragraph, 0, pageSize.GetHeight(), TextAlignment.LEFT, VerticalAlignment.TOP);
                canvas.ShowTextAligned(paragraph, pageSize.GetWidth() / 2, 0, TextAlignment.CENTER, VerticalAlignment.BOTTOM);
                canvas.ShowTextAligned(paragraph, pageSize.GetWidth() / 2, pageSize.GetHeight() / 2, TextAlignment.CENTER, VerticalAlignment.MIDDLE);
                canvas.ShowTextAligned(paragraph, pageSize.GetWidth() / 2, pageSize.GetHeight(), TextAlignment.CENTER, VerticalAlignment.TOP);
                canvas.ShowTextAligned(paragraph, pageSize.GetWidth(), 0, TextAlignment.RIGHT, VerticalAlignment.BOTTOM);
                canvas.ShowTextAligned(paragraph, pageSize.GetWidth(), pageSize.GetHeight() / 2, TextAlignment.RIGHT, VerticalAlignment.MIDDLE);
                canvas.ShowTextAligned(paragraph, pageSize.GetWidth(), pageSize.GetHeight(), TextAlignment.RIGHT, VerticalAlignment.TOP);
                canvas.Close();
            }
            pdfDoc.Close();
        }

        private void ApplyImageWatermarkCenter(string srcFilePath, string targetFilePath)
        {
            PdfDocument pdfDoc = null;
            if (SampleFile == SampleFileEnum.PasswordProtected)
            {
                var passwordBytes = new System.Text.ASCIIEncoding().GetBytes(PasswordProtectedSamplePassword);
                pdfDoc = new PdfDocument(
                    new PdfReader(srcFilePath, new ReaderProperties().SetPassword(passwordBytes)),
                    new PdfWriter(targetFilePath),
                    new StampingProperties().PreserveEncryption());
            }
            else
            {
                pdfDoc = new PdfDocument(new PdfReader(srcFilePath), new PdfWriter(targetFilePath));
            }
            ImageData img = ImageDataFactory.Create(GetImageWatermarkPath());

            PdfExtGState gs = new PdfExtGState().SetFillOpacity(.4f);

            // Implement transformation matrix usage in order to scale image
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var pdfPage = pdfDoc.GetPage(i);
                var pageSize = pdfPage.GetPageSizeWithRotation();
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                var pdfCanvas = new PdfCanvas(pdfPage);
                pdfCanvas.SetExtGState(gs);
                var rectangle = pdfDoc.GetPage(i).GetMediaBox();
                var imgWidth = img.GetWidth();
                var imgHeight = img.GetHeight();
                var transformX = (rectangle.GetWidth() / 2) - (imgWidth / 2);
                var transformY = (rectangle.GetHeight() / 2) - (imgHeight / 2);
                var transformAt = AffineTransform.GetTranslateInstance(transformX, transformY);
                var scaleAt = AffineTransform.GetScaleInstance(imgWidth, imgHeight);
                var rotateAt = AffineTransform.GetRotateInstance((float)Math.Atan(pageSize.GetHeight() / pageSize.GetWidth()));
                AffineTransform at = transformAt;
                at.Concatenate(rotateAt);
                at.Concatenate(scaleAt);
                float[] matrix = new float[6];
                at.GetMatrix(matrix);
                pdfCanvas.AddImageWithTransformationMatrix(img, matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);
            }
            pdfDoc.Close();
        }

        private void ApplyImageWatermarkTiled(string srcFilePath, string targetFilePath)
        {
            PdfDocument pdfDoc = null;
            if (SampleFile == SampleFileEnum.PasswordProtected)
            {
                var passwordBytes = new System.Text.ASCIIEncoding().GetBytes(PasswordProtectedSamplePassword);
                pdfDoc = new PdfDocument(
                    new PdfReader(srcFilePath, new ReaderProperties().SetPassword(passwordBytes)),
                    new PdfWriter(targetFilePath),
                    new StampingProperties().PreserveEncryption());
            }
            else
            {
                pdfDoc = new PdfDocument(new PdfReader(srcFilePath), new PdfWriter(targetFilePath));
            }
            ImageData img = ImageDataFactory.Create(GetImageWatermarkPath());

            PdfExtGState gs = new PdfExtGState().SetFillOpacity(.4f);

            // Implement transformation matrix usage in order to scale image
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var pdfPage = pdfDoc.GetPage(i);
                var pageSize = pdfPage.GetPageSizeWithRotation();
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                var pdfCanvas = new PdfCanvas(pdfPage);
                pdfCanvas.SetExtGState(gs);
                var rectangle = pdfDoc.GetPage(i).GetMediaBox();
                var imgWidth = img.GetWidth();
                var imgHeight = img.GetHeight();
                var scaleAt = AffineTransform.GetScaleInstance(imgWidth, imgHeight);

                var transforms = new List<AffineTransform>();

                // Top Left
                var topLeftTransform = AffineTransform.GetTranslateInstance(0, rectangle.GetHeight() - imgHeight);
                topLeftTransform.Concatenate(scaleAt);
                transforms.Add(topLeftTransform);

                // Top
                var topTransform = AffineTransform.GetTranslateInstance((rectangle.GetWidth() / 2) - (imgWidth / 2), rectangle.GetHeight() - imgHeight);
                topTransform.Concatenate(scaleAt);
                transforms.Add(topTransform);

                // Top Right
                var topRightTransform = AffineTransform.GetTranslateInstance(rectangle.GetWidth() - imgWidth, rectangle.GetHeight() - imgHeight);
                topRightTransform.Concatenate(scaleAt);
                transforms.Add(topRightTransform);

                // Middle Left
                var middleLeftTransform = AffineTransform.GetTranslateInstance(0, (rectangle.GetHeight() / 2) - (imgHeight / 2));
                middleLeftTransform.Concatenate(scaleAt);
                transforms.Add(middleLeftTransform);

                // Middle
                var middleTransform = AffineTransform.GetTranslateInstance((rectangle.GetWidth() / 2) - (imgWidth / 2), (rectangle.GetHeight() / 2) - (imgHeight / 2));
                middleTransform.Concatenate(scaleAt);
                transforms.Add(middleTransform);

                // Middle Right
                var middleRightTransform = AffineTransform.GetTranslateInstance(rectangle.GetWidth() - imgWidth, (rectangle.GetHeight() / 2) - (imgHeight / 2));
                middleRightTransform.Concatenate(scaleAt);
                transforms.Add(middleRightTransform);

                // Bottom Left
                var bottomLeftTransform = AffineTransform.GetTranslateInstance(0, 0);
                bottomLeftTransform.Concatenate(scaleAt);
                transforms.Add(bottomLeftTransform);

                // Bottom
                var bottomTransform = AffineTransform.GetTranslateInstance((rectangle.GetWidth() / 2) - (imgWidth / 2), 0);
                bottomTransform.Concatenate(scaleAt);
                transforms.Add(bottomTransform);

                // Bottom Right
                var bottomRightTransform = AffineTransform.GetTranslateInstance(rectangle.GetWidth() - imgWidth, 0);
                bottomRightTransform.Concatenate(scaleAt);
                transforms.Add(bottomRightTransform);

                foreach (var transform in transforms)
                {
                    float[] matrix = new float[6];
                    transform.GetMatrix(matrix);
                    pdfCanvas.AddImageWithTransformationMatrix(img, matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);
                }
            }
            pdfDoc.Close();
        }

        private void ApplyImageWatermarkFooter(string srcFilePath, string targetFilePath)
        {
            PdfDocument pdfDoc = null;
            if (SampleFile == SampleFileEnum.PasswordProtected)
            {
                var passwordBytes = new System.Text.ASCIIEncoding().GetBytes(PasswordProtectedSamplePassword);
                pdfDoc = new PdfDocument(
                    new PdfReader(srcFilePath, new ReaderProperties().SetPassword(passwordBytes)),
                    new PdfWriter(targetFilePath),
                    new StampingProperties().PreserveEncryption());
            }
            else
            {
                pdfDoc = new PdfDocument(new PdfReader(srcFilePath), new PdfWriter(targetFilePath));
            }
            ImageData img = ImageDataFactory.Create(GetImageWatermarkPath());

            PdfExtGState gs = new PdfExtGState().SetFillOpacity(.4f);

            // Implement transformation matrix usage in order to scale image
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var pdfPage = pdfDoc.GetPage(i);
                var pageSize = pdfPage.GetPageSizeWithRotation();
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                var pdfCanvas = new PdfCanvas(pdfPage);
                pdfCanvas.SetExtGState(gs);
                var rectangle = pdfDoc.GetPage(i).GetMediaBox();
                var imgWidth = img.GetWidth();
                var imgHeight = img.GetHeight();
                var transformX = (rectangle.GetWidth()) - imgWidth;
                var transformY = 0;
                var transformAt = AffineTransform.GetTranslateInstance(transformX, transformY);
                var scaleAt = AffineTransform.GetScaleInstance(imgWidth, imgHeight);
                AffineTransform at = transformAt;
                at.Concatenate(scaleAt);
                float[] matrix = new float[6];
                at.GetMatrix(matrix);
                pdfCanvas.AddImageWithTransformationMatrix(img, matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);
            }
            pdfDoc.Close();
        }

        private Point CalculateStartPoint(string pos, float imgWidth, float imgHeight, float left, float bottom, float width, float height, float spaceX, float spaceY)
        {
            float x, y = 0;
            switch (pos.ToLower())
            {
                case "top_left":
                    x = left + spaceX;
                    y = bottom + height - imgHeight - spaceY;
                    break;
                case "top_right":
                    x = left + width - imgWidth - spaceX;
                    y = bottom + height - imgHeight - spaceY;
                    break;
                case "bottom_left":
                    x = left + spaceX;
                    y = bottom + spaceY;
                    break;
                case "bottom_right":
                    x = left + width - imgWidth - spaceX;
                    y = bottom + spaceY;
                    break;
                default:
                    x = left + spaceX;
                    y = bottom + imgHeight - spaceY;
                    break;
            }
            return new Point(x, y);
        }

        private List<float[]> divideRectangles(float width, float height, int count)
        {
            var result = new List<float[]>();
            for (var x = 0; x < count; x++)
            {
                for (var y = 0; y < count; y++)
                {
                    result.Add(new float[]{
                        (x + 0) * width / count,
                        (y + 0) * height / count,
                        width / count,
                        height / count
                    });
                }
            }
            return result;
        }
    }
}
