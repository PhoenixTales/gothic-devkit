@ECHO OFF
IF "%OS%" == "Windows_NT" GOTO WinNT
ECHO.
ECHO This script is intended to run on Windows NT
ECHO.
GOTO Leave
:WinNT
SETLOCAL
SETLOCAL ENABLEEXTENSIONS

PUSHD
SET PATH=%CD%;%PATH%
CD ..\..\Data\Textures

ECHO.
ECHO Searching for all ./.../*.dds and
ECHO converting to ./_compiled/*-C.TEX
ECHO.
PAUSE

SET _date_time_=%date% %time%
SET _options_=
SET _ddsfile_="%%i"
SET _texfile_="_compiled\%%~ni-C.TEX"

FOR /R %%i IN (*.dds) DO IF NOT EXIST %_texfile_% dds2ztex %_options_% -- %_ddsfile_% %_texfile_%

ECHO.
ECHO started : %_date_time_%
ECHO finished: %date% %time%
ECHO.

POPD
PAUSE

ENDLOCAL
:Leave
