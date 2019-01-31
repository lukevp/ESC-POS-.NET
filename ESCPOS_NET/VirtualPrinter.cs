using System;

namespace ESCPOS_NET
{
    public class VirtualPrinter: BasePrinter
    {
        private CommandEmitter _emitter = new CommandEmitter();

        public byte[] Output { get; private set; }

        public int GetAttribute(PrinterAttribute printerAttribute)
        {
            throw new NotImplementedException();
        }

        public override void Print()
        {
            Output = _emitter.GetAllCommands();
        }
    }
}
