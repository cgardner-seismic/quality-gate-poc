using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using foxit.common;
using foxit.common.fxcrt;
using foxit.pdf;
using foxit.pdf.annots;
using static foxit.pdf.PDFPage;

namespace Seismic.WatermarkingPoc.Strategies
{
    [MemoryDiagnoser]
    [BenchmarkCategory("Foxit")]
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 20)]
    public class FoxitWatermarkingStrategy : WatermarkingStrategy
    {
        //[Params(SampleFileEnum.Simple, SampleFileEnum.Large, SampleFileEnum.Form, SampleFileEnum.Scientific)]
        [Params(SampleFileEnum.Large)]
        public override SampleFileEnum SampleFile { get; set; }

        [Params(WatermarkTypeEnum.TextNewFile, WatermarkTypeEnum.ImageNewFile)]
        public override WatermarkTypeEnum WatermarkType { get; set; }

        //[Params(PositionTypeEnum.Center, PositionTypeEnum.Tiled, PositionTypeEnum.Footer)]
        [Params(PositionTypeEnum.Tiled, PositionTypeEnum.Tiled_With_Rotation)]
        public override PositionTypeEnum PositionType { get; set; }

        protected override string Name => "Foxit";

        //In future, we can introduce TILES_X_COUNT, TILES_Y_COUNT, if the tiles counts are different between x and y axis.
        const int TILES_COUNT = 4;

        public FoxitWatermarkingStrategy()
        {
            string sn = "PiisqZrklmql2fN8Euj9TznSMaCq7fdFtJDIBZYeGSW86PESom2srQ==";
            string key = "8f0YFUGMvRkN+ddwhrBxubTv1X8GzmILMFDAylGsl9gEt82edDI+7gO68ihK2XQ0RPBmHmpqUP+FpAM0xlSJ0Z4UeNCm3a5JPrfnujo8z55rYeYksnkouIp1WKW7za9gzAv7iI50TK9HfyrAp0G6mI7r2gLfrGzS5iUsamXQ2JqcWEZ41jDfUmV16U6uTYX/yK2bai0VjLvrVC8oh5QMRswcMEDoX+mAo11DtxOptnB25dOssAtBPNn6DnJ2i4X5DndSz787/Id1gMbLfjqQwkXKsJ3mYvPbDnSmgwxZoKgvsZGvLLEHNSh3ZZqfCHQod2Ac5KzxsqVh/2aPuIyI7guTVdnlqOLHCjYLB7sqlUpJvxCgprJh1Xs8QduOAT6wkCU4A7qFHhTJG3yrPwpY+JKA4uKbdDjzYZidf65oGrWnetuKD4sBpBZrm1WMz2/BX/h7nzds9L+zq/IcKvatD1FgZAkXn0+FeG5PawP38ZfSK6b32VfW2NkGyHrIFPiQwX0l+TPz0mKw+uhtm29CckhYE3cJ82VNthdwdq7VeiM80Wm6x5PRaZxfGP/UPK5ORL70WI+HpclJHrdrvDuokbCjnGEr099e0Zc4e8JIp+N4rtlz32Y7PKJModXRvS61S8htam9kaBcIl3IURm0bDCCzVRYhR1PrqmSF6O3kW8U636yi94lQADl/5TP3zVtTV8lbeEcUfWRmZ9TqDErVMG3ArG605//THsKQcOPnbvzFY8VgUYpxH8Hm1oAGM29gLC6M22x47sThHOxjt31n0OgBjyPdU53P29+o/csFzkuo0aDq8n4ukYxXezYxtb1HOCgM9NIWHnlfLj42h9nAgi/QwLL4r2un6oaz24tPDtofJUpbXzzSD0jqgKwloQyWmb5h/akhzDKL4AbGr+ZhVfkFFxzyL4r9i4GE1Thy9ytYiXMzCrhvR2iUy2u3WWYKFnK6bwRJCEadcNV3PYZUsUE24xS+jfsv0TxjFUz5hwUi45CMXq7egVmW8GYxgw/FoR/mOgugA7MpQXM4A70k8b0qoIOq1vpDabovoN4EvvwtTBKqdMucGP4YQcUFtpZbWUAunVs8GTa7fbL/Hn4oWwC3B8p/xFo27rHgakNiYwqdD970m4qkv9bMRaiiszCFfJ1ln8zE38ytQ3ek8OE69iQIpeBg5TaGQ0EOu6Pym2aJtDmvh8XW24DCoCWHNrk4OfvewUwego3mTRWaax0ulcKNfgVY/ueQIIacGlxI9NzXT7on5zUEkhOWUcC13XUWGmg56sjDZ3gDPvYEMK4r4Tgxg4GtFo/Eyyjf6c58/d+3R8944q0atYgMxg==";
            ErrorCode error_code = Library.Initialize(sn, key);
            if (error_code != ErrorCode.e_ErrSuccess)
            {
                throw new InvalidOperationException("Cannot intialize Foxit due to bad license");
            }
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
                    ApplyTextWatermarksIncremental();
                    break;

                case WatermarkTypeEnum.ImageIncremental:
                    ApplyImageWatermarksIncremental();
                    break;
            }
        }

        private void ApplyImageWatermarksIncremental()
        {
            using (var doc = new PDFDoc(GetFilePathForSample(SampleFile)))
            {
                var error_code = doc.LoadW(SampleFile == SampleFileEnum.PasswordProtected ? PasswordProtectedSamplePassword : string.Empty);
                if (error_code != ErrorCode.e_ErrSuccess)
                {
                    throw new InvalidOperationException("Document load issue");
                }

                var allWatermarks = new Dictionary<Tuple<float, float>, List<Watermark>>();
                for (var i = 0; i < doc.GetPageCount(); i++)
                {
                    var page = doc.GetPage(i);
                    var isParsed = page.StartParse((int)ParseFlags.e_ParsePageNormal, null, false);
                    while (isParsed.GetRateOfProgress() != 100)
                    {
                        isParsed.Continue();
                    }

                    using (var pageInfo = doc.GetPageBasicInfo(i))
                    {
                        var heightAndWidth = new Tuple<float, float>(pageInfo.width, pageInfo.height);
                        if (!allWatermarks.ContainsKey(heightAndWidth))
                        {
                            allWatermarks.Add(heightAndWidth, GetImageWatermarks(doc, pageInfo.width, pageInfo.height));
                        }

                        foreach (var watermark in allWatermarks[heightAndWidth])
                        {
                            watermark.InsertToPage(page);
                        }

                        /* Add rectangles for debugging the positioning on the second page*/
                        if (i == 1)
                        {
                            var rectangles = DivideRectangles(pageInfo.width, pageInfo.height, 6);
                            rectangles.ForEach(r =>
                            {
                                page.AddAnnot(Annot.Type.e_Square, new RectF(r[0], r[1], r[0] + r[2], r[1] + r[3]));
                            });
                        }
                    }
                }

                doc.SaveAs(GetFilePathForResult(SampleFile, WatermarkType, PositionType), (int)(PDFDoc.SaveFlags.e_SaveFlagIncremental));
            }
        }

        private void ApplyTextWatermarksIncremental()
        {
            using (var doc = new PDFDoc(GetFilePathForSample(SampleFile)))
            {
                var error_code = doc.LoadW(SampleFile == SampleFileEnum.PasswordProtected ? PasswordProtectedSamplePassword : string.Empty);
                if (error_code != ErrorCode.e_ErrSuccess)
                {
                    throw new InvalidOperationException("Document load issue");
                }

                var allWatermarks = new Dictionary<Tuple<float, float>, List<Watermark>>();
                for (var i = 0; i < doc.GetPageCount(); i++)
                {
                    var page = doc.GetPage(i);
                    var isParsed = page.StartParse((int)ParseFlags.e_ParsePageNormal, null, false);
                    while (isParsed.GetRateOfProgress() != 100)
                    {
                        isParsed.Continue();
                    }

                    using (var pageInfo = doc.GetPageBasicInfo(i))
                    {
                        var heightAndWidth = new Tuple<float, float>(pageInfo.width, pageInfo.height);
                        if (!allWatermarks.ContainsKey(heightAndWidth))
                        {
                            allWatermarks.Add(heightAndWidth, GetTextWatermarks(doc, pageInfo.width, pageInfo.height));
                        }

                        foreach (var watermark in allWatermarks[heightAndWidth])
                        {
                            watermark.InsertToPage(page);
                        }
                    }
                }

                doc.SaveAs(GetFilePathForResult(SampleFile, WatermarkType, PositionType), (int)(PDFDoc.SaveFlags.e_SaveFlagIncremental));
            }
        }

        private void ApplyImageWatermarksNewFile()
        {
            using (var doc = new PDFDoc(GetFilePathForSample(SampleFile)))
            {
                var error_code = doc.LoadW(SampleFile == SampleFileEnum.PasswordProtected ? PasswordProtectedSamplePassword : string.Empty);
                if (error_code != ErrorCode.e_ErrSuccess)
                {
                    throw new InvalidOperationException("Document load issue");
                }

                var allWatermarks = new Dictionary<Tuple<float, float>, List<Watermark>>();
                for (var i = 0; i < doc.GetPageCount(); i++)
                {
                    var page = doc.GetPage(i);
                    var isParsed = page.StartParse((int)ParseFlags.e_ParsePageNormal, null, false);
                    while (isParsed.GetRateOfProgress() != 100)
                    {
                        isParsed.Continue();
                    }

                    using (var pageInfo = doc.GetPageBasicInfo(i))
                    {
                        var heightAndWidth = new Tuple<float, float>(pageInfo.width, pageInfo.height);
                        if (!allWatermarks.ContainsKey(heightAndWidth))
                        {
                            allWatermarks.Add(heightAndWidth, GetImageWatermarks(doc, pageInfo.width, pageInfo.height));
                        }

                        foreach (var watermark in allWatermarks[heightAndWidth])
                        {
                            watermark.InsertToPage(page);
                        }

                        /* Add rectangles for debugging the tiling positioning on each page*/
                        if(PositionType == PositionTypeEnum.Tiled || PositionType == PositionTypeEnum.Tiled_With_Rotation)
                        {
                            var rectangles = DivideRectangles(pageInfo.width, pageInfo.height, TILES_COUNT);
                            rectangles.ForEach(r =>
                            {
                                page.AddAnnot(Annot.Type.e_Square, new RectF(r[0], r[1], r[0] + r[2], r[1] + r[3]));
                            });
                        }
                    }
                }

                doc.SaveAs(GetFilePathForResult(SampleFile, WatermarkType, PositionType), (int)PDFDoc.SaveFlags.e_SaveFlagNoOriginal);
            }
        }

        private void ApplyTextWatermarksNewFile()
        {
            using (var doc = new PDFDoc(GetFilePathForSample(SampleFile)))
            {
                var error_code = doc.LoadW(SampleFile == SampleFileEnum.PasswordProtected ? PasswordProtectedSamplePassword : string.Empty);
                if (error_code != ErrorCode.e_ErrSuccess)
                {
                    throw new InvalidOperationException("Document load issue");
                }

                // We store watermarks in a dictionary, preventing us from having to create them in each iteration
                var allWatermarks = new Dictionary<Tuple<float, float>, List<Watermark>>(); // The tuple stores the pages width and height, respectively, allowing us to have different watermarks for differently sized pages
                for (var i = 0; i < doc.GetPageCount(); i++)
                {
                    var page = doc.GetPage(i);
                    var isParsed = page.StartParse((int)ParseFlags.e_ParsePageNormal, null, false);
                    while (isParsed.GetRateOfProgress() != 100)
                    {
                        isParsed.Continue();
                    }

                    using (var pageInfo = doc.GetPageBasicInfo(i))
                    {
                        var heightAndWidth = new Tuple<float, float>(pageInfo.width, pageInfo.height);
                        if (!allWatermarks.ContainsKey(heightAndWidth))
                        {
                            allWatermarks.Add(heightAndWidth, GetTextWatermarks(doc, pageInfo.width, pageInfo.height));
                        }

                        foreach (var watermark in allWatermarks[heightAndWidth])
                        {
                            watermark.InsertToPage(page);
                        }

                        /* Add rectangles for debugging the tiling positioning on each page*/
                        if (PositionType == PositionTypeEnum.Tiled || PositionType == PositionTypeEnum.Tiled_With_Rotation)
                        {
                            var rectangles = DivideRectangles(pageInfo.width, pageInfo.height, TILES_COUNT);
                            rectangles.ForEach(r =>
                            {
                                page.AddAnnot(Annot.Type.e_Square, new RectF(r[0], r[1], r[0] + r[2], r[1] + r[3]));
                            });
                        }
                    }
                }

                doc.SaveAs(GetFilePathForResult(SampleFile, WatermarkType, PositionType), (int)PDFDoc.SaveFlags.e_SaveFlagNoOriginal);
            }
        }

        private List<Watermark> GetTextWatermarks(PDFDoc doc, float width, float height)
        {
            var watermarkText = "Seismic Software\nTim Cardwell\nSoftware Engineer\n2022-01-06T11:45:00Z";
            var textProperties = new WatermarkTextProperties
            {
                alignment = Alignment.e_AlignmentCenter,
                color = 0xF1592A,
                line_space = 1,
                font = new Font(Font.StandardID.e_StdIDCourier),
                font_size = 100f,
                font_style = WatermarkTextProperties.FontStyle.e_FontStyleNormal,
            };

            var watermarkSettings = new WatermarkSettings
            {
                flags = (int)(WatermarkSettings.Flags.e_FlagASPageContents | WatermarkSettings.Flags.e_FlagOnTop),
                opacity = 40,
                scale_x = 1f,
                scale_y = 1f,
                position = Position.e_PosCenter
            };

            // We create a watermark so that Foxit can give us the height and width of the new object
            var watermarkForSizing = new Watermark(doc, watermarkText, textProperties, watermarkSettings);
            var watermarkWidth = watermarkForSizing.GetWidth();
            var watermarkHeight = watermarkForSizing.GetHeight();

            var watermarks = new List<Watermark>();
            switch (PositionType)
            {
                case PositionTypeEnum.Center:
                    var centerSettings = new WatermarkSettings(watermarkSettings);
                    centerSettings.rotation = 45.0f;

                    // Calculate scale
                    var centerScaleX = width > watermarkWidth ? 1 : width / watermarkWidth;
                    var centerScaleY = height > watermarkHeight ? 1 : height / watermarkHeight;
                    centerSettings.scale_x = Math.Min(centerScaleX, centerScaleY);
                    centerSettings.scale_y = Math.Min(centerScaleX, centerScaleY);

                    watermarks.Add(new Watermark(doc, watermarkText, textProperties, centerSettings));
                    break;

                case PositionTypeEnum.Footer:
                    var footerSettings = new WatermarkSettings(watermarkSettings);
                    footerSettings.position = Position.e_PosBottomRight;

                    // Calculate scale
                    var footerScaleX = width > watermarkWidth ? 1 : width / watermarkWidth;
                    var footerScaleY = height > watermarkHeight ? 1 : height / watermarkHeight;
                    footerSettings.scale_x = Math.Min(footerScaleX, footerScaleY);
                    footerSettings.scale_y = Math.Min(footerScaleX, footerScaleY);

                    watermarks.Add(new Watermark(doc, watermarkText, textProperties, footerSettings));
                    break;

                case PositionTypeEnum.Tiled:
                case PositionTypeEnum.Tiled_With_Rotation:
                    var tiledScaleX = (width / TILES_COUNT) > watermarkWidth ? 1 : width / TILES_COUNT / watermarkWidth;
                    var tiledScaleY = (height / TILES_COUNT) > watermarkHeight ? 1 : height / TILES_COUNT / watermarkHeight;
                    var scaledWatermarkWidth = watermarkWidth * Math.Min(tiledScaleX, tiledScaleY);
                    var scaledWatermarkHeight = watermarkHeight * Math.Min(tiledScaleX, tiledScaleY);
                    var rectangles = DivideRectangles(width, height, TILES_COUNT);
                    var settings = new WatermarkSettings(watermarkSettings);
                    settings.scale_x = Math.Min(tiledScaleX, tiledScaleY);
                    settings.scale_y = Math.Min(tiledScaleX, tiledScaleY);
                    settings.position = Position.e_PosBottomLeft;
                    rectangles.ForEach(r =>
                    {
                        var offset_x = 0.0f;
                        var offset_y = 0.0f;
                        if (PositionType == PositionTypeEnum.Tiled)
                        {
                            offset_x = r[0] + (r[2] - scaledWatermarkWidth) / 2;
                            offset_y = r[1] + (r[3] - scaledWatermarkHeight) / 2;
                        }
                        else
                        {
                            offset_x = r[0] + (r[2] - scaledWatermarkWidth) / 2;
                            offset_y = r[1];
                            settings.rotation = (float)(Math.Atan(r[3] / r[2]) / Math.PI * 180);
                        }
                        settings.offset_x = offset_x;
                        settings.offset_y = offset_y;
                        watermarks.Add(new Watermark(doc, watermarkText, textProperties, settings));
                    });
                    break;
            }

            return watermarks;
        }

        private List<Watermark> GetImageWatermarks(PDFDoc doc, float width, float height)
        {
            var tempImage = new Image(GetImageWatermarkPath());
            var tempBitmap = tempImage.GetFrameBitmap(0);

            var settings = new WatermarkSettings
            {
                flags = (int)(WatermarkSettings.Flags.e_FlagASPageContents | WatermarkSettings.Flags.e_FlagOnTop),
                opacity = 50,
                scale_x = 1f,
                scale_y = 1f,
                position = Position.e_PosCenter
            };

            var watermarks = new List<Watermark>();
            switch (PositionType)
            {
                case PositionTypeEnum.Center:
                    var centerScaleX = width > tempBitmap.GetWidth() ? 1 : width / tempBitmap.GetWidth();
                    var centerScaleY = height > tempBitmap.GetHeight() ? 1 : height / tempBitmap.GetHeight();

                    var centerSettings = new WatermarkSettings(settings);
                    centerSettings.scale_x = Math.Min(centerScaleX, centerScaleY);
                    centerSettings.scale_y = Math.Min(centerScaleX, centerScaleY);
                    centerSettings.rotation = 45.0f;

                    var centerImage = new Image(GetImageWatermarkPath());
                    var centerBitmap = centerImage.GetFrameBitmap(0);

                    watermarks.Add(new Watermark(doc, centerImage, 0, centerSettings));
                    break;
                case PositionTypeEnum.Footer:
                    var footerScaleX = width > tempBitmap.GetWidth() ? 1 : width / tempBitmap.GetWidth();
                    var footerScaleY = height > tempBitmap.GetHeight() ? 1 : height / tempBitmap.GetHeight();

                    var footerSettings = new WatermarkSettings(settings);
                    footerSettings.scale_x = Math.Min(footerScaleX, footerScaleY);
                    footerSettings.scale_y = Math.Min(footerScaleX, footerScaleY);
                    footerSettings.position = Position.e_PosBottomRight;

                    var footerImage = new Image(GetImageWatermarkPath());
                    var footerBitmap = footerImage.GetFrameBitmap(0);

                    watermarks.Add(new Watermark(doc, footerImage, 0, footerSettings));
                    break;
                case PositionTypeEnum.Tiled:
                case PositionTypeEnum.Tiled_With_Rotation:
                    var tiledImage = new Image(GetImageWatermarkPath());
                    var tiledScaleX = (width / TILES_COUNT) > tempBitmap.GetWidth() ? 1 : width / TILES_COUNT / tempBitmap.GetWidth();
                    var tiledScaleY = (height / TILES_COUNT) > tempBitmap.GetHeight() ? 1 : height / TILES_COUNT / tempBitmap.GetHeight();
                    var watermarkWidth = tempBitmap.GetWidth() * Math.Min(tiledScaleX, tiledScaleY);
                    var watermarkHeight = tempBitmap.GetHeight() * Math.Min(tiledScaleX, tiledScaleY);
                    var rectangles = DivideRectangles(width, height, TILES_COUNT);
                    var tiledSettings = new WatermarkSettings(settings);
                    tiledSettings.scale_x = Math.Min(tiledScaleX, tiledScaleY);
                    tiledSettings.scale_y = Math.Min(tiledScaleX, tiledScaleY);
                    tiledSettings.position = Position.e_PosBottomLeft;
                    rectangles.ForEach(r => {
                        var offset_x = 0.0f;
                        var offset_y = 0.0f;
                        if(PositionType == PositionTypeEnum.Tiled)
                        {
                            offset_x = r[0] + (r[2] - watermarkWidth) / 2;
                            offset_y = r[1] + (r[3] - watermarkHeight) / 2;
                        } else
                        {
                            offset_x = r[0] + (r[2] - watermarkWidth) / 2;
                            offset_y = r[1];
                            tiledSettings.rotation = (float)(Math.Atan(r[3] / r[2]) / Math.PI * 180);
                        }
                        tiledSettings.offset_x = offset_x;
                        tiledSettings.offset_y = offset_y;
                        watermarks.Add(new Watermark(doc, tiledImage, 0, tiledSettings));
                    });
                    break;
            }

            return watermarks;
        }

        private List<float[]> DivideRectangles(float width, float height, int count)
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
