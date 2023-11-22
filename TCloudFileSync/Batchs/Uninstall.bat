@echo Desinstala programa e deleta pastas

cd /d "%~dp0"

set "Angular=TCloudFileSync.Angular.exe"
    taskkill /f /im %Angular%
tasklist /fi "imagename eq %Angular%" | findstr /i "%Angular%"
if %errorlevel% equ 0 (
    echo O programa foi encerrado.
) else (
    echo O programa nao esta em execucao, nenhum processo foi encerrado.
)

set "Api=TCloudFileSync.Api.exe"
tasklist /fi "imagename eq %Api%" | findstr /i "%Api%"
if %errorlevel% equ 0 (
    taskkill /f /im %Api%
    echo O programa foi encerrado.
) else (
    echo O programa nao esta em execucao, nenhum processo foi encerrado.
)

if exist "C:\C5Client\TOTVS\TCloudFileSync.Setup" (
	rmdir /s /q "C:\C5Client\TOTVS\TCloudFileSync.Setup"	
)

if exist "%userprofile%\desktop\TOTVS_AGENTE_TCLOUD.url"  (
    del "%userprofile%\desktop\TOTVS_AGENTE_TCLOUD.url"
)

if exist "%userprofile%\desktop\TCloudFileSync.Api.lnk"  (
    del "%userprofile%\desktop\TCloudFileSync.Api.lnk"
)

if exist "%userprofile%\desktop\TCloudFileSync.Angular.lnk"  (
    del "%userprofile%\desktop\TCloudFileSync.Angular.lnk"
)

if exist "%ProgramData%\Microsoft\Windows\Start Menu\Programs\Startup\TCloudFileSync.Api.lnk" (
    del "%ProgramData%\Microsoft\Windows\Start Menu\Programs\Startup\TCloudFileSync.Api.lnk"
)

if exist "%ProgramData%\Microsoft\Windows\Start Menu\Programs\Startup\TCloudFileSync.Angular.lnk" (
    del "%ProgramData%\Microsoft\Windows\Start Menu\Programs\Startup\TCloudFileSync.Angular.lnk"
)

if exist "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\TCloudFileSync.Api.lnk" (
  del "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\TCloudFileSync.Api.lnk"
)

if exist "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\TCloudFileSync.Angular.lnk" (
    del "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\TCloudFileSync.Angular.lnk"
)

@echo TOTVS AGENTE TCLOUD desinstalado com sucesso

pause
