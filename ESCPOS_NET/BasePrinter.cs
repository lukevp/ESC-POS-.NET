using System.Collections.Generic;

namespace ESCPOS_NET
{
    public abstract class BasePrinter : IPrinter
    {
        public BasePrinter()
        {
            // TODO: set all active attributes to their defaults.
        }

        private Dictionary<PrinterAttribute, int> _activeAttributes = new Dictionary<PrinterAttribute, int>();

        public abstract void Print();

        public void SetAttribute(PrinterAttribute printerAttribute, int value)
        {
            _activeAttributes[printerAttribute] = value;
            // TODO: emit command.
            //_emitter.Attribute(printerAttribute, value);
        }
        public int GetAttribute(PrinterAttribute printerAttribute)
        {
            _activeAttributes.TryGetValue(printerAttribute, out int value);
            return value;
        }
    }
}
