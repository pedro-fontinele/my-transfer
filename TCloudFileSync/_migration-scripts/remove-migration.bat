@echo off

echo Removendo migration...
cd ..\Backend\src\TCloudFileSync.Api

dotnet ef migrations remove --project ..\TCloudFileSync.Infra

pause;