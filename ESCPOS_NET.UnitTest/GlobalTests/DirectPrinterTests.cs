using ESCPOS_NET.Emitters;
using ESCPOS_NET.Printers;
using System;
using System.Reflection;
using Xunit;

namespace ESCPOS_NET.UnitTest.GlobalTests
{
    public class DirectPrinterTests
    {
        // When new emitters exist, add their class name to new InlineData
        [InlineData("EPSON")]
        [Theory]
        public void VerifyInternalBufferIsUpdated(string emitter)
        {
            var type = Assembly.LoadFrom("ESCPOS_NET").GetType($"ESCPOS_NET.Emitters.{emitter}", true);
            ICommandEmitter e = (ICommandEmitter)Activator.CreateInstance(type);
            
            //Use Print Command so Buffer is updated internally
            var directPrinter = new DirectPrinter(e, null);
            directPrinter.Print("ABC").Print("DEF");

            byte[] result = { 65, 66, 67, 68, 69, 70 };
            Assert.Equal(result, directPrinter.Buffer);
        }

        [Theory]
        [InlineData("EPSON")]
        public void VerifyInternalBufferIsCleared(string emitter)
        {
            var type = Assembly.LoadFrom("ESCPOS_NET").GetType($"ESCPOS_NET.Emitters.{emitter}", true);
            ICommandEmitter e = (ICommandEmitter)Activator.CreateInstance(type);

            //Use Print Command so Buffer is updated internally
            var directPrinter = new DirectPrinter(e, null);
            directPrinter.Print("ABC").Print("DEF");

            //Clear Buffer and adds different bytes
            directPrinter.ClearAll();
            directPrinter.Print("GHI").Print("JKL");

            byte[] result = { 71, 72, 73, 74, 75, 76 };
            Assert.Equal(result, directPrinter.Buffer);            
        }

        [Theory]
        [InlineData("EPSON")]
        public void VerifyQRCodeIsSameAsCallingFromEmitter(string emitter)
        {
            var type = Assembly.LoadFrom("ESCPOS_NET").GetType($"ESCPOS_NET.Emitters.{emitter}", true);
            ICommandEmitter e = (ICommandEmitter)Activator.CreateInstance(type);

            //Use Print Command so Buffer is updated internally
            var directPrinter = new DirectPrinter(e, null);
            directPrinter.PrintQRCode("ABC");

            byte[] expected = e.PrintQRCode("ABC");
            Assert.Equal(expected, directPrinter.Buffer);
        }
    }
}
