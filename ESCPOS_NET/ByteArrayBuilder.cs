using System;
using System.IO;
/* FROM: https://www.codeproject.com/Tips/674256/ByteArrayBuilder-a-StringBuilder-for-Bytes 2/9/2019 */
namespace UtilityClasses
    {
    /// <summary>
    /// Provides similar functionality to a StringBuilder, but for bytes
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
    /// </example>
    public class ByteArrayBuilder : IDisposable
        {
        #region Constants
        /// <summary>
        /// True in a byte form of the Line
        /// </summary>
        const byte streamTrue = (byte)1;
        /// <summary>
        /// False in the byte form of a line
        /// </summary>
        const byte streamFalse = (byte)0;
        #endregion

        #region Fields
        #region Internal
        /// <summary>
        /// Holds the actual bytes.
        /// </summary>
        MemoryStream store = new MemoryStream();
        #endregion

        #region Property bases
        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Bytes in the store.
        /// </summary>
        public int Length
            {
            get { return (int)store.Length; }
            }
        #endregion

        #region Regular Expressions
        #endregion

        #region Enums
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new, empty builder ready to be filled.
        /// </summary>
        public ByteArrayBuilder()
            {
            }
        /// <summary>
        /// Create a new builder from a set of data
        /// </summary>
        /// <param name="data">Data to preset the builder from</param>
        public ByteArrayBuilder(byte[] data)
            {
            store.Close();
            store.Dispose();
            store = new MemoryStream(data);
            }
        /// <summary>
        /// Create a new builder from the Base64 string representation of an 
        /// existing instance.
        /// The Base64 representation can be retrieved using the ToString override
        /// </summary>
        /// <param name="base64">Base64 string representation of an 
        /// existing instance.</param>
        public ByteArrayBuilder(string base64)
            {
            store.Close();
            store.Dispose();
            store = new MemoryStream(Convert.FromBase64String(base64));
            }
        #endregion

        #region Events
        #region Event Constructors
        #endregion

        #region Event Handlers
        #endregion
        #endregion

        #region Public Methods
        #region Append overloads
        /// <summary>
        /// Adds a bool to an array
        /// </summary>
        /// <param name="b">Value to append to existing builder data</param>
        public void Append(bool b)
            {
            store.WriteByte(b ? streamTrue : streamFalse);
            }
        /// <summary>
        /// Adds a byte to an array
        /// </summary>
        /// <param name="b">Value to append to existing builder data</param>
        public void Append(byte b)
            {
            store.WriteByte(b);
            }
        /// <summary>
        /// Adds an array of bytes to an array
        /// </summary>
        /// <param name="b">Value to append to existing builder data</param>
        /// <param name="addLength">
        /// If true, the length is added before the value.
        /// This allows extraction of individual elements back to the original input form.
        /// </param>
        public void Append(byte[] b, bool addLength = true)
            {
            if (addLength) Append(b.Length);
            AddBytes(b);
            }
        /// <summary>
        /// Adds a char to an array
        /// </summary>
        /// <param name="c">Value to append to existing builder data</param>
        public void Append(char c)
            {
            store.WriteByte((byte)c);
            }
        /// <summary>
        /// Adds an array of characters to an array
        /// </summary>
        /// <param name="c">Value to append to existing builder data</param>
        /// <param name="addLength">
        /// If true, the length is added before the value.
        /// This allows extraction of individual elements back to the original input form.
        /// </param>
        public void Append(char[] c, bool addLength = true)
            {
            if (addLength) Append(c.Length);
            Append(System.Text.Encoding.Unicode.GetBytes(c));
            }
        /// <summary>
        /// Adds a DateTime to an array
        /// </summary>
        /// <param name="dt">Value to append to existing builder data</param>
        public void Append(DateTime dt)
            {
            Append(dt.Ticks);
            }
        /// <summary>
        /// Adds a decimal value to an array
        /// </summary>
        /// <param name="d">Value to append to existing builder data</param>
        public void Append(decimal d)
            {
            // GetBits always returns four ints.
            // We store them in a specific order so that they can be recovered later.
            int[] bits = decimal.GetBits(d);
            Append(bits[0]);
            Append(bits[1]);
            Append(bits[2]);
            Append(bits[3]);
            }
        /// <summary>
        /// Adds a double to an array
        /// </summary>
        /// <param name="d">Value to append to existing builder data</param>
        public void Append(double d)
            {
            AddBytes(BitConverter.GetBytes(d));
            }
        /// <summary>
        /// Adds a float to an array
        /// </summary>
        /// <param name="f">Value to append to existing builder data</param>
        public void Append(float f)
            {
            AddBytes(BitConverter.GetBytes(f));
            }
        /// <summary>
        /// Adds a Guid to an array
        /// </summary>
        /// <param name="g">Value to append to existing builder data</param>
        public void Append(Guid g)
            {
            Append(g.ToByteArray());
            }
        /// <summary>
        /// Adds an integer to an array
        /// </summary>
        /// <param name="i">Value to append to existing builder data</param>
        public void Append(int i)
            {
            AddBytes(BitConverter.GetBytes(i));
            }
        /// <summary>
        /// Adds a long integer to an array
        /// </summary>
        /// <param name="l">Value to append to existing builder data</param>
        public void Append(long l)
            {
            AddBytes(BitConverter.GetBytes(l));
            }
        /// <summary>
        /// Adds a short integer to an array
        /// </summary>
        /// <param name="i">Value to append to existing builder data</param>
        public void Append(short i)
            {
            AddBytes(BitConverter.GetBytes(i));
            }
        /// <summary>
        /// Adds a string to an array
        /// </summary>
        /// <param name="s">Value to append to existing builder data</param>
        /// <param name="addLength">
        /// If true, the length is added before the value.
        /// This allows extraction of individual elements back to the original input form.
        /// </param>
        public void Append(string s, bool addLength = true)
            {
            byte[] data = System.Text.Encoding.Unicode.GetBytes(s);
            if (addLength) Append(data.Length);
            AddBytes(data);
            }
        /// <summary>
        /// Adds an unsigned integer to an array
        /// </summary>
        /// <param name="ui">Value to append to existing builder data</param>
        public void Append(uint ui)
            {
            AddBytes(BitConverter.GetBytes(ui));
            }
        /// <summary>
        /// Adds a unsigned long integer to an array
        /// </summary>
        /// <param name="ul">Value to append to existing builder data</param>
        public void Append(ulong ul)
            {
            AddBytes(BitConverter.GetBytes(ul));
            }
        /// <summary>
        /// Adds a unsigned short integer to an array
        /// </summary>
        /// <param name="us">Value to append to existing builder data</param>
        public void Append(ushort us)
            {
            AddBytes(BitConverter.GetBytes(us));
            }
        #endregion

        #region Extraction
        /// <summary>
        /// Gets a bool from an array
        /// </summary>
        /// <returns></returns>
        public bool GetBool()
            {
            return store.ReadByte() == streamTrue;
            }
        /// <summary>
        /// Gets a byte from an array
        /// </summary>
        /// <returns></returns>
        public byte GetByte()
            {
            return (byte)store.ReadByte();
            }
        /// <summary>
        /// Gets an array of bytes from an array
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteArray()
            {
            int length = GetInt();
            return GetBytes(length);
            }
        /// <summary>
        /// Gets a char from an array
        /// </summary>
        /// <returns></returns>
        public char GetChar()
            {
            return (char)store.ReadByte();
            }
        /// <summary>
        /// Gets an array of characters from an array
        /// </summary>
        /// <returns></returns>
        public char[] GetCharArray()
            {
            int length = GetInt();
            return System.Text.Encoding.Unicode.GetChars(GetBytes(length));
            }
        /// <summary>
        /// Gets a DateTime value from an array
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTime()
            {
            return new DateTime(GetLong());
            }
        /// <summary>
        /// Gets a decimal value from an array
        /// </summary>
        /// <returns></returns>
        public decimal GetDecimal()
            {
            // GetBits always returns four ints.
            // We store them in a specific order so that they can be recovered later.
            int[] bits = new int[] { GetInt(), GetInt(), GetInt(), GetInt() };
            return new decimal(bits);
            }
        /// <summary>
        /// Gets a double from an array
        /// </summary>
        /// <returns></returns>
        public double GetDouble()
            {
            return BitConverter.ToDouble(GetBytes(8), 0);
            }
        /// <summary>
        /// Gets a float from an array
        /// </summary>
        /// <returns></returns>
        public float GetFloat()
            {
            return BitConverter.ToSingle(GetBytes(4), 0);
            }
        /// <summary>
        /// Gets a Guid from an array
        /// </summary>
        /// <returns></returns>
        public Guid GetGuid()
            {
            return new Guid(GetByteArray());
            }
        /// <summary>
        /// Gets an integer from an array
        /// </summary>
        /// <returns></returns>
        public int GetInt()
            {
            return BitConverter.ToInt32(GetBytes(4), 0);
            }
        /// <summary>
        /// Gets a long integer from an array
        /// </summary>
        /// <returns></returns>
        public long GetLong()
            {
            return BitConverter.ToInt64(GetBytes(8), 0);
            }
        /// <summary>
        /// Gets a short integer from an array
        /// </summary>
        /// <returns></returns>
        public short GetShort()
            {
            return BitConverter.ToInt16(GetBytes(2), 0);
            }
        /// <summary>
        /// Gets a string from an array
        /// </summary>
        /// <returns></returns>
        public string GetString()
            {
            int length = GetInt();
            return System.Text.Encoding.Unicode.GetString(GetBytes(length));
            }
        /// <summary>
        /// Gets an unsigned integer from an array
        /// </summary>
        /// <returns></returns>
        public uint GetUint()
            {
            return BitConverter.ToUInt32(GetBytes(4), 0);
            }
        /// <summary>
        /// Gets a unsigned long integer from an array
        /// </summary>
        /// <returns></returns>
        public ulong GetUlong()
            {
            return BitConverter.ToUInt64(GetBytes(8), 0);
            }
        /// <summary>
        /// Gets a unsigned short integer from an array
        /// </summary>
        /// <returns></returns>
        public ushort GetUshort()
            {
            return BitConverter.ToUInt16(GetBytes(2), 0);
            }
        #endregion

        #region Interaction
        /// <summary>
        /// Clear all content from the builder
        /// </summary>
        public void Clear()
            {
            store.Close();
            store.Dispose();
            store = new MemoryStream();
            }
        /// <summary>
        /// Rewind the builder ready to read data
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
        /// <param name="position"></param>
        public void Seek(int position)
            {
            store.Seek((long)position, SeekOrigin.Begin);
            }
        /// <summary>
        /// Returns the builder as an array of bytes
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
            {
            byte[] data = new byte[Length];
            Array.Copy(store.GetBuffer(), data, Length);
            return data;
            }
        #endregion
        #endregion

        #region Overrides
        /// <summary>
        /// Returns a text based (Base64) string version of the current content
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            {
            return Convert.ToBase64String(ToArray());
            }
        #endregion

        #region Private Methods
        /// <summary>
        /// Add a string of raw bytes to the store
        /// </summary>
        /// <param name="b"></param>
        private void AddBytes(byte[] b)
            {
            store.Write(b, 0, b.Length);
            }
        /// <summary>
        /// Reads a specific number of bytes from the store
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
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
        #endregion

        #region IDisposable Implememntation
        /// <summary>
        /// Dispose of this builder and it's resources
        /// </summary>
        public void Dispose()
            {
            store.Close();
            store.Dispose();
            }
        #endregion  
        }
    }
