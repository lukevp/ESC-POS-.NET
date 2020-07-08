using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/* FROM: https://www.codeproject.com/Tips/674256/ByteArrayBuilder-a-StringBuilder-for-Bytes 2/9/2019 */
namespace ESCPOS_NET.Utilities
{
    /// <summary>
    /// Provides similar functionality to a StringBuilder, but for bytes.
    /// </summary>
    /// <remarks>
    /// To fill the builder, construct a new, empty builder, and call the
    /// appropriate Append method overloads.
    /// To read data from the builder, either use Rewind on an existing
    /// builder, or construct a new builder by passing it the byte array
    /// from a previous builder - which you can get with the ToArray
    /// method.
    /// </remarks>
    /// <example>
    ///
    ///    ByteArrayBuilder bab = new ByteArrayBuilder();
    ///    string[] lines = File.ReadAllLines(@"D:\Temp\myText.txt");
    ///    bab.Append(lines.Length);
    ///    foreach (string s in lines)
    ///        {
    ///        bab.Append(s);
    ///        }
    ///    byte[] data = bab.ToArray();
    ///  ...
    ///    ByteArrayBuilder babOut = new ByteArrayBuilder(data);
    ///    int count = bab.GetInt();
    ///    string[] linesOut = new string[count];
    ///    for (int lineNo = 0; lineNo &lt; count; lineNo++)
    ///        {
    ///        linesOut[lineNo](babOut.GetString());
    ///        }
    ///     .
    /// </example>
    public class ByteArrayBuilder : IDisposable
    {
        /// <summary>
        /// True in a byte form of the Line.
        /// </summary>
        private const byte StreamTrue = (byte)1;

        /// <summary>
        /// False in the byte form of a line.
        /// </summary>
        private const byte StreamFalse = (byte)0;

        /// <summary>
        /// Holds the actual bytes.
        /// </summary>
        private MemoryStream store = new MemoryStream();

        /// <summary>
        /// Gets bytes in the store.
        /// </summary>
        public int Length => (int)store.Length;

        /// <summary>
        /// Create a new, empty builder ready to be filled.
        /// </summary>
        public ByteArrayBuilder()
        {
        }

        public ByteArrayBuilder Append(byte b)
        {
            AddBytes(new byte[] { b });
            return this;
        }

        /// <summary>
        /// Adds an IEnumerable of bytes to an array.
        /// </summary>
        /// <param name="b">Value to append to existing builder data.</param>
        /// </param>
        public ByteArrayBuilder Append(IEnumerable<byte> b)
        {
            if (b is byte[])
            {
                AddBytes((byte[])b);
                return this;
            }

            AddBytes(b.ToArray());
            return this;
        }

        /// <summary>
        /// Clear all content from the builder.
        /// </summary>
        public void Clear()
        {
            store.Close();
            store.Dispose();
            store = new MemoryStream();
        }

        /// <summary>
        /// Rewind the builder ready to read data.
        /// </summary>
        public void Rewind()
        {
            store.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Set an absolute position in the builder.
        /// **WARNING**
        /// If you add any variable size objects to the builder, the results of
        /// reading after a Seek to a non-zero value are unpredictable.
        /// A builder does not store just objects - for some it stores additional
        /// information as well.
        /// </summary>
        /// <param name="position">the position to seek.</param>
        public void Seek(int position)
        {
            store.Seek((long)position, SeekOrigin.Begin);
        }

        /// <summary>
        /// Returns the builder as an array of bytes.
        /// </summary>
        /// <returns>An array.</returns>
        public byte[] ToArray()
        {
            byte[] data = new byte[Length];
            Array.Copy(store.GetBuffer(), data, Length);
            return data;
        }

        /// <summary>
        /// Returns a text based (Base64) string version of the current content.
        /// </summary>
        /// <returns>The converted string.</returns>
        public override string ToString()
        {
            return Convert.ToBase64String(ToArray());
        }

        /// <summary>
        /// Add a string of raw bytes to the store.
        /// </summary>
        /// <param name="byteArray">the byte array.</param>
        private void AddBytes(byte[] byteArray)
        {
            store.Write(byteArray, 0, byteArray.Length);
        }

        /// <summary>
        /// Reads a specific number of bytes from the store.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>The byte array.</returns>
        private byte[] GetBytes(int length)
        {
            byte[] data = new byte[length];
            if (length > 0)
            {
                int read = store.Read(data, 0, length);
                if (read != length)
                {
                    throw new ApplicationException("Buffer did not contain " + length + " bytes");
                }
            }

            return data;
        }

        /// <summary>
        /// Dispose of this builder and its resources.
        /// </summary>
        public void Dispose()
        {
            store.Close();
            store.Dispose();
        }
    }
}