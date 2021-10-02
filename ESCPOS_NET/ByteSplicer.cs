namespace ESCPOS_NET.Utilities
{
    public static class ByteSplicer
    {
        public static byte[] Combine(params byte[][] byteArrays)
        {
            ByteArrayBuilder builder = new ByteArrayBuilder();
            foreach (var byteArray in byteArrays)
            {
                // For easier usage, ignore null byte arrays
                if (byteArray != null)
                    builder.Append(byteArray);
            }
            return builder.ToArray();
        }
    }
}
