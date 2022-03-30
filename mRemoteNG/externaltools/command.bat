@ECHO OFF
cd\
cd C:\Code\debuggingtools\mRemoteNG\externaltools

REM CLEAR SCREEN
CLS

REM Set the window title
SET title=%1%
TITLE %title%

CALL %1 %2 %3 %4 %5 %6 %7

REM  End of application
FOR /l %%a in (5,-1,1) do (TITLE %title% -- closing in %%as&ping -n 2 -w 1 127.0.0.1>NUL)
TITLE Press any key to close the application&ECHO.&GOTO:EOF