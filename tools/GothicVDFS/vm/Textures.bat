@ECHO OFF
IF NOT EXIST ..\..\..\..\Vdfs.cfg GOTO WrongDir

..\GothicVdfs.exe -B Textures.vm

IF "%1"=="" GOTO BatchEnd
IF NOT EXIST %1.bat GOTO BatchEnd

START %1.bat %2 %3 %4 %5 %6 %7 %8 %9

GOTO BatchEnd
:WrongDir
ECHO.
ECHO Wrong directory!
ECHO Run this batch in [gothic]/_work/tools/vdfs/vm/.
ECHO.
PAUSE
:BatchEnd
EXIT
