[CmdLetBinding()]
param(
    [string]$process, # Attaches the debugger to the specified process
    [IO.FileInfo]$dump, # Attaches the debugger to the specified memory dump file
    [string]$remote, # Connects to a remote debugger server on the address supplied
    [string]$command, # Launches a process under the debugger. This command line is executed as-is. Prefix with /q to skip initial breakpoint
    [string]$symbols,       # set the _NT_SYMBOL_PATH environment variable
    [string]$serverAddress = "tcp:port=10456,server=$env:computername",  # Server address used when WinDbg UI is shown
    [switch]$UI # Show the underlying debugging session (in a WinDbg window)
)


# PowerDBGConsole is a simplified front-end to using PowerDBG with a dump file
# It automatically sets up a debugging session against the dump file for you
# drops you into a nested, customised PowerShell prompt
# and cleans up behind you.
# You can still use PowerDBG scripts (or any other scripts) in the console
# but this is supposed to 'feel' more like you are in a powershell-based debugger
# 
# Piers Williams, 2010

$erroractionpreference = 'stop';
$verbose = $VerbosePreference -eq 'continue';

if((-not $dump) -and (-not $processName) -and (-not $command)){
    throw 'One of dump, processName or command must be specified'
}

if($dump){
    pushd (split-path $myInvocation.MyCommand.Path);
    $dump = (resolve-path $dump).Path
    popd
}

if ($symbols){
	# set _NT_SYMBOL_PATH to SRV*C:\symbols*http://msdl.microsoft.com/download/symbols
    if(-not (test-path $symbols)){
        $null = mkdir $symbols
    }
	Set-Item "Env:\_NT_SYMBOL_PATH" "SRV*$symbols*http://msdl.microsoft.com/download/symbols"
}

# Load the module without getting all the errors from the legacy cmdlets
$WarningPreferenceOld = $WarningPreference
$WarningPreference = 'silentlycontinue';
try{
    Import-Module PowerDbg -ArgumentList $null,$verbose -Verbose:$false
}finally{
    $WarningPreference = $WarningPreferenceOld;
}

if ($UI){
	# Launch WinDBG in server mode and connect to it
    # This a bit flaky as no reliable way to wait for WinDbg to be 'ready' before connecting
    # and no easy way of cancelling / timing out the CDB connection if it fails
	$debugger = Start-DebuggerOnly -process:$process -dump:$dump -server:$serverAddress -command:$command
	start-sleep -Seconds 2
	# [void] $debugger.WaitForInputIdle(5000) # doesn't seem to work...

	write-host "Connecting to server using $serverAddress..."
	New-DbgSession -remote:$serverAddress
}else{
    # Launch CDB and drive that directly
	write-host "Opening $process$dump$remote$command..."
	New-DbgSession -process:$process -dump:$dump -remote:$remote -command:$command
}
$modules = Get-DbgModules
if (@($modules | ? { $_.Name -eq 'mscorwks' }).length -gt 0){
    $clrLoaded = $true
}else{
    write-warning 'CLR not loaded (yet). Use Load-DbgExtension to load SOS later if required'
}

if($clrLoaded){
    Load-DbgExtension sos mscorwks

    # Show threads of interest (currently = those with an exception on the stack)
    $threads = Get-DbgClrThreads | ? { $_.ExceptionAddress }
    $threads | % {
        $thread = $_;
        $exception = dumpobj $_.ExceptionAddress;
        $message = (dumpobj $exception._message).__String;
        select-object -InputObject $_ DbgId,Exception,@{Name='Message';Expression={$message}}
    } | format-table -AutoSize
}

# Modify the prompt for the rest of the script lifetime just so we know we're debugging...
function Prompt { "PowerDbg $global:PowerDbgPrompt" }

try{
	# User now gets to work against the debugging session with PowerDBG all loaded and connected
    # help Dbg
    $host.EnterNestedPrompt();
    
}finally{
	# Cleanup and go home
    # (though this doesn't trap if they hit Ctrl-C which is nasty
    End-DbgSession -quit;
    if ($debugger -and (-not $debugger.HasExited)){
        write-warning "Having to explicitly kill debugger process $($debugger.Id)"
	   $debugger.Kill();
    }
    Remove-Module PowerDbg -Verbose:$false
}