# ESC-POS-.NET
.NET (C#) Implementation of the Epson ESC/POS standard


## Initializing

- call init
- recover from recoverable errors
- cancel user defined fonts (needed or does init do this?)


## In Text
- Newlines will be preserved in input text
- Tab characters will also be preserved

# Completed and Tested Commands 
All of the commands below are covered by a test print.

## Print Commands
TODO: String sanitation

Print(string) - Outputs the ASCII character data to the printer as it is passed in.  Does not add a trailing new line.  Standardizes platform-specific newlines to LF characters (eg.  Windows CRLF and OS-X CR and Linux LFs will all be converted to LFs to maintain compatibility with the ESC-POS standard).

PrintLine(string) - Outputs the ASCII string passed in with newlines stripped out, and a trailing newline added.

FeedLines(int n)
Feeds the paper n lines.

FeedLinesReverse(int n)
Feeds the paper n lines in reverse. Doesn't work on TM-T20II.

FeedDots(int n)
Feeds the paper n dots, where n < 255.


#  Untested Commands

## Not Yet Implemented Commands


## Line Spacing Commands


## Character Commands


## Print Position Commands

## Panel Button Commands

## Mechanism Control Commands

## Bit Image Commands

## Graphics Commands

## NV Graphics Commands


## Download Graphics Commands

## Logo Print Commands

## Bar Code Commands

## Two-Dimensional Code Commands

## Status Commands

## Macro Function Commands

## Miscellaneous Commands


### User Setup Commands

TODO: 
Line Spacing
SetLineSpacing()
SetLineSpacing(n dots) 0 <= n <= 255

Not In Scope:


Drawer Kick Out Commands

Page Mode Commands