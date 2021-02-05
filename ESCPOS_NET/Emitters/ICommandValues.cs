namespace ESCPOS_NET.Emitters
{
    public interface ICommandValues
    {
        /* Barcodes */
        byte PrintBarcode { get; }
        byte SetBarcodeHeightInDots { get; }
        byte SetBarWidth { get; }
        byte SetBarLabelPosition { get; }
        byte SetBarLabelFont { get; }

        byte Set2DCode { get; }
        byte AutoEnding { get; }
        byte[] SetPDF417NumberOfColumns { get; }
        byte[] SetPDF417NumberOfRows { get; }
        byte[] SetPDF417DotSize { get; }
        byte[] SetPDF417CorrectionLevel { get; }
        byte[] StorePDF417Data { get; }
        byte[] PrintPDF417 { get; }

        byte[] SelectQRCodeModel { get; }
        byte[] SetQRCodeDotSize { get; }
        byte[] SetQRCodeCorrectionLevel { get; }
        byte[] StoreQRCodeData { get; }
        byte[] PrintQRCode { get; }

        /* Characters */
        byte StyleMode { get; }
        byte Alignment { get; }
        byte ReversePrintMode { get; }
        byte RightCharacterSpacing { get; }
        byte UpsideDownMode { get; }
        byte CodePage { get; }

        /* Command */
        byte ESC { get; }
        byte GS { get; }

        /* Display */
        byte CLR { get; }

        /* Functions */
        byte PaperCutFullCut { get; }
        byte PaperCutFullCutWithFeed { get; }
        byte PaperCutPartialCut { get; }
        byte PaperCutPartialCutWithFeed { get; }

        /* Images */
        byte ImageCmdParen { get; }
        byte ImageCmdLegacy { get; }
        byte ImageCmd8 { get; }
        byte ImageCmdL { get; }

        /* Operational */
        byte Initialize { get; }
        byte EnableDisable { get; }
        byte PaperCut { get; }
        byte CashDrawerPulse { get; }

        /* Status */
        byte AutomaticStatusBack { get; }
        byte AutomaticInkStatusBack { get; }

        /* Whitespace */
        byte Linefeed { get; }
        byte FeedLines { get; }
        byte FeedLinesReverse { get; }
        byte FeedDots { get; }
        byte ResetLineSpacing { get; }
        byte LineSpacingInDots { get; }
    }
}