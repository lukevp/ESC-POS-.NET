namespace ESCP_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter
    {
        public byte[] InitializePrinter()
        {
            return new[] {AsciiTable.ESC, AsciiTable.AT};
        }

        public byte[] CancelLine()
        {
            return new[] {AsciiTable.CAN};
        }

        public byte[] DeleteLastCharacter()
        {
            return new[] {AsciiTable.DEL};
        }

        public byte[] SelectPrinter()
        {
            return new[] {AsciiTable.DC1};
        }

        public byte[] DeselectPrinter()
        {
            return new[] {AsciiTable.DC3};
        }

        public byte[] CancelMsbControl()
        {
            return new[] {AsciiTable.ESC, AsciiTable.HASH};
        }

        public byte[] SetMsbTo0()
        {
            return new[] {AsciiTable.ESC, AsciiTable.EQUAL};
        }

        public byte[] SetMsbTo1()
        {
            return new[] {AsciiTable.ESC, AsciiTable.GREATER};
        }
    }
}