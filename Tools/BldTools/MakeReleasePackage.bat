@echo off
IF "%~1." == "." GOTO Help
IF NOT "%~1." == "." GOTO Build

:Help
@echo on
echo "Usage: MakeReleasePackage.bat <version>"
@echo off
GOTO End

:Build
@echo on
rmdir BuildPackage /s /q

mkdir BuildPackage
cd BuildPackage
mkdir Release
cd Release
mkdir dotnet48
mkdir WindowsInstaller3_1
cd ..

mkdir ESSInstaller_%1
mkdir ESSInstaller_%1\dotnet48
mkdir ESSInstaller_%1\WindowsInstaller3_1
cd ../../../Source/

echo "Restoring nutget packages"
dotnet restore Utilties\Utilties.csproj
dotnet restore BinaryLogDecoder\BinaryLogDecoder.csproj
dotnet restore Business\Business.csproj
dotnet restore CGRS_Core\CGRS_Core.csproj
dotnet restore ESS\ESS.csproj
dotnet restore IPI_Core\IPI_Core.csproj
dotnet restore LogViewer\LogViewer.csproj
dotnet restore Oasis.Agent\Oasis.Agent.csproj
dotnet restore Oasis.Common\Oasis.Common.csproj
dotnet restore UnitTest\UnitTest.csproj

echo "ReBuilding Release"
devenv "CGRS.sln" /Rebuild Release  > ..\Tools\BldTools\BuildPackage\Release\buildlog_Release.txt

copy ESSSetup\Release\setup.exe ..\Tools\BldTools\BuildPackage\Release\setup.exe /y
copy ESSSetup\Release\ESSSetup.msi ..\Tools\BldTools\BuildPackage\Release\ESSSetup.msi /y
copy ..\Tools\BldTools\additionalBinaries\ndp48-x86-x64-allos-enu.exe ..\Tools\BldTools\BuildPackage\Release\dotnet48\ndp48-x86-x64-allos-enu.exe /y
copy ..\Tools\BldTools\additionalBinaries\WindowsInstaller-KB893803-v2-x86.exe ..\Tools\BldTools\BuildPackage\Release\WindowsInstaller3_1\WindowsInstaller-KB893803-v2-x86.exe /y

echo "Build Complete"

echo "Creating Installation Package"

copy ESSSetup\Release\setup.exe ..\Tools\BldTools\BuildPackage\ESSInstaller_%1\setup.exe /y
copy ESSSetup\Release\ESSSetup.msi ..\Tools\BldTools\BuildPackage\ESSInstaller_%1\ESSSetup.msi /y
copy ..\Tools\BldTools\additionalBinaries\ndp48-x86-x64-allos-enu.exe ..\Tools\BldTools\BuildPackage\ESSInstaller_%1\dotnet48\ndp48-x86-x64-allos-enu.exe /y
copy ..\Tools\BldTools\additionalBinaries\WindowsInstaller-KB893803-v2-x86.exe ..\Tools\BldTools\BuildPackage\ESSInstaller_%1\WindowsInstaller3_1\WindowsInstaller-KB893803-v2-x86.exe /y

echo "Zipping Installation Package"
cd ..\Tools\BldTools\BuildPackage\ESSInstaller_%1\
powershell Compress-Archive  -DestinationPath ..\ESSInstaller_%1.zip *

echo "Installation Package Ready"

cd ../../../../


:End