using ESCPOS_NET.Extensions;
using System;
using System.Linq;

namespace ESCPOS_NET.FiscalEmitters
{
    public class DATECS : BaseCommandEmitter
    {
        public DATECS()
        {
            this.DataValidator = new DATECSValidator();
        }
    }

    public class DATECSValidator : IDataValidator
    {
        /// <summary>
        /// Registers an item in the opened fiscal receipt
        /// </summary>
        /// <param name="itemDescription">Can be a 2-line description with 36 chars each. If longer, it will be truncated. All line breaks will be removed except the first one.</param>
        /// <param name="taxCode">Single letter that represents the tax code of the item.</param>
        /// <param name="price">Must be between 0.01 and 99999999.99 .</param>
        /// <param name="quantity">1.000 if not informed. Max of 3 decimal places. Quantity times Price must result in a number from 0.01 to 99999999.99.</param>
        /// <param name="percDiscount">Must be between -99.00 and 99.00 .</param>
        /// <param name="surcharge">Must be between 0.01 and 99999999.99 and won't be used if discount is also informed.</param>
        public void ValidateItemRegistration(ref string itemDescription, char taxCode, decimal price, decimal quantity = 1, decimal percDiscount = 0, decimal surcharge = 0)
        {
            if (!char.IsLetter(taxCode))
            {
                throw new ArgumentException("Tax Code must be a letter", nameof(taxCode));
            }

            if (price < 0.01m || price > 99999999.99m)
            {
                throw new ArgumentException("Price must be between 0.01 and 99999999.99", nameof(price));
            }

            decimal aux = quantity * 1000;
            if (aux - Math.Truncate(aux) > 0)
            {
                throw new ArgumentException("Quantity can't have more than 3 decimal places", nameof(quantity));
            }

            var total = quantity * price;
            if (total > 99999999.99m || total < 0.01m)
            {
                throw new ArgumentException("Quantity * price must be between 0.01 and 99999999.99", nameof(quantity));
            }

            if (percDiscount > 99m || percDiscount < -99m)
            {
                throw new ArgumentException("Discount must be between -99.00% and 99.00%", nameof(percDiscount));
            }

            if (surcharge < 0m || surcharge > 99999999.99m)
            {
                throw new ArgumentException("Surcharge must be between 0.00 and 99999999.99", nameof(surcharge));
            }

            itemDescription = SanitizeDescription(itemDescription);
        }

        private string SanitizeDescription(string description)
        {
            int firstLineBreak = description.IndexOfAny(new char[] { '\n', '\r' });
            if (firstLineBreak < 0)
            {
                return description.Truncate(36);
            }

            var first = description.Substring(0, firstLineBreak).Truncate(36);
            var second = description.Substring(firstLineBreak).RemoveAll('\n', '\r').Truncate(36);
            return $"{first}\n{second}";
        }

        public void ValidateReceiptOpening(int opCode = 1, string opPwd = "000000", int posNumber = 0)
        {
            if (opCode > 16 || opCode < 1)
            {
                throw new ArgumentException("Operator's Code must be from 1 to 16 only", nameof(opCode));
            }

            if (!int.TryParse(opPwd, out _))
            {
                throw new ArgumentException("Operator's password must be numbers only", nameof(opPwd));
            }

            if (opPwd.Length < 4 || opPwd.Length > 8)
            {
                throw new ArgumentException("Operator's password length must be between 4 and 8 digits", nameof(opPwd));
            }

            if (posNumber > 99999 || posNumber < 0)
            {
                throw new ArgumentException("Point of Sale number must be from 0 to 99999 only", nameof(posNumber));
            }
        }

        public void ValidateTotalRegistration(ref string totalDescription, char? paidMode, decimal? amount)
        {
            char[] validModes = new char[] { 'P', 'N', 'C', 'D', 'I', 'J', 'K', 'L' };
            if (paidMode is char mode && !validModes.Contains(mode))
            {
                throw new ArgumentException($"Paid mode must be one of ({string.Join(",", validModes.Select(c => $"'{c}'"))})", nameof(paidMode));
            }

            if (amount is decimal a && (a < 0.01m || a > 999999999.99m))
            {
                throw new ArgumentException("Amount must be between 0.01 and 999999999.99", nameof(amount));
            }

            if (!string.IsNullOrEmpty(totalDescription))
            {
                totalDescription = SanitizeDescription(totalDescription);
            }
        }

        public void ValidateTextPrinting(ref string text)
            => text = text.Truncate(48);
    }
}
