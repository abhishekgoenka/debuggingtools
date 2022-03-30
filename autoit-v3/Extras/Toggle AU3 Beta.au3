#RequireAdmin
; ----------------------------------------------------------------------------
;
; AutoIt Version: 3.1.1
; Author:         Ejoc/JPM/Jon
;
; Script Function:
; Toggle AutoIt beta.
;
; ----------------------------------------------------------------------------

#include <Constants.au3>

Opt("MustDeclareVars", 1)

Local $InstallDir = RegRead("HKLM\Software\AutoIt v3\AutoIt", "InstallDir")
If $InstallDir = "" Then $InstallDir = RegRead("HKLM\Software\Wow6432Node\AutoIt v3\AutoIt", "InstallDir")
If $InstallDir = "" Then
	MsgBox($MB_SYSTEMMODAL, 'Error', 'Cannot find AutoIt Installation directory')
	Exit
EndIf

Const $Key = "HKCR\.au3"
Const $UserChoiceKey = "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts"
Local $CurrentAssoc = RegRead($Key, "")

; Nuke the UserChoice key which interferes (doesn't work programatically)
RegDelete($UserChoiceKey & "\.au3\UserChoice")
RegDelete($UserChoiceKey & "\.a3x\UserChoice")

If RegRead($UserChoiceKey & "\.au3\UserChoice", "ProgID") <> "" Then
	MsgBox($MB_SYSTEMMODAL, 'Error', "Unable to change the association due to OS restrictions. Please right-click a script and select 'Open with \ Choose default program'.")
	Exit
EndIf

If $CurrentAssoc = "AutoIt3ScriptBeta" Then
	; Already using beta switch to prod
	RegWrite($Key, "", "REG_SZ", "AutoIt3Script")
	; make sure that the right AutoItX.dll is installed
	RunWait('regsvr32 /s "' & $InstallDir & '\AutoItX\AutoItX3.dll"')
	MsgBox($MB_SYSTEMMODAL, "Beta Toggle", "Now using RELEASE version v" & FileGetVersion($InstallDir & '\AutoIt3.exe') & " of AutoIt", 2)
ElseIf $CurrentAssoc = "AutoIt3Script" Then
	; Using prod, change to beta
	RegWrite($Key, "", "REG_SZ", "AutoIt3ScriptBeta")
	; make sure that the right AutoItX.dll is installed
	RunWait('regsvr32 /s "' & $InstallDir & '\beta\AutoItX\AutoItX3.dll"')
	MsgBox($MB_SYSTEMMODAL, "Beta Toggle", "Now using BETA version v" & FileGetVersion($InstallDir & '\Beta\AutoIt3.exe') & " of AutoIt", 2)
Else
	MsgBox($MB_SYSTEMMODAL, "Beta Toggle", "AutoIt installation appears to be customised - please manually edit file associations.")
EndIf
