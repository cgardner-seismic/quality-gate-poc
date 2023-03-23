using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using foxit.common;
using foxit.pdf;
using static foxit.pdf.PDFPage;

namespace Seismic.WatermarkingPoc.Strategies
{
    [MemoryDiagnoser]
    [BenchmarkCategory("Foxit")]
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 20)]
    public class FoxitWatermarkingStrategy : WatermarkingStrategy
    {
        [Params(SampleFileEnum.Simple, SampleFileEnum.Large, SampleFileEnum.Form, SampleFileEnum.Scientific)]
        public override SampleFileEnum SampleFile { get; set; }

        [Params(WatermarkTypeEnum.TextNewFile, WatermarkTypeEnum.ImageNewFile)]
        public override WatermarkTypeEnum WatermarkType { get; set; }

        [Params(PositionTypeEnum.Center, PositionTypeEnum.Tiled, PositionTypeEnum.Footer)]
        public override PositionTypeEnum PositionType { get; set; }

        protected override string Name => "Foxit";

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

                var watermarks = GetImageWatermarks(doc); // Do not re-create the watermarks for each page, it has performance consequences
                for (var i = 0; i < doc.GetPageCount(); i++)
                {
                    var page = doc.GetPage(i);
                    var isParsed = page.StartParse((int)ParseFlags.e_ParsePageNormal, null, false);
                    while (isParsed.GetRateOfProgress() != 100)
                    {
                        isParsed.Continue();
                    }

                    foreach (var watermark in watermarks)
                    {
                        watermark.InsertToPage(page);
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

                var watermarks = GetTextWatermarks(doc); // Do not re-create the watermarks for each page, it has performance consequences
                for (var i = 0; i < doc.GetPageCount(); i++)
                {
                    var page = doc.GetPage(i);
                    var isParsed = page.StartParse((int)ParseFlags.e_ParsePageNormal, null, false);
                    while (isParsed.GetRateOfProgress() != 100)
                    {
                        isParsed.Continue();
                    }

                    foreach (var watermark in watermarks)
                    {
                        watermark.InsertToPage(page);
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

                var watermarks = GetImageWatermarks(doc); // Do not re-create the watermarks for each page, it has performance consequences
                for (var i = 0; i < doc.GetPageCount(); i++)
                {
                    var page = doc.GetPage(i);
                    var isParsed = page.StartParse((int)ParseFlags.e_ParsePageNormal, null, false);
                    while (isParsed.GetRateOfProgress() != 100)
                    {
                        isParsed.Continue();
                    }

                    foreach (var watermark in watermarks)
                    {
                        watermark.InsertToPage(page);
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

                var watermarks = GetTextWatermarks(doc); // Do not re-create the watermarks for each page, it has performance consequences
                for (var i = 0; i < doc.GetPageCount(); i++)
                {
                    var page = doc.GetPage(i);
                    var isParsed = page.StartParse((int)ParseFlags.e_ParsePageNormal, null, false);
                    while (isParsed.GetRateOfProgress() != 100)
                    {
                        isParsed.Continue();
                    }

                    foreach (var watermark in watermarks)
                    {
                        watermark.InsertToPage(page);
                    }
                }

                doc.SaveAs(GetFilePathForResult(SampleFile, WatermarkType, PositionType), (int)PDFDoc.SaveFlags.e_SaveFlagNoOriginal);
            }
        }

        private List<Watermark> GetTextWatermarks(PDFDoc doc)
        {
            var watermarks = new List<Watermark>();
            switch (PositionType)
            {
                case PositionTypeEnum.Center:
                    var centerSettings = new WatermarkSettings();
                    centerSettings.flags = (int)(WatermarkSettings.Flags.e_FlagASPageContents | WatermarkSettings.Flags.e_FlagOnTop);
                    centerSettings.opacity = 40;
                    centerSettings.position = Position.e_PosCenter;
                    centerSettings.rotation = 45.0f;
                    centerSettings.scale_x = 1.0f;
                    centerSettings.scale_y = 1.0f;

                    var centerTextProperties = new WatermarkTextProperties();
                    centerTextProperties.alignment = Alignment.e_AlignmentCenter;
                    centerTextProperties.color = 0xF1592A;
                    centerTextProperties.font_style = WatermarkTextProperties.FontStyle.e_FontStyleNormal;
                    centerTextProperties.line_space = 1;
                    centerTextProperties.font_size = 60.0f;
                    centerTextProperties.font = new Font(Font.StandardID.e_StdIDCourierB);

                    watermarks.Add(new Watermark(doc, "Seismic Software\nTim Cardwell", centerTextProperties, centerSettings));
                    break;
                case PositionTypeEnum.Footer:
                    var footerSettings = new WatermarkSettings();
                    footerSettings.flags = (int)(WatermarkSettings.Flags.e_FlagASPageContents | WatermarkSettings.Flags.e_FlagOnTop);
                    footerSettings.opacity = 40;
                    footerSettings.position = Position.e_PosBottomRight;
                    footerSettings.scale_x = 1.0f;
                    footerSettings.scale_y = 1.0f;

                    var footerTextProperties = new WatermarkTextProperties();
                    footerTextProperties.alignment = Alignment.e_AlignmentCenter;
                    footerTextProperties.color = 0xF1592A;
                    footerTextProperties.font_style = WatermarkTextProperties.FontStyle.e_FontStyleNormal;
                    footerTextProperties.line_space = 1;
                    footerTextProperties.font_size = 32f;
                    footerTextProperties.font = new Font(Font.StandardID.e_StdIDCourierI);

                    watermarks.Add(new Watermark(doc, "Seismic Software\nTim Cardwell", footerTextProperties, footerSettings));
                    break;
                case PositionTypeEnum.Tiled:
                    var positions = new List<Position> {
                        Position.e_PosTopLeft,
                        Position.e_PosTopCenter,
                        Position.e_PosTopRight,
                        Position.e_PosCenterLeft,
                        Position.e_PosCenter,
                        Position.e_PosCenterRight,
                        Position.e_PosBottomLeft,
                        Position.e_PosBottomCenter,
                        Position.e_PosBottomRight
                    };

                    foreach (var position in positions)
                    {
                        var settings = new WatermarkSettings();
                        settings.flags = (int)(WatermarkSettings.Flags.e_FlagASPageContents | WatermarkSettings.Flags.e_FlagOnTop);
                        settings.opacity = 40;
                        settings.position = position;
                        settings.scale_x = 1.0f;
                        settings.scale_y = 1.0f;

                        var textProperties = new WatermarkTextProperties();
                        textProperties.alignment = Alignment.e_AlignmentCenter;
                        textProperties.color = 0xF1592A;
                        textProperties.font_style = WatermarkTextProperties.FontStyle.e_FontStyleNormal;
                        textProperties.line_space = 1;
                        textProperties.font_size = 20.0f;
                        textProperties.font = new Font(Font.StandardID.e_StdIDCourier);

                        watermarks.Add(new Watermark(doc, "Seismic Software\nTim Cardwell", textProperties, settings));
                    }
                    break;
            }

            return watermarks;
        }

        private List<Watermark> GetImageWatermarks(PDFDoc doc)
        {
            var watermarks = new List<Watermark>();
            switch (PositionType)
            {
                case PositionTypeEnum.Center:
                    var centerSettings = new WatermarkSettings();
                    centerSettings.flags = (int)(WatermarkSettings.Flags.e_FlagASPageContents | WatermarkSettings.Flags.e_FlagOnTop);
                    centerSettings.opacity = 50;
                    centerSettings.position = Position.e_PosCenter;
                    centerSettings.rotation = 45.0f;

                    var centerImage = new Image(GetImageWatermarkPath());
                    var centerBitmap = centerImage.GetFrameBitmap(0);

                    centerSettings.scale_x = 1f;
                    centerSettings.scale_y = 1f;

                    watermarks.Add(new Watermark(doc, centerImage, 0, centerSettings));
                    break;
                case PositionTypeEnum.Footer:
                    var footerSettings = new WatermarkSettings();
                    footerSettings.flags = (int)(WatermarkSettings.Flags.e_FlagASPageContents | WatermarkSettings.Flags.e_FlagOnTop);
                    footerSettings.opacity = 50;
                    footerSettings.position = Position.e_PosBottomRight;

                    var footerImage = new Image(GetImageWatermarkPath());
                    var footerBitmap = footerImage.GetFrameBitmap(0);

                    footerSettings.scale_x = 1f;
                    footerSettings.scale_y = 1f;

                    watermarks.Add(new Watermark(doc, footerImage, 0, footerSettings));
                    break;
                case PositionTypeEnum.Tiled:
                    var positions = new List<Position> {
                        Position.e_PosTopLeft,
                        Position.e_PosTopCenter,
                        Position.e_PosTopRight,
                        Position.e_PosCenterLeft,
                        Position.e_PosCenter,
                        Position.e_PosCenterRight,
                        Position.e_PosBottomLeft,
                        Position.e_PosBottomCenter,
                        Position.e_PosBottomRight
                    };

                    foreach (var position in positions)
                    {
                        var settings = new WatermarkSettings();
                        settings.flags = (int)(WatermarkSettings.Flags.e_FlagASPageContents | WatermarkSettings.Flags.e_FlagOnTop);
                        settings.opacity = 50;
                        settings.position = position;

                        var image = new Image(GetImageWatermarkPath());
                        var bitmap = image.GetFrameBitmap(0);

                        settings.scale_x = 1f;
                        settings.scale_y = 1f;

                        watermarks.Add(new Watermark(doc, image, 0, settings));
                    }
                    break;
            }

            return watermarks;
        }
    }
}
