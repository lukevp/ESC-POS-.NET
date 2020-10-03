## Line Spacing Commands
* ESC 2 Select default line spacing
* ESC 3 Set line spacing

## Character Commands
* `ESC SP` Set right-side character spacing
* `ESC –` Turn underline mode on/off
* `ESC E` Turn emphasized mode on/off
* `ESC G` Turn double-strike mode on/off
* `ESC M` Select character font
* `ESC V` Turn 90° clockwise rotation mode on/off
* `ESC r` Select print color
* `ESC t` Select character code table
* `ESC {` Turn upside-down print mode on/off
* `GS !` Select character size
* `GS b` Turn smoothing mode on/off

## Panel Button Commands
* `ESC c 5` Enable/disable panel buttons

## Paper Sensor Commands
@@this is for parallel only  ESC c 3 Select paper sensor(s) to output paper-end signals
@@this should stop by default ESC c 4 Select paper sensor(s) to stop printing

## Print Position Commands
* `HT` Horizontal tab
* `ESC $` Set absolute print position
* `ESC D` Set horizontal tab positions
* `ESC a` Select justification
* `GS L` Set left margin
* `GS W` Set print area width

## Bit Image Commands
* `ESC *` Select bit-image mode



## Mechanism Control Commands
* `ESC U` Turn unidirectional print mode on/off
* `GS V` Select cut mode and cut paper

## Miscellaneous Commands
* `ESC @` Initialize printer
* `DLE ENQ` Send real-time request to printer
* `DLE DC4 (fn = 1)` Generate pulse in real-time
* `DLE DC4 (fn = 2)` Execute power-off sequence

TODO: check for DLE DC4 other functions and any other real time commands to make sure they're not part of graphics data because they will be processed immediately.  also can use GS ( D to disable realtime commands before processing graphics data

* `ESC p` Generate pulse
* `GS ( A` Execute test print
* `GS ( D` Enable/disable real-time command
* `GS ( H` Request transmission of response or status
* `GS ( K` Select print control method(s)
    *    Select the print control mode
    *    Select the print density
    *    Select the print speed
    *    Select the number of parts for the thermal head energizing
* `GS ( P` Page mode control
    *    Printable area setting when page mode is selected
* `GS ( Q` Commands for drawing graphics
    *    Draw line
    *    Draw rectangle

## Two Dimension Code Commands (QR Codes)
* `GS ( k` Set up and print the symbol

## Customize Commands
* `GS ( C` Edit NV user memory
    *    Delete the specified record
    *    Store the data in the specified record
    *    Transmit the data in the specified record
    *    Transmit capacity of the NV user memory currently being used
    *    Transmit the remaining capacity of the NV user memory
    *    Transmit the key code list
    *    Delete all data in the NV user memory
* `GS ( E` Set user setup commands
    *    Change into the user setting mode
    *    End the user setting mode session
    *    Change the memory switch
    *    Transmit the settings of the memory switch
    *    Set the customized setting values
    *    Transmit the customized setting values
    *    Copy the user-defined page
    *    Define the data (column format) for the character code page
    *    Define the data (raster format) for the character code page
    *    Delete the data for the character code page
    *    Set the configuration item for the serial interface
    *    Transmit the configuration item for the serial interface
    *    Set the configuration item for the Bluetooth interface
    *    Transmit the configuration item for the Bluetooth interface
    *    Delete the paper layout
    *    Set the paper layout

## Counter Printing Commands

## Printing Paper Commands











## All Commands

## Print Commands
* LF Print and line feed
* FF (in page mode) Print and return to standard mode (in page mode)
* CR Print and carriage return
* ESC FF Print data in page mode
* ESC J Print and feed paper
* ESC K Print and reverse feed
* ESC d Print and feed n lines
* ESC e Print and reverse feed n lines

## Line Spacing Commands
* ESC 2 Select default line spacing
* ESC 3 Set line spacing

## Character Commands
* CAN Cancel print data in page mode
* ESC SP Set right-side character spacing
* ESC ! Select print mode(s)
* ESC % Select/cancel user-defined character set
* ESC & Define user-defined characters
* ESC – Turn underline mode on/off
* ESC ? Cancel user-defined characters
* ESC E Turn emphasized mode on/off
* ESC G Turn double-strike mode on/off
* ESC M Select character font
* ESC R Select an international character set
* ESC V Turn 90° clockwise rotation mode on/off
* ESC r Select print color
* ESC t Select character code table
* ESC { Turn upside-down print mode on/off
* GS ( N Select character effects
    *     Select character color
    *    Select background color
    *     Turn shading mode on/off
* GS ! Select character size
* GS B Turn white/black reverse print mode on/off
* GS b Turn smoothing mode on/off


## Panel Button Commands
* ESC c 5 Enable/disable panel buttons


## Paper Sensor Commands
* ESC c 3 Select paper sensor(s) to output paper-end signals
* ESC c 4 Select paper sensor(s) to stop printing

## Print Position Commands
* HT Horizontal tab
* ESC $ Set absolute print position
* ESC D Set horizontal tab positions
* ESC T Select print direction in page mode
* ESC W Set print area in page mode
* ESC \ Set relative print position
* ESC a Select justification
* GS $ Set absolute vertical print position in page mode
* GS L Set left margin
* GS T Set print position to the beginning of print line
* GS W Set print area width
* GS \ Set relative vertical print position in page mode

## Bit Image Commands
* ESC ✻ Select bit-image mode
* FS p Print NV bit image
* GS ( L GS 8 L Set graphics data
    *     Transmit the NV graphics memory capacity.
    *     Set the reference dot density for graphics.
    *     Print the graphics data in the print buffer.
    *     Transmit the remaining capacity of the NV graphics memory.
    *     Transmit the remaining capacity of the download graphics memory.
    *     Transmit the key code list for defined NV graphics.
    *     Delete all NV graphics data.
    *     Delete the specified NV graphics data.
    *     Define the NV graphics data (raster format).
    *     Define the NV graphics data (column format).
    *     Print the specified NV graphics data.
    *     Transmit the key code list for defined download graphics.
    *     Delete all NV graphics data.
    *     Delete the specified download graphics data.
    *     Define the downloaded graphics data (raster format).
    *     Define the downloaded graphics data (column format).
    *     Print the specified download graphics data.
    *     Store the graphics data in the print buffer (raster format).
    *     Store the graphics data in the print buffer (column format).
* FS q Define NV bit image
* GS v 0 Print raster bit image
* GS ✻ Define downloaded bit image
* GS / Print downloaded bit image
* GS Q 0 Print variable vertical size bit image

## Status Commands
* DLE EOT Transmit real-time status
* ESC u Transmit peripheral device status
* ESC v Transmit paper sensor status
* GS a Enable/disable Automatic Status Back (ASB)
* GS j Enable/disable Automatic Status Back (ASB) for ink
* GS r Transmit status

## Bar Code Commands
* GS H Select print position of HRI characters
* GS f Select font for HRI characters
* GS h Set bar code height
* GS k Print bar code
* GS w Set bar code width

## Macro Function Commands
* GS : Start/end macro definition
* GS ^ Execute macro

## Mechanism Control Commands
* ESC < Return home
* ESC U Turn unidirectional print mode on/off
* ESC i Partial cut (one point left uncut)
* ESC m Partial cut (three points left uncut)
* GS V Select cut mode and cut paper

## Miscellaneous Commands
* DLE ENQ Send real-time request to printer
* DLE DC4 (fn = 1) Generate pulse in real-time
* DLE DC4 (fn = 2) Execute power-off sequence
* DLE DC4 (fn = 7) Transmit specified status in real time
* DLE DC4 (fn = 8) Clear buffer (s)
* ESC ( A Control beeper tones
    *     Beep integrated beeper in TM-U230 models
    *     Set integrated beeper when offline factors occur in TM-U230 models
    *     Set integrated beeper except when offline factors occur in TM-U230 models
* ESC = Select peripheral device
* ESC @ Initialize printer
* ESC L Select page mode
* ESC S Select standard mode
* ESC p Generate pulse
* GS ( A Execute test print
* GS ( D Enable/disable real-time command
* GS ( H Request transmission of response or status
* GS ( K Select print control method(s)
    *     Select the print control mode
    *     Select the print density
    *     Select the print speed
    *     Select the number of parts for the thermal head energizing
* GS ( P Page mode control
    *     Printable area setting when page mode is selected
* GS ( Q Commands for drawing graphics
    *     Draw line
    *     Draw rectangle

## Kanji Commands
* FS ! Select print mode(s) for Kanji characters
* FS & Select Kanji character mode
* FS ( A Select Kanji character style(s)
* Select Kanji character font
* FS – Turn underline mode on/off for Kanji characters
* FS . Cancel Kanji character mode
* FS 2 Define user-defined Kanji characters
* FS C Select Kanji character code system
* FS S Set Kanji character spacing
* FS W Turn quadruple-size mode on/off for Kanji characters
* FS ? Cancel user-defined Kanji characters

## Two Dimension Code Commands (QR Codes)
* GS ( k Set up and print the symbol

## Customize Commands
* FS g 1 Write to NV user memory
* FS g 2 Read from NV user memory
* GS ( C Edit NV user memory
    *     Delete the specified record
    *     Store the data in the specified record
    *     Transmit the data in the specified record
    *     Transmit capacity of the NV user memory currently being used
    *     Transmit the remaining capacity of the NV user memory
    *     Transmit the key code list
    *     Delete all data in the NV user memory
* GS ( E Set user setup commands
    *     Change into the user setting mode
    *     End the user setting mode session
    *     Change the memory switch
    *     Transmit the settings of the memory switch
    *     Set the customized setting values
    *     Transmit the customized setting values
    *     Copy the user-defined page
    *     Define the data (column format) for the character code page
    *     Define the data (raster format) for the character code page
    *     Delete the data for the character code page
    *     Set the configuration item for the serial interface
    *     Transmit the configuration item for the serial interface
    *     Set the configuration item for the Bluetooth interface
    *     Transmit the configuration item for the Bluetooth interface
    *     Delete the paper layout
    *     Set the paper layout

## Counter Printing Commands
* GS C 0 Select counter print mode
* GS C 1 Select count mode (A)
* GS C 2 Set counter
* GS C ; Select counter mode (B)
* GS c Print counter

## Printing Paper Commands
* FS ( L Select label and black mark control function(s)
    *     Paper layout setting
    *     Paper layout information transmission
    *    Transmit the positioning information
    *     Feed paper to the label peeling position
    *     Feed paper to the cutting position
    *     Feed paper to the print starting position
    *     Paper layout error special margin setting




## Mechanism Control Commands






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
* Line Spacing
* SetLineSpacing()
* SetLineSpacing(n dots) 0 <= n <= 255

Not In Scope:
* Drawer Kick Out Commands
* Page Mode Commands




# COMMANDS IN ALPHANUMERIC ORDER
Command|Name|Function type
---|---|---
HT |Horizontal tab |PRINT POSITION COMMANDS
LF |Print and line feed |PRINT COMMANDS
FF (in page mode) |Print and return to standard mode| PRINT COMMANDS
CR |Print and carriage return |PRINT COMMANDS
CAN |Cancel print data in page mode |CHARACTER COMMANDS
DLE EOT |Real-time status transmission |STATUS COMMANDS
DLE ENQ |Real-time request to printer| MISCELLANEOUS COMMANDS
DLE DC4 (fn = 1) |Generate pulse at real-time |MISCELLANEOUS COMMANDS
DLE DC4 (fn = 2) |Turn off the power| MISCELLANEOUS COMMANDS
DLE DC4 (fn = 7) |Transmit specified status in real time |MISCELLANEOUS COMMANDS
DLE DC4 (fn = 8) |Clear buffer| MISCELLANEOUS COMMANDS
ESC FF |Print data in page mode |PRINT COMMANDS
ESC SP |Set right-side character spacing |CHARACTER COMMANDS
ESC ! |Select print mode(s) |CHARACTER COMMANDS
ESC $ |Set absolute print position |PRINT POSITION COMMANDS
ESC % |Select/cancel user-defined character set| CHARACTER COMMANDS
ESC & |Define user-defined characters |CHARACTER COMMANDS
ESC ( A |Control of the beeper |MISCELLANEOUS COMMANDS
ESC ✻ |Select bit-image mode| BIT-IMAGE COMMANDS
ESC – |Turn underline mode on/off| CHARACTER COMMANDS
ESC 2 |Select default line spacing| LINE SPACING COMMANDS
ESC 3 |Set line spacing |LINE SPACING COMMANDS
ESC < |Return home| MECHANISM CONTROL COMMANDS
ESC = |Select peripheral device |MISCELLANEOUS COMMANDS
ESC ? |Cancel user-defined characters| CHARACTER COMMANDS
ESC @ |Initialize printer| MISCELLANEOUS COMMANDS
ESC D |Set horizontal tab positions |PRINT POSITION COMMANDS
ESC E |Turn emphasized mode on/off |CHARACTER COMMANDS
ESC G |Turn double-strike mode on/off| CHARACTER COMMANDS
ESC J |Print and feed paper |PRINT COMMANDS
ESC K |Print and reverse feed |PRINT COMMANDS
ESC L |Select page mode |MISCELLANEOUS COMMANDS
ESC M |Select character font| CHARACTER COMMANDS
ESC R |Select an international character set| CHARACTER COMMANDS
ESC S |Select standard mode |MISCELLANEOUS COMMANDS
ESC T |Select print direction in page mode |PRINT POSITION COMMANDS
ESC U |Turn unidirectional printing mode on/off |MECHANISM CONTROL COMMANDS
ESC V |Turn 90° clockwise rotation mode on/off |CHARACTER COMMANDS
ESC W |Set printing area in page mode |PRINT POSITION COMMANDS
ESC \ |Set relative print position| PRINT POSITION COMMANDS
ESC a |Select justification |PRINT POSITION COMMANDS
ESC c 3 |Select paper sensor(s) to output paper-end signals |PAPER SENSOR COMMANDS
ESC c 4 |Select paper sensor(s) to stop printing |PAPER SENSOR COMMANDS
ESC c 5 |Enable/disable panel buttons| PANEL BUTTON COMMAND
ESC d |Print and feed n lines |PRINT COMMANDS
ESC e |Print and reverse feed n lines |PRINT COMMANDS
ESC i |Partial cut (one point left uncut) |MECHANISM CONTROL COMMANDS
ESC m |Partial cut (three points left uncut) |MECHANISM CONTROL COMMANDS
ESC p |Generate pulse| MISCELLANEOUS COMMANDS
ESC r |Select print color| CHARACTER COMMANDS
ESC t |Select character code table |CHARACTER COMMANDS
ESC u |Transmit peripheral device status| STATUS COMMANDS
ESC { |Turn upside-down printing mode on/off| CHARACTER COMMANDS
FS ! |Set print mode(s) for Kanji characters |KANJI COMMANDS
FS & |Select Kanji character mode |KANJI COMMANDS
FS ( A |Define character effects of Kanji characters. |KANJI COMMANDS
FS ( L |Control of the label paper / black mark paper |PRINTING PAPER COMMANDS
FS – |Turn underline mode on/off for Kanji characters |KANJI COMMANDS
FS . |Cancel Kanji character mode |KANJI COMMANDS
FS 2 |Define user-defined Kanji characters| KANJI COMMANDS
FS C |Select Kanji character code system |KANJI COMMANDS
FS S |Set Kanji character spacing |KANJI COMMANDS
FS W |Turn quadruple-size mode on/off for Kanji characters| KANJI COMMANDS
FS ? |Cancel user-defined Kanji characters |KANJI COMMANDS
FS g 1 |Write to NV user memory |CUSTOMIZE COMMANDS
FS g 2 |Read from NV user memory |CUSTOMIZE COMMANDS
FS p |Print NV bit image |BIT-IMAGE COMMANDS
FS q |Define NV bit image |BIT-IMAGE COMMANDS
GS ! |Select character size |CHARACTER COMMANDS
GS $ |Set absolute vertical print position in page mode| PRINT POSITION COMMANDS
GS ( A |Execute test print| MISCELLANEOUS COMMANDS
GS ( C |Edit NV user memory |CUSTOMIZE COMMANDS
GS ( D |Enable/disable real-time command| MISCELLANEOUS COMMANDS
GS ( E |User setup commands |CUSTOMIZE COMMANDS
GS ( H |Require response transmission |MISCELLANEOUS COMMANDS
GS ( K |Select printing control| MISCELLANEOUS COMMANDS
GS ( L GS 8 L |Specify graphics data| BIT-IMAGE COMMANDS
GS ( M |Customize printer control value(s)| CUSTOMIZE COMMANDS
GS ( N |Select character effects| CHARACTER COMMANDS
GS ( k |Specify and print the symbol |TWO DIMENSION CODE COMMANDS
GS ✻ |Define downloaded bit image |BIT-IMAGE COMMANDS
GS / |Print downloaded bit image |BIT-IMAGE COMMANDS
GS : |Start/end macro definition |MACRO FUNCTION COMMANDS
GS B |Turn white/black reverse printing mode on/off| CHARACTER COMMANDS
GS C 0 |Select counter print mode| COUNTER PRINTING COMMANDS
GS C 1 |Select count mode (A) |COUNTER PRINTING COMMANDS
GS C 2 |Select counter print mode| COUNTER PRINTING COMMANDS
GS C ; |Select counter mode (B) |COUNTER PRINTING COMMANDS
GS H |Select printing position of HRI characters |BAR CODE COMMANDS
GS I |Transmit printer |ID MISCELLANEOUS COMMANDS
GS L |Set left margin |PRINT POSITION COMMANDS
GS P |Set horizontal and vertical motion units| MISCELLANEOUS COMMANDS
GS Q 0 |Print variable vertical size bit image |BIT-IMAGE COMMANDS
GS T |Set print position to the beginning of print line| PRINT POSITION COMMANDS
GS V |Select cut mode and cut paper |MECHANISM CONTROL COMMANDS
GS W |Set printing area width PRINT |POSITION COMMANDS
GS \ |Set relative vertical print position in page mode| PRINT POSITION COMMANDS
GS ^ |Execute macro |MACRO FUNCTION COMMANDS
GS a |Enable/disable Automatic Status Back (ASB) |STATUS COMMANDS
GS b |Turn smoothing mode on/off |CHARACTER COMMANDS
GS c |Print counter| COUNTER PRINTING COMMANDS
GS f |Select font for HRI characters |BAR CODE COMMANDS
GS g 0 |Initialize maintenance counter| MISCELLANEOUS COMMANDS
GS g 2 |Transmit maintenance counter |MISCELLANEOUS COMMANDS
GS h |Set bar code height |BAR CODE COMMANDS
GS j |Enable/disable Ink Automatic Status Back (ASB) |STATUS COMMANDS
GS k |Print bar code |BAR CODE COMMANDS
GS r |Transmit status |STATUS COMMANDS
GS v 0 |Print raster bit image |BIT-IMAGE COMMANDS
GS w |Set bar code width |BAR CODE COMMANDS
GS z 0 |Set online recovery wait time| MISCELLANEOUS COMMANDS
GS z 0 |(TM-L90 w/ Peeler) Set online recovery wait time| MISCELLANEOUS COMMANDS

## The following commands are supported only by Simplified Chinese/Traditional Chinese/Japanese models.

Command|Name|Function type
---|---|---
FS ! |Set print mode(s) for Kanji characters |KANJI COMMANDS
FS & |Select Kanji character mode |KANJI COMMANDS
FS ( A |Define character effects of Kanji characters |KANJI COMMANDS
FS – |Turn underline mode on/off for Kanji characters |KANJI COMMANDS
FS . |Cancel Kanji character mode |KANJI COMMANDS
FS 2 |Define user-defined Kanji characters| KANJI COMMANDS
FS C |Select Kanji character code system |KANJI COMMANDS
FS S |Set Kanji character spacing| KANJI COMMANDS
FS W |Turn quadruple-size mode on/off for Kanji characters |KANJI COMMANDS
FS ? |Cancel user-defined Kanji characters| KANJI COMMANDS



