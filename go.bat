@echo off
powershell.exe -NoProfile -ExecutionPolicy unrestricted -Command "import-module .\Tools\PSake\psake.psm1; invoke-psake build.ps1 %1 -properties @{config='Release'; buildNumber='dev'} -framework 4.0x64;if ($psake.build_success -eq $false) { exit 1 } else { exit 0;}"