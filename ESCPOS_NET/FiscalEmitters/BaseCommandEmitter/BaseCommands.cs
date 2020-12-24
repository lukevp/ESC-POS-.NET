using ESCPOS_NET.FiscalEmitters.BaseCommandValues;
using ESCPOS_NET.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.FiscalEmitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        const int H20 = 32;
        const int H30 = 48;
        const int FIXED_LEN = 4;

        int seq = 0;

        private int Seq
        {
            get
            {
                if (seq == 0)
                {
                    Random rnd = new Random();
                    seq = rnd.Next(H20, 255);
                }

                return seq;
            }

            set { seq = value < 255 ? value : H20; }
        }

        public byte[] CmdWrapper(byte cmdCode, IEnumerable<byte> data)
            => CmdWrapper(cmdCode, data.ToArray());

        public byte[] CmdWrapper(byte cmdCode, byte[] data)
        {
            // <Preamble><LEN><SEQ><CMD><DATA><Post-amble><BCC><Terminator>
            var midbytes = ByteSplicer.Combine(new byte[] { (byte)(FIXED_LEN + data.Length + H20), (byte)Seq++, cmdCode }, data, new byte[] { Cmd.Postamble });
            return ByteSplicer.Combine(new byte[] { Cmd.Preamble }, midbytes, CalcSum(midbytes).ToArray(), new byte[] { Cmd.Terminator });
        }

        private IEnumerable<byte> CalcSum(byte[] bytes)
        {
            byte sum = 0;
            foreach (var bt in bytes)
            {
                sum += bt;
            }

            string strSum = sum.ToString("X4");
            for (int i = 0; i < 4; i++)
            {
                var bt = Convert.ToByte(strSum[i].ToString(), 16);
                bt += H30;
                yield return bt;
            }
        }
    }
}
