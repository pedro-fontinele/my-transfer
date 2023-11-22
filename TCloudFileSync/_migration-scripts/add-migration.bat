sa@echo off

set /p MigrationName=Insira o nome da migration: 

echo Adicionando migration...
cd ..\Backend\src\TCloudFileSync.Api

dotnet ef migrations add %MigrationName% --project ..\TCloudFileSync.Infra

pause;