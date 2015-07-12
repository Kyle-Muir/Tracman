properties {
	#----------
	#Properties
	#PSake properties, these cannot be changed once they're defined
	#----------

	#----------	
	#Deployment Variables
	#----------	

	#----------	
	#Environmental Variables for Development
	#----------	
	$projectName = "Tracman"
    $buildNumber = "DEV"
	$config = "Release"
    
	#----------	
	#Directories
	#----------
	$baseDir = Resolve-Path .
    $sourceDir = "$baseDir\Source"
	$toolsDir = "$baseDir\Tools"
	$scriptsDir = "$baseDir\Scripts"
    $artefactsDir = "$baseDir\Artefacts"
		
	#----------	
	#Variable dependent properties
	#Properties that depend on other variables to be defined first
	#----------
	$solutionFilePath = "$sourceDir\$projectName.sln"
	$mainProjectFile = "$sourceDir\$projectName\$projectName.csproj"
	
	#----------	
	#Project Specific Variables
	#----------
	$visualStudioVersion = "12.0"
}

#----------	
#Includes
#Script files to include for function definitions
#----------
$path = Resolve-Path .
Include Scripts\Logger.ps1 -ErrorAction Stop
Include Scripts\VisualStudioExtensions.ps1 -ErrorAction Stop
Include Scripts\NUnitExtensions.ps1 -ErrorAction Stop

#-------------
#Build Targets
#Targets to build
#-------------

#-------------------------
#Targets
#-------------------------

task default -depends initial

task initial -depends preamble, clean, nugetRestore, compile, runUnitTests, postamble

task buildServer -depends initial

#-----------
#Build Steps
#Steps to execute
#-----------

task preamble {
	Log "Executing build number: $buildNumber Mode: $config" Green
}

task postamble {
	$date = Get-Date
	Log "Build number $buildNumber complete at $date" Green
}

task clean {
	Log "Cleaning solution $solutionFilePath" Green
	Clean-VisualStudioSolution  $solutionFilePath $config $isBuildServerTarget $visualStudioVersion
}

task nugetRestore {
	Log "Restoring nuget packages for the solution" Green
	Push-Location $sourceDir
	$nugetExe = ".nuget\nuget.exe"
	&$nugetExe "restore"
	Pop-Location
	Log "Nuget packages successfully restored" Green
}

task compile {
	Log "Building solution $sourceDir\$projectName.sln in $config mode" Green
    Build-VisualStudioSolution $solutionFilePath $config $isBuildServerTarget $visualStudioVersion
}

task runUnitTests {
	Log "Recreating artefacts directory" Green
    Remove-Item $artefactsDir -Recurse -Confirm:$false -ErrorAction SilentlyContinue
    New-Item -ItemType Directory $artefactsDir
    Log "Artefacts directory recreated" Green
    
    Log "Running Unit Tests" Green	
    Copy-FilesFromDirectories "$sourceDir\Test\*Test*\bin\$config" $artefactsDir
	$tests = Get-ChildItem $artefactsDir\*.Test*.dll -Exclude $excludedProjectRegex
	Foreach ($test in $tests) {
		$testPath = $test.FullName
		$testName = $test.Name
		Log "Running test suite: $testName" Yellow
		Run-Nunit $toolsDir $testPath $resultsDir
	}
}

#-----------
#Generalised functions
#Any other generic functions can be stored here
#-----------