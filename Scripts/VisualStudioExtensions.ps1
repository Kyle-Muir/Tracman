
function Build-VisualStudioSolution ($thingToBuild, $configuration, $buildTarget, $visualStudioVersion = "12.0") {
	exec { msbuild $thingToBuild /t:Build /p:Configuration=$configuration /m /nologo /p:VisualStudioVersion=$visualStudioVersion }
}

function Clean-VisualStudioSolution ($thingToClean, $configuration, $buildTarget, $visualStudioVersion = "12.0") {
	exec { msbuild $thingToClean /t:Clean /p:Configuration=$configuration /m /nologo /p:VisualStudioVersion=$visualStudioVersion } 
}

function WebDeploy-MVCSolution ($thingToPublish, $publishProfile, $configuration, $buildTarget, $visualStudioVersion = "12.0") {
	exec { msbuild $thingToPublish /p:DeployOnBuild=true /p:PublishProfile=$publishProfile /p:Configuration=$configuration /m /nologo /p:VisualStudioVersion=$visualStudioVersion } 
}

function Package-MVCSolution ($thingToPublish, $configuration, $packagePath, $projectName, $packageName, $buildTarget, $buildNumber, $iisAppPath, $visualStudioVersion = "12.0") {	
	exec { msbuild $thingToPublish /p:DeployOnBuild=true /P:Configuration="$configuration;PackageLocation=$packagePath\$projectName-$buildNumber-$configuration-$packageName.zip" /P:DeployIisAppPath="$iisAppPath" /m /nologo /p:VisualStudioVersion=$visualStudioVersion } 
}

function Package-WebApiSolution ($thingToPublish, $configuration, $packagePath, $projectName, $packageName, $buildTarget, $buildNumber, $iisAppPath, $visualStudioVersion = "12.0") {	
	exec { msbuild $thingToPublish /T:Package /P:Configuration="$configuration;PackageLocation=$packagePath\$projectName-$buildNumber-$configuration-$packageName.zip" /P:DeployIisAppPath="$iisAppPath" /m /nologo /p:VisualStudioVersion=$visualStudioVersion } 
}