<img src="https://raw.githubusercontent.com/lukevp/ESC-POS-.NET/master/banner2.jpg" />
<h1 align="center">ESCPOS.NET - Easy to use, Cross-Platform, Fast and Efficient.</h1>
<br />
<p align="center">
  <a href="https://raw.githubusercontent.com/lukevp/ESC-POS-.NET/master/LICENSE">
    <img src="https://img.shields.io/github/license/lukevp/ESC-POS-.NET" />
  </a>
  <a href="https://github.com/lukevp/ESC-POS-.NET/issues">
    <img src="https://img.shields.io/github/issues/lukevp/ESC-POS-.NET" /> 
    <img alt="GitHub closed issues" src="https://img.shields.io/github/issues-closed/lukevp/ESC-POS-.NET">
  </a>
  <a href="https://www.nuget.org/packages/ESCPOS_NET/">
    <img alt="Nuget" src="https://img.shields.io/nuget/dt/ESCPOS_NET?label=NuGet%20downloads">
    </a>
   <a href="https://github.com/lukevp/ESC-POS-.NET/graphs/contributors">
    <img alt="GitHub contributors" src="https://img.shields.io/github/contributors/lukevp/ESC-POS-.NET">
    </a>
</p>
ESCPOS.NET is a super easy to use library that supports the most common functionality of the ESC/POS standard by Epson.  It is highly compatible, and runs on full framework .NET as well as .NET Core.

It works with Serial, USB, Ethernet, and WiFi printers, and works great on Windows, Linux and OSX.

This library is used for thermal receipt printers, line displays, cash drawers, and more!

ESC/POS is a binary protocol that's a type of "raw" text, which means you do not need drivers to use it.  

This library encompasses helper functions that assist in creating the binary command stream that is needed to control this hardware, as well as the underlying communications that are needed to interface with the hardware.  

This means that Bluetooth, WiFi, Ethernet, USB, and Serial printers are all usable with just this software library and nothing else.

# Get Started

## Step 1: Create a Printer object
```csharp
// Ethernet or WiFi (This uses an Immediate Printer, no live paper status events, but is easier to use)
var hostnameOrIp = "192.168.1.50";
var port = 9100;
var printer = new ImmediateNetworkPrinter(new ImmediateNetworkPrinterSettings() { ConnectionString = $"{hostnameOrIp}:{port}", PrinterName = "TestPrinter" });

// USB, Bluetooth, or Serial
var printer = new SerialPrinter(portName: "COM5", baudRate: 115200);

// Linux output to USB / Serial file
var printer = new FilePrinter(filePath: "/dev/usb/lp0");

// Samba
var printer = new SambaPrinter(tempFileBasePath: @"C:\Temp", filePath: "\\computer\printer");
```
## Step 1a (optional): Monitor for Events - out of paper, cover open... 
```csharp
// Define a callback method.
static void StatusChanged(object sender, EventArgs ps)
{
    var status = (PrinterStatusEventArgs)ps;
    Console.WriteLine($"Status: {status.IsPrinterOnline}");
    Console.WriteLine($"Has Paper? {status.IsPaperOut}");
    Console.WriteLine($"Paper Running Low? {status.IsPaperLow}");
    Console.WriteLine($"Cash Drawer Open? {status.IsCashDrawerOpen}");
    Console.WriteLine($"Cover Open? {status.IsCoverOpen}");
}

... 

// In your program, register event handler to call the method when printer status changes:
printer.StatusChanged += StatusChanged;

// and start monitoring for changes.
printer.StartMonitoring();

```

## Step 2: Write a receipt to the printer
```csharp
var e = new EPSON();
printer.Write( // or, if using and immediate printer, use await printer.WriteAsync
  ByteSplicer.Combine(
    e.CenterAlign(),
    e.PrintImage(File.ReadAllBytes("images/pd-logo-300.png"), true),
    e.PrintLine(""),
    e.SetBarcodeHeightInDots(360),
    e.SetBarWidth(BarWidth.Default),
    e.SetBarLabelPosition(BarLabelPrintPosition.None),
    e.PrintBarcode(BarcodeType.ITF, "0123456789"),
    e.PrintLine(""),
    e.PrintLine("B&H PHOTO & VIDEO"),
    e.PrintLine("420 NINTH AVE."),
    e.PrintLine("NEW YORK, NY 10001"),
    e.PrintLine("(212) 502-6380 - (800)947-9975"),
    e.SetStyles(PrintStyle.Underline),
    e.PrintLine("www.bhphotovideo.com"),
    e.SetStyles(PrintStyle.None),
    e.PrintLine(""),
    e.LeftAlign(),
    e.PrintLine("Order: 123456789        Date: 02/01/19"),
    e.PrintLine(""),
    e.PrintLine(""),
    e.SetStyles(PrintStyle.FontB),
    e.PrintLine("1   TRITON LOW-NOISE IN-LINE MICROPHONE PREAMP"),
    e.PrintLine("    TRFETHEAD/FETHEAD                        89.95         89.95"),
    e.PrintLine("----------------------------------------------------------------"),
    e.RightAlign(),
    e.PrintLine("SUBTOTAL         89.95"),
    e.PrintLine("Total Order:         89.95"),
    e.PrintLine("Total Payment:         89.95"),
    e.PrintLine(""),
    e.LeftAlign(),
    e.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
    e.PrintLine("SOLD TO:                        SHIP TO:"),
    e.SetStyles(PrintStyle.FontB),
    e.PrintLine("  FIRSTN LASTNAME                 FIRSTN LASTNAME"),
    e.PrintLine("  123 FAKE ST.                    123 FAKE ST."),
    e.PrintLine("  DECATUR, IL 12345               DECATUR, IL 12345"),
    e.PrintLine("  (123)456-7890                   (123)456-7890"),
    e.PrintLine("  CUST: 87654321"),
    e.PrintLine(""),
    e.PrintLine("")
  )
);
```
## Step 3: Print special characters 
Before you proceed, please make sure you understand the CodePages your printer supports, and if necessary use the [CodePage](https://github.com/lukevp/ESC-POS-.NET/blob/7edaa3f0d9f7298ffe33517074fbc3622e4192a9/ESCPOS_NET/Emitters/BaseCommandEmitter/CharacterCommands.cs#L24) command to change it accordingly to the wanted [encoding](https://en.wikipedia.org/wiki/Character_encoding).

The problem folks usually run into is that C# strings are Unicode, so you are able to declare a string with, for example, the value `€` in C#, but unfortunately, that value does not correctly map to an ASCII 8-bit value that is printable using the standard 7-bit + extended codepages. See [here](https://www.compart.com/en/unicode/U+20AC) for more information

So below an example of how to print `€`
```cs
    // € is char 0xD5 at PC858_EURO CodePage
    var EURO = new byte[] { 0xD5 };
    printer.Write(ByteSplicer.Combine(e.CodePage(CodePage.PC858_EURO), EURO));

    //Optionally you can return to whatever default CodePage you had before. PC437 in this example
    printer.Write(e.CodePage(CodePage.PC437_USA_STANDARD_EUROPE_DEFAULT));
```
Refer to [this code](https://github.com/lukevp/ESC-POS-.NET/blob/7edaa3f0d9f7298ffe33517074fbc3622e4192a9/ESCPOS_NET.ConsoleTest/TestCodePages.cs#L6-L41) to test your current CodePage, and to [this post](https://github.com/lukevp/ESC-POS-.NET/issues/103#issuecomment-778874734) for a full explanation.

### Printing Chinese characters
Assuming your printer has its default CodePage to match [GBK Enconding](https://en.wikipedia.org/wiki/GBK_(character_encoding)) you could accomplish printing chinese characters in 2 ways:

#### 1 - Defining the Encoding in the Emitter
```cs
var e = new EPSON { Encoding = System.Text.Encoding.GetEncoding("GBK") };
string chineseCharactersString = "汉字";

printer.Write( 
  ByteSplicer.Combine(
    e.CenterAlign(),
    e.Print("------------------------------------------"),
    e.PrintLine(),
    e.Print(chineseCharactersString),
    e.PrintLine(),
    e.Print("------------------------------------------"),
    e.RightAlign(),
    e.PrintLine()
  )
);
```

#### 2 - Encoding all strings and directly using them in the Write() method
```cs
var encoding = System.Text.Encoding.GetEncoding("GBK");
var e = new EPSON();
string chineseCharactersString = "汉字";
printer.Write( 
  ByteSplicer.Combine(
    e.CenterAlign(),
    e.Print("------------------------------------------"),
    e.PrintLine(),
    encoding.GetBytes(chineseCharactersString),
    e.PrintLine(),
    e.Print("------------------------------------------"),
    e.RightAlign(),
    e.PrintLine()
  )
);
```
#### Important Note
If you are using this library with .NET and not .NET Framework, an [extra step](https://learn.microsoft.com/en-us/dotnet/api/system.text.codepagesencodingprovider.instance?view=net-8.0) might be needed before you pass on the Enconding instance to the library

![image](https://github.com/lukevp/ESC-POS-.NET/assets/10572656/01c9f3d3-ff83-4450-9178-0a3d2ea7eeaf)

This means you need to register the provider once with the line below before you instantiate the Encoding with `System.Text.Encoding.GetEncoding` method.
```cs
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
```

More details about .NET and Encoding [here](https://learn.microsoft.com/en-us/dotnet/standard/base-types/character-encoding).

# Supported Platforms
Desktop support (WiFI, Ethernet, Bluetooth, USB, Serial):
* Windows
  - Windows 7+ can support .NET Core or the .NET 471 runtime, and can use this library.
* Linux 
  - ARM platforms such as Raspberry Pi
  - x86/64 platform
* Mac OSX
  - Tested from High Sierra to Monterrey, both Intel and M1 architectures

Mobile support (WiFi/Ethernet only):
**ImmediateNetworkPrinter is the recommended integration type for mobile usage, since mobile applications can background your application at any time**
* Xamarin.Forms
* iOS
  - Xamarin.iOS
* Android
  - Xamarin.Android
* Windows
  - UWP

# Supported Hardware
Epson thermal receipt printers are supported, and most common functions such as test printing, styling, alignment, image printing, and barcode printing.

Generic thermal printers that implement ESC/POS typically work, for example the Royal PT-300, and BemaTech printers are also tested by some members of the community, @juliogamasso and @ivanmontilla.

Cash Drawers are supported, as are Line Displays.


## Further Documentation and Usage
Check out the ESCPOS_NET.ConsoleTest for a comprehensive test suite that covers all implemented functions.

This package is available on NuGet @ https://www.nuget.org/packages/ESCPOS_NET/

Please comment / DM / open issues and let me know how the library is working for you!

## Contributors
Thanks to all of our contributors working to make this the best .NET thermal printer library out there!! 

* [@lukevp](https://github.com/lukevp)
* [@juliogamasso](https://github.com/juliogamasso)
* [@naaeef](https://github.com/naaeef)
* [@netgg93](https://github.com/netgg93)
* [@igorocampos](https://github.com/igorocampos)
* [@kodejack](https://github.com/kodejack)
* [@hollandar](https://github.com/hollandar)
* [@nickcharlton](https://github.com/nickcharlton)

# USB Usage Guide

For cross-platform support and ease of maintenance, all USB printers are supported over Serial-USB interfaces.  These are full-speed and work just as well as native USB as long as you have your port settings optimized.

On Linux and Mac, USB for Epson printers is exposed as a serial port directly by the os under /dev/ttyusb or something similar based on your platform, and doesn't require drivers.  

On Windows, you must install some type of virtual COM port driver for native USB support, and then map your printer to a specific port, or use a USB-Serial cable and use a serial printer.

If you have an official Epson printer, the link to install it from Epson is here: https://download.epson-biz.com/modules/pos/index.php?page=single_soft&cid=6481&scat=36&pcat=5

If you do not have an official Epson printer, you will have to find a compatible way to expose the USB interface as a virtual Serial port.

NOTE: The cross platform .NET library we use from Microsoft only supports COM ports 8 and below on windows, so be sure not to use a very high # COM port.


# Implemented Commands

Most common commands are implemented natively in the library's included emitter.
## Bit Image Commands
* `ESC ✻` Select bit-image mode
* `GS ( L` OR `GS 8 L` Set graphics data
    * Set the reference dot density for graphics.
    * Print the graphics data in the print buffer.
    * Store the graphics data in the print buffer (raster format).

## Character Commands
* `ESC !` Select print mode(s)
* `GS B` Turn white/black reverse print mode on/off - Thanks @juliogamasso!

## Print Commands
* `LF` Print and line feed
* `CR` Print and carriage return
* `ESC J` Print and feed paper
* `ESC K` Print and reverse feed
* `ESC d` Print and feed n lines
* `ESC e` Print and reverse feed n lines

## Bar Code Commands
* `GS H` Select print position of HRI characters
* `GS f` Select font for HRI characters
* `GS h` Set bar code height
* `GS k` Print bar code
* `GS w` Set bar code width
* `GS ( k` Print 2D bar codes (QRCode and PDF417)

## Status Commands
* `GS a` Enable/disable Automatic Status Back (ASB)

## Open Cash Drawer Commands
* `ESC p 0` Open cash drawer pin 2
* `ESC p 1` Open cash drawer pin 5

## Miscellaneous Commands
* `ESC @` Initialize printer
