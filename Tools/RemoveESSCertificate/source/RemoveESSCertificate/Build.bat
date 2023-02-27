echo "Starting Release Build"

dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true
copy bin\Release\net6.0\win-x64\publish\RemoveESSCertificate.exe ..\..\RemoveESSCertificateTool /y

echo "Build Completed"