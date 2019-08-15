using System;
using System.Collections.Generic;
using System.Text;

namespace ESCPOS_NET.Emitters
{
   
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        public byte[] PrintQrCode(string qrdata, int qrsize, int qrmodel, int erc)
        {
            Encoding m_encoding = Encoding.GetEncoding("iso-8859-1");
            string qrprint = "";
            qrprint += m_encoding.GetString(SetErrorCorrection(erc));
            qrprint += m_encoding.GetString(SetSize(qrsize));
            qrprint += m_encoding.GetString(SetModel(qrmodel, 0));
            qrprint += m_encoding.GetString(StoreQrCode(qrdata));
            qrprint += qrdata;
            qrprint += m_encoding.GetString(new byte[] { Cmd.GS, QrCode.cmdqrparen, QrCode.cmdqrk, 03, 00, 0x31, 0x51, 48 });
            byte[] data = Encoding.ASCII.GetBytes(qrprint);
            return data;
        }

       
        public byte[] StoreQrCode(string qrdata)    //function180
        {
            int storelen = qrdata.Length + 3;
            byte store_pL = (byte)(storelen % 256);
            byte store_pH = (byte)(storelen / 256);
            var qrstore = new byte[] { Cmd.GS, QrCode.cmdqrparen, QrCode.cmdqrk, store_pL, store_pH, 0x31, 0x50, 0x30 };
            return qrstore;

        }
        public byte[] SetErrorCorrection(int erc) => new byte[] { Cmd.GS, QrCode.cmdqrparen, QrCode.cmdqrk, 03, 00, 0x31, 0x45, (byte)erc };  //function169
        public byte[] SetSize(int size) => new byte[] { Cmd.GS, QrCode.cmdqrparen, QrCode.cmdqrk, 03, 00, 0x31, 0x43, (byte)size };   //function167
        public byte[] SetModel(int model, int n2) => new byte[] { Cmd.GS, QrCode.cmdqrparen, QrCode.cmdqrk, 04, 00, 0x31, 0x41, (byte)model, (byte)n2 };   //function165




    }
}
