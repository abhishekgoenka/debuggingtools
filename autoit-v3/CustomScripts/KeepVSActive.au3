#include <Constants.au3>

; Wait for the Notepad to become active. The classname "Notepad" is monitored instead of the window title
; Prompt the user to run the script - use a Yes/No prompt with the flag parameter set at 4 (see the help file for more details)
Local $iAnswer = MsgBox(BitOR($MB_YESNO, $MB_SYSTEMMODAL), "Runner", "The program will bring visual studio window in focus and will keep it active.  Do you want to run it?")

; Check the user's answer to the prompt (see the help file for MsgBox return values)
; If "No" was clicked (7) then exit the script
If $iAnswer = 7 Then
	MsgBox($MB_SYSTEMMODAL, "Runner", "OK.  Bye!")
	Exit
 EndIf

; Run the On-Screen Keyboard
Run("osk.exe")

; Wait for the On-Screen Keyboard to become active. The classname "OSKMainClass" is monitored instead of the window title
WinWaitActive("[CLASS:OSKMainClass]")

; Use AutoItSetOption to slow down the typing speed so we can see it
AutoItSetOption("SendKeyDelay", 400)

While(1)
   ; Now that the Notepad window is active type some special characters
   Send("a")
   Sleep(200)
WEnd

