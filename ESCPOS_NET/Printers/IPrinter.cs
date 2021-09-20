using System;
using System.Threading.Tasks;

namespace ESCPOS_NET
{
    public interface IPrinter
    {
        PrinterStatusEventArgs GetStatus();
        /// <summary>
        /// Write byte array of array to the printer stream. This function discards the <see cref="WriteTimeout"/> and the <paramref name="byteArrays"/> parameter will still be written 
        /// to the printer stream when the connection restored <b>if this instance is disposed yet
        /// </summary>
        /// <param name="byteArrays">Array of byte array which to be flattened to the overloaded function.</param>
        /// <returns></returns>
        void Write(params byte[][] byteArrays);
        /// <summary>
        /// Write byte array of array to the printer stream. This function discards the <see cref="WriteTimeout"/> and the <paramref name="byteArrays"/> parameter will still be written 
        /// to the printer stream when the connection restored <b>if this instance is disposed yet
        /// </summary>
        /// <param name="bytes">Byte array to write to the printer stream.</param>
        /// <returns></returns>
        void Write(byte[] bytes);
        /// <summary>
        /// Write byte array of array to the printer stream.
        /// </summary>
        /// <param name="byteArrays">Array of byte array which to be flattened to the overloaded function.</param>
        /// <returns></returns>
        /// <remarks>
        /// await or Wait() this function to await the operation and it would properly capture the exception otherwise the exception will be swallowed.
        /// Not await nor Wait() this function would discard the <see cref="WriteTimeout"/> and the <paramref name="byteArrays"/> parameter will still be written 
        /// to the printer stream when the connection restored <b>if this instance is disposed yet</b>.
        /// </remarks>
        /// <exception cref="IOException">The <see cref="WriteTimeout"/> is reach or Attempt to write stream to the disconnected connection</exception>
        Task WriteAsync(params byte[][] byteArrays);
        /// <summary>
        /// Write byte array of array to the printer stream.
        /// </summary>
        /// <param name="bytes">Byte array to write to the printer stream.</param>
        /// <returns></returns>
        /// <remarks>
        /// await or Wait() this function to await the operation and it would properly capture the exception otherwise the exception will be swallowed.
        /// Not await nor Wait() this function would discard the <see cref="WriteTimeout"/> and the <paramref name="bytes"/> parameter will still be written 
        /// to the printer stream when the connection restored <b>if this instance is disposed yet</b>.
        /// </remarks>
        /// <exception cref="IOException">The <see cref="WriteTimeout"/> is reach or Attempt to write stream to the disconnected connection</exception>
        Task WriteAsync(byte[] bytes);

        event EventHandler StatusChanged;
        event EventHandler Disconnected;
        event EventHandler Connected;
        //event EventHandler WriteFailed;
        //event EventHandler Idle;
        //event EventHandler IdleDisconnected; is this useful? to know that it disconnected because of idle? probably better to have this as info in disconnected event object instead.
    }
}