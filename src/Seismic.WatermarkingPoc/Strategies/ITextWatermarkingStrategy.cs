using System.IO;
using iText.IO.Font.Constants;
using iText.IO.Font;
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
using Microsoft.Diagnostics.Runtime.Interop;

namespace Seismic.WatermarkingPoc.Strategies
{
    /// <summary>
    /// |    Method | SampleFile |      WatermarkType |       Mean |      Error |     StdDev |      Gen 0 |     Gen 1 |     Gen 2 | Allocated |
    /// |---------- |----------- |------------------- |-----------:|-----------:|-----------:|-----------:|----------:|----------:|----------:|
    /// | Benchmark |     Simple |        TextNewFile |   8.488 ms |  0.3392 ms |  0.3906 ms |          - |         - |         - |      4 MB |
    /// | Benchmark |     Simple |       ImageNewFile |   6.395 ms |  1.2995 ms |  1.4444 ms |          - |         - |         - |      4 MB |
    /// | Benchmark |     Simple |  TextCornerNewFile |   6.386 ms |  1.0292 ms |  1.1853 ms |          - |         - |         - |      3 MB |
    /// | Benchmark |     Simple | ImageCornerNewFile |   4.338 ms |  0.3394 ms |  0.3773 ms |          - |         - |         - |      3 MB |
    /// | Benchmark |      Large |        TextNewFile | 703.824 ms | 15.5366 ms | 15.2590 ms | 70000.0000 | 7000.0000 | 1000.0000 |    426 MB |
    /// | Benchmark |      Large |       ImageNewFile | 284.165 ms | 14.3137 ms | 16.4837 ms | 63000.0000 | 6000.0000 | 1000.0000 |    374 MB |
    /// | Benchmark |      Large |  TextCornerNewFile | 395.844 ms |  6.5099 ms |  6.9655 ms | 62000.0000 | 4000.0000 | 1000.0000 |    375 MB |
    /// | Benchmark |      Large | ImageCornerNewFile | 187.966 ms |  6.5939 ms |  7.3291 ms | 58000.0000 | 4000.0000 | 1000.0000 |    344 MB |
    /// | Benchmark |       Form |        TextNewFile |   5.865 ms |  0.3290 ms |  0.3657 ms |          - |         - |         - |      2 MB |
    /// | Benchmark |       Form |       ImageNewFile |   4.331 ms |  0.3291 ms |  0.3658 ms |          - |         - |         - |      2 MB |
    /// | Benchmark |       Form |  TextCornerNewFile |   4.377 ms |  0.3022 ms |  0.3359 ms |          - |         - |         - |      2 MB |
    /// | Benchmark |       Form | ImageCornerNewFile |   3.791 ms |  0.1865 ms |  0.2073 ms |          - |         - |         - |      2 MB |
    /// | Benchmark | Scientific |        TextNewFile |  22.500 ms |  0.7521 ms |  0.8359 ms |  1000.0000 |         - |         - |     11 MB |
    /// | Benchmark | Scientific |       ImageNewFile |   9.919 ms |  0.3325 ms |  0.3558 ms |  1000.0000 |         - |         - |     10 MB |
    /// | Benchmark | Scientific |  TextCornerNewFile |  11.936 ms |  0.4264 ms |  0.4563 ms |  1000.0000 |         - |         - |     10 MB |
    /// | Benchmark | Scientific | ImageCornerNewFile |   8.656 ms |  0.3801 ms |  0.4377 ms |  1000.0000 |         - |         - |      9 MB |
    /// </summary>
    [MemoryDiagnoser]
    [BenchmarkCategory("IText")]
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 20, id: "IText")]
    public class ITextWatermarkingStrategy : WatermarkingStrategy
    {
        //[Params(SampleFileEnum.Simple, SampleFileEnum.Large, SampleFileEnum.Form, SampleFileEnum.Scientific)]
        [Params(SampleFileEnum.Large)]
        public override SampleFileEnum SampleFile { get; set; }

        [Params(WatermarkTypeEnum.TextNewFile, WatermarkTypeEnum.ImageNewFile)]
        public override WatermarkTypeEnum WatermarkType { get; set; }

        //[Params(PositionTypeEnum.Center, PositionTypeEnum.Tiled, PositionTypeEnum.Footer)]
        [Params(PositionTypeEnum.Tiled)]
        public override PositionTypeEnum PositionType { get; set; }

        protected override string Name { get => "IText"; }

        //In future, we can introduce TILES_X_COUNT, TILES_Y_COUNT, if the tiles counts are different between x and y axis.
        const int TILES_COUNT = 5;

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
                case PositionTypeEnum.Tiled:
                    ApplyTextWatermarkCenterOrTiled(GetFilePathForSample(SampleFile), GetFilePathForResult(SampleFile, WatermarkTypeEnum.TextNewFile, PositionType), PositionType);
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

        private void ApplyTextWatermarkCenterOrTiled(string srcFilePath, string targetFilePath, PositionTypeEnum pos, float scale=0.05f, float opacity=0.1f)
        {
            PdfDocument pdfDoc = null;

            if(SampleFile == SampleFileEnum.PasswordProtected)
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
            pdfDoc.SetCloseReader(true);
            pdfDoc.SetCloseWriter(true);
            pdfDoc.SetFlushUnusedObjects(true);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.COURIER);

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {

                PdfPage pdfPage = pdfDoc.GetPage(i);
                Rectangle pageSize = pdfPage.GetPageSizeWithRotation();
                var pageWidth = pageSize.GetWidth();
                var pageHeight = pageSize.GetHeight();
                //var fontSize = Math.Min(pageWidth, pageHeight) * scale;
                var fontSize = 20.0f;
                Text company = new Text("mike@digify.com\n")
                    .SetFont(font)
                    .SetFontColor(ColorConstants.RED)
                    .SetOpacity(opacity)
                    .SetFontSize(fontSize);
                Text statement = new Text("CONFIDENTIAL")
                    .SetFont(font)
                    .SetFontColor(ColorConstants.RED)
                    .SetFontSize(fontSize)
                    .SetOpacity(opacity)
                    .SetBold();
                Paragraph paragraph = new Paragraph()
                    .Add(company)
                    .Add(statement)
                    .SetMaxWidth(pageWidth/TILES_COUNT)
                    .SetMaxHeight(pageHeight/TILES_COUNT);
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                var rectangles = divideRectangles(pageSize.GetWidth(), pageSize.GetHeight(), TILES_COUNT);
                PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);

                rectangles.ForEach(r =>
                {
                    var rectangle = new Rectangle(r[0], r[1], r[2], r[3]);
                    pdfCanvas.Rectangle(rectangle);
                    pdfCanvas.Stroke();
                    var canvas = new Canvas(pdfCanvas, rectangle);
                    var radian = (pos == PositionTypeEnum.Center) ? 0: (float)Math.Atan(pageSize.GetHeight() / pageSize.GetWidth());
                    canvas.ShowTextAligned(paragraph,
                        r[0] + pageSize.GetWidth() / TILES_COUNT / 2,
                        r[1] + pageSize.GetHeight() / TILES_COUNT / 2,
                        i,
                        TextAlignment.CENTER,
                        VerticalAlignment.MIDDLE,
                        radian);
                    canvas.Close();
                });
            }
            pdfDoc.Close();
        }
        private void ApplyTextWatermarkFooter(string srcFilePath, string targetFilePath, string pos = "top_left", float scale = 0.05f, float opacity = 0.1f, int spaceX = 20, int spaceY = 20, int count = 1)
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
            pdfDoc.SetCloseReader(true);
            pdfDoc.SetCloseWriter(true);
            pdfDoc.SetFlushUnusedObjects(true);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.COURIER);

            // Implement transformation matrix usage in order to scale image
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                PdfPage pdfPage = pdfDoc.GetPage(i);
                Rectangle pageSize = pdfPage.GetPageSizeWithRotation();
                var fontSize = Math.Min(pageSize.GetWidth(), pageSize.GetHeight()) * scale;
                Text company = new Text("mike@digify.com\n")
                    .SetFont(font)
                    .SetFontColor(ColorConstants.RED)
                    .SetFontSize(fontSize)
                    .SetOpacity(opacity);
                Text statement = new Text("CONFIDENTIAL")
                    .SetFont(font)
                    .SetFontColor(ColorConstants.RED)
                    .SetFontSize(fontSize)
                    .SetOpacity(opacity)
                    .SetBold();
                Paragraph paragraph = new Paragraph()
                        .Add(company)
                        .Add(statement);
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);
                var rectangles = divideRectangles(pageSize.GetWidth(), pageSize.GetHeight(), count);
                rectangles.ForEach(r =>
                {
                    var rectangle = new Rectangle(r[0], r[1], r[2], r[3]);
                    var canvas = new Canvas(pdfCanvas, rectangle);
                    switch (pos.ToLower())
                    {
                        case "top_left":
                            canvas.ShowTextAligned(paragraph, r[0] + spaceX, r[1] + r[3] - spaceY, TextAlignment.LEFT, VerticalAlignment.TOP);
                            break;
                        case "top_right":
                            canvas.ShowTextAligned(paragraph, r[0] + r[2] - spaceX, r[1] + r[3] - spaceY, TextAlignment.RIGHT, VerticalAlignment.TOP);
                            break;
                        case "bottom_left":
                            canvas.ShowTextAligned(paragraph, r[0] + spaceX, r[1] + spaceY, TextAlignment.LEFT, VerticalAlignment.BOTTOM);
                            break;
                        case "bottom_right":
                            canvas.ShowTextAligned(paragraph, r[0] + r[2] - spaceX, r[1] + spaceY, TextAlignment.RIGHT, VerticalAlignment.BOTTOM);
                            break;
                    }
                    canvas.Close();
                });
            }
            pdfDoc.Close();
        }

        private void ApplyImageWatermarkCenter(string srcFilePath, string targetFilePath, float opacity = 0.1f)
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
            pdfDoc.SetCloseReader(true);
            pdfDoc.SetCloseWriter(true);
            pdfDoc.SetFlushUnusedObjects(true);
            ImageData img = ImageDataFactory.Create(GetImageWatermarkPath());
            var imgWidth = img.GetWidth();
            var imgHeight = img.GetHeight();
            PdfExtGState gs = new PdfExtGState().SetFillOpacity(opacity);

            // Implement transformation matrix usage in order to scale image
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                PdfPage pdfPage = pdfDoc.GetPage(i);
                Rectangle pageSize = pdfPage.GetPageSizeWithRotation();
                var pageWidth = pageSize.GetWidth();
                var pageHeight = pageSize.GetHeight();
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                var rectangles = divideRectangles(pageSize.GetWidth(), pageSize.GetHeight(), TILES_COUNT);
                PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);
                pdfCanvas.SetExtGState(gs);
                rectangles.ForEach(r =>
                {
                    var rectangle = new Rectangle(r[0], r[1], r[2], r[3]);
                    pdfCanvas.Rectangle(rectangle);
                    pdfCanvas.Stroke();
                    var scaleX = (pageWidth / TILES_COUNT) > imgWidth ? 1 : pageWidth / TILES_COUNT / imgWidth;
                    var scaleY = (pageHeight / TILES_COUNT) > imgHeight ? 1 : pageHeight / TILES_COUNT / imgHeight;
                    var scale = Math.Min(scaleX, scaleY);
                    var scaledImgWidth = img.GetWidth() * scale;
                    var scaledImgHeight = img.GetHeight() * scale;
                    var transformX = (r[2] - scaledImgWidth) / 2 + r[0];
                    var transformY = (r[3] - scaledImgHeight) / 2 + r[1];
                    var transformAt = AffineTransform.GetTranslateInstance(transformX, transformY);
                    var scaleAt = AffineTransform.GetScaleInstance(scaledImgWidth, scaledImgHeight);
                    AffineTransform at = transformAt;
                    at.Concatenate(scaleAt);
                    float[] matrix = new float[6];
                    at.GetMatrix(matrix);
                    pdfCanvas.AddImageWithTransformationMatrix(img, matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);
                });
            }
            pdfDoc.Close();
        }

        private void ApplyImageWatermarkTiled(string srcFilePath, string targetFilePath, float opacity = 0.1f)
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
            pdfDoc.SetCloseReader(true);
            pdfDoc.SetCloseWriter(true);
            pdfDoc.SetFlushUnusedObjects(true);
            ImageData img = ImageDataFactory.Create(GetImageWatermarkPath());
            var imgWidth = img.GetWidth();
            var imgHeight = img.GetHeight();
            PdfExtGState gs = new PdfExtGState().SetFillOpacity(opacity);

            // Implement transformation matrix usage in order to scale image
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                PdfPage pdfPage = pdfDoc.GetPage(i);
                Rectangle pageSize = pdfPage.GetPageSizeWithRotation();
                var pageWidth = pageSize.GetWidth();
                var pageHeight = pageSize.GetHeight();
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                var rectangles = divideRectangles(pageWidth, pageHeight, TILES_COUNT);
                PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);
                pdfCanvas.SetExtGState(gs);
                rectangles.ForEach(r =>
                {
                    var rectangle = new Rectangle(r[0], r[1], r[2], r[3]);
                    pdfCanvas.Rectangle(rectangle);
                    pdfCanvas.Stroke();
                    var radian = (float)Math.Atan(pageHeight/ pageWidth);
                    //var radian = 75 * Math.PI / 180;
                    var hypotenuse = Math.Min(1 / Math.Sin(radian) * r[3], 1 / Math.Cos(radian) * r[2]);
                    var scaleX = (pageWidth / TILES_COUNT) > imgWidth ? 1 : pageWidth  / TILES_COUNT / imgWidth;
                    var scaleY = (pageHeight / TILES_COUNT) > imgHeight ? 1 : pageHeight / TILES_COUNT / imgHeight;
                    var scale = Math.Min(scaleX, scaleY);
                    var scaledImgWidth = imgWidth * scale;
                    var scaledImgHeight = imgHeight * scale;
                    var translateAt1 = AffineTransform.GetTranslateInstance(r[0], r[1]);
                    var translateAt2 = AffineTransform.GetTranslateInstance(hypotenuse / 2 - scaledImgWidth / 2, -scaledImgHeight / 2);
                    var scaleAt = AffineTransform.GetScaleInstance(scaledImgWidth, scaledImgHeight);
                    var rotateAt = AffineTransform.GetRotateInstance(radian);
                    AffineTransform at = translateAt1;
                    at.Concatenate(rotateAt);
                    at.Concatenate(translateAt2);
                    at.Concatenate(scaleAt);
                    float[] matrix = new float[6];
                    at.GetMatrix(matrix);
                    pdfCanvas.AddImageWithTransformationMatrix(img, matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);
                });
            }
            pdfDoc.Close();
        }

        private void ApplyImageWatermarkFooter(string srcFilePath, string targetFilePath, string pos = "top_left", float scale=0.5f, float opacity = 0.1f, int spaceX = 20, int spaceY = 20, int count = 1)
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
            pdfDoc.SetCloseReader(true);
            pdfDoc.SetCloseWriter(true);
            pdfDoc.SetFlushUnusedObjects(true);
            ImageData img = ImageDataFactory.Create(GetImageWatermarkPath());

            PdfExtGState gs = new PdfExtGState().SetFillOpacity(opacity);

            // Implement transformation matrix usage in order to scale image
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                PdfPage pdfPage = pdfDoc.GetPage(i);
                Rectangle pageSize = pdfPage.GetPageSizeWithRotation();
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);
                pdfCanvas.SetExtGState(gs);
                var rectangles = divideRectangles(pageSize.GetWidth(), pageSize.GetHeight(), count);
                rectangles.ForEach(r =>
                {
                    var imgWidth = img.GetWidth() * scale;
                    var imgHeight = img.GetHeight() * scale;
                    var imgStartPoint = CalculateStartPoint(pos, imgWidth, imgHeight, r[0], r[1], r[2], r[3], spaceX, spaceY);

                    var translateAt = AffineTransform.GetTranslateInstance(imgStartPoint.x, imgStartPoint.y);
                    var scaleAt = AffineTransform.GetScaleInstance(imgWidth, imgHeight);
                    AffineTransform at = translateAt;
                    at.Concatenate(scaleAt);
                    float[] matrix = new float[6];
                    at.GetMatrix(matrix);
                    pdfCanvas.AddImageWithTransformationMatrix(img, matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);
                });
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
