using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[] Images(ICommandEmitter e) =>
            ByteSplicer.Combine(
            e.CenterAlign(),
            e.PrintLine("Test PNG images with widths 100 - 600 px at native resolution"),
            e.PrintLine("-- pd-logo-100.png --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-100.png"), true),
            e.PrintLine("-- pd-logo-200.png --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-200.png"), true),
            e.PrintLine("-- pd-logo-300.png --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), true),
            e.PrintLine("-- pd-logo-400.png --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-400.png"), true),
            e.PrintLine("-- pd-logo-500.png --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-500.png"), true),
            e.PrintLine("-- pd-logo-600.png --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-600.png"), true),
            //e.PrintImage(File.ReadAllBytes("abe-lincoln.png"), true, 257),
            e.PrintLine("Test resizing 600 px image to 300px"),
            e.PrintLine("-- pd-logo-600.png /300 --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-600.png"), true, 300),
            e.PrintLine("Test image with taller height than width"),
            e.PrintLine("-- abe-lincoln.png /300 --"),
            e.PrintImage(File.ReadAllBytes("images/abe-lincoln.png"), true, 300),
            e.PrintLine("Test 300px image resized to all remainders of 8 to ensure overflow checking"),
            e.PrintLine("-- pd-logo-300.png /296 (0 remainder) --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), true, 296),
            e.PrintLine("-- pd-logo-300.png /297 (1 remainder) --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), true, 297),
            e.PrintLine("-- pd-logo-300.png /298 (2 remainder) --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), true, 298),
            e.PrintLine("-- pd-logo-300.png /299 (3 remainder) --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), true, 299),
            e.PrintLine("-- pd-logo-300.png /300 (4 remainder) --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), true, 300),
            e.PrintLine("-- pd-logo-300.png /301 (5 remainder) --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), true, 301),
            e.PrintLine("-- pd-logo-300.png /302 (6 remainder) --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), true, 302),
            e.PrintLine("-- pd-logo-300.png /303 (7 remainder) --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), true, 303),
            e.PrintLine("Test 300px image in JPG format"),
            e.PrintLine("-- pd-logo-300.jpg --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.jpg"), true),
            e.PrintLine("Test 300px image in BMP format"),
            e.PrintLine("-- pd-logo-300.jpg --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.bmp"), true),
            e.PrintLine("Test 300px image in GIF format"),
            e.PrintLine("-- pd-logo-300.jpg --"),
            e.PrintImage(File.ReadAllBytes("images/pd-logo-300.gif"), true),
            e.PrintLine("Test full color image converted to black and white"),
            e.PrintLine("-- kitten.jpg --"),
            e.PrintImage(File.ReadAllBytes("images/kitten.jpg"), true, 400)
        );
    }
}
