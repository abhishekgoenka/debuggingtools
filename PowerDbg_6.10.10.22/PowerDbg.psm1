#
# PowerDBG build 6.10.10.22
# 

# ----------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\PowerDbg.Core.ps1
#
param(
    $debuggerRoot,    # Auto-located if not specified
    [switch]$verbose  # Triggers diagnostic logging
) 

########################################################################################################
# PowerDbg v6 (alpha)
#
# Automate WinDbg/CDB/NTSD debugging with PowerShell scripting
# http://powerdbg.codeplex.com/
#
# Basic usage:
# In WinDbg, set up a server: 
#  .server tcp:Port=10456 
#
# In PowerShell:
#  Import-Module PowerDbg
#  Connect-Debugger "tcp:Port=10456,Server=<server>" # other connections / protocols work too
#  Invoke-DbgCommand k 
#  Disconnect-Debugger
#
# Better still, use any of the cmdlets that wrap WinDbg / SOS commands
# and return output as structured objects that can be pipelined
# eg
#  Get-DbgClrThreads (equiv to !threads)
#  Get-DbgObject <address> (equiv to !dumpobj)
#
# See the website for more examples / walkthroughs etc...
# TODO or help about_PowerDbg TODO
#
# New for v6
# - Focus on passthro objects (converting most output to PSObject) to enable better pipelining
# - Focus on cmdlets that match the WinDBG / SOS debugging commands (easy to learn)
# - Revisited interface to CDB / WinDBG now just streams lines as they are read
# - PowerDBGConsole for simplified debugging experience
# - Bundled sample scenarios, for testing and training
#
#
# Roberto Alexis Farah - http://blogs.msdn.com/debuggingtoolbox/
# Piers Williams - http://piers7.blogspot.com/
#
# Contributions from (and big thanks to):
# Brad Linscott - Premier Field Engineer
# Lee Holmes    - Software Development Engineer - http://www.leeholmes.com/blog/default.aspx
#
# Tell us what you think: http://powerdbg.codeplex.com/Thread/List.aspx
########################################################################################################

# See module-init below for startup-sequence (is itself dependent on util functions)

#############################
# Connect-Debugger

Set-Alias Connect-Debugger New-DbgSession
Set-Alias Connect-Dbg New-DbgSession
Set-Alias Connect-WinDbg New-DbgSession

# .Synopsis
# Creates a PowerDbg session, by attaching to or launching the debugger
#
# .Description
# A PowerDbg session can be instantiated in one of three ways:
# - connecting to an existing debugger configured as a server (-remote)
# - opening a memory dump in the debugger (-dump)
# - launching a process under the debugger (-command)
# Once a PowerDbg session exists you can interact with it using any of the 
# Get-Dbg* cmdlets, or by using Invoke-DbgCommand to send commands directly
#
function New-DbgSession
{ 
    [CmdLetBinding()]
    param( 
        [string]$process, # Attaches the debugger to the specified process
        [IO.FileInfo]$dump, # Attaches the debugger to the specified memory dump file
        [string]$remote, # Connects to a remote debugger server on the address supplied
        [string]$command, # Launches a process under the debugger. This command line is executed as-is

        [switch]$sos # Loads SOS on connection, for Managed code debugging support
    ) 

    $launchArgs = @{}
    if ($process){
        $launchArgs.Add('process',$process);
        $target = $process;
    }elseif($dump){
        $launchArgs.Add('dump',$dump);
        $target = $dump;  
    }elseif($remote){
        $launchArgs.Add('remote',$remote);
        $target = $remote;
    }elseif($command){
        $launchArgs.Add('command',$command);
        $target = $command;
    }else{
        throw 'One of process, dump, remote, or command must be specified'
    }
        
    # If we already have an active connection...
    if($SCRIPT:windbgProcess -and (-not $SCRIPT:windbgProcess.HasExited)){

        # ... and that's where we're connecting to...
        if ($SCRIPT:currentConnection -eq $target){ 
            # Nothing to do: still have an existing connection to that
            write-verbose "Connection to $($SCRIPT:currentConnection) still active"
            return;
        }
        
        # ... otherwise fail
        throw "Already connected to $($script:currentConnection). Use Disconnect-Windbg, " +  
            "then connect to another instance." 
    }
    
    ## Launch cdb.exe, the command-line version of WinDbg. 
    ## Take control of its input and output streams, which we'll use 
    ## to capture commands and their output. 
    $SCRIPT:windbgProcess = Start-DebuggerOnly @launchArgs -debugger:CDB -captureIO
    $SCRIPT:currentConnection = $target 

    # Give debugger a chance to write initial errors / usage info etc...
    start-sleep -Milliseconds 500
    
    $initial = ReadToBufferEnd $SCRIPT:windbgProcess.StandardOutput -split [Environment]::NewLine;
    switch -regex ($initial) {
        'DebugConnect failed, HRESULT'
        {
            throw "Failed to connect to debugger at {0}:{1}" -f $remote,$_;
        }
        '^usage\: cdb' {
            Disconnect-Debugger
            $initial
            throw "Invalid command line options passed to debugger";
        }
    }       

    if($SCRIPT:windbgProcess.HasExited) {
        $initial
        throw "CDB process exited with error code $($SCRIPT:windbgProcess.ExitCode)"
    } 
    
    # Check we are now properly connected by issuing a 'dummy' command
    Invoke-DbgCommand ".echo PowerDbg connected" | out-null

    # Get the bittyness of the process /dump being examined:
    $pointerResponse = @(Invoke-DbgCommand '.printf "%d\n", @$ptrsize'); # TODO: Use Roberto's method for this
    $pointerSize = @($pointerResponse | ? { $_ -match '^\d+' })[0]
    switch($pointerSize){
        4 { $SCRIPT:bits = 32; break; }
        8 { $SCRIPT:bits = 64; break; }
        default { throw "Unsupported pointer size '$pointerResponse'" ; break; }
    }
    write-verbose "Connected to $SCRIPT:bits bit process/dump";
    if ($SCRIPT:hostbits -ne $SCRIPT:bits){
        # This doesn't appear to work. The macro expanded by the debugger
        # appears to be based on the OS system architecture pointer,
        # and not that of either the debugger or debuggee's process space :-(
        # Write-Warning "Using $SCRIPT:hostbits bit debugger against $SCRIPT:bits bit process/dump. This will cause problems"
    }
        
    if ($sos){
        $loadedModules = Get-DbgModules | % { $_.Name }
        if ($loadedModules -contains 'clr'){
            # .Net 4 loaded
            Load-DbgExtension sos clr    
        }elseif($loadedModules -contains 'mscorwks'){
            Load-DbgExtension sos mscorwks
        }else{
            Write-Warning 'CLR not yet loaded. Use Load-DbgExtension later (or Invoke-DbgCommand sxe clrn)'
        }
    }
} 


#############################
# Disconnect-Debugger

Set-Alias Disconnect-WinDbg Exit-DbgSession
Set-Alias Disconnect-Dbg Exit-DbgSession
Set-Alias Disconnect-Debugger Exit-DbgSession

# .Synopsis
# Terminates the current PowerDbg session, and optionally shuts down the debugger
#
# .Description
# When terminating a session against a remote debugger, use the -quit
# switch to force the debugger to close also.
# When terminating a session against a process or memory dump, the underlying
# debugger is always shut down.
#
function Exit-DbgSession([switch]$quit)
{ 
    # wait for up to a 1/2 second in case it is already closing down
    if($SCRIPT:windbgProcess `
        -and (-not $SCRIPT:windbgProcess.HasExited) `
        -and (-not $SCRIPT:windbgProcess.WaitForExit(500)) `
    ) 
    { 
        try{
            if($quit){
                # TODO: Not 100% sure this is working yet
                # qq used because this will also terminate the remote server (if debugging remotely / using 'UI mode')
                $SCRIPT:windbgProcess.StandardInput.WriteLine('qq');
            }
            $SCRIPT:windbgProcess.StandardOutput.Close() 
            $SCRIPT:windbgProcess.StandardInput.Close() 
        }finally{
            # closing the streams (above) can allow the process to exit, so...
            if(-not $SCRIPT:windbgProcess.HasExited){
                $SCRIPT:windbgProcess.Kill() 
            }
            
            $SCRIPT:windbgProcess = $null 
            $SCRIPT:currentConnection = $null 
        }
    } 
    
    $SCRIPT:currentConnection = $null 
    $SCRIPT:windbgProcess = $null 
} 

# .Synopsis
# Spools a textreader to the end of its current receive buffer
# without entering a blocking read.
# Behavior is different from TextReader.ReadToEnd() which
# reads to the end of the stream (ie until the stream is closed)
# not the end of the *available* section of the stream
# (private)
function ReadToBufferEnd(
    [system.io.textreader]$reader
){
    $sb = new-object System.Text.StringBuilder;
    while($reader.Peek() -ge 0){ # -1 indicates no chars to be read
        [void] $sb.Append([char]($reader.Read()));
    }
    $sb.ToString();
}

#############################
# Invoke-DbgCommand

$promptPattern = "^((\d\:)?\d\:\d{3}(\:x\d{2})?> ?)" # (0:)?0:000(:x86)?  nb: one optional space at the end
$promptRegex = new-object System.Text.RegularExpressions.Regex $promptPattern

Set-Alias idc Invoke-DbgCommand

# .Synopsis
# Invokes a command in the connected debugger session, and return its output 
# .Description
# This is the primary mechanism by which PowerDbg interacts with the debugger session.
# You can use this to execute your own arbitary commands where direct CmdLet support
# is not provided.
#
function Invoke-DbgCommand(
    [switch]$noThrow, # TODO: implement error checking
    $command
){
    # Append additional arguments if supplied (makes calling this simpler)
    if($args){
        $command += ' ' + ($args -join ' ')
    }
    
    if(-not $SCRIPT:windbgProcess){
        throw 'Create a PowerDbg session using Connect-Dbg first';
    }
        
    # Create a special tag so that we can detect when we get to
    # the end of the command response 
    $sent = "PowerDbg_Complete_{0:HHmmss.fff}" -f [DateTime]::Now  
    
    # Move to end of output stream, discarding anything in buffer
    # (this caters for interactive user-generated output from WinDBG
    # and also for race condition vaguaries as to when we see the command prompt back
    $read = ReadToBufferEnd $SCRIPT:windbgProcess.StandardOutput
    write-verbose "RX (discard) $read"
    
    # Send the command and our completion tag
    write-verbose "TX: $command"
    $SCRIPT:windbgProcess.StandardInput.WriteLine("$command");
    $SCRIPT:windbgProcess.StandardInput.WriteLine(".echo $sent");
    # Alternative approach using batching ($command ; .echo $sent)
    # tried, but doesn't seem any more reliable, 
    # and creates misleading error messages when syntax is incorrect

    # TODO: We should also add a timeout here
    # so if the command never comes back (or never detects the end command somehow)
    # we don't sit around forever

    # NB: Appears initially the streamreader for CDB doesn't support seeking ?!
    # (peek() reports -1)
    
    $hadData = $false;
    while(-not $SCRIPT:windbgProcess.StandardOutput.EndOfStream){
        
        # Read lines until we get the command end tag        
        $line = $SCRIPT:windbgProcess.StandardOutput.ReadLine();
        write-verbose "RX: $line";
    
        if ($line -eq $null){
            throw 'EndOfStream reached'
        }
        
        # Strip any number of command prompts off the start of the line, if present
        # Were previously only doing this for the first line in each batch
        # but have seen cases where that wasn't enough
        # and the prompt came back interleaved with command response output
        # We will *always* get one prompt at the start of the command response
        # (that being the one that followed the PSWINDBG_COMPLETE previously)
        $match = $promptRegex.Match($line);
        while($match.Success){
            # capture the prompt, for use later
            $global:PowerDbgPrompt = $match.Captures[0].Value;
            Write-Debug "Captured prompt $($global:PowerDbgPrompt)"
            Write-Debug "Line now '$line'"
            
            # loop round
            $line = $line.Substring($match.Length);
            $match = $promptRegex.Match($line);
        }

        if ($line.EndsWith($sent)){
            # Straight equality check is preferred, however some of the v5 cmdlets send CRLF sequences
            # This exposed that there are still cases where we can get the 'end' indicator prompt-prefixed
            # rather than (as is supposed to happen) on a line on its own
            # (ie if they can break it, anyone can)
            # so this is safer
            write-verbose "RX Complete"
            break;
        }
        
        switch -regex ($line){
            '^Invalid parameter' {}
            '^No export [^$]+ found$' {}
            '^Failed to load data access DLL' {
                # clear the remaining data in the buffer
                $read = ReadToBufferEnd $SCRIPT:windbgProcess.StandardOutput

                # Throw that line as an error
                throw "Command '$command' failed: $line"
                
                break;
            }
            
            default {
                # release the line to the pipeline
                $line;
                $hadData = $true;
            }
        }
    }
    
    if($SCRIPT:windbgProcess.HasExited -and (-not $noThrow)) 
    { 
        throw "CDB process exited with error code $($SCRIPT:windbgProcess.ExitCode)"
    } 

}

# .Synopsis
# Allows the attached debugger to proceed to the next breakpoint.
# This command is only relevant for debugging sessions
# attached to live processes (ie not dumps)
function Send-DbgGo{
    Invoke-DbgCommand g
}
Set-Alias g Send-DbgGo

# .Synopsis
# Sends the 'break' command to the debugger, instructing it to enter a breakpoint.
# This command is only relevant for debugging sessions
# attached to live processes (ie not dumps)
function Send-DbgBreak{
    Invoke-DbgCommand "\x3" # Sends CTRL-C
}
# Set-Alias break Send-DbgBreak

# .Synopsis
# Loads an extension dll into the debugger
#
# .Description
# If only using the 'name' parameter, the 
# This command exposes the .load / .loadby debugger commands
function Load-DbgExtension{
    param(
        $name, # Name of the extension. Must be fully qualified path if loadByModule not specified
        $loadByModule # A loaded module, the location of which is used to infer the extension path
    )
    if($loadByModule){
        $results = Invoke-DbgCommand .loadby $name $loadByModule
    }else{
        $results = Invoke-DbgCommand .load $name
    }
    # If we get anything back, it's always bad
    # Just take care to not raise one error per error line, which is messy
    if($results){
        $errormessage = $results -join "`n"
        write-error $errormessage
    }
}

#############################
# Module-specific plumbing
if ($MyInvocation.MyCommand.ScriptBlock.Module){
    $MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = { Exit-DbgSession } 
   
    Export-ModuleMember -Function New-DbgSession
    Export-ModuleMember -Function Exit-DbgSession
    Export-ModuleMember -Function Invoke-DbgCommand
    Export-ModuleMember -Function Send-DbgGo
    Export-ModuleMember -Function Load-DbgExtension

    Export-ModuleMember -Alias *
}
#
# ----------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\PowerDbg.Util.ps1
#
#############################
# Get-DbgToolsLocation
#
# .Synopsis
# Auto-locates the Windows Debugging Toolkit
function Get-DbgToolsLocation(){

    if ($env:DebuggingTools){
        # Use the ambient environment variable
        write-verbose "Using debugging tools from $($env:DebuggingTools)";
        return $env:DebuggingTools;

    }else{
        # Set up locations to search in
        # Note that since env:programfiles changes depending on bittyness of powershell host
        # we will only load the 64 bit version in a 64 bit process
        # and we will only load the 32 bit version in a 32 bit process
        $searchPaths =  "$env:programfiles\Debugging Tools for Windows (x64)",
                        "$env:programfiles\Debugging Tools for Windows (x86)",
                        "C:\debuggers"  # for Microsoft internal installs
                        ;
        foreach($path in $searchPaths){
            if (test-path $path){
                $debuggerRoot = $path;
                break;
            }
        }
        if (-not $debuggerRoot){
            throw @"
Debugging Tools not located.
Supply 'debuggerRoot' parameter explicitly,
or set the DebuggingTools environment variable.
"@;
        }
        write-verbose "Using debugging tools from $debuggerRoot";
        return $debuggerRoot;
    }
}

#############################
# Start-DebuggerOnly
#
# .Synopsis
# Launches one of the Windows Debuggers (WinDbg by default), and returns the process handle.
# This cmdlet is for convenience only, and *does not establish a PowerDbg session*
# See also Connect-Debugger
#
function Start-DebuggerOnly{
    [CmdLetBinding()]
    param(
        [string]$process, # Attaches the debugger to the specified process
        [IO.FileInfo]$dump, # Attaches the debugger to the specified memory dump file
        [string]$server, # Start the debugger as a server, using the address supplied
        [string]$remote, # Connects to a remote debugger server on the address supplied
        [string]$command, # Launches a process under the debugger. This command line is executed as-is
        [switch]$captureIO, # Start the debugger with StdIn/StdOut streams redirected to the process object
        $debugger="WinDBG.exe",
        $debuggerRoot
    )
    
    # allow the user to specify 'cdb' not 'cdb.exe' if they want a different debugger
    if ([io.path]::GetExtension($debugger) -eq ''){
        $debugger += '.exe';
    }
    
    # locate debugger root if not specified
    if(-not $debuggerRoot){
        $debuggerRoot = Get-DbgToolsLocation
    }
    
    $raw = '';
    $startArgs = @();
    if($debugger -eq "WinDbg.exe"){
        $startArgs +='-Q' # Prevent whinging about discarding default workspace
    }

    if($remote){
        # If present, -remote must be the first argument, so add it *at the front*
        $startArgs = ('-remote',$remote) + $startArgs
    }else{
        if ($server){
            $startArgs += '-server',$server
        }

        # These options are mutually exclusive, so only send one
        if ($process){
            $startArgs += '-pn',$process
        }elseif ($dump){
            $startArgs += '-z',$dump.FullName
        }elseif($command){
            $startArgs +='-2' # Create a seperate console window for debugee
            $raw = $command;
        }
    }
    
    # Flatten args into a string
    $commandLine = PrepareCommandArgs $startArgs
    # Suffix command, if specified, but treat this opaquely (no parsing/preparing)
    if($command){
        $commandLine += ' ' + $raw;
    }    
       
    $startInfo = New-Object System.Diagnostics.ProcessStartInfo 
    $startInfo.FileName = "$debuggerRoot\$debugger" 
    $startInfo.WorkingDirectory = (Get-Location).Path 
    $startInfo.Arguments = $commandLine;
    $startInfo.UseShellExecute = $false;
    $startInfo.RedirectStandardInput = $captureIO;
    $startInfo.RedirectStandardOutput = $captureIO;
    $startInfo.RedirectStandardError = $captureIO;

    write-verbose ('{0} {1}' -f $startInfo.FileName,$startInfo.Arguments);

    if ($VerbosePreference -eq 'continue'){
        $startInfo | format-list | out-string | write-verbose
    }

    [System.Diagnostics.Process]::Start($startInfo) # nb: returns process handle as function return
}

<#
.Synopsis
Breaks into a process (invasively) and takes an immediate memory dump
The process will be terminated when the debugger detaches
#>
function Get-DbgHangDump{
    param(
        [Parameter(Mandatory=$true)]
        $process,      # the process to attach to
        $output,       # output dump path, defaults to temp dir + pattern based filename
        [switch]$crash # waits for the first exception before taking the dump. The default is to dump directly (hang mode)
    )
    
    $debuggerInput = "$env:temp\PowerDbg_CDB.Script"
    
    if(-not $output){
        $output = '{0}\{1}_HangMode_{2:yyyyMMdd-HHmmss}.dump' -f $env:temp,$process,(get-date)
    }
    
    @"
.dump /o /ma $output
Q
"@ | out-file $debuggerInput -encoding:ASCII

    $dbgcommand = @"
-pn $process -c "`$<$debuggerInput"
"@

    $debugger = Start-DebuggerOnly -debugger:CDB -command:$dbgcommand
    $debugger.WaitForExit()
    Assert-FileExists $output
}

# .Synopsis
# Prepares a 'header data' hashtable structure for use with Parse-Table
#
# .Description
# Expecting a header like this (right hand column = variable length):
# ID=4,OSID=4,ThreadOBJ=16!,State=8,GC=8,GC Alloc Context=33,Domain=16!,Lock Count=4,APT=4,Exception=-1
#
function Crack-Header([string]$headerData){
    $offset = 0;
    $headerData.Split(',') | % { 
        $parts = $_.Split('=');
        $name = $parts[0].Trim();
        $value = $parts[1].Trim();
        $exact = $false;
        #if ($value.EndsWith('!')){
        #    $exact = $true;
        #    $value = $value.TrimEnd('!');
        #}
        $length = [int]$value;
        
        new-object PSObject -Property @{
            Name = $name;
            Start = $offset;
            End = $offset + $length;
            Length = $length;
            # Exact = $exact;
        };
        $offset += $length + 1;
    }
}

# .Synopsis
# Parses a fixed-width-column text table, into PSObject data, based on a 'header data' structure
# One variable length column is allowed, typically the right hand one
# This is denoted by a negative length in the header metadata (see above)
function Parse-Table(
    $headerData,
    [switch]$verbose # don't tie this to VerbosePreference because this produces *ultra-verbose* output
)
{
    begin{}
    process{
        $line = $_;
        if (-not $line) { return; }
        trap{
            write-host "Failed to parse:"
            write-host "$line"
            break;
        }
        # NB: Objects you add properties to dynamically (using Add-Member) 
        # need to created as PSObject (not Object)
        # or the added members are lost when you add them to an ArrayList (!)
        $item = new-object PSObject;
        $start = 0;
        foreach($header in $headerData){
            if ($start -gt $line.length){
                $value = '';
                
            }elseif ($header.Length -gt 0){

                # Extract a fixed width column (but don't overrun the input string...)                
                $cutWidth = [Math]::Min($header.Length, $line.length - $start)
                $value = $line.Substring($start,$cutWidth);
                $start += $header.Length + 1; # 1 = the column seperator width
                
            }else{
            
                # Extract a variable-length column (normally the rightmost)
                # This needs to always be the last column, or everthing falls apart :-/
                $value = $line.Substring($start);               
            }
            $name = $header.Name;
            
            write-verbose "Cut $name='$value'" -verbose:$verbose
            $value = $value.Trim();
            Add-Member NoteProperty -inputObject:$item -name:$name -value:$value;
        }
        $item
    }
    end{}
}

function Split-Line(){
    param(
        [Parameter(Mandatory=$true)]
        $line,
        [Parameter(Mandatory=$true)]
        [string[]] $headers
    )
    $parts = $line.Trim() -split '\s+',$headers.length;
    $output = new-object PSObject
    for($i=0; $i -lt $headers.length; $i++){
        Add-Member NoteProperty -InputObject $output -Name:$headers[$i] -Value:$parts[$i]
    }
    $output;
}

function New-ObjectFromRegexMatch(){
    param(
        [Parameter(Mandatory=$true)]
        [system.text.regularexpressions.match] $match,
        [Parameter(Mandatory=$true)]
        [string[]] $headers        
    )
    $new = New-Object psobject
    foreach($name in $headers){
        $value = $match.Groups[$name].Value.Trim()
        Add-Member -InputObject:$new -MemberType NoteProperty -Name:$name -Value:$value
    }
    $new
}

# .Synopsis
# Converts an array of arguments into a single string, 
# quoting as required to preserve whitespace
# and escaping existing quotes using the backslash
function PrepareCommandArgs([string[]] $commandArgs){
    # see http://weblogs.asp.net/jgalloway/archive/2006/10/05/_5B002E00_NET-Gotcha_5D00_-Commandline-args-ending-in-_5C002200_-are-subject-to-CommandLineToArgvW-whackiness.aspx
    # and http://msdn.microsoft.com/library/default.asp?url=/library/en-us/shellcc/platform/shell/reference/functions/commandlinetoargv.asp
    # pity we can't just call that actually, well we probably could if we p/invoked
    
    ($commandArgs | % {
        $arg = $_.Replace('"','\"');  # always escape quotes
        if ($arg.Contains(' ')){
            '"' + $arg + '"' # wrap in quotes if arg contains whitespace
        }else{
            $arg;
        }
    }) -join ' '
}

function Assert-Equal(
    $expected,
    $actual,
    $message
){
    if (-not ($expected -eq $actual)){
        $fullmessage = "$expected != $actual : $message" -f $args
        throw $fullMessage
    }
}

function Assert-True(
    $actual,
    $message
){
    if (-not $actual){
        $fullmessage = $message -f $args
        throw $fullMessage
    }
}

function Assert-NotNull(
    $expected,
    $message
){
    if ($expected -eq $null){
        throw ($message -f $args)
    }
}

function Assert-FileExists(
$path
){
    if (-not (test-path $path)) { 
        throw new-object System.IO.FileNotFoundException "$path not found",$path 
    }
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    Export-ModuleMember -Function Start-DebuggerOnly
    Export-ModuleMember -Function Get-DbgHangDump
}
#
# ----------------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\PowerDbg.ModuleInit.ps1
#

# Control verbosity within the module
# Unless we do this, or handle it on each cmdlet
# the 'ambient' $VerbosePreference doesn't seem to transmit
# to the module
if ($verbose) { $VerbosePreference = 'continue' }

# Auto-locate the Windows Debugging Toolkit if not specified in module args
if (-not $debuggerRoot){
    $debuggerRoot = Get-DbgToolsLocation
}
# Publish the location as an environment variable so test scripts 
# or the 'open dump' script can spin up WinDBG for us to attach to
$env:DebuggingTools = $debuggerRoot;

$SCRIPT:windbgProcess = $null 
$SCRIPT:currentConnection = $null 

$hostPtrSize = [IntPtr]::Size
switch($hostPtrSize){
    4 { $SCRIPT:hostbits = 32; break; }
    8 { $SCRIPT:hostbits = 64; break; }
    default { throw "Unsupported pointer size '$hostPtrSize'" ; break; }
}
write-verbose "Running in $script:hostbits bit process"
#
# ---------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgArray.ps1
#
<#
Get-DbgArray

.Synopsis
Executes SOS's !dumparray command, and returns the output as PSObjects
#>
function Get-DbgArray(
    [Parameter(Mandatory=$true)]
	$address,
    [int]$start = 0,
    [int]$length,
	[switch]$raw,
	[switch]$details
){
    if ($raw){
        Invoke-DumpArray -address:$address
    
    }elseif ($details){
        Invoke-DumpArray -address:$address -start:$start -length:$length -details:$details | 
        PreProcess-DumpArray | 
        Parse-DumpObject

    }else{
        Invoke-DumpArray -address:$address | Parse-DumpArray
    }
}

# In this case the invoke is explicitly broken out
# because we re-use this from other cmdlets, with
# alternate parsers
function Invoke-DumpArray(
    $address,
    [int]$start = 0,
    [int]$length,
    [switch]$details
){
    trap{
        write-host "Failed dumping array with address $address";
        break;
    }
    $command = '!da ';
    if ($start -gt 0){
        $command += "-start $start ";
    }
    if ($length -gt 0){
        $command += "-length $length ";
    }
    if ($details){
        $command += '-details '
    }
    $command += $address;
    
    Invoke-DbgCommand $command;
}

# Strips out the extra header you get from dump array
# do we can parse the raw object data with Parse-Object
function PreProcess-DumpArray(){
    begin{
        $state = 0;
    }
    process{
        $line = $_;
        # [0] 00000000024e2dd8
        if ($line -match '\[\d+\]'){
            $state = 1; # Now reading item headers
            
            # Spit out the object address for parse-dumpobject to consume
            "Address: " + ($line -split ' ')[-1]
            
        }elseif($line){	# Ignore blank lines (tends to be one at the end)
            switch($state){
                0 {
                    # we basically throw away the array header info
                }
                1 { 
                    # emit the lines which are Get-DbgObject output
                    $line.substring(4);
                }
            }
        }
    }
    end{}
}

function Parse-DumpArray(){
    begin{
        $values = @{};
    }
    process{
        $line = $_;
        
        if($line -match '[\w\s]+\:'){
            $parts = $line.Split(':');
            $values.Add($parts[0],$parts[1].Trim());
            
        }elseif ($line -match '\[(\d+)\] ([^$]+)'){
            $index = $matches[1];
            $value = $matches[2].Trim();
            
            $arrayItem = new-object PSObject -Property $values;

            Add-Member NoteProperty -inputObject $arrayItem "Index" $index;
            Add-Member NoteProperty -inputObject $arrayItem "Address" $value;
            
            $arrayItem;
        }    
    }
    end{}
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    Set-Alias da Get-DbgArray
    Set-Alias dumparray Get-DbgArray

    Export-ModuleMember -Function Get-DbgArray -Alias *
}
#
# -----------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgClrHeap.ps1
#
<#
Get-DbgClrHeap

.Synopsis
Executes SOS's !dumpheap command, and returns the output as PSObjects
Parameters are as per the SOS command - see http://msdn.microsoft.com/en-us/library/bb190764.aspx
If -stat param is used, the addresses of instances will not be available

NB: Thoughts on group-object
could just flatten the whole thing, and expect the user to use group-object
however you'd basically lose the 'TotalSize' column then
This implementation produces more or less what you'd get out of group-object
but potentially more immediately, due to lookup pattern employed
#>
function Get-DbgClrHeap(
    [string]$type, # (partial) type name to filter returned results
    [string]$mt,   # filter based on methodtable
    [switch]$stat, # Generates type stats only, doesn't list instances
    [switch]$details, # Expands addresses into objects. Best used with 'MT'
    [switch]$raw
){
    if ($raw){
        Invoke-DumpHeap @PSBoundParameters

    }elseif($details){       
        Invoke-DumpHeap @PSBoundParameters | Parse-DumpObject
        
    }else{
        Invoke-DumpHeap @PSBoundParameters | Parse-DumpHeap    
    }
}

function Invoke-DumpHeap(
    [string]$type,
    [string]$mt,
    [switch]$stat,
    [switch]$details
){
    # DumpHeap [-stat] [-min <size>][-max <size>] [-thinlock] [-mt <MethodTable address>] [-type <partial type name>][start [end]]

    # also available is -short which spits out object addresses only
    # this is most handy when you execute this using the MT parameter
    # (so you know exactly what you are getting already)
        
    $command = "!dumpheap"
    if ($stat){ $command += " -stat" }
    if ($mt)  { $command += " -mt $mt" }
    if ($details) { $command += " -short" }
    if ($type){ $command += " -type $type" }
    
    if($details){
        # Attempt to get the debugger to do the dumpobject call on the server
        # Concatenation here much less brittle than (very delecate) use of escape character
        $command = '.foreach ( obj { ' + $command + ' } ) { .echo "Address: ${obj}" ; !do ${obj} }'
    }
    Invoke-DbgCommand $command
}

function Parse-DumpHeap(){
    begin{
        $state = 'wait';
        # $instances = @(); # empty array
        $instancesLookup = @{}; # hashtable
    }
    process{
        $line = $_;
        switch($state){
            'wait' {
                if ($line -match '^\s+Address'){
                    $state = 'instances';
                }elseif($line -match '^\s+MT'){
                    $state = 'stats';
                }
            }
            'instances' {
                if ($line -match '^\s*[\da-f]{8,16}'){
                    $instance = Split-Line $line @('Address','MT','Size')
                    
                    # Build the lookup as we go
                    # Previous implementation seemed to have some kind of race in it
                    # very odd
                    # NB: Because instances are custom PSObjects, don't store them in a List or ArrayList
                    # otherwise all their add-member NoteProperties are lost...
                    $list = $instancesLookup[$instance.MT];
                    if ($list -eq $null){
                        $list = @($instance);
                    }else{
                        $list += $instance;
                    }
                    $instancesLookup[$instance.MT] = $list;
                }else{
                    # Turn instances into a lookup
                    # $instances | Group-Object MT | % { 
                    #    $instancesLookup.Add($_.Name, $_.Group);
                    #};
                    $state = 'wait';
                }
            }
            'stats' {
                if ($line -match '^\s*[\da-f]{8,16}'){
                    $stat = Split-Line $line @('MT','Count','TotalSize','ClassName');
                    # attach instances if present
                    $typeInstances = $instancesLookup[$stat.MT];
                    Add-Member -inputObject:$stat NoteProperty 'Instances' $typeInstances;

                    $stat # finally emit the object
                }else{
                    $state = 'wait';
                }            
            }
        }
    }
    end{
    }
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    Set-Alias dumpheap Get-DbgClrHeap

    Export-ModuleMember -Function Get-DbgClrHeap -Alias *
}
#
# --------------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgClrThreads.ps1
#
<#
Get-DbgClrThreads

.Synopsis
Executes SOS's !threads command, and returns the output as PSObjects
Parameters are as per the SOS command - see http://msdn.microsoft.com/en-us/library/bb190764.aspx
#>
function Get-DbgClrThreads(
    [switch]$live,      # Shows only live threads
    [switch]$special,   # Includes special CLR threads
    [switch]$raw
){
    $command = "!threads";
    if ($live) { $command += " -live" };
    if ($special) { $command += " -special" };

	if($raw){
	    Invoke-DbgCommand $command
    }else{
	    Invoke-DbgCommand $command | Parse-ClrThreads -bits:$SCRIPT:bits
    }
}

# $clrThreadsHeaders32 = Crack-Header "DbgID=4,ID=4,OSID=4,ThreadOBJ=9,State=8,PreEmptiveGC=8,GCAllocContext=17,Domain=8,LockCount=4,APT=3,Exception=-1"
# $clrThreadsHeaders64 = Crack-Header "DbgID=4,ID=4,OSID=4,ThreadOBJ=16,State=8,PreEmptiveGC=8,GCAllocContext=34,Domain=16,LockCount=4,APT=3,Exception=-1"
$clrThreadsRegex = new-object System.Text.RegularExpressions.RegEx (@'
^\s*(?<DbgId>[\da-f]{1,4}|X{4}) # DbgId, or XXXX
\s+(?<Id>[\da-f]{1,4})          # Id 4 hex
\s+(?<OsId>[\da-f]{1,4})?       # OsId 4 hex
\s+(?<ThreadObj>[\da-f]{16}|[\da-f]{8})       # ThreadObj 8/16 hex
\s+(?<State>[\da-f]{1,8})       # State up to 8 hex (min 4?)
\s+(?<PreEmptiveGC>Enabled|Disabled)
\s+(?<GCAllocContext>(?:[\da-f]{16}|[\da-f]{8}):(?:[\da-f]{16}|[\da-f]{8})) # 8/16h:8/16h
\s+(?<Domain>[\da-f]{16}|[\da-f]{8}) # Domain is 8 or 16 hex
\s+(?<LockCount>[\da-f]{1,4})        # Lock count assumed up to 4 hex
\s+(?<Apt>MTA|STA|Ukn)               # COM model - are there others?
(\s+(?<Exception>.+))?
'@, 'IgnorePatternWhitespace');
$clrThreadsRegexFields = "DbgId,Id,OsId,ThreadObj,State,PreEmptiveGC,GCAllocContext,Domain,LockCount,Apt,Exception" -split ','

function Parse-ClrThreads(
#    [int]$bits = 64
){
    begin{
        # Will also need to reset this per loop if piping multiple lots of data, but can do that another day...
        $headerRead = $false
        # $header = $clrThreadsHeaders64;
        # if ($bits -eq 32){
        #    $header = $clrThreadsHeaders32;
        #}
    }
    process{
        $line = $_;
        if ($line -match '\s+ID\s+OSID'){
            $headerRead = $true;
        }elseif($headerRead -and $line){
            # $parsed = $_ | Parse-Table $header

            $match = $clrThreadsRegex.Match($line)
            if(-not ($match.Success)){
                Write-Verbose "Failing line $line"
                throw "Regex failing on '$line'"
            }

            $parsed = New-ObjectFromRegexMatch $match $clrThreadsRegexFields;
            $exception = $parsed.Exception
            if ($exception -match ' \(([\da-f]{16}|[\da-f]{8})\)$'){
                $bitToStrip = $matches[0]
                $exceptionAddress = $matches[1];
                Add-Member NoteProperty -inputObject:$parsed ExceptionAddress $exceptionAddress
                
                $parsed.Exception = $exception.Substring(0, $exception.Length - $bitToStrip.Length);
            }
            $parsed;
        }    
    }
    end{}
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    Set-Alias clrthreads Get-DbgClrThreads
    Export-ModuleMember -Function Get-DbgClrThreads -Alias *
}
#
# ------------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgComState.ps1
#
<#
Get-DbgComState

.Synopsis
Lists the COM apartment model for each thread, as well as a Context pointer if provided
#>
function Get-DbgComState(
    [CmdletBinding()]
    [switch]$raw
){
    if($raw){
        Invoke-DbgCommand "!comstate"
    }else{
        Invoke-DbgCommand "!comstate" | Parse-ComState -bits:$SCRIPT:bits
    }
}

$comStateHeader32 = Crack-Header "DbgId=3,OsID=4,TEB=8,APT=3,APTId=8,CallerTID=9,Context=8"
$comStateHeader64 = Crack-Header "DbgId=3,OsID=4,TEB=16,APT=3,APTId=8,CallerTID=9,Context=16"
      
filter Parse-ComState(
    [int]$bits = 64
){
    $header = $comStateHeader64
    if ($bits -eq 32){
        $header = $comStateHeader32
    }
    $line = $_;
    if ($line -match '\w+ID'){
        # ignore
    }else{
        $line | Parse-Table $header
    }
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    Set-Alias comstate Get-DbgComState

    Export-ModuleMember -Function Get-DbgComState -Alias *
}
#
# --------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgData.ps1
#
<#
Get-DbgData

.Synopsis
Displays raw data from the debugee
#>
function Get-DbgData{
    param(
        $address,
        [int]$length
    )
    
    $callArgs = 'dd','/c1',$address
    if ($length -gt 0){
        $callArgs += "L$length"
    }
    Invoke-DbgCommand @callArgs | Parse-dd
}

Set-Alias dd Get-DbgData

filter Parse-dd{
    # At the moment the parse function requires single-dword-per-line input
    # though it wouldn't be _too_ hard to make it parse the default 4 dword output format
    $parts = -split $_
    Select-Object -InputObject:$parts `
        @{Name='Address';Expression={$_[0]}}, `
        @{Name='Value';Expression={$_[1]}}
}
#
# ----------------------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgDictionaryContents.ps1
#
<#
Get-DbgDictionaryContents

.Synopsis
Returns the contents of a dictionary as a list of key,value object addresses

.Description
The (suppled) address of the dictionary is used to locate the backing array, resolve
each dictionary entry and extract that entry's key and value address fields.
The output from this can easily be piped to Get-DbgObject to expand the key or values
into their original object representations
#>
function Get-DbgDictionaryContents(
    [string]$address
    # ,[switch]$expandKeys # todo
    # ,[switch]$expandValues # todo
){

    $dictionary = Get-DbgObject $address

    if (-not ($dictionary.__Name -match '^System\.Collections\.Generic\.Dictionary`2\[\[')){
        throw "Object at $address does not appear to be a Dictionary<T1,T2>, is a $($dictionary.__Name)";
    }
	
	Get-DbgArray $dictionary.entries -length:$dictionary.count -details | Select-Object key,value
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    Set-Alias dumpdict Get-DbgDictionaryContents

    Export-ModuleMember -Function Get-DbgDictionaryContents -Alias *
}
#
# ---------------------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgHashtableContents.ps1
#
<#
.Synopsis
Enumerates the key/value pairs contained within a hashtable
#>
function Get-DbgHashtableContents{
    param(
        [Parameter(Mandatory=$true)]
        $address
    )
    
    $object = Get-DbgObject $address;
    $buckets = Get-DbgArray $object.buckets -details;
    # Output the key/value pairs from the buckets
    # Note that the bucket field is actually 'val' so we alias that to 'value'
    # to be consistent with dictionaries (and also the .net enumerator object for a hashtable)
    $buckets | ? { $_.val -ne '00000000'; } | select-object key,@{Name='value';Expression={$_.val}};
}
#
# ----------------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgListContents.ps1
#
<#
Get-DbgListContents

.Synopsis
Returns the contents of a List<T> as object addresses, or resolved objects (using -details)

.Description
The (supplied) address of the list is simply used to locate the backing array, and
return the 'in use' portion of the array

.Link
 
#>
function Get-DbgListContents(
    [string]$address,
    [switch]$details # whether the contents are returned as addresses, or expanded to objects
){

    $list = Get-DbgObject $address

    if (-not ($list.__Name -match '^System\.Collections\.Generic\.List`1\[\[')){
        throw "Object at $address does not appear to be a List<T>, is a $($list.__Name)";
    }
	
	Get-DbgArray $list._items -length:$list._size -details:$details
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    Set-Alias dumplist Get-DbgListContents

    Export-ModuleMember -Function Get-DbgListContents -Alias *
}
#
# -----------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgModules.ps1
#
<#
.Synopsis
Gets the modules currently loaded (or deferred) within the process being debugged
This command executes 'lm k' against the debugger

.Notes
This is a streaming function. Do not make calls to Invoke-DbgCommand
based on output from this function within the process{} block of your function.
#>
function Get-DbgModules {
    Invoke-DbgCommand lm k | Parse-ListLoadedModulesK
}

function Parse-ListLoadedModulesK {
    begin{
        $firstLine = $true;    
    }
    process{
        if($firstLine){
            $firstLine = $false
        }else{
            # Pretty straightforward here
            # Just split on whitespace, collapsing multiple whitespace into a single break
            select-object -InputObject ($_.Split((,' '),'RemoveEmptyEntries')) `
                @{Name='Start';Expression={$_[0]}}, `
                @{Name='End';Expression={$_[1]}}, `
                @{Name='Name';Expression={$_[2]}}, `
                @{Name='Path';Expression={$_[3]}} `
        }
    }
    end{
    }
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    Set-Alias lm Get-DbgModules

    Export-ModuleMember -Function Get-DbgModules -Alias *
}
#
# ----------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgObject.ps1
#
<#
Get-DbgObject

.Synopsis
Uses !dumpobj to retrieve the structure of an object at an address,
and returns a custom PSObject that mimics the original object

.Description
The returned object will have:
Fields (containing addresses) that match the original object fields
A __Fields array, containing the detailed field metadata from SOS
Other instance level metadata as __prefixed properties (eg __MethodTable)

Accepts either an address, or objects on the pipeline (from other cmdlets)
Pipeline data can be:
    object address (strings or ints)
	objects with an 'Address' property
	objects with an 'Address' property and a 'Element Methodtable' property (triggers Get-DbgValueClass)

Use the -raw switch to see the textual response as retrieved from SOS, without parsing

.Notes
Objects returned will (normally) have an __Address field added by PowerDbg
(ie: in addition to what you'd see in the SOS output)
This simplifies piping objects around, but care should be taken not to assume it's
actually come from the object metadata somehow, because it ain't there (of course)
#> 
function Get-DbgObject(
    [CmdLetBinding()]
    $address, # The address of the object to retrieve. This can also be piped in
    [switch]$raw # Disable parsing and return the raw SOS output

# nb: not currently using PropertyFromPipeline / PropertyFromPipelineByPropertyName as issues with property naming
# should look more at this in the future

){
    begin{
        if ($address) {
            if ($raw){
                Invoke-DumpObject $address
            }else{
                Invoke-DumpObject $address | Parse-DumpObject
            }
        }
    }
    process{
        if ($_){
            if($raw){
                Get-DbgObjectInternal $_
            }else{
                Get-DbgObjectInternal $_ | Parse-DumpObject
            }
        }
    }
    end{
    }
}

# .Synopsis
# Internal function used by dump object to determine what type of dump to do
# If pipeline data is provided, then we can select to Get-DbgValueClass 'smartly'
function Get-DbgObjectInternal(
    [CmdletBinding()]
    $item
){
    if ($item."Element Methodtable"){
        Write-Verbose "Dumping ValueClass $($item.'Element Methodtable') $($item.Address)" ;
        Invoke-DumpValueClass $item."Element Methodtable" $item.Address;
        
    }elseif($item.Address){
        Write-Verbose "Dumping Object $($item.Address)" ;
        Invoke-DumpObject $item.Address;

    }else{        
        Write-Verbose "Dumping Object $item" ;
        Invoke-DumpObject $item;
    }
}


# .Synopsis
# Uses !dumpvc to retrieve the structure of a value-class at an address (given a method table),
# and returns a custom PSObject that mimics the original object
# See Get-DbgObject for more details
function Get-DbgValueClass(
    $methodTable, # The methodtable for the value class (this is stored on the containing class/array)
    $address # The address of the value class
){
    Invoke-DumpValueClass $methodTable $address | Parse-DumpObject ;
}


function Invoke-DumpObject($address){
    trap{
        write-host "Failed dumping object with address $address";
        break;
    }

    # put the input address into the output pipeline so it gets stuck on the object too
    "Address: $address"
    Invoke-DbgCommand "!do $address";
}

function Invoke-DumpValueClass($methodTable, $address){
    trap{
        write-host "Failed dumping valueclass with address $address, methodTable $methodTable";
        break;
    }
    "Address: $address"
    Invoke-DbgCommand "!dumpvc $methodTable $address"
}

# $dumpobjHeaders32 = Crack-Header "MT=8,Field=8,Offset=8,Type=20,VT=2,Attr=8"
# $dumpobjHeaders64 = Crack-Header "MT=16,Field=8,Offset=8,Type=20,VT=2,Attr=8"
$dumpobjRegex = new-object System.Text.RegularExpressions.Regex (@'
^\s*(?<MT>[\da-f]{8}|[\da-f]{16})     # MT is always 8 or 16 hex
\s+(?<Field>[\da-f]{1,8})    # Field is up-to 8 hex
\s+(?<Offset>[\da-f]{1,8})   # Offset is up-to 8 hex
\s+(?<Type>[ \.\w,-_\[\]\`]{0,24})? # Type is 20 chars, 4 extra w/sp in .net 4, optional
\s+(?<VT>[01])                  # VT? 0 or 1
\s+(?<Attr>static|instance|TLstatic|shared\s+static)
\s+(?<Value>-?[\d\.a-f]+)?   # hex address or numeric value
\s*(?<Name>[^\s]+)           #field name
'@, 'IgnorePatternWhitespace')
$dumpobjRegexFields = 'MT,Field,Offset,Type,VT,Attr,Value,Name' -split ','


# .Synopsis
# Parses the output from !dumpobj into discrete PSobjects
# Should now cope with both 32 bit and 64 bit SOS output
function Parse-DumpObject(){
    begin{
        # NB: We don't use the -Property syntax to create *all* the 'meta' fields
        # in the output object, because they are then sorted in hashtable order
        # rather than the 'natural' order they come out from WinDbg
        
        $fields = new-object System.Collections.ArrayList;
        $object = new-object PSObject -Property @{
            "__Fields" = $fields;
        };
        $parsingFields =  $false;
        
        # $dumpobjHeaders = $dumpobjHeaders64; # Default only - auto detected below
    }
    process{
        $line = $_;
        try{
            if ($line -match "^Fields"){
                # Ignore this line
                # Just tells us the table header is next...
                
            }elseif($line.Trim().StartsWith("MT")){
                # Table header
                $parsingFields = $true;
                Write-Verbose "Switching to parsing content"
                
            }elseif($line -match '^\w\(([^\)]+)\)$'){
                # The full path to the assembly
                Add-Member NoteProperty -inputObject:$object Assembly $matches[1];
                
            }elseif ($line -match "^(\w+\s*)\:\s?([^$]*)"){
                # One of the object headers, like Name, EEClass etc...
                if ($parsingFields){
                    # hit header data whislt parsing existing object
                    # assume this is because we have been piped data
                    # from multiple objects: ie emit the object being built:
			        $null = AddObjectMethods $object;
                    $object; # yield to pipeline
                    
                    # ...and start building another one
                    $fields = new-object System.Collections.ArrayList;
                    $object = new-object PSObject -Property @{
                        "__Fields" = $fields;
                    };
                    $parsingFields =  $false;
                }
                
                # Add the object headers to the output object
                $name = $matches[1].Trim();
                $value = $matches[2].Trim();
                Write-Verbose "Set header $name = $value"
                Add-Member NoteProperty -inputObject:$object "__$name" $value;
                
                # Use the EEClass header (which all objects have) to sniff if we need 32 bit parsing
                # if (($name -eq 'EEClass') -and ($value.length -eq 8)){
                #     $dumpobjHeaders = $dumpobjHeaders32;
                # }

            }elseif ($parsingFields -and ($line -match "^\s*[\da-f]{8,16} ")){
                # Actual object field data
                
                # The format here is a real pain in the rear.
                # Apart from the differences between 32 bit and 64 bit output,
                # the format itself is irregular, part fixed-width column, part whitespace seperated
                # (eg the type column can be blank, so must be parsed fixed-width
                # but the value column can overrun/underrun its column
                # We now use a regular expression that attempts to describe these various formats
                
                # Parse a line using regex
                $match = $dumpobjRegex.Match($line)
                if(-not ($match.Success)){
                    Write-Verbose "Failing line $line"
                    throw "Regex failing on '$line'"
                }
                $field = New-ObjectFromRegexMatch $match $dumpobjRegexFields;

                if($field.Attr -match '^shared'){
                    # normalise whitespace in 'shared static'
                    $field.Attr = $field.Attr -replace '\s+',' '
                }
                $fieldName = $field.Name;
                $fieldValue = $field.Value;
                               
                # Resolve backing fields for C# autoproperties back to their 'sensible' names
                # (Note the original name is retained in the __Fields array)
                if ($fieldName -match '<([^>]+)>k__BackingField'){
                    $fieldName = $matches[1];
                }
                
                # Add the field value as a 'simple' property on the output object
                # NB: We use -force so we cater for duplicated field names (eg from base classes)
                Write-Verbose "Adding field $($fieldName)";
                Add-Member NoteProperty -InputObject:$object -Name:$fieldName -Value:$fieldValue -Force;
                
                # Also add the complete field object to the __Fields list
                # allowing access to all the other metadata we captured
                [void] $fields.Add($field);

            }else{
                Write-Verbose "Ignoring line $line"
            }       
        }catch{
            write-warning $line;
            throw;
        }
    }
    end{
        # Emit the object being built:
        $null = AddObjectMethods $object;
        $object; 
    }
}

# .Synopsis
# Adds custom methods to expanded objects, to facilitate navigating between related objects
#
# .Notes
# Approach stolen from Lee Holmes with best intentions...
function AddObjectMethods($object){
    
    # type specific:
	switch -regex ($object.__Name){
    
		'^System\.Collections\.Generic\.Dictionary`2\[' { 
            Add-Member -InputObject:$object -MemberType ScriptMethod GetContents {
                Get-DbgDictionaryContents -Address $object.__Address -details
            }.GetNewClosure()
            break;			
		}

		'^System\.Collections\.Generic\.List`1\[' { 
            Add-Member -InputObject:$object -MemberType ScriptMethod GetContents {
                Get-DbgListContents -Address $object.__Address -details
            }.GetNewClosure()
            break;			
		}
        
   		'^System\.Collections\.Hashtable' { 
            Add-Member -InputObject:$object -MemberType ScriptMethod GetContents {
                Get-DbgHashtableContents -Address $object.__Address -details
            }.GetNewClosure()
            break;			
		}

	}
    
    # invariant:
    if ($object.__MethodTable){
        $mt = $object.__MethodTable
        # concatenation here much less brittle than delecate use of escape character0
        $command = '.foreach ( obj { !dumpheap -mt ' + $mt + ' -short } ) { .echo "Address: ${obj}" ; !do ${obj} }'

        Add-Member -InputObject:$object -MemberType ScriptMethod GetOtherInstances {
            Get-DbgClrHeap -mt:$mt -details
        }.GetNewClosure()
    }
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    # can't alias 'do' as is a powershell keyword (doh!)
    # Set-Alias do Get-DbgObject
    
    Set-Alias dumpobj Get-DbgObject
    Set-Alias dumpvc Get-DbgValueClass

    Export-ModuleMember -Function Get-DbgObject,Get-DbgValueClass -Alias *
}
#
# -------------------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgPerfmonCounters.ps1
#
$gcCounters = @"
GenCollection0
GenCollection1
GenCollection2
PromotedMemory
PromotedMemory1
PromotedFinalizationMem0
ProcessId
GenHeapSize0
GenHeapSize1
GenHeapSize2
TotalCommittedBytes
TotalReservedBytes
LargeObjectSize
SurviveFinalize
Handles
Alloc
LargeAlloc
InducedGCs
TimeInGC
TimeInGCBase
PinnedObjects
SinkBlocks
"@ -split "`n"

# .Synopsis
# Extracts current values for selected perfmon counters from the debugee
#
# .Notes
# Based on Roberto's WinDbg script:
# http://blogs.msdn.com/b/debuggingtoolbox/archive/2007/04/19/windbg-script-extracting-performance-monitor-counters-from-net-application.aspx
# Since this is all based on absolute positions within 
#
function Get-DbgPerfmonCounters{
    [CmdLetBinding()]
    param(
        [switch]$raw
    )
    
    $clrs = @(idc lm1m | ? { $_ -match 'mscor(wks|svr)' })
    if ($clrs.length -eq 0){
        throw 'CLR not loaded'
    }
    $clr = $clrs[0];
    
    # Get the pointer where the perfmon data is stored into temp register 0
    Invoke-DbgCommand "r @`$t0 = $clr!PerfCounters::m_pPrivatePerf";
    
    # Get the pointer for the GCCounters into temp register 1
    Invoke-DbgCommand 'r @$t1 = poi(@$t0) + @$ptrsize'
        
    # Create a command to dereference pointers starting at this address
    # for the length of the GC perfmon counters array (defined above)
    $command = new-object System.Text.StringBuilder
    for($i = 0; $i -lt $gcCounters.length; $i++){
        # Could actually just eval each one rather than use printf
        # however this allows us to embed the name in the output
        # which makes it harder for us to mis-assign values to counter names
        [void] $command.AppendFormat('.printf "{0}=%d\n", poi(@$t1+@$ptrsize*0x{1:x});', $gcCounters[$i], $i);
        [void] $command.AppendLine();
    }
    Invoke-DbgCommand $command | Parse-CounterData 'GCCounters' -raw:$raw
}

filter Parse-CounterData($category){
    if($raw){
        $_;
    }elseif($_){
        $parts = $_ -split '=';
        select-object -InputObject:$parts `
            @{Name='Category';Expression={$category}},`
            @{Name='Counter';Expression={$_[0]}},`
            @{Name='Value';Expression={$_[1]}}
    }
}
#
# -----------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\Get-DbgRunaway.ps1
#
<#
# Get-DbgRunaway
#
# .Synopsis
# Retrieves information about the time consumed by each thread (kernel mode vs user mode).
#>
function Get-DbgRunaway(
    [CmdletBinding()]
    [switch]$raw
){
    if($raw){
        Invoke-DbgCommand "!runaway 3"
    }else{
        Invoke-DbgCommand "!runaway 3" | Parse-Runaway
    }
}

$runawayRegex = new-object system.text.regularexpressions.regex("\s+(\d+)\:([\da-f]+)\s+(\d+) days ([\d\:\.]+)")

function Parse-Runaway(){
    begin{
        $timeColumn = '';
        $items = @{}; # use a hashtable as just an easy lookup structure
    }
    process{
        $line = $_;
        if ($line -match 'User Mode Time'){
            $timeColumn = 'UserMode';
        }elseif($line -match 'Kernel Mode Time'){
            $timeColumn = 'KernelMode';
        }elseif($line -match 'Thread'){
            # ignore
        }else{
            # parse a line relating to a thread's wait time
            $match = $runawayRegex.Match($line);
            
            $debuggerThreadId = $match.Groups[1].Value;
            $osThreadId = $match.Groups[2].Value;
            $days = $match.Groups[3].Value;
            $time = $match.Groups[4].Value;
            
            # locate the output item for this thread (or create it if we didn't see it yet)
            $item = $items.Item($debuggerThreadId);
            if (-not $item){
                $item = new-object psobject -Property @{DbgID=$debuggerThreadID;OsId=$osThreadId};
                $items.Add($debuggerThreadID,$item);
            }
            $timespan = [timespan]::Parse("$days.$time");
            
            # add the time to the item in the appropriate column / property
            Add-Member -InputObject:$item NoteProperty $timeColumn $timespan;
         }
    }
    end{
        # yeild the items we put in the hashtable
        $items.Values;
    }
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    Set-Alias runaway Get-DbgRunaway

    Export-ModuleMember -Function Get-DbgRunaway -Alias *
}
#
# ----------------------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\PowerDbg.DumpStackObjects.ps1
#

function PowerDbg-DumpStackObjects(){
    PowerDbg-DumpStackObjects-Raw | PowerDbg-DumpStackObjects-Parse
}

function PowerDbg-DumpStackObjects-Raw(){
    trap{
        write-host "Failed !dumpstackobjects";
        break;
    }

    Invoke-DbgCommand "!dumpstackobjects"
}

$dsoHeader = Crack-Header "RSP/REG=16,Object=16,Name=-1";

function PowerDbg-DumpStackObjects-Parse(){
    begin{}
    process{
        if ($_ -match "^\d+"){
            $_ | Parse-Table $dsoHeader
        }
    }
    end{}
}

if ($MyInvocation.MyCommand.ScriptBlock.Module){
    Set-Alias Dump-StackObjects PowerDbg-DumpStackObjects
    Set-Alias dso PowerDbg-DumpStackObjects
    
    Export-ModuleMember -Function PowerDbg-DumpStackObjects -Alias *
}
#
# --------------------------
# C:\dev\PowerDbg\dev\src\ModuleParts\PowerDbg.v5Compat.ps1
#
########################################################################################################
# PowerDbg v5.3
#
# New for 5.3:
#
# Fixed small bug in Parse-PowerDbgDSO. Thanks to Igor Dvorkin.
#
# New Features for 5.2:
#
# - Easy installation.
# - Just one file.
# - Faster, results are cached in memory not using POWERDBG.LOG anymore.
#
# The new changes are based on suggestions from:
# Brad Linscott - Premier Field Engineer
# Lee Holmes    - Software Development Engineer - http://www.leeholmes.com/blog/default.aspx
#
# Note: PowerDbg v5.2 breaks compatibility with some of my previous scripts, however, they were already changed
#       and compatiblized. You just have to download them again.
#       They are:
#       - PowerDbgScriptDumpDict.ps1
#       - PowerDbgScriptExceptions.ps1
#
# PowerDbg v 5.1
#
# New Features for 5.1:
#
# - Load-PowerDbgSymbols
# - Parse-PowerDbgASPXPAGES
# - Parse-PowerDbgCLRSTACK
# - Parse-PowerDbgTHREADS
# - Parse-PowerDbgDSO
#
# New Features for 5.0:
#
# - No more SendKeys communication! Now PowerDbg uses the approach created by Lee Holmes:
#   http://www.leeholmes.com/blog/ScriptingWinDbgWithPowerShell.aspx
# - Easier to use and much more performance.
#
# New Features for 4.0:
#
# - Send-PowerDbgCTRL-BREAK
# - Parse-PowerDbgGCHANDLELEAKS
# - Parse-PowerDbgDUMPOBJ
# - Parse-PowerDbgDD-L1
# - Send-PowerDbgResumeExecution
# - Increased default timeout to 1 hour
# - Fixed three subtles bug in Send-PowerDbgCommand.
#
# New Features for 3.1:
#
# - Parse-PowerDbgPRINTEXCEPTION
# - No more Sleep calls to wait until a command is finished. Now, whenenever a command is sent to the debugger
#   the cmdlet Send-PowerDbgCommand waits until the command finish. Using this approach there won't happen
#   errors when you send a command that may take a long time to execute. At the same time the scripts using
#   PowerDbg don't need to implement delays anymore. It's a big performance improvement.
# - The script were revised and changed to be compatible with this new PowerDbg. To compatibilize I've
#   removed start-sleep and the regular "r" command (without arguments). Get the newest version.
# - Fixed bug that appeared when using WinDbg from the default path or when using a path that has
#   spaces, like: c:\Program Files
#
# For information about usage or to download PowerDbg scripts read: 
# http://blogs.msdn.com/debuggingtoolbox/archive/tags/PowerDbg+Library/default.aspx
#
# Roberto Alexis Farah
# Copyright (c) Microsoft Corporation.  All rights reserved. 
########################################################################################################

# Global variables.
########################################################################################################

$global:g_fileParsedOutput       = "POWERDBG-PARSED.LOG"
$global:g_CSVDelimiter           = ","
$global:g_frameDelimiter         = "#$#@"
[string] $global:g_commandOutput = ""

########################################################################################################
# .Description
# Backwards-compatability shim to keep existing v5 cmdlets working
function Send-PowerDbgCommand
{ 
    # Basically we only populate the global string for explicitly backwards-compat commands
    $lines = @(Invoke-DbgCommand $args)
    [string] $global:g_commandOutput = [string]::Join("`r`n", $lines) + "`r`n";
    # nb: trailing crlf is to mimic legacy behaviour exactly
    # whether strictly required or not I'm not sure
} 


########################################################################################################
<#
.SYNOPSIS
    Maps the output from the "dt" command using a hash table and saves the output into the file POWERDBG-PARSED.LOG.
.DESCRIPTION
    Maps the output from the "dt" command using a hash table and saves the output into the file POWERDBG-PARSED.LOG.
    All Parse functions should use the same outputfile.
    You can easily map the POWERDBG-PARSED.LOG to a hash table.
    Convert-PowerDbgCSVtoHashTable() does that.

.PARAMETER useFieldNames
    Switch flag. If $useFieldNames is present then the function saves the field
    names from struct/classes and their values. Otherwise, it saves the offsets
    and their values.
                                         
.OUTPUTS
    Nothing

.EXAMPLE
    Parse-PowerDbgDT

.EXAMPLE
    Parse-PowerDbgDT -useFieldNames

.NOTES
    Changes History:

    Roberto Alexis Farah
    All my functions are provided "AS IS" with no warranties, and confer no rights. 
#>
########################################################################################################
function Parse-PowerDbgDT([switch] $useFieldNames)
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    # \s+   --> Scans for one or more spaces.
    # (\S+) --> Gets one or more chars/digits/numbers without spaces.
    # \s+   --> Scans for one or more spaces.
    # (\w+) --> Gets one or more chars.
    # .+    --> Scans for one or more chars (any char except for new line).
    # \:    --> Scans for the ':' char.
    # \s+   --> Scans for one or more spaces.
    # (.+.) --> Gets the entire remainder string including the spaces.
    if($useFieldNames)
    {
        # This is to be able to read each line.
        $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
        # Scan the symbols for each thread, line by line.
        while(($line = $stringReader.ReadLine()) -ne $null)
        { 
           if($line -match "0x\S+\s+(?<key>\w+).+\:\s+(?<value>.+)")
           {
               $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
           } 
        }         
    }
    else
    {
        # This is to be able to read each line.
        $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
        # Scan the symbols for each thread, line by line.
        while(($line = $stringReader.ReadLine()) -ne $null)
        { 
           if($line -match "(?<key>0x\S+).+\:\s+(?<value>.+)")
           {
               $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
           } 
        }  
    }
    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}


########################################################################################################
# Function:    Convert-PowerDbgCSVtoHashTable
#
# Parameters:  None.
#                                         
# Return:      Hash table.
#
# Purpose:     Sometimes the Parse-PowerDbg#() functions return a CSV file. This function
#              loads the data using a hash table. 
#              However, it works just when the CSV file has two fields: key and value.
#
# Changes History: 04/03/2009 The cmdlet was changed to ignore the new line between the header from the CSV file and data.
#                  So now it ignores the new line that counted as a hash table item.
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Convert-PowerDbgCSVtoHashTable()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    $hashTable = @{}
    import-csv -path $global:g_fileParsedOutput | foreach {if($_.key -ne ""){$hashTable[$_.key] = $_.value}}

    return $hashTable
}



########################################################################################################
# Function:    Send-PowerDbgDML
#
# Parameters:  [string] <$hyperlinkDML>
#              Hyperlink for the DML command.
#
#              [string] <$commandDML>
#              Command to execute when the hyperlink is clicked.
#                                         
# Return:      Nothing.
#
# Purpose:     Creates a DML command and sends it to Windbg. 
#              DML stands for Debug Markup Language. Using DML you can create hyperlinks that
#              run a command when the user click on them.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Send-PowerDbgDML(
                          [string] $hyperlinkDML = $(throw "Error! You must provide the hyperlink for DML."),
                          [string] $commandDML   = $(throw "Error! You must provide the command for DML.")
                         )
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    Send-PowerDbgCommand ".printf /D `"<link cmd=\`"$commandDML\`"><b>$hyperlinkDML</b></link>\n\`"`""
}


########################################################################################################
# Function:    Parse-PowerDbgNAME2EE
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output from the "!name2ee" command using a hash table. The output 
#              is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgNAME2EE()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    # \s+   --> Scans for one or more spaces.
    # (\S+) --> Gets one or more chars/digits/numbers without spaces.
    # \s+   --> Scans for one or more spaces.
    # (\w+) --> Gets one or more chars.
    # .+    --> Scans for one or more chars (any char except for new line).
    # \:    --> Scans for the ':' char.
    # \s+   --> Scans for one or more spaces.
    # (.+.) --> Gets the entire remainder string including the spaces.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
       # Attention! The Name: doesn't map to the right value, however, it should be the same method name provide as argument.
       if($line -match "(?<key>\w+\:)\s+(?<value>\S+)")
       {
           $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
       } 
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}


########################################################################################################
# Function:    Parse-PowerDbgDUMPMD
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output from the "!dumpmd" command using a hash table. The output 
#              is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgDUMPMD()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    # \s+   --> Scans for one or more spaces.
    # (\S+) --> Gets one or more chars/digits/numbers without spaces.
    # \s+   --> Scans for one or more spaces.
    # (\w+) --> Gets one or more chars.
    # .+    --> Scans for one or more chars (any char except for new line).
    # \:    --> Scans for the ':' char.
    # \s+   --> Scans for one or more spaces.
    # (.+.) --> Gets the entire remainder string including the spaces.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
       if($line -match "(?<key>((^Method Name :)|(^MethodTable)|(^Module:)|(^mdToken:)|(^Flags :)|(^Method VA :)))\s+(?<value>\S+)")
       {
           $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
       } 
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}


########################################################################################################
# Function:    Parse-PowerDbgDUMPMODULE
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output from the "!dumpmodule" command using a hash table. The output 
#              is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgDUMPMODULE()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    [int] $countFields = 0
    
    # \s+   --> Scans for one or more spaces.
    # (\S+) --> Gets one or more chars/digits/numbers without spaces.
    # \s+   --> Scans for one or more spaces.
    # (\w+) --> Gets one or more chars.
    # .+    --> Scans for one or more chars (any char except for new line).
    # \:    --> Scans for the ':' char.
    # \s+   --> Scans for one or more spaces.
    # (.+.) --> Gets the entire remainder string including the spaces.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
       # Fields for .NET Framework 2.0
       if($line -match "(?<key>((^dwFlags)|(^Assembly:)|(^LoaderHeap:)|(^TypeDefToMethodTableMap:)|(^TypeRefToMethodTableMap:)|(^MethodDefToDescMap:)|(^FieldDefToDescMap:)|(^MemberRefToDescMap:)|(^FileReferencesMap:)|(^AssemblyReferencesMap:)|(^MetaData start address:)))\s+(?<value>\S+)")
       {
           $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
           $countFields++
       } 
    }  

    # If nothing was found, let's try to use the .NET Framework 1.1 fields.
    if($countFields -lt 3)
    {
        # This is to be able to read each line.
        $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
        # Scan the symbols for each thread, line by line.
        while(($line = $stringReader.ReadLine()) -ne $null)
        { 
           # Fields for .NET Framework 2.0
           if($line -match "(?<key>((^dwFlags)|(^Assembly\*)|(^LoaderHeap\*)|(^TypeDefToMethodTableMap\*)|(^TypeRefToMethodTableMap\*)|(^MethodDefToDescMap\*)|(^FieldDefToDescMap\*)|(^MemberRefToDescMap\*)|(^FileReferencesMap\*)|(^AssemblyReferencesMap\*)|(^MetaData starts at)))\s+(?<value>\S+)")
           {
               $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
               $hasFound = $true
           } 
        }  
    
    }
    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}

########################################################################################################
# Function:    Parse-PowerDbgLMI
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output from the "lmi" command using a hash table. The output 
#              is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgLMI()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    # \s+   --> Scans for one or more spaces.
    # (\S+) --> Gets one or more chars/digits/numbers without spaces.
    # \s+   --> Scans for one or more spaces.
    # (\w+) --> Gets one or more chars.
    # .+    --> Scans for one or more chars (any char except for new line).
    # \:    --> Scans for the ':' char.
    # \s+   --> Scans for one or more spaces.
    # (.+.) --> Gets the entire remainder string including the spaces.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
       if($line -match "(?<key>((^.+\:)))\s+(?<value>\S+)")
       {     
           $strNoLeftSpaces = $matches["key"]
           $strNoLeftSpaces = $strNoLeftSpaces.TrimStart()
           $builder = $builder.AppendLine($strNoLeftSpaces + $global:g_CSVDelimiter + $matches["value"])    
       } 
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}


########################################################################################################
# Function:    Has-PowerDbgCommandSucceeded
#
# Parameters:  None.
#                                         
# Return:      Return $true if the last command succeeded or $false if not.
#
# Purpose:     Return $true if the last command succeeded or $false if not.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Has-PowerDbgCommandSucceeded
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # This is to be able to read each line.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
        # PW: Exception removed from this list as otherwise triggers off on an Exception on a stack frame!
       if($line -imatch "(Fail) | (Failed) | (Error) | (Invalid) | (Unable to get)")
       {     
           return $false   
       } 
    }  
    return $true
}


########################################################################################################
# Function:    Send-PowerDbgComment
#
# Parameters:  [string] $comment
#              Comment to be sent to the debugger.
#                                         
# Return:      Nothing.
#
# Purpose:     Sends a bold comment to the debugger. Uses DML.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Send-PowerDbgComment(
                                 [string] $comment = $(throw "Error! You must provide a comment.")
                             )
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    Send-PowerDbgCommand ".printf /D `"\n<b>$comment</b>\n\n\`"`""
}


########################################################################################################
# Function:    Parse-PowerDbgVERTARGET
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output of Kernel time and User time from "vertarget" command, using a hash table. The output 
#              is saved into the file POWERDBG-PARSED.LOG 
#              The number of days is ignored in this version.
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgVERTARGET()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    # \s+   --> Scans for one or more spaces.
    # (\S+) --> Gets one or more chars/digits/numbers without spaces.
    # \s+   --> Scans for one or more spaces.
    # (\w+) --> Gets one or more chars.
    # .+    --> Scans for one or more chars (any char except for new line).
    # \:    --> Scans for the ':' char.
    # \s+   --> Scans for one or more spaces.
    # (.+.) --> Gets the entire remainder string including the spaces.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
       if($line -match "(?<key>((Kernel time:)|(User time:)))\s+\d+\s+\S+\s+(?<value>\d+\:\d+\:\d+\.\d+)")   
       {
           $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
       } 
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}


########################################################################################################
# Function:    Parse-PowerDbgRUNAWAY
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output of "!runaway 1" or "!runaway 2" command, using a hash table. 
#              For this version the number of days is not being considered.
#              The output is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
#              Attention! If you need to know the top threads consuming CPU time use the Convert-PowerDbgRUNAWAYtoArray
#              instead of this command. With Convert-PowerDbgRUNAWAYtoArray, the array has the exact same order of the
#              original command.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgRUNAWAY()
{   
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    # \s+   --> Scans for one or more spaces.
    # (\S+) --> Gets one or more chars/digits/numbers without spaces.
    # \s+   --> Scans for one or more spaces.
    # (\w+) --> Gets one or more chars.
    # .+    --> Scans for one or more chars (any char except for new line).
    # \:    --> Scans for the ':' char.
    # \s+   --> Scans for one or more spaces.
    # (.+.) --> Gets the entire remainder string including the spaces.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
        if($line -match "(?<key>(\d+))\:\S+\s+\d+\s+\S+\s+(?<value>\d+\:\d+\:\d+\.\d+)")   
        {
            $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
        } 
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}



########################################################################################################
# Function:    Convert-PowerDbgRUNAWAYtoArray
#
# Parameters:  None.
#                                         
# Return:      Two dimensions array.
#
# Purpose:     After executing the !runaway 1 or !runaway 2, use this command to put the information into
#              an array.
#              
# Changes History: 04/03/09 Changes to accomodate PowerDbg v5.2 that doesn't use log files anymore.
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Convert-PowerDbgRUNAWAYtoArray()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}
   
    # Now, we create a multidimensional array.
    # We need to discard 3 lines that corresponds to:
    # User Mode Time
    #  Thread       Time
    $arrayFromRUNAWAY = new-Object 'object[,]' (($global:g_commandOutput.Split("`r`n").count - 5) / 2),2
    
    [System.Int32] $i = 0   # Counter.
    
    # This is to be able to read each line.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
        if($line -match  "(?<key>(\d+))\:\S+\s+\d+\s+\S+\s+(?<value>\d+\:\d+\:\d+\.\d+)")   
        {
            $arrayFromRUNAWAY[$i, 0] = $matches["key"]
            
            $arrayFromRUNAWAY[$i, 1] = $matches["value"]
        
            $i++
        } 
    }  
    
    # The cmoma below is very important, otherwise the function will return a single dimension array.
    return ,$arrayFromRUNAWAY
}



########################################################################################################
# Function:    Parse-PowerDbgK
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output of "k" command and variations like "kv, kbn, kpn", etc..., using a hash table. 
#              It doesn't work with "kPn".
#              The key is the thread number if you use something like "~* kbn" or the key is "#" if you use 
#              something like "kb" just to show the stack from the current thread.
#              Frame are separated by '$global:g_frameDelimiter'. So, to display the frames using newline you need to
#              replace before displaying.
#
#              The output is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
#              Attention! 
#              1- It doesn't work with "kPn".
#              2- It replaces "," by '$global:g_frameDelimiter' to avoid conflict with CSV delimiter.
#
# Changes History: 12/21/2007 - The number of threads couldn't exceed 2 digits. Now it works until 999 threads.
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgK()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    $key = ""
    
    # [ #]  --> One space or #
    # \s    --> Just one space.
    # (d+)  --> Returns the decimal digits.
    # \s+   --> 1 or more spaces.
    # |     --> or
    # \s    --> One space.
    # (#)   --> Returns #.
    # \s    --> One space.
    # |     --> or
    # (\w+\s\w+\s.+) --> Returns two blocks of words and the remaining string.
    # |     --> or
    # (\d+\s\w+\s\w+\s.+) --> Returns one block of digits + two blocks of words + remaining string.
    # [ #]\s(\d+)\s+|\s(#)\s|(\w+\s\w+\s.+)|(\d+\s\w+\s\w+\s.+)  <-- The actual implementation differs a little bit.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    {     
       # For each call stack we have the key that is the thread number or # and the value that are a set
       # of lines, including all frames, etc...
       # Here we first identify the thread number if it was not identified before.
       if($line -match "([ #]\s*(?<key>(\d+))\s+Id)") 
       {              
           # If key changed append a new line. Do not consider one single thread, that is represented by "#"
           if($key -ne $matches["key"])
           {
               # The string assignment is a small tricky to avoid displaying contents in PS window.
               $builder = $builder.AppendLine("")
               $key = $matches["key"]           
           }
        
           # Just add the key. The stack is the value and it's going to be added below.
           # The string assignment is a small tricky to avoid displaying contents in PS window.        
           $builder = $builder.Append($matches["key"] + $global:g_CSVDelimiter)        
       } 
       elseif($line -match "(?<value>(\w+\s\w+\s.+))")  # Gets the stack.    
       {
           # If there is just one thread the thread number doesn't appear. For this case the thread number
           # will be "#".
           if($key -eq "")
           {
               $key = "#"
            
               # Just add the key. The stack is the value and it's going to be added below.
               # The string assignment is a small tricky to avoid displaying contents in PS window.        
               $builder = $builder.Append($key + $global:g_CSVDelimiter)                                   
           }
        
           # Append each frame replacing any commas by ";".
           # The value part of the hash table is a long string with all frames. At the end of each frame there is 
           # a delimiter. When you want to show the stack you know you can replace the delimiter by `r`n.
           # Using a delimiter is easy to do that.
           $builder = $builder.Append($matches["value"].Replace(",",";") + $global:g_frameDelimiter)              
       }
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}




########################################################################################################
# Function:    Parse-PowerDbgSymbolsFromK
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps just the symbols of "k" command and variations like "kv, kbn, kpn", etc..., using a hash table. 
#              It doesn't work with "kPn".
#              The key is the thread number if you use something like "~* kbn" or the key is "#" if you use 
#              something like "kb" just to show the stack from the current thread.
#              Frame are separated by '$global:g_frameDelimiter'. So, to display the frames using newline you need to
#              replace before displaying.
#
#              The output is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
#              Attention! 
#              1- It doesn't work with "kPn".
#              2- It replaces "," by '$global:g_frameDelimiter' to avoid conflict with CSV delimiter.
#
# Changes History: 12/21/2007 - The number of threads couldn't exceed 2 digits. Now it works until 999 threads.
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgSymbolsFromK()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    $key = ""
    
    # [ #]  --> One space or #
    # \s    --> Just one space.
    # (d+)  --> Returns the decimal digits.
    # \s+   --> 1 or more spaces.
    # |     --> or
    # \s    --> One space.
    # (#)   --> Returns #.
    # \s    --> One space.
    # |     --> or
    # (\w+\s\w+\s.+) --> Returns two blocks of words and the remaining string.
    # |     --> or
    # (\d+\s\w+\s\w+\s.+) --> Returns one block of digits + two blocks of words + remaining string.
    # [ #]\s(\d+)\s+|\s(#)\s|(\w+\s\w+\s.+)|(\d+\s\w+\s\w+\s.+)  <-- The actual implementation differs a little bit.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    {     
       # For each call stack we have the key that is the thread number or # and the value that are a set
       # of lines, including all frames, etc...
       # Here we first identify the thread number if it was not identified before.
       if($line -match "([ #]\s*(?<key>(\d+))\s+Id)")  
       {              
           # If key changed append a new line. Do not consider one single thread, that is represented by "#"
           if($key -ne $matches["key"])
           {
               # The string assignment is a small tricky to avoid displaying contents in PS window.
               $builder = $builder.AppendLine("")
               $key = $matches["key"]
           }
        
           # Just add the key. The stack is the value and it's going to be added below.
           # The string assignment is a small tricky to avoid displaying contents in PS window.        
           $builder = $builder.Append($matches["key"] + $global:g_CSVDelimiter)        
       } 
       elseif($line -match "\s(?<value>(\w+!\w+::\w+))|(?<value>(\w+!\w+)|(?<value>(\w+_ni)))")  # Gets the symbols from the stack.
       {
           # \s(\w+!\w+::\w+)|(\w+!\w+)
           # (\w+!\w+::\w+) <-- Find possible C++ methods.
           # (\w+!\w+)      <-- Find regular symbols.
           # The order is important here because the "or" is not going to evaluate the second expression if the
           # first expression returns true. 
        
           # If there is just one thread the thread number doesn't appear. For this case the thread number
           # will be "#".
           if($key -eq "")
           {
               $key = "#"
            
               # Just add the key. The stack is the value and it's going to be added below.
               # The string assignment is a small tricky to avoid displaying contents in PS window.        
               $builder = $builder.Append($key + $global:g_CSVDelimiter)                                   
           }
        
           # Append each symbol frame found plus the delimiter.
           $builder = $builder.Append($matches["value"] + $global:g_frameDelimiter)              
       }
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}


########################################################################################################
# Function:    Parse-PowerDbgLM1M
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output of "lm1m".
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgLM1M()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    # This is to be able to read each line.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
        if($line -match "(?<key>(\w+))")   
        {
            # Value and key has the same value for this particular parser.
            $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["key"])    
        } 
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}



########################################################################################################
# Function:    Classify-PowerDbgThreads
#
# Parameters:  None.
#                                         
# Return:      Array where the index is the thread number and the element is one of these values:
#              0 UNKNOWN_SYMBOL
#              1 WAITING_FOR_CRITICAL_SECTION
#              2 DOING_IO
#              3 WAITING
#              4 GC_THREAD
#              5 WAIT_UNTIL_GC_COMPLETE
#              6 SUSPEND_FOR_GC
#              7 WAIT_FOR_FINALIZE
#              8 TRYING_MANAGED_LOCK
#              9 DATA_FROM_WINSOCK
#              
#              The constants above are stored in global variables. 
#
# Purpose:     Returns an array which the index corresponds to thread numbers and the content is a value represented
#              by the constants above. This cmdlet gives you an idea of what the threads are doing.
#              Notice that is very easy to add more symbols and more constants to get a more granular analysis.
#
# Changes History: 01/25/08 - More symbols added to the hash table. The analysis became more granular.
#
# Mike McIntyre
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
# It's not possible to create an enum like C++/C# using a PowerShell keyword.
# The PowerShell blog has a solution for it, but here I'm going to use local variables.
$global:g_unknownSymbol             = 0
$global:g_waitingForCriticalSection = 1
$global:g_doingIO                   = 2
$global:g_threadWaiting             = 3
$global:g_GCthread                  = 4
$global:g_waitUntilGCComplete       = 5
$global:g_suspendForGC              = 6
$global:g_waitForFinalize           = 7
$global:g_tryingGetLock             = 8
$global:g_winSockReceivingData      = 9
$global:g_finalizerThread           = 10
$global:g_CLRDebuggerThread         = 11
$global:g_w3wpMainThread            = 12
$global:g_threadPool                = 13
$global:g_CompressionThreaad        = 14
$global:g_COMcall                   = 15
$global:g_CLRWorkerThread           = 16
$global:g_completionPortIOThread    = 17
$global:g_gateThread                = 18
$global:g_timerThread               = 19
$global:g_unloadAppDomainThread     = 20
$global:g_RPCWorkerThread           = 21
$global:g_LPRCWorkerThread          = 22
$global:g_hostSTAThread             = 23

# The array below has the meaning of each constant above. It can be used to display high level information
# to the user.
$global:g_whatThreadIsDoing = 
@(
    "Thread working and doing unknown activity.",                                                   # 0
    "Thread waiting for Critical Section.",                                                         # 1
    "Thread doing I/O operation.",                                                                  # 2
    "Thread in wait state.",                                                                        # 3
    "Thread from Garbage Collector.",                                                               # 4
    "Thread waiting for the Garbage Collector to complete.",                                        # 5
    "Thread is being suspended to perform a Garbage Collector."                                     # 6
    "Thread waiting for the Finalizer event. The Finalizer thread might be blocked.",               # 7
    "Thread trying to get a managed lock.",                                                         # 8
    "Thread receiving or trying to receive data. The data might be from the database.",             # 9
    "Thread is the Finalizer Thread.",                                                              # 10
    "Thread is the CLR Debugger Thread.",                                                           # 11
    "Thread is the W3WP.EXE main thread.",                                                          # 12
    "Thread is from the W3WP.EXE pool of threads.",                                                 # 13
    "Thread is a Compression Thread from W3WP.EXE.",                                                # 14
    "Thread doing a COM call.",                                                                     # 15
    "Thread is a CLR Worker Thread.",                                                               # 16
    "Thread is a Completion Port I/O Thread.",                                                      # 17
    "Thread is a CLR Gate Thread.",                                                                 # 18
    "Thread is a CLR Timer Thread.",                                                                # 19
    "Thread is an Unload Application Domain Thread.",                                               # 20
    "Thread is a RPC Worker Thread.",                                                               # 21
    "Thread is an LRPC Worker Thread.",                                                             # 22
    "Thread is the Host STA Thread."                                                                # 23
)


function Classify-PowerDbgThreads()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}
    
    $symbolsForEachThread = @{}
    
    # Let's save resource. We need kL with just 8 frames.
    Send-PowerDbgCommand "~* kL 8"
    
    $hasCommandSucceeded = Has-PowerDbgCommandSucceeded

    # Check if the command was executed with success.
    # It's unlikely to have a failure with the "k" command, but I'm being proactive.
    if($false -eq $hasCommandSucceeded)
    {
        throw "Couldn't get call stacks!"
    }
    
    # Let's parse the output of the "k" command and variations, however, we want the symbols
    # not the complete call stack.
    Parse-PowerDbgSymbolsFromK
    
    # Now let's get the symbols for each thread. We don't need the stack details here.
    $symbolsForEachThread = Convert-PowerDbgCSVtoHashTable
        
    # IMPORTANT!!!
    # The symbols need to be written in uppercase, because the comparison is doing using uppercase letters to avoid
    # mismatches.
    $symbols = @{ 
                  "KERNEL32!COPYFILE"                                 = $global:g_doingIO;
                  "KERNEL32!CREATEDIRECTORY"                          = $global:g_doingIO;
                  "KERNEL32!CREATEFILE"                               = $global:g_doingIO;  
                  "KERNEL32!DELETEFILE"                               = $global:g_doingIO;
                  "KERNEL32!FILEIOCOMPLETIONROUTINE"                  = $global:g_doingIO; 
                  "KERNEL32!FINDCLOSE"                                = $global:g_doingIO;      
                  "KERNEL32!FINDCLOSECHANGENOTIFICATION"              = $global:g_doingIO;
                  "KERNEL32!FINDFIRSTCHANGENOTIFICATION"              = $global:g_doingIO;
                  "KERNEL32!FINDFIRSTFILE"                            = $global:g_doingIO;          
                  "KERNEL32!FINDFIRSTFILEEX"                          = $global:g_doingIO;              
                  "KERNEL32!FINDNEXTCHANGENOTIFICATION"               = $global:g_doingIO;
                  "KERNEL32!FINDNEXTFILE"                             = $global:g_doingIO;              
                  "KERNEL32!FLUSHFILEBUFFERS"                         = $global:g_doingIO;
                  "KERNEL32!GETBINARYTYPE"                            = $global:g_doingIO;                 
                  "KERNEL32!GETCURRENTDIRECTORY"                      = $global:g_doingIO;
                  "KERNEL32!GETDRIVETYPE"                             = $global:g_doingIO;                 
                  "KERNEL32!GETFILEATTRIBUTES"                        = $global:g_doingIO;                     
                  "KERNEL32!GETFILEATTRIBUTESEX"                      = $global:g_doingIO;                     
                  "KERNEL32!GETFILEINFORMATIONBYHANDLE"               = $global:g_doingIO;                     
                  "KERNEL32!GETFILESIZE"                              = $global:g_doingIO;                     
                  "KERNEL32!GETFILESIZEEX"                            = $global:g_doingIO;                     
                  "KERNEL32!GETFULLPATHNAME"                          = $global:g_doingIO;                         
                  "KERNEL32!GETTEMPFILENAME"                          = $global:g_doingIO;                          
                  "KERNEL32!GETTEMPPATH"                              = $global:g_doingIO;                          
                  "KERNEL32!LOCKFILE"                                 = $global:g_doingIO;                          
                  "KERNEL32!LOCKFILEEX"                               = $global:g_doingIO;                                            
                  "KERNEL32!MOVEFILE"                                 = $global:g_doingIO;                                            
                  "KERNEL32!READDIRECTORYCHANGESW"                    = $global:g_doingIO;                                            
                  "KERNEL32!READFILE"                                 = $global:g_doingIO;                                            
                  "KERNEL32!READFILEEX"                               = $global:g_doingIO;                                            
                  "KERNEL32!REMOVEDIRECTORY"                          = $global:g_doingIO;
                  "KERNEL32!SEARCHPATH"                               = $global:g_doingIO;    
                  "KERNEL32!SETCURRENTDIRECTORY"                      = $global:g_doingIO;   
                  "KERNEL32!SETENDOFFILE"                             = $global:g_doingIO;          
                  "KERNEL32!SETFILEATTRIBUTES"                        = $global:g_doingIO; 
                  "KERNEL32!SETFILEPOINTER"                           = $global:g_doingIO;  
                  "KERNEL32!SETFILEPOINTEREX"                         = $global:g_doingIO;                  
                  "KERNEL32!UNLOCKFILE"                               = $global:g_doingIO;                      
                  "KERNEL32!UNLOCKFILEEX"                             = $global:g_doingIO;                      
                  "KERNEL32!WRITEFILE"                                = $global:g_doingIO;                          
                  "KERNEL32!WRITEFILEEX"                              = $global:g_doingIO;
                  "NTDLL!ZWREMOVEIOCOMPLETION"                        = $global:g_doingIO;
                  "NTDLL!RTLPWAITFORCRITICALSECTION"                  = $global:g_waitingForCriticalSection;  
                  "NTDLL!RTLENTERCRITICALSECTION"                     = $global:g_waitingForCriticalSection;  
                  "KERNEL32!ENTERCRITICALSECTION"                     = $global:g_waitingForCriticalSection;  
                  "KERNEL32!MSGWAITFORMULTIPLEOBJECTS"                = $global:g_threadWaiting;
                  "KERNEL32!MSGWAITFORMULTIPLEOBJECTSEX"              = $global:g_threadWaiting;
                  "KERNEL32!REGISTERWAITFORSINGLEOBJECT"              = $global:g_threadWaiting;
                  "KERNEL32!SIGNALOBJECTANDWAIT"                      = $global:g_threadWaiting;
                  "KERNEL32!UNREGISTERWAIT"                           = $global:g_threadWaiting;
                  "KERNEL32!UNREGISTERWAITEX"                         = $global:g_threadWaiting; 
                  "KERNEL32!WAITFORMULTIPLEOBJECTS"                   = $global:g_threadWaiting;
                  "KERNEL32!WAITFORMULTIPLEOBJECTSEX"                 = $global:g_threadWaiting; 
                  "KERNEL32!WAITFORSINGLEOBJECT"                      = $global:g_threadWaiting; 
                  "KERNEL32!WAITFORSINGLEOBJECTEX"                    = $global:g_threadWaiting; 
                  "KERNEL32!WAITORTIMERCALLBACK"                      = $global:g_threadWaiting;
                  "USER32!NTUSERGETMESSAGE"                           = $global:g_threadWaiting;
                  "USER32!NTUSERMESSAGECALL"                          = $global:g_threadWaiting;
                  "USER32!NTUSERWAITMESSAGE"                          = $global:g_threadWaiting;
                  "NTDLL!DBGBREAKPOINT"                               = $global:g_threadWaiting; 
                  "NTDLL!RTLPWAITTHREAD"			                  = $global:g_threadWaiting; 
                  "KERNEL32!SLEEPEX"                                  = $global:g_threadWaiting;
                  "KERNEL32!SLEEP"                                    = $global:g_threadWaiting;
                  "NTDLL!NTDELAYEXECUTION"                            = $global:g_threadWaiting;
                  "MFC80D!AFXINTERNALPUMPMESSAGE"                     = $global:g_threadWaiting;
                  "MFC80!AFXINTERNALPUMPMESSAGE"                      = $global:g_threadWaiting;
                  "MSCORWKS!SVR::GC_HEAP::GC_THREAD_STUB"             = $global:g_GCthread;
                  "MSCORSVR!SVR::GC_HEAP::GC_THREAD_STUB"             = $global:g_GCthread;
                  "MSCORSVR!GCHEAP::WAITUNTILGCCOMPLETE"              = $global:g_waitUntilGCComplete;
                  "MSCORWKS!GCHEAP::WAITUNTILGCCOMPLETE"              = $global:g_waitUntilGCComplete;
                  "MSCORWKS!THREAD::SYSSUSPENDFORGC"                  = $global:g_suspendForGC;
                  "MSCORSVR!THREAD::SYSSUSPENDFORGC"                  = $global:g_suspendForGC;
                  "MSCORWKS!WAITFORFINALIZEREVENT"                    = $global:g_waitForFinalize;
                  "MSCORSVR!WAITFORFINALIZEREVENT"                    = $global:g_waitForFinalize;
                  "MSCORWKS!SVR::WAITFORFINALIZEREVENT"               = $global:g_waitForFinalize;                
                  "MSCORSVR!SVR::WAITFORFINALIZEREVENT"               = $global:g_waitForFinalize;                
                  "MSCORWKS!WKS::WAITFORFINALIZEREVENT"               = $global:g_waitForFinalize; 
                  "MSCORSVR!WKS::WAITFORFINALIZEREVENT"               = $global:g_waitForFinalize; 
                  "MSCORWKS!JITUNTIL_MONCONTENTION"                   = $global:g_tryingGetLock;
                  "MSCORSVR!JITUNTIL_MONCONTENTION"                   = $global:g_tryingGetLock;
                  "MSWSOCK!WSPRECV"                                   = $global:g_winSockReceivingData;
                  "WS2_32!RECV"                                       = $global:g_winSockReceivingData;
                  "MSCORSVR!GCHeap::FINALIZETHREADSTART"              = $global:g_finalizerThread;
                  "MSCORWKS!GCHeap::FINALIZETHREADSTART"              = $global:g_finalizerThread;
                  "MSCORSVR!DEBUGGERRCTHREAD::MAINLOOP"               = $global:g_CLRDebuggerThread;
                  "MSCORWKS!DEBUGGERRCTHREAD::MAINLOOP"               = $global:g_CLRDebuggerThread;
                  "W3DT!WP_CONTEXT::RUNMAINTHREADLOOP"                = $global:g_w3wpMainThread;
                  "W3TP!THREAD_POOL_DATA::THREADPOOLTHREAD"           = $global:g_threadPool;
                  "W3CORE!HTTP_COMPRESSION::COMPRESSIONTHREAD"        = $global:g_CompressionThreaad;
                  "OLE32!CRPCCHANNELBUFFER::SENDRECEIVE2"             = $global:g_COMcall;
                  "MSCORWKS!CLREVENT::WAIT"                           = $global:g_CLRWorkerThread;
                  "MSCORSVR!CLREVENT::WAIT"                           = $global:g_CLRWorkerThread;
                  "MSCORWKS!THREADPOOLMGR::COMPLETIONPORTTHREADSTART" = $global:g_completionPortIOThread;
                  "MSCORSVR!THREADPOOLMGR::COMPLETIONPORTTHREADSTART" = $global:g_completionPortIOThread;
                  "MSCORWKS!THREADPOOLMGR::GATETHREADSTART"           = $global:g_gateThread;
                  "MSCORSVR!THREADPOOLMGR::GATETHREADSTART"           = $global:g_gateThread;
                  "MSCORWKS!THREADPOOLMGR::TIMERTHREADSTART"          = $global:g_timerThread;
                  "MSCORSVR!THREADPOOLMGR::TIMERTHREADSTART"          = $global:g_timerThread;                
                  "MSCORWKS!APPDOMAIN::ADUNLOADTHREADSTART"           = $global:g_unloadAppDomainThread;
                  "MSCORSVR!APPDOMAIN::ADUNLOADTHREADSTART"           = $global:g_unloadAppDomainThread;
                  "RPCRT4!THREADSTARTROUTINE"                         = $global:g_RPCWorkerThread;
                  "RPCRT4!COMMON_PROCESSCALLS"                        = $global:g_RPCWorkerThread;
                  "RPCRT4!LRPC_ADDRESS::RECEIVELOTSACALLS"            = $global:g_LPRCWorkerThread;
                  "OLE32!CDLLHOST::STAWORKERLOOP"                     = $global:g_hostSTAThread
                }
    
    # The array has the right size to fit all threads.
    $array = 1..$symbolsForEachThread.count
        
    # Now we scan all threads and for each threads we scan all frames until symbols mapped to the hash table are found
    # or there are no more frames.
    # Below, we need to discount the hash table "key" and "value" strings because they are considered one element.
    for([System.Int32] $i = 0; $i -lt $symbolsForEachThread.count; $i++)
    {              
        # The delimiter is converted to new line. Now it's easy to process each line.
        $stack = $symbolsForEachThread[$i.ToString()].Replace($global:g_FrameDelimiter, "`n")
        
        # Sets the default value.
        $array[$i] = $global:g_unknownSymbol
        
        # This is to be able to read each line.
        $stringReader = [System.IO.StringReader] $stack
        
        # Scan the symbols for each thread, line by line.
        while(($frame = $stringReader.ReadLine()) -ne $null)
        {                                  
            # Now we try to locate the symbol in our hash table.
            # Always using uppercase.
            if($symbols[$frame.ToUpper()] -ne $null)
            {
                # If symbol not located we don't assign $null to the array.
                if($symbols[$frame.ToUpper()] -ne $null)
                {
                    # If found we assign the constant to the array element.
                    $array[$i] = $symbols[$frame.ToUpper()]
                }
            }
        }
    }
    
    # Force resources to be freed from memory.                         
    $symbols              = $null                 
    $symbolsForEachThread = $null
    
    return $array
}


########################################################################################################
# Function:    Analyze-PowerDbgThreads
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Analyzes and displays what each thread is doing and the CPU time, sorted by User time.
#              This cmdlet is very useful for hangs, high CPU and crashes scenarios.
#
#              Attention! If you have a mini-dump with no thread information you may want to create
#              a simplified cmdlet that doesn't use the information from !runaway.
#              To do that just remove all parts of this script that use User and Kernel time. :)
#              This script tries to use the CPU time because it's unlikely to have a dump that not have it.
#
# Changes History: 01/25/08 - Threads with unknown symbol appear in red color.
#                  04/03/09 - Small changes for version 5.2. 
#
# Mike McIntyre
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Analyze-PowerDbgThreads()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}
    
    # Let's get User time.
    Send-PowerDbgCommand "!runaway 1"
    
    $hasCommandSucceeded = Has-PowerDbgCommandSucceeded

    if($false -eq $hasCommandSucceeded)
    {
        throw "This dump has no threads information!"
    }
    
    # Gets the array of user mode time.
    $arrayOfUserTime = Convert-PowerDbgRUNAWAYtoArray
     
    # Let's get Kernel time.
    # It's not necessary to validate the command output because we already did that.
    Send-PowerDbgCommand "!runaway 2"
    
    # Parses to get a hash table.
    Parse-PowerDbgRUNAWAY
    
    # Gets a hash table from the CSV file created with the cmdlet above.
    $kernelTime = Convert-PowerDbgCSVtoHashTable
    
    # Analyze the call stack for each thread to know what they are doing.
    $arrayOfClassification = Classify-PowerDbgThreads
   
    # Simple header.
    write-Host "Threads sorted by User Time...`n" -foreground Green -background Black      
    write-Host "Thread Number`tUser Time`tKernel Time`t`tActivity" -foreground Green -background Black            
          
    # Displays information to user.
    for([System.Int32] $i = 0; $i -lt [System.Math]::truncate(($arrayOfUserTime.Count / 2)); $i++)
    {
        # If unknown activity, put it in red.
        if($arrayOfClassification[$arrayOfUserTime[$i, 0]] -eq $global:g_unknownSymbol)
        {
            write-Host "  " $arrayOfUserTime[$i, 0] "`t`t" $arrayOfUserTime[$i, 1] "`t" $kernelTime[$arrayOfUserTime[$i, 0]] "`t" $global:g_whatThreadIsDoing[$arrayOfClassification[$arrayOfUserTime[$i, 0]]] -foreground Red -background Black          
        }
        else
        {
            write-Host "  " $arrayOfUserTime[$i, 0] "`t`t" $arrayOfUserTime[$i, 1] "`t" $kernelTime[$arrayOfUserTime[$i, 0]] "`t" $global:g_whatThreadIsDoing[$arrayOfClassification[$arrayOfUserTime[$i, 0]]] -foreground Green -background Black          
        }
    }
    Send-PowerDbgComment "Analyze-PowerDbgThreads finished execution."
}




########################################################################################################
# Function:    Parse-PowerDbgPRINTEXCEPTION
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output from the "!PrintException" command using a hash table. The output 
#              is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
#              Note: For this version the fields being considered are: 
#              Exception object:
#              Exception type:
#              Message:
#              InnerException:
#              HResult:
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgPRINTEXCEPTION()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    # \s+   --> Scans for one or more spaces.
    # (\S+) --> Gets one or more chars/digits/numbers without spaces.
    # \s+   --> Scans for one or more spaces.
    # (\w+) --> Gets one or more chars.
    # .+    --> Scans for one or more chars (any char except for new line).
    # \:    --> Scans for the ':' char.
    # \s+   --> Scans for one or more spaces.
    # (.+.) --> Gets the entire remainder string including the spaces.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
       if($line -match "((?<key>(^Exception object:)|(^Exception type:)|(^Message:)|(^InnerException:)|(^HResult:))\s+(?<value>.+))")
       {
           $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
       } 
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}



########################################################################################################
# Function:    Parse-PowerDbgDD-L1
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output from the "dd <address> L1" or "dd poi(<address>) L1" command using a hash table. 
#              The key is the address and the output from the command is the value.
#              The output is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgDD-L1()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")
    
    # This is to be able to read each line.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
        if($line -match "((?<key>(^\w+))\s+(?<value>(\w+)))")
        {
            $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])            
        }   
    }  
   
    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"   
}





########################################################################################################
# Function:    Parse-PowerDbgGCHANDLELEAKS
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output from the "!GCHandleLeaks" command using a hash table. The key is the handle and 
#              the value is the object corresponding to the handle. The output is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgGCHANDLELEAKS()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder    = $builder.AppendLine("key,value")
    $foundLeak  = $false
    [int] $i    = 0
    [System.Collections.ArrayList] $arrHandles = @()
    
    # This is to be able to read each line.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
        # If leak was found, map all handles that are leaking.
        # Valid output from 'g_fileCommandOutput' may be:
        # 022014d0        022014d4        022014d8        022014dc
        # 022014e0        022014e4        022014ec
        # 022014f4        022014f8
        # 022014f4
        # The expression below should be optmized.
        if($foundLeak -and (($line -match "((?<key>(^\w+))\t(?<key2>(\w+))\t(?<key3>(\w+))\t(?<key4>(\w+)))") -or
                            ($line -match "((?<key>(^\w+))\t(?<key2>(\w+))\t(?<key3>(\w+))\t)") -or
                            ($line -match "((?<key>(^\w+))\t(?<key2>(\w+))\t)") -or
                            ($line -match "((?<key>(^\w+))\t)")))
        {
            # Save the keys just if they have value, ignoring spaces or tabs.
            # We use a temporary variable to not display the results.
            if($matches["key"].Length -ge 1)
            {
                $a= $arrHandles.Add($matches["key"])
            }

            if($matches["key2"].Length -ge 1)
            {
                $a = $arrHandles.Add($matches["key2"])
            }
            
            if($matches["key3"].Length -ge 1)
            {          
                $a = $arrHandles.Add($matches["key3"])
            }
            
            if($matches["key4"].Length -ge 1)
            {         
                $a = $arrHandles.Add($matches["key4"])
            }
        }   
        elseif(($foundLeak -eq $false) -and ($line -match "(?<key>(^Didn))") )
        {
            # Verify is there are handles leaking. We use part of the sentence "Didn't find..." to check if there are leaks.        
            $foundLeak = $true
        }
        elseif($line -match "((?<key>(^All handles found.)))")
        {
            break
        }
    }  

    # Now we scan each handle, get the corresponding object and save it in a CSV file.
    for($i = 0; $i -lt $arrHandles.Count; $i++)
    {       
        [string] $handle = $arrHandles[$i]  
        
        Send-PowerDbgCommand "dd $handle L1"   
               
        Parse-PowerDbgDD-L1
        
        $content = Convert-PowerDbgCSVToHashTable

        $builder = $builder.AppendLine($handle + $global:g_CSVDelimiter + $content["$handle"])
    }
    
    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
    
}



########################################################################################################
# Function:    Parse-PowerDbgDUMPOBJ
#
# Parameters:  None.
#                                         
# Return:      Nothing.
#
# Purpose:     Maps the output from "!DumpObj" command using a hash table. 
#              The assembly path and file name are saved with the key name 'Assembly:'.
#              If the object is invalid the Name: will have the string "Invalid object". You may want to check
#              this string to make sure you got valid data.
#
#              The key are the fields or Method Table and the value is the corresponding value.
#              The output is saved into the file POWERDBG-PARSED.LOG 
#              All Parse functions should use the same outputfile.
#              You can easily map the POWERDBG-PARSED.LOG to a hash table.
#              Convert-PowerDbgCSVtoHashTable() does that.
#
#              Attention! This version maps the fields below "Fields:" using the MethodTable as key and Value as value.
#              The problem with this approach is that the same MethodTable may appear more than once. If it happens
#              just the latest MethodTable and Value will be saved in the CSV file.
#              Based on users' feedback I may change this approach in the future. (and break compatibility :))
#
# Changes History:
#
# Roberto Alexis Farah
# All my functions are provided "AS IS" with no warranties, and confer no rights. 
########################################################################################################
function Parse-PowerDbgDUMPOBJ()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")
    
    # This is to be able to read each line.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
        if($line -match "((?<key>(^Name:)|(^MethodTable:)|(^EEClass:)|(^Size:)|(^String:))\s+(?<value>(.+)))")
        {              
            $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])            
        }
        elseif($line -match "((?<key>(^\w+))\s+\w+\s+\w+\s+\w+\.\w+\s+\w+\s+\w+\s+(?<value>(\w+)))")
        {
            $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])                    
        }
        elseif($line -match "((?<key>(^\s\())(?<value>(.+))\))")
        {
            # The assembly name and path is saved under the key 'Assembly:'.
            $builder = $builder.AppendLine("Assembly:" + $global:g_CSVDelimiter + $matches["value"])                            
        }
        elseif($line -match "((?<key>(Note: this object has an invalid CLASS field)|(^Invalid object)))")
        {
            # Object is invalid.
            $builder = $builder.AppendLine("Name:" + $global:g_CSVDelimiter + "Invalid object")                            
        }   
    }  
   
    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"   
}




########################################################################################################
<#
.SYNOPSIS 
    Load symbols for PowerDbg
.PARAMETER symbols
    Directory to store the symbols and symbols path.
.EXAMPLE
    Load-PowerDbgSymbols "SRV*c:\PUBLICSYMBOLS*http://msdl.microsoft.com/download/symbols"
.OUTPUTS
    Nothing
.NOTES
    Changes history:
        <none>

    Roberto Alexis Farah
    All my functions are provided "AS IS" with no warranties, and confer no rights. 
#>
########################################################################################################
function Load-PowerDbgSymbols([string] $symbols = $(throw "Error! You must provide the directory to store symbols and the symbols path."))
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    Send-PowerDbgCommand ".sympath $symbols"
}




########################################################################################################
<#
.SYNOPSIS
    Maps the output from the "!ASPXPages" command and saves it into the file POWERDBG-PARSED.LOG 
.DESCRIPTION
    Maps the output from the "!ASPXPages" command and saves it into the file POWERDBG-PARSED.LOG 
    You can easily map the POWERDBG-PARSED.LOG to a hash table.
    Convert-PowerDbgCSVtoHashTable does that.

    Note: For this version the fields being considered are: 
    HttpContext    Timeout+Completed+Running+ThreadId+ReturnCode+Verb+RequestPath+QueryString

    HttpContext is the key and all other fields together are the value.
.OUTPUTS
    Nothing
.NOTES
    Changes History:

    Roberto Alexis Farah
    All my functions are provided "AS IS" with no warranties, and confer no rights. 
#>
########################################################################################################
function Parse-PowerDbgASPXPAGES()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    # (\S+) --> Scans for any character except for white spaces.
    # \s+   --> Scans for one or more spaces.
    # (.+)  --> Scans for one or more chars (any char except for new line).
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
       if($line -match "((?<key>(0x\S+))\s+(?<value>(.+)))")
       {
           $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
       } 
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}




########################################################################################################
<#
.SYNOPSIS
    Maps the output from the "!clrstack" command or "~* e !clrstack" and saves it into the file POWERDBG-PARSED.LOG 

.DESCRIPTION
    Maps the output from the "!clrstack" command or "~* e !clrstack" and saves it into the file POWERDBG-PARSED.LOG 
    You can easily map the POWERDBG-PARSED.LOG to a hash table using Convert-PowerDbgCSVtoHashTable.
    The key is the thread number and the value is the call stack separated by '$global:g_frameDelimiter'.

    Attention!   Commas "," are replaced for ";" to avoid confusing with the comma used by the CSV file.
    If you use this cmdlet to parse the output from "~* e !clrstack" the threads not running managed code are ignored.

.OUTPUTS
    Nothing

.NOTES
    Changes History:

    Roberto Alexis Farah
    All my functions are provided "AS IS" with no warranties, and confer no rights. 
#>
########################################################################################################
function Parse-PowerDbgCLRSTACK()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")
    $key     = ""
    
    # \S+   --> Searches for any character except for white spaces.
    # \s+   --> Searches for one or more spaces.
    # (\S+) --> Searches for any character except white space and save it.
    # (.+)  --> Saves the remainder characters until new line.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
    
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    {         
        # Verifies if this is the line that has the thread number.
        if($line -match "^OS Thread Id:.+\((?<key>(\d+))\)")
        {
            $key = $matches["key"]            
        }
        elseif($line.Contains("Unable to walk the managed stack") -or $line.StartsWith("ESP") -or $line.StartsWith("managed thread") -or $line.StartsWith("the process") ) 
        {  
            continue  # Not running managed code or the line has garbage. 
        }
        elseif($line -match "(?<value>(.+))")
        {
            # If content is an empty string ignore it.
            if(($matches["value"] -ne "") -and ($matches["value"] -ne " "))
            {
                # We save the key just the first time.
                if($key -ne "")
                {
                    # Each new CSV line should start from the beginning not as a continuation from the previous line.
                    $builder = $builder.AppendLine("")        
                    $builder = $builder.Append($key + $global:g_CSVDelimiter)                                           
                }
            
                $builder = $builder.Append($matches["value"].Replace(",", ";") + $global:g_frameDelimiter)             
            }
            
            $key = ""            
        }   
        
        else
        {
            $key = ""
            continue # If this is not our pattern ignore it.
        }
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}




########################################################################################################
<#
.SYNOPSIS
    Maps the output from the "!threads" command and saves it into the file POWERDBG-PARSED.LOG 

.DESCRIPTION
    Maps the output from the "!threads" command and saves it into the file POWERDBG-PARSED.LOG 
    You can easily map the POWERDBG-PARSED.LOG to a hash table using Convert-PowerDbgCSVtoHashTable.

        The following fields are extracted:
        Thread Number                                                 - Key
        ID+OSID+ThreadOBJ+State+GC+Context+Domain+Count+APT+Exception - Value
.OUTPUTS
    Nothing

.NOTES
    Changes History:

    Roberto Alexis Farah
    All my functions are provided "AS IS" with no warranties, and confer no rights. 
#>
########################################################################################################
function Parse-PowerDbgTHREADS()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")

    # ^\s*  --> Searches for 0 or more spaces from the beginning of the line.
    # (\d+) --> Searches for decimal characters and save them.
    # \s+   --> Searches for 1 or more spaces.
    # (.+)  --> Saves the remainder characters until new line.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    { 
        if($line -match "^\s*((?<key>(\d+))\s+(?<value>(.+)))")
        {
            $builder = $builder.AppendLine($matches["key"] + $global:g_CSVDelimiter + $matches["value"])    
        }        
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}






########################################################################################################
<#
.SYNOPSIS
    Maps the output from the "!dso" command and saves it into the file POWERDBG-PARSED.LOG 

.DESCRIPTION
    Maps the output from the "!dso" command and saves it into the file POWERDBG-PARSED.LOG 
    You can easily map the POWERDBG-PARSED.LOG to a hash table using Convert-PowerDbgCSVtoHashTable.
    The Thread Number is the key and the stack is the value.

    Attention! Commas are replaced by ";" and '$global:g_FrameDelimiter' is used to separate frames.

.OUTPUTS
    Nothing

.NOTES
    Changes History: 
        - 12/21/09 Added line $key = $matches["key"] within the last "elseif". Thanks to Igor Dvorkin
          who found the bug.

    Roberto Alexis Farah
    All my functions are provided "AS IS" with no warranties, and confer no rights. 
#>
########################################################################################################
function Parse-PowerDbgDSO()
{
    set-psdebug -strict
    $ErrorActionPreference = "stop"
    trap {"Error message: $_"}

    # Extract output removing commands.
    $builder = New-Object System.Text.StringBuilder

    # Title for the CSV fields.
    $builder = $builder.AppendLine("key,value")
    $key     = ""
    
    # ^\S+ --> Searches any characters that stars with char or number from the beginning of the line.
    # \s*   --> Searches for one or more spaces.
    # (\S+) --> Searches for 1 or more spaces.
    # (.+)  --> Saves the remainder characters until new line.
    $stringReader = [System.IO.StringReader] $global:g_commandOutput
        
    # Scan the symbols for each thread, line by line.
    while(($line = $stringReader.ReadLine()) -ne $null)
    {
        # Verifies if this is the line that has the thread number.
        if($line -match "^OS Thread Id:.+\((?<key>(\d+))\)")
        {
            $key = $matches["key"]                
        }    
        elseif($line.StartsWith("ESP/REG  Object   Name"))
        {
            continue
        }
        elseif($line -match "^\S+\s*((?<key>(\S+))\s+(?<value>(.+)))")
        {          
            # If content is an empty string ignore it.
            if(([string] $matches["value"] -eq "") -or ([string] $matches["value"] -eq " "))
            {              
                continue
            }
            
            $key = $matches["key"]
            
            # We save the key just the first time.
            if($key -ne "")
            {
                # Each new CSV line should start from the beginning not as a continuation from the previous line.
                $builder = $builder.AppendLine("")        
                $builder = $builder.Append($key + $global:g_CSVDelimiter)                                           
            }

            $builder = $builder.Append($matches["value"].Replace(",", ";") + $global:g_frameDelimiter)             
              
            $key = ""          
        }
    }  

    # Send output to our default file.
    out-file -filepath $global:g_fileParsedOutput -inputobject "$builder"
}


if ($MyInvocation.MyCommand.ScriptBlock.Module){
    # Exported only for b/wards compatability with other scripts
    # This function is essentially depricated
    Export-ModuleMember -Function Send-PowerDbgCommand 
    
    Export-ModuleMember -Function Load-PowerDbgSymbols 
    Export-ModuleMember -Function Parse-PowerDbgASPXPAGES
    Export-ModuleMember -Function Parse-PowerDbgCLRSTACK
    Export-ModuleMember -Function Parse-PowerDbgTHREADS
    Export-ModuleMember -Function Parse-PowerDbgDSO
    Export-ModuleMember -Function Parse-PowerDbgDT 
    Export-ModuleMember -Function Convert-PowerDbgCSVToHashTable
    Export-ModuleMember -Function Send-PowerDbgDML 
    Export-ModuleMember -Function Parse-PowerDbgNAME2EE
    Export-ModuleMember -Function Parse-PowerDbgDUMPMD
    Export-ModuleMember -Function Parse-PowerDbgDUMPMODULE
    Export-ModuleMember -Function Parse-PowerDbgLMI
    Export-ModuleMember -Function Has-PowerDbgCOMMANDSUCCEEDED
    Export-ModuleMember -Function Send-PowerDbgComment
    Export-ModuleMember -Function Parse-PowerDbgVERTARGET
    Export-ModuleMember -Function Parse-PowerDbgRUNAWAY
    Export-ModuleMember -Function Convert-PowerDbgRUNAWAYtoArray
    Export-ModuleMember -Function Parse-PowerDbgK
    Export-ModuleMember -Function Parse-PowerDbgSymbolsFromK
    Export-ModuleMember -Function Parse-PowerDbgLM1M
    Export-ModuleMember -Function Classify-PowerDbgThreads
    Export-ModuleMember -Function Analyze-PowerDbgThreads
    Export-ModuleMember -Function Parse-PowerDbgPRINTEXCEPTION
    Export-ModuleMember -Function Parse-PowerDbgDD-L1
    Export-ModuleMember -Function Parse-PowerDbgGCHANDLELEAKS
    Export-ModuleMember -Function Parse-PowerDbgDUMPOBJ
}
#
