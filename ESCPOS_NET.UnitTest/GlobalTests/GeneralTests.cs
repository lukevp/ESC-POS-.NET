using ESCPOS_NET.Emitters;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace ESCPOS_NET.UnitTest.GlobalTests
{
    public class GeneralTests
    {
        private const string NO_NEW_LINE_TEXT = "Just a text";
        private const string CHAR10_CHAR13_TEXT = "Another \r\ntext";
        private const string CHAR13_TEXT = "Char\r13";
        private const string CHAR10_TEXT = "Char\n10";
        private const string MULTIPLE_CHAR10_CHAR13_LONG_TEXT = "ASKdjlaklsjdlkajsdklajsl kjdsalkjdslakj sdlkjalksdja skjdlkas\r\ndaskljaklsjdalksjdlakjsdlkj lkjas lkajdlkjalsjd \r\n ASLKJD ASJDLjlakjs KJASlkjsLKJS LJadslkjalkdjLAKJSDL Kj\r\n&^#@#(@*$&U SAJND AKSJ KJHSDK(#Y$(*U#@\r\n \\r\\n";
        private const string LONG_TEXT_WITHOUT_CHAR10 = "ASKdjlaklsjdlkajsdklajsl kjdsalkjdslakj sdlkjalksdja skjdlkas\ndaskljaklsjdalksjdlakjsdlkj lkjas lkajdlkjalsjd \n ASLKJD ASJDLjlakjs KJASlkjsLKJS LJadslkjalkdjLAKJSDL Kj\n&^#@#(@*$&U SAJND AKSJ KJHSDK(#Y$(*U#@\n \\r\\n";

        [Theory]
        [InlineData("EPSON", NO_NEW_LINE_TEXT)]
        [InlineData("EPSON", CHAR10_CHAR13_TEXT)]
        [InlineData("EPSON", CHAR13_TEXT)]
        [InlineData("EPSON", CHAR10_TEXT)]
        [InlineData("EPSON", MULTIPLE_CHAR10_CHAR13_LONG_TEXT)]
        [InlineData("EPSON", "")]

        public void PrintChar13Removal_Success(string emitter, string text)
        {
            var type = Assembly.LoadFrom("ESCPOS_NET").GetType($"ESCPOS_NET.Emitters.{emitter}", true);
            ICommandEmitter e = (ICommandEmitter)Activator.CreateInstance(type);

            var bytes = e.Print(text);

            Assert.True(!bytes.Any( b => b == 13), $"{nameof(e.Print)} didn't remove all \\r characters from the text.");
        }

        [Theory]
        [InlineData("EPSON", NO_NEW_LINE_TEXT, NO_NEW_LINE_TEXT)]
        [InlineData("EPSON", CHAR10_CHAR13_TEXT, "Another \ntext")]
        [InlineData("EPSON", CHAR13_TEXT, "Char\n13")]
        [InlineData("EPSON", CHAR10_TEXT, CHAR10_TEXT)]
        [InlineData("EPSON", MULTIPLE_CHAR10_CHAR13_LONG_TEXT, LONG_TEXT_WITHOUT_CHAR10)]
        [InlineData("EPSON", "", "")]

        public void PrintChar13Replacement_Success(string emitter, string text, string expectedResult)
        {
            var type = Assembly.LoadFrom("ESCPOS_NET").GetType($"ESCPOS_NET.Emitters.{emitter}", true);
            ICommandEmitter e = (ICommandEmitter)Activator.CreateInstance(type);

            var bytes = e.Print(text);
            Assert.Equal(expectedResult, Encoding.UTF8.GetString(bytes));
        }

        [Theory]
        [InlineData("EPSON")]
        public void PrintThrowIfTextIsNull(string emitter)
        {
            var type = Assembly.LoadFrom("ESCPOS_NET").GetType($"ESCPOS_NET.Emitters.{emitter}", true);
            ICommandEmitter e = (ICommandEmitter)Activator.CreateInstance(type);

            Assert.ThrowsAny<Exception>(() => e.Print(null));
        }

    }
}
