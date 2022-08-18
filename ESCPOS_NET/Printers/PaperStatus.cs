using System;
namespace ESCPOS_NET.Printers
{
    public class PaperStatus
    {
        public bool IsPaperLow { get; set; }

        public bool IsPaperOut { get; set; }
    }
}
