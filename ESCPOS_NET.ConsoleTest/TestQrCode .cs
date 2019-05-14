using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[] QrCode(ICommandEmitter e) =>
            ByteSplicer.Combine(
                e.PrintLine("QrCode size 5"),
                e.PrintQrcode(@"http://www.sefazexemplo.gov.br/nfce/qrcode?p=28170800156225000131650110000151349562040824|2|1|02|60.90|797a4759685578312f5859597a6b7357422b6650523351633530633d|1|4615A93BB0D7C4E780F8D30EE77EDD5BA55C7D66", 5),

                e.CenterAlign(),
                e.PrintLine("QrCode size 4"),
                e.PrintQrcode(@"https://github.com/lukevp/ESC-POS-.NET", 4),

                e.RightAlign(),
                e.PrintLine("QrCode size 3"),
                e.PrintQrcode(@"https://www.nuget.org/packages/ESCPOS_NET/", 3)
             );
    }
}
