namespace ESCPOS_NET
{
    public interface IPrinter
    {
        void Print();
        void SetAttribute(PrinterAttribute printerAttribute, int value);
        int GetAttribute(PrinterAttribute printerAttribute);
    }
}
