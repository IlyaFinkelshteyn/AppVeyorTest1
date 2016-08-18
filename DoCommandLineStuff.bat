ECHO ON
CD /d %~dp0

SET ipaddress=%1
SET certpath=%2
SET silent=%3
IF "%certpath%"=="" (
	SET certpath=Certificates
)
IF "%ipaddress%"=="" (
	ECHO:
	CALL SetColor.bat 0F "---- You must provide an ipaddress as first argument!"
	ECHO(
	ECHO:
	PAUSE
	goto :EOF
)


REM Check if the batch file is running elevated
net session >nul 2>&1
IF not %errorlevel% equ 0 (
	ECHO:
	CALL SetColor.bat 0F "---- You must run this batch file as administrator!"
	ECHO(
	ECHO:
	PAUSE
	goto :EOF
)

REM Setup the Visual Studio Build Environment
REM call reg query HKLM\SOFTWARE\Microsoft\VisualStudio\SxS\VS7 /v 12.0
REM IF %errorlevel% neq 0 GOTO :error
CALL "%vs120comntools%\VsDevCmd.bat"
IF %errorlevel% neq 0 GOTO :error

ECHO:
CALL SetColor.bat 0A "---- All Good it worked OK!"
ECHO(
ECHO:

IF NOT "%silent%"=="q" PAUSE
EXIT /b 0

:error
CD /d %~dp0
ECHO:
ECHO:
CALL SetColor.bat 04 "---- Failed Install! error #%errorlevel% ----"
ECHO(
PAUSE
EXIT /b %errorlevel%
