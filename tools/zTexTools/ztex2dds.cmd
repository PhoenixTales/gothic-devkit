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
ECHO Searching for all ./.../*.tga and
ECHO converting ./_compiled/*-C.TEX to
ECHO ./.../*.dds
ECHO.
PAUSE

SET _date_time_=%date% %time%
SET _options_=--hack-atip8
SET _texfile_="_compiled\%%~ni-C.TEX"
SET _ddsfile_="%%~dpni.dds"

FOR /R %%i IN (*.tga) DO IF NOT EXIST %_ddsfile_% ztex2dds %_options_% -- %_texfile_% %_ddsfile_%

ECHO.
ECHO started : %_date_time_%
ECHO finished: %date% %time%
ECHO.

POPD
PAUSE

ENDLOCAL
:Leave
