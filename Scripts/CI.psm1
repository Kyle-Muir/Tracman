#This PowerShell module contains a number of useful functions for creating CI scripts

Set-StrictMode -Version 3

# Get the directory of the current script
$scriptPath = Split-Path -parent $PSCommandPath


# ----------------------------------------------------------------------------------------------------------------------
#
# Configuration file functions
#
# ----------------------------------------------------------------------------------------------------------------------



#Load a configuration file defined as key value pairs key=value over multiple lines.
function Get-ConfigurationFile
{
    Param(        
        [String]$SettingsFile
    )

    if ((Test-Path $SettingsFile) -eq $false)
    {
	    Write-Verbose "unable to find setting file : $SettingsFile"
	    throw
    }

    #$tmp = gc $SettingsFile | %{$configuration = @{}} {if ($_ -match "(.*)=(.*)") {$configuration[$matches[1]]=$matches[2];}}

    $configuration = @{}
    $file = Get-Content -Path $SettingsFile
    $file | %{ `
        if ($_ -match '(?<Name>[^=]+)=(?<Value>.*)') `
        { `
            $configuration.($matches.Name) = $matches.Value.Trim() `
        } `
        elseif ($configuration.Count) `
        { `
            $configuration;$configuration.Clear() `
        }}


    return $configuration;
}

#Load a setting from the configuration
function Get-Setting
{
    Param(        
        [String]$key,
        $configuration
    )

	if ($configuration.ContainsKey($key))
	{
		return $configuration.Get_Item($key);
	}
	else
	{
		Write-Error "Configuration file does not contain expected setting '$key'"
		throw
	}
}

# ----------------------------------------------------------------------------------------------------------------------
#
# Process mangement
#
# ----------------------------------------------------------------------------------------------------------------------


#Run a process.
function Start-Process([string] $application, [string] $arguments, [bool] $failOnError )
{
  Write-Host "Running process: $application $arguments"
   
  $tmpProcess = New-Object -TypeName Diagnostics.Process
  $tmpProcessInfo = New-Object -Typename Diagnostics.ProcessStartInfo
  $tmpProcessInfo.FileName = $application
  $tmpProcessInfo.Arguments = $arguments
  $tmpProcessInfo.UseShellExecute = $false  
  $tmpProcessInfo.RedirectStandardOutput = $true
  $tmpProcessInfo.RedirectStandardError = $true
  $tmpProcess.StartInfo = $tmpProcessInfo
    
  $startResult = $tmpProcess.Start()
  $outputMsg = $tmpProcess.StandardOutput.ReadToEnd();
  $errorMsg = $tmpProcess.StandardError.ReadToEnd();
  $tmpProcess.WaitForExit()        
  $exitCode = $tmpProcess.ExitCode    
     
  if ($exitCode -ne 0 -and $failOnError)
  {
    Write-Error "Error running process: $application $arguments
    
    Error:  $errorMsg
    Output: $outputMsg"
    
    throw
  }   
  
  return $outputMsg
}


# ----------------------------------------------------------------------------------------------------------------------
#
# SQL Processing
#
# ----------------------------------------------------------------------------------------------------------------------



#Run a sql command
function Start-SqlCommand([string] $arguments)
{
  Write-Verbose "Running SQL CMD with arguments '$arguments'"
  
  $fileName = "sqlcmd"
  $logFile = Join-Path $scriptPath "run-sql-cmd.log"
  $arguments = "$arguments -o ""$logFile"""
  
  Write-Verbose "Running process: $fileName $arguments"
  
  $tmpProcess = New-Object -TypeName Diagnostics.Process
  $tmpProcessInfo = New-Object -Typename Diagnostics.ProcessStartInfo
  $tmpProcessInfo.FileName = $fileName
  $tmpProcessInfo.Arguments = $arguments
  $tmpProcessInfo.UseShellExecute = $false  
  $tmpProcessInfo.RedirectStandardOutput = $true
  $tmpProcessInfo.RedirectStandardError = $true
  $tmpProcess.StartInfo = $tmpProcessInfo
    
  $startResult = $tmpProcess.Start()
  $outputMsg = $tmpProcess.StandardOutput.ReadToEnd();
  $errorMsg = $tmpProcess.StandardError.ReadToEnd();
  $tmpProcess.WaitForExit()      
  
  $exitCode = $tmpProcess.ExitCode  
  $logFileContents = Get-FileText $logFile   
    
  if ($exitCode -ne 0)
  {
    Write-Error "Error running sql cmd: $fileName $arguments
    
    Error:  $errorMsg
    Output: $outputMsg
    SQL Log: $logFileContents"
    
    throw
  }
}


# ----------------------------------------------------------------------------------------------------------------------
#
# Solution Info Rewrite Using Build Labels
#
# ----------------------------------------------------------------------------------------------------------------------



#-------------------------------------------------------------------------------
# Update version numbers of AssemblyInfo.cs or SolutionInfo.cs file passed in based on the build label and changeset
# Expected build label format is : $(Date:yyyyMMdd)$(Rev:.r)
#-------------------------------------------------------------------------------
function Set-SolutionInfoAssemblyVersionFromStandardMsBuildLabelAndChangeSet
{
    Param([string]$solutionInfoPath, [string] $buildLabel, [string] $changeSet)
    
    $formattedChangeSet = $changeSet.Replace('C', '')
    $formattedChangeSet = $formattedChangeSet.Replace(',', '')
    $buildDate = $buildLabel.Split('.')[0]
    $buildNumber = $buildLabel.Split('.')[1]
    
    Write-Output @"
----------------------------------------------------------------
Updating the solution info file : $solutionInfoPath
Build date : $buildDate
Build number : $buildNumber
Change set : $formattedChangeSet
----------------------------------------------------------------
"@
    
    Set-SolutionInfoAssemblyVersion -solutionInfoPath $solutionInfoPath `
                                    -fileVersionBuildNumber "0" `
                                    -fileVersionRevisionNumber $formattedChangeSet `
                                    -informationVersionBuildNumber $buildDate `
                                    -informationVersionRevisionNumber $buildNumber
}


#-------------------------------------------------------------------------------
# Update version numbers of AssemblyInfo.cs or SolutionInfo.cs file passed in
#-------------------------------------------------------------------------------
function Set-SolutionInfoAssemblyVersion
{
    Param( [string]$solutionInfoPath, [string] $fileVersionBuildNumber, [string] $fileVersionRevisionNumber, [string] $informationVersionBuildNumber, [string] $informationVersionRevisionNumber)

    $assemblyVersionPattern = '(^\[assembly: AssemblyVersion\(\"[0-9]+.[0-9]+)(.[0-9]+)(.[0-9]+)(\"\)\])'
	$fileVersionPattern = '(^\[assembly: AssemblyFileVersion\(\"[0-9]+.[0-9]+)(.[0-9]+)(.[0-9]+)(\"\)\])'
	$assemblyInformationalVersionPattern = '(^\[assembly: AssemblyInformationalVersion\(\"[0-9]+.[0-9]+)(.[0-9]+)(.[0-9]+)(\"\)\])'
    $filename = 'Shared\SharedAssemblyInfo.cs'
    
    $fileVersionBuildNumberShort = $fileVersionBuildNumber % 50000
	$fileVersionRevisionNumberShort = $fileVersionRevisionNumber % 50000	
        
	Get-Item -Path $solutionInfoPath | Set-ItemProperty -Name IsReadOnly -Value $false
	
	(Get-Content $solutionInfoPath) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, "`$1.${fileVersionBuildNumberShort}.${fileVersionRevisionNumberShort}`$4" } |
            % {$_ -replace $fileVersionPattern, "`$1.${fileVersionBuildNumberShort}.${fileVersionRevisionNumberShort}`$4" } |
			% {$_ -replace $assemblyInformationalVersionPattern, "`$1.${informationVersionBuildNumber}.${informationVersionRevisionNumber}`$4" } 
    } | Set-Content $solutionInfoPath -Encoding UTF8	
    
    $updatedSolutionInfoText = Get-FileText $solutionInfoPath	

    Write-Output $updatedSolutionInfoText
}



# ----------------------------------------------------------------------------------------------------------------------
#
# File management
#
# ----------------------------------------------------------------------------------------------------------------------


#Read all of the text from a given file
function Get-FileText([string] $fileName)
{
  $lines = Get-Content $fileName
  $contents = ""
  
  foreach($line in $lines)
  {
    $contents += $line
  }
  
  return $contents
}



#Responsible for outputing every file and directory recursively in a path.
#This is very useful to work out where things are stored on your build server.
function GetFiles($path)
{
    foreach ($item in Get-ChildItem $path)
    {        
        $item
        if (Test-Path $item.FullName -PathType Container)
        {
            GetFiles $item.FullName
        }
    }
} 




# ----------------------------------------------------------------------------------------------------------------------
#
# Azure Functions
#
# ----------------------------------------------------------------------------------------------------------------------


# Responsible for purging an Azure database
function Remove-AzureDatabaseContents 
{   
    Param([string] $serverName, [string] $databaseName, [string] $userName, [string] $password)

    Write-Host "Purging data from: $serverName, $databaseName"	

    $purgeSql = @"
-- Drop all constraints
while(exists(select 1 from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_TYPE='FOREIGN KEY'))
begin
 declare @sql nvarchar(2000)
 SELECT TOP 1 @sql=('ALTER TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME + '] DROP CONSTRAINT [' + CONSTRAINT_NAME + ']')
 FROM information_schema.table_constraints
 WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'
 exec (@sql)
end
go

-- Drop all views
while(exists(select 1 from INFORMATION_SCHEMA.VIEWS))
begin
 declare @sql3 nvarchar(2000)
 SELECT TOP 1 @sql3=('DROP VIEW ' + TABLE_SCHEMA + '.[' + TABLE_NAME + ']')
 FROM INFORMATION_SCHEMA.VIEWS
exec (@sql3)
end
go

-- Drop all tables
while(exists(select 1 from INFORMATION_SCHEMA.TABLES))
begin
 declare @sql2 nvarchar(2000)
 SELECT TOP 1 @sql2=('DROP TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME + ']')
 FROM INFORMATION_SCHEMA.TABLES
exec (@sql2)
end
go

"@

    $arguments = "-S $serverName -d $databaseName -U $userName -P $password -b -m1 -Q ""$purgeSql"""
    Start-SqlCommand $arguments
}




# Creates a new Azure blob container if it does not already exist.
function New-AzurePrivateStorageContainerOrDefault
{
    param(
        [string]$name,
        $context
    )    
    
    $blobContainer = Get-AzureStorageContainer -Context $context -Container $name  -ErrorAction SilentlyContinue

    if ($blobContainer -eq $null)
    {
        $blobContainer = New-AzureStorageContainer -Context $context -Name $name -Permission Off
    }

    return $blobContainer
}

# Creates a new Azure blob container if it does not already exist.
function New-AzureStorageQueueOrDefault
{
    param(
        [string]$name,
        $context
    )    
    
    $storageQueue = Get-AzureStorageQueue -Context $context -Name $name -ErrorAction SilentlyContinue

    if ($storageQueue -eq $null)
    {
        $storageQueue = New-AzureStorageQueue -Context $context -Name $name
    }

    return $storageQueue
}

# Checks to see if a blob exists in the given container or not
function Has-AzureStorageBlob
{
    Param(                
        $BlobContainerContents,
        $BlobName
    )
    
    $blobExists = $false

    foreach($blob in $BlobContainerContents)
    {
        if ($blob.Name -eq $BlobName)
        {
            $blobExists = $true
        }
    }

    return $blobExists
}

# Asserts that a blob exists in the given container or not
function Assert-BlobDoesNotExist
{
    Param(                
        $BlobContainerContents,
        $BlobName
    )
   
    if (Has-AzureStorageBlob -BlobContainerContents $BlobContainerContents -BlobName $BlobName)
    {
        throw "Blob with name $BlobName already exists"
    }
}

# ----------------------------------------------------------------------------------------------------------------------
#
# Error formatting
#
# ----------------------------------------------------------------------------------------------------------------------

function WriteFormattedError([string] $errorMessage)
{
	Write-Error "
	
    -------------------------------------------------------------------------------
	
	$errorMessage
	
	-------------------------------------------------------------------------------
	
	"
	Stop-Transcript	
}



# ----------------------------------------------------------------------------------------------------------------------
#
# Windows Service Management
#
# -----------------------------------------------------------------------------------------------------------------------

$ServiceStateRunning = "RUNNING"
$ServiceStateStartPending = "START PENDING"
$ServiceStatePaused = "PAUSED"
$ServiceStatePausePending = "PAUSE PENDING"
$ServiceStateStopped = "STOPPED"
$ServiceStateStopPending = "STOP PENDING"
$ServiceStateContinuePending = "CONTINUE PENDING"
$ServiceStateUnknown = "UNKNOWN"

function QueryRemoteServiceState([string] $serverName, [string] $serviceName)
{
  $result = Start-Process "sc" "\\$serverName query ""$serviceName""" $true  
  $state = $ServiceStateUnknown

  foreach($line in $result.split("`n"))
  {
    $line = $line.Trim();
    if ($line.StartsWith("STATE"))
    {           
      $words = [regex]::Split($line, "\s")
      $lastWord = $words[$words.length - 1]    
      $state = $lastWord.Trim();
      return $state;
    }    
  }
  
  return $state;
}

function WaitForRemoteServiceState([string] $serverName, [string] $serviceName, [string] $requiredState, [int] $timoutInSeconds)
{
  $finished = $false;
  $secondsRun = 0;
  while($finished -eq $false)
  {
    $state = QueryRemoteServiceState $serverName $serviceName
    if ($state -eq $requiredState)
    {
      $finished = $true;
      return
    }
    
    Start-Sleep -s 1    
    $secondsRun += 1
    
    if ($secondsRun -gt $timoutInSeconds)
    {
      $finished = $true
      return
    }
  }
}

function StopRemoteService([string] $serverName, [string] $serviceName)
{
  Write-Host "Stopping the service '$serviceName' on \\$serverName"

  $state = QueryRemoteServiceState $serverName $serviceName
  
  if (($state -eq $ServiceStateRunning) -or ($state -eq $ServiceStateStartPending) -or 
      ($state -eq $ServiceStatePaused) -or ($state -eq $ServiceStatePausePending))
  {
    $output = Start-Process "sc" "\\$serverName stop ""$serviceName""" $true  
  }
  
   WaitForRemoteServiceState $serverName $serviceName $ServiceStateStopped 30
   
   $state = QueryRemoteServiceState $serverName $serviceName
   
   if ($state -ne $ServiceStateStopped)
   {
      WriteFormattedError("Failed to stop service '$serviceName' on \\$serverName. Status is '$state'")
      Throw
   }
   
   Write-Host "The service '$serviceName' on \\$serverName has been stopped"
}

function StartRemoteService([string] $serverName, [string] $serviceName)
{
  Write-Host "Starting the service '$serviceName' on \\$serverName"

  $state = QueryRemoteServiceState $serverName $serviceName
  
  if (($state -eq $ServiceStateStopped) -or ($state -eq $ServiceStateStopPending) -or ($state -eq $ServiceStatePaused) -or 
      ($state -eq $ServiceStatePausePending) -or ($state -eq $ServiceStateContinuePending))
  {
    $output = Start-Process "sc" "\\$serverName start ""$serviceName""" $true  
  }
  
   WaitForRemoteServiceState $serverName $serviceName $ServiceStateRunning 30
   
   $state = QueryRemoteServiceState $serverName $serviceName
   
   if ($state -ne $ServiceStateRunning)
   {
      WriteFormattedError("Failed to start service '$serviceName' on \\$serverName. Status is '$state'")
      Throw
   }
   
   Write-Host "The service '$serviceName' on \\$serverName has been started"  
}

function RestartRemoteService([string] $serverName, [string] $serviceName)
{
  StopRemoteService $serverName $serviceName
  StartRemoteService $serverName $serviceName
}

function WaitForRemoteServiceToBeRemoved([string] $serviceName, [int] $timoutInSeconds = 30)
{
  $finished = $false;
  $secondsRun = 0;
  while($finished -eq $false)
  {
    if (!(Get-Service $serviceName -ErrorAction SilentlyContinue))
    {
        $finished = $true
        return
    }
    
    
    Start-Sleep -s 1    
    $secondsRun += 1
    
    if ($secondsRun -gt $timoutInSeconds)
    {
      $finished = $true
      return
    }
  }
}



function Confirm-WindowsServiceExists([string] $serviceName, [string] $server)
{   
    if (Get-Service -Name $serviceName -ComputerName $server -ErrorAction SilentlyContinue)
    {
        return $true
    }
    return $false
}

function Remove-WindowsServiceIfItExists([string] $serviceName, [string] $server)
{   
    Write-Output "Removing old service $serviceName"
    $exists = Confirm-WindowsServiceExists $serviceName $server
    if ($exists)
    {    
        Write-Output "Service $serviceName existing stopping the service"

        StopRemoteService $server $serviceName

        Write-Output "Deleting the service $serviceName"

        Start-Process sc.exe "\\$server delete $serviceName" $true

        WaitForRemoteServiceToBeRemoved $serviceName
    }       
}

function Install-WindowsService([string] $serviceName, [string] $server, [string] $serviceExePath, [string] $userName, [string] $password, [string] $displayName, [string] $description)
{
    Write-Output "Installing service $serviceName"
    Start-Process sc.exe "\\$server create ""$serviceName"" binpath= ""$serviceExePath"" start= auto displayname= ""$displayName"" obj= ""$userName"" password= ""$password"" " $true
    Start-Process sc.exe "\\$server description ""$serviceName"" ""$description""" $true    
}



