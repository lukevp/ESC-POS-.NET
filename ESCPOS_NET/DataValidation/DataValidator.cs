using ESCPOS_NET.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.DataValidation
{
    public static class DataValidator
    {
        private static Dictionary<BarcodeType, DataConstraint> Constraints = new Dictionary<BarcodeType, DataConstraint>()
        {
            { BarcodeType.UPC_A, new DataConstraint() { MinLength = 11, MaxLength = 12, ValidChars = "0123456789" } },
            { BarcodeType.UPC_E, new DataConstraint() { ValidLengths = new List<int>() { 6, 7, 8, 11, 12 }, ValidChars = "0123456789" } },
            { BarcodeType.JAN13_EAN13, new DataConstraint() { MinLength = 12, MaxLength = 13, ValidChars = "0123456789" } },
            { BarcodeType.JAN8_EAN8, new DataConstraint() { MinLength = 7, MaxLength = 8, ValidChars = "0123456789" } },
            { BarcodeType.CODE39, new DataConstraint() { MinLength = 1, MaxLength = 255, ValidChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./" } },
            { BarcodeType.ITF, new DataConstraint() { MinLength = 2, MaxLength = 255, ValidChars = "0123456789" } },
            { BarcodeType.CODABAR_NW_7, new DataConstraint() { MinLength = 2, MaxLength = 255, ValidChars = "0123456789ABCDabcd$+-./:" } },
            { BarcodeType.CODE93, new DataConstraint() { MinLength = 1, MaxLength = 255, ValidChars = "7BIT-ASCII" } },
            { BarcodeType.CODE128, new DataConstraint() { MinLength = 1, MaxLength = 253, ValidChars = "7BIT-ASCII" } },
            { BarcodeType.GS1_128, new DataConstraint() { MinLength = 1, MaxLength = 253, ValidChars = "7BIT-ASCII" } },
            { BarcodeType.GS1_DATABAR_OMNIDIRECTIONAL, new DataConstraint() { MinLength = 13, MaxLength = 13, ValidChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./" } },
            { BarcodeType.GS1_DATABAR_TRUNCATED, new DataConstraint() { MinLength = 13, MaxLength = 13, ValidChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./" } },
            { BarcodeType.GS1_DATABAR_LIMITED, new DataConstraint() { MinLength = 13, MaxLength = 13, ValidChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./" } },
            { BarcodeType.GS1_DATABAR_EXPANDED, new DataConstraint() { MinLength = 2, MaxLength = 255, ValidChars = "0123456789ABCDabcd !\"%$'()*+,-./:;<=>?_{" } },
        };

        public static void ValidateBarcode(BarcodeType type, string barcode)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            // Validate constraints on barcode.
            Constraints.TryGetValue(type, out var constraints);
            if (constraints != null)
            {
                // Check lengths
                if (constraints.ValidLengths != null)
                {
                    if (!constraints.ValidLengths.Contains(barcode.Length))
                    {
                        throw new ArgumentException($"Barcode {barcode} is not a valid length from: [{string.Join(", ", constraints.ValidLengths)}].");
                    }
                }
                else if (barcode.Length < constraints.MinLength)
                {
                    throw new ArgumentException($"Barcode {barcode} is shorter than minimum length {constraints.MinLength}.");
                }
                else if (barcode.Length > constraints.MaxLength)
                {
                    throw new ArgumentException($"Barcode {barcode} is longer than maximum length {constraints.MaxLength}.");
                }

                // Check if barcode contains invalid characters.

                if (constraints.ValidChars == "7BIT-ASCII")
                {
                    if (!barcode.All(x => x <= 127 && x >= 0))
                    {
                        throw new ArgumentException($"Barcode {barcode} contained invalid characters not in: {constraints.ValidChars}.");
                    }
                }
                else if (!barcode.All(x => constraints.ValidChars.Contains(x)))
                {
                    throw new ArgumentException($"Barcode {barcode} contained invalid characters not in: {constraints.ValidChars}.");
                }

                if (type == BarcodeType.UPC_E)
                {
                    if (barcode.Length != 6 && !barcode.StartsWith("0", StringComparison.InvariantCulture))
                    {
                        throw new ArgumentException($"UPC_E Barcode {barcode} with length of 7, 8, 11, or 12 must start with 0.");
                    }
                }
                else if (type == BarcodeType.ITF)
                {
                    if (barcode.Length % 2 != 0)
                    {
                        throw new ArgumentException($"ITF Barcode {barcode} has length {barcode.Length}, which is not an even number.");
                    }
                }
                else if (type == BarcodeType.CODE39)
                {
                    if (!barcode.StartsWith("*", StringComparison.InvariantCulture) || !barcode.EndsWith("*", StringComparison.InvariantCulture))
                    {
                        throw new ArgumentException($"CODE39 Barcode {barcode} must start and end with * characters.");
                    }
                }
                else if (type == BarcodeType.CODABAR_NW_7)
                {
                    if (!"ABCD".Contains(barcode[0]) || !"ABCD".Contains(barcode[barcode.Length - 1]))
                    {
                        throw new ArgumentException($"CODABAR_NW_7 Barcode {barcode} must start and end with an ABCD character.");
                    }
                    if (barcode.Skip(1).Take(barcode.Length - 2).Any(x => "ABCD".Contains(x)))
                    {
                        throw new ArgumentException($"CODABAR_NW_7 Barcode {barcode} must not include ABCD characters in the body of the barcode.");
                    }
                }
            }
        }
    }
}
