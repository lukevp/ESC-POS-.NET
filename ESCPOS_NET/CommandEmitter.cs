using System;
using System.Collections.Generic;

namespace ESCPOS_NET
{
    public class CommandEmitter
    {
        List<byte> commands = new List<byte>();


        public byte[] GetAllCommands()
        {
            throw new NotImplementedException();
        }

        public void AddLines(string input)
        {
            // Replace CRLF with LF
            
        }

    }
}
