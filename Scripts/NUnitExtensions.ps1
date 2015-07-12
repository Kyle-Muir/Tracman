function Copy-FilesFromDirectories($filter, $dest) {
	$itemsInFolder = Get-ChildItem $filter -Recurse | where { !$_.Name.EndsWith(".pdb") } | foreach { $_.FullName }
	Foreach($itemInFolder in $itemsInFolder) {
		Copy-Item -path $itemInFolder -dest $dest
	}
}

function Run-Nunit ($toolsDir, $testAssembly, $resultsDir)
{
    $nunit = "$toolsDir\NUnit\bin\nunit-console.exe"
    exec { &$nunit $testAssembly }
}