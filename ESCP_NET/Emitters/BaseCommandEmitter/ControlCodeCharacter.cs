namespace ESCP_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter
    {
        public byte[] EnablePrintingOfUpperControlCodes()
        {
            return new[] {AsciiTable.ESC, AsciiTable.SIX};
        }

        public byte[] EnableUpperControlCodes()
        {
            return new[] {AsciiTable.ESC, AsciiTable.SEVEN};
        }
    }
}