@echo off

echo Removendo migration...
cd ..\Backend\src\TCloudFileSync.Api

dotnet ef migrations revert --project ..\TCloudFileSync.Infra

pause;