using ESCPOS_NET.Extensions;
using ESCPOS_NET.FiscalEmitters.BaseCommandValues;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ESCPOS_NET.FiscalEmitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        public virtual byte[] OpenReceipt(int opCode = 1, string opPwd = "000000", int posNumber = 0)
        {
            this.DataValidator.ValidateReceiptOpening(opCode, opPwd, posNumber);
            opPwd = $",{opPwd},";
            var cmd = new List<byte>();
            cmd.AddRange((byte)opCode, opPwd.ToBytes(), (byte)posNumber);
            return CmdWrapper(Fiscal.OpenReceipt, cmd);
        }

        public virtual byte[] RegisterItem(string itemDescription, char taxCode, decimal price, decimal quantity = 1, decimal percDiscount = 0, decimal surcharge = 0)
        {
            this.DataValidator.ValidateItemRegistration(ref itemDescription, taxCode, price, quantity, percDiscount, surcharge);
            var cmd = new List<byte>();
            cmd.AddRange(
                            itemDescription.ToBytes(),
                            Cmd.Tab,
                            taxCode.ToString().ToBytes(),
                            price.ToString(CultureInfo.InvariantCulture).ToBytes()
                        );

            if (quantity != 1)
            {
                cmd.AddRange(Cmd.Times, quantity.ToString(CultureInfo.InvariantCulture));
            }

            if (percDiscount != 0)
            {
                cmd.AddRange($",{percDiscount.ToString("f2", CultureInfo.InvariantCulture)}".ToBytes());
            }
            else if (surcharge != 0)
            {
                cmd.AddRange($":{surcharge.ToString("f2", CultureInfo.InvariantCulture)}".ToBytes());
            }

            return CmdWrapper(Fiscal.ItemRegistration, cmd);
        }

        public virtual byte[] RegisterTotal(string totalizationDescription, char? paidMode, decimal? amount)
        {
            this.DataValidator.ValidateTotalRegistration(ref totalizationDescription, paidMode, amount);
            var cmd = new List<byte>();
            if (!string.IsNullOrEmpty(totalizationDescription))
            {
                cmd.AddRange(totalizationDescription.ToBytes());
            }

            cmd.Add(Cmd.Tab);

            if (paidMode is char mode)
            {
                cmd.Add((byte)mode);
            }

            if (amount is decimal amt)
            {
                cmd.AddRange(amt.ToString("f2", CultureInfo.InvariantCulture).ToBytes());
            }

            return CmdWrapper(Fiscal.TotalRegistration, cmd);
        }

        public virtual byte[] PrintText(string text)
        {
            this.DataValidator.ValidateTextPrinting(ref text);
            return CmdWrapper(Fiscal.PrintText, text.ToBytes());
        }

        public virtual byte[] CloseReceipt()
        {
            return CmdWrapper(Fiscal.CloseReceipt, Cmd.Empty);
        }
    }
}
