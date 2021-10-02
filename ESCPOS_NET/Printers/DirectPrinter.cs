using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;

namespace ESCPOS_NET.Printers
{   
    public class DirectPrinter
    {
        private ICommandEmitter Emitter { get; }
        private IPrinter Printer { get; }
        public byte[] Buffer { get; private set; }

        public DirectPrinter(ICommandEmitter emitter, IPrinter printer)
        {
            this.Emitter = emitter;
            this.Printer = printer;
        }

        public DirectPrinter Send()
        {
            this.Printer.Write(this.Buffer);
            ClearAll();
            return this;
        }

        public DirectPrinter Print(string str)
        {
            UpdateBuffer(this.Emitter.Print(str));
            return this;
        }

        public DirectPrinter Initialize()
        {
            UpdateBuffer(this.Emitter.Initialize());
            return this;
        }

        public DirectPrinter PrintLine(string line = null)
        {
            UpdateBuffer(this.Emitter.PrintLine(line));
            return this;
        }

        public DirectPrinter FeedLines(int lineCount)
        {
            UpdateBuffer(this.Emitter.FeedLines(lineCount));
            return this;
        }

        public DirectPrinter FeedLinesReverse(int lineCount)
        {
            UpdateBuffer(this.Emitter.FeedLinesReverse(lineCount));
            return this;
        }

        public DirectPrinter FeedDots(int dotCount)
        {
            UpdateBuffer(this.Emitter.FeedDots(dotCount));
            return this;
        }

        public DirectPrinter ResetLineSpacing()
        {
            UpdateBuffer(this.Emitter.ResetLineSpacing());
            return this;
        }

        public DirectPrinter SetLineSpacingInDots(int dots)
        {
            UpdateBuffer(this.Emitter.SetLineSpacingInDots(dots));
            return this;
        }

        public DirectPrinter Enable()
        {
            UpdateBuffer(this.Emitter.Enable());
            return this;
        }

        public DirectPrinter Disable()
        {
            UpdateBuffer(this.Emitter.Disable());
            return this;
        }

        public DirectPrinter CashDrawerOpenPin2(int pulseOnTimeMs = 120, int pulseOffTimeMs = 240)
        {
            UpdateBuffer(this.Emitter.CashDrawerOpenPin2(pulseOnTimeMs, pulseOffTimeMs));
            return this;
        }

        public DirectPrinter CashDrawerOpenPin5(int pulseOnTimeMs = 120, int pulseOffTimeMs = 240)
        {
            UpdateBuffer(this.Emitter.CashDrawerOpenPin5(pulseOnTimeMs, pulseOffTimeMs));
            return this;
        }

        public DirectPrinter SetStyles(PrintStyle style)
        {
            UpdateBuffer(this.Emitter.SetStyles(style));
            return this;
        }

        public DirectPrinter LeftAlign()
        {
            UpdateBuffer(this.Emitter.LeftAlign());
            return this;
        }

        public DirectPrinter RightAlign()
        {
            UpdateBuffer(this.Emitter.RightAlign());
            return this;
        }

        public DirectPrinter CenterAlign()
        {
            UpdateBuffer(this.Emitter.CenterAlign());
            return this;
        }

        public DirectPrinter ReverseMode(bool activate)
        {
            UpdateBuffer(this.Emitter.ReverseMode(activate));
            return this;
        }

        public DirectPrinter RightCharacterSpacing(int spaceCount)
        {
            UpdateBuffer(this.Emitter.RightCharacterSpacing(spaceCount));
            return this;
        }

        public DirectPrinter UpsideDownMode(bool activate)
        {
            UpdateBuffer(this.Emitter.UpsideDownMode(activate));
            return this;
        }

        public DirectPrinter CodePage(CodePage codePage)
        {
            UpdateBuffer(this.Emitter.CodePage(codePage));
            return this;
        }

        public DirectPrinter Color(Color color)
        {
            UpdateBuffer(this.Emitter.Color(color));
            return this;
        }

        public DirectPrinter FullCut()
        {
            UpdateBuffer(this.Emitter.FullCut());
            return this;
        }

        public DirectPrinter PartialCut()
        {
            UpdateBuffer(this.Emitter.PartialCut());
            return this;
        }

        public DirectPrinter FullCutAfterFeed(int lineCount)
        {
            UpdateBuffer(this.Emitter.FullCutAfterFeed(lineCount));
            return this;
        }

        public DirectPrinter PartialCutAfterFeed(int lineCount)
        {
            UpdateBuffer(this.Emitter.PartialCutAfterFeed(lineCount));
            return this;
        }

        public DirectPrinter SetImageDensity(bool isHiDPI)
        {
            UpdateBuffer(this.Emitter.SetImageDensity(isHiDPI));
            return this;
        }

        public DirectPrinter BufferImage(byte[] image, int maxWidth, bool isLegacy = false, int color = 1)
        {
            UpdateBuffer(this.Emitter.BufferImage(image, maxWidth, isLegacy, color));
            return this;
        }

        public DirectPrinter WriteImageFromBuffer()
        {
            UpdateBuffer(this.Emitter.WriteImageFromBuffer());
            return this;
        }

        public DirectPrinter PrintImage(byte[] image, bool isHiDPI, bool isLegacy = false, int maxWidth = -1, int color = 1)
        {
            UpdateBuffer(this.Emitter.PrintImage(image, isHiDPI, isLegacy, maxWidth, color));
            return this;
        }

        public DirectPrinter EnableAutomaticStatusBack()
        {
            UpdateBuffer(this.Emitter.EnableAutomaticStatusBack());
            return this;
        }

        public DirectPrinter EnableAutomaticInkStatusBack()
        {
            UpdateBuffer(this.Emitter.EnableAutomaticInkStatusBack());
            return this;
        }

        public DirectPrinter PrintBarcode(BarcodeType type, string barcode, BarcodeCode code = BarcodeCode.CODE_B)
        {
            UpdateBuffer(this.Emitter.PrintBarcode(type, barcode, code));
            return this;
        }

        public DirectPrinter PrintQRCode(string data, TwoDimensionCodeType type = TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode size = Size2DCode.NORMAL, CorrectionLevel2DCode correction = CorrectionLevel2DCode.PERCENT_7)
        {
            UpdateBuffer(this.Emitter.PrintQRCode(data, type, size, correction));
            return this;
        }

        public DirectPrinter Print2DCode(TwoDimensionCodeType type, string data, Size2DCode size = Size2DCode.NORMAL, CorrectionLevel2DCode correction = CorrectionLevel2DCode.PERCENT_7)
        {
            UpdateBuffer(this.Emitter.Print2DCode(type, data, size, correction));
            return this;
        }

        public DirectPrinter SetBarcodeHeightInDots(int height)
        {
            UpdateBuffer(this.Emitter.SetBarcodeHeightInDots(height));
            return this;
        }

        public DirectPrinter SetBarWidth(BarWidth width)
        {
            UpdateBuffer(this.Emitter.SetBarWidth(width));
            return this;
        }

        public DirectPrinter SetBarLabelPosition(BarLabelPrintPosition position)
        {
            UpdateBuffer(this.Emitter.SetBarLabelPosition(position));
            return this;
        }

        public DirectPrinter SetBarLabelFontB(bool fontB)
        {
            UpdateBuffer(this.Emitter.SetBarLabelFontB(fontB));
            return this;
        }

        public DirectPrinter ClearAll()
        {
            this.Buffer = null;
            return this;
        }

        private void UpdateBuffer(byte[] newBytes)
            => this.Buffer = ByteSplicer.Combine(Buffer, newBytes);
    }
}
