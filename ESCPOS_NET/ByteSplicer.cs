using System;

namespace ESCPOS_NET
{
    public static class ByteSplicer
    {
        public static byte[] Combine(params object[] byteArrays)
        {
            var builder = new ByteArrayBuilder();
            foreach (var byteArray in byteArrays)
            {
                if (!(byteArray is byte[])) throw new ArgumentException("All passed in objects must be byte arrays.");

                builder.Append((byte[]) byteArray);
            }

            return builder.ToArray();
        }
    }
}