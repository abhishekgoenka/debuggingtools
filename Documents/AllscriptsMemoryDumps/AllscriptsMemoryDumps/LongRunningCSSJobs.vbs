Set WshNetwork = WScript.CreateObject("WScript.Network")
Set objWSHShell = CreateObject("WScript.Shell")
Set objFSO = CreateObject("Scripting.FileSystemObject")

strComputer = "."
strProcDump = "C:\AllscriptsMemoryDumps\procdump.exe" & " "
Set objWMIService = GetObject("winmgmts:" _
    & "{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
Set colProcessList = objWMIService.ExecQuery _
    ("Select * from Win32_Process Where Name = 'iexplore.exe'")
For Each objProcess in colProcessList
    if(DumpCount() < 10) then
	    strProcDumpSwitches = "-ma " & objProcess.ProcessId & " -accepteula"
	    objWSHShell.Run strProcDump & strProcDumpSwitches
    end if
Next


public function DumpCount()
    Dim fso, count, src, folder, file
    Set fso = CreateObject("Scripting.FileSystemObject")
    src = "C:\AllscriptsMemoryDumps\"
    Set folder = fso.GetFolder(src)
    count = 0
    For Each file In folder.files
	    If LCase(fso.GetExtensionName(file)) = "dmp" Then
		    count = count + 1
	    End If
    Next
    DumpCount = count
    'WScript.Echo "Count: " & count 
end function