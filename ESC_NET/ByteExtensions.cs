﻿namespace ESC_NET
{
    public static class ByteExtensions
    {
        public static bool IsBitSet(this byte b, int offset)
        {
            return (b & (1 << offset)) != 0;
        }

        public static bool IsBitNotSet(this byte b, int offset)
        {
            return (b & (1 << offset)) == 0;
        }
    }
}