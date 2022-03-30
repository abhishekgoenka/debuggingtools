@echo off
rem Basic install batch just to copy the module into your PowerShell Modules directory
pushd %~dp0
set moduleDir=%USERPROFILE%\Documents\WindowsPowerShell\Modules\PowerDbg
if not exist "%moduleDir%" mkdir "%moduleDir%"
copy .\PowerDbg.psm1 "%moduleDir%"
popd
exit /b %ERRORLEVEL%