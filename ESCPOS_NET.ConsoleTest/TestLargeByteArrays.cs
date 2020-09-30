using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using System;
using System.IO;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[] TestLargeByteArrays(ICommandEmitter e)
        {
            var kitten = e.PrintImage(File.ReadAllBytes("images/kitten.jpg"), true, true, 500);
            var cube = e.PrintImage(File.ReadAllBytes("images/Portal_Companion_Cube.jpg"), true, true, 500);
            var expectedResult = ByteSplicer.Combine(
                e.CenterAlign(),
                kitten,
                cube,
                kitten,
                cube,
                kitten,
                cube,
                kitten,
                cube,
                kitten,
                cube,
                kitten,
                cube,
                kitten,
                cube,
                kitten,
                cube,
                kitten,
                cube,
                kitten,
                cube
            );
            MemoryPrinter mp = new MemoryPrinter();
            mp.Write(expectedResult);
            var response = mp.GetAllData();
            bool hasErrors = false;
            if (expectedResult.Length != response.Length)
            {
                Console.WriteLine($"Error: MemoryPrinter length mismatch - ${response.Length}, expected ${expectedResult.Length}");
                hasErrors = true;
            }
            else
            {
                for (int i = 0; i < expectedResult.Length; i++)
                {
                    if (expectedResult[i] != response[i])
                    {
                        Console.WriteLine($"Error: MemoryPrinter data mismatch - ${expectedResult[i]}, expected ${response[i]}, at location ${i}");
                        hasErrors = true;
                    }
                }
            }
            if (!hasErrors)
            {
                Console.WriteLine("MemoryPrinter: ALL OK!");
            }
            else
            {
                Console.WriteLine("MemoryPrinter: Errors occured during testing, aborting!");
                throw new ArgumentException();
            }

            Random r = new Random();
            var filename = $"{r.NextDouble().ToString()}.tmp";
            using (FilePrinter fp = new FilePrinter(filename, true))
            { 
                fp.Write(expectedResult);
            }
            response = File.ReadAllBytes(filename);
            hasErrors = false;
            if (expectedResult.Length != response.Length)
            {
                Console.WriteLine($"Error: FilePrinter length mismatch - ${response.Length}, expected ${expectedResult.Length}");
                hasErrors = true;
            }
            else
            {
                for (int i = 0; i < expectedResult.Length; i++)
                {
                    if (expectedResult[i] != response[i])
                    {
                        Console.WriteLine($"Error: FilePrinter data mismatch - ${expectedResult[i]}, expected ${response[i]}, at location ${i}");
                        hasErrors = true;
                    }
                }
            }

            if (!hasErrors)
            {
                Console.WriteLine("FilePrinter: ALL OK!");
            }
            else
            {
                Console.WriteLine("FilePrinter: Errors occured during testing, aborting!");
                throw new ArgumentException();
            }

            return expectedResult;
        }
    }
}
