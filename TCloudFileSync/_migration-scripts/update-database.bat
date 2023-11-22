@echo off

echo Atualizando migration no banco de dados...
cd ..\Backend\src\TCloudFileSync.Api

dotnet ef database update --project ..\TCloudFileSync.Infra

pause;