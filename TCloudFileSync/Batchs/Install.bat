@echo Cria as pastas do programa e deletar elas se existirem
cd /d "%~dp0"

if not exist "C:\C5Client" (
   mkdir "C:\C5Client"
)

if exist "C:\C5Client\TOTVS\TCloudFileSync.Setup" (
	del /q "C:\C5Client\TOTVS\TCloudFileSync.Setup\*"
	for /D %%p in ("C:\C5Client\TOTVS\TCloudFileSync.Setup\*.*") do rmdir "%%p" /s /q
) else (
	mkdir "C:\C5Client\TOTVS\TCloudFileSync.Setup\Frontend\wwwroot\assets\images"
	mkdir "C:\C5Client\TOTVS\TCloudFileSync.Setup\Backend"
)

@echo Move pasta do front para pastar criada acima
copy "Frontend" "C:\C5Client\TOTVS\TCloudFileSync.Setup\Frontend"
copy "Frontend\wwwroot" "C:\C5Client\TOTVS\TCloudFileSync.Setup\Frontend\wwwroot"
copy "Frontend\wwwroot\assets" "C:\C5Client\TOTVS\TCloudFileSync.Setup\Frontend\wwwroot\assets"
copy "Frontend\wwwroot\assets\images" "C:\C5Client\TOTVS\TCloudFileSync.Setup\Frontend\wwwroot\assets\images"

copy "Backend" "C:\C5Client\TOTVS\TCloudFileSync.Setup\Backend"

SET AppDescription=TOTVS_AGENTE_TCLOUD
SET IconName=totvs.ico
SET Shortcut_Name=TOTVS_AGENTE_TCLOUD.url
SET URL_PATH=http://localhost:4200

SET WORKING_PATH=%~dp0
SET ICONDEST=c:\ProgramData\%AppDescription%
SET LinkPath=%userprofile%\Desktop\%Shortcut_Name%

@echo. Copy Icon 
IF EXIST "%ICONDEST%" (GOTO _CopyIcon) 
mkdir "%ICONDEST%"
:_CopyIcon 
copy "%WORKING_PATH%%IconName%" "%ICONDEST%"

echo. Create desktop shortcut... 
echo [InternetShortcut] > "%LinkPath%"
echo URL=%URL_PATH% >> "%LinkPath%"
echo IDList= >> "%LinkPath%"
echo IconFile=%ICONDEST%\%IconName% >> "%LinkPath%"
echo IconIndex=0 >> "%LinkPath%"
echo HotKey=0 >> "%LinkPath%"
echo.You should now have a shortcut to %AppDescription% on your desktop... 

@echo Copia bat install para pasta de publicacao
copy "TCloudFileSync.Api.lnk" "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\"
copy "TCloudFileSync.Angular.lnk" "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup\"

copy "TCloudFileSync.Api.lnk" "%ProgramData%\Microsoft\Windows\Start Menu\Programs\Startup\"
copy "TCloudFileSync.Angular.lnk" "%ProgramData%\Microsoft\Windows\Start Menu\Programs\Startup\"

@REM copy "TCloudFileSync.Api.lnk" "%userprofile%\desktop"
@REM copy "TCloudFileSync.Angular.lnk" "%userprofile%\desktop"

echo TOTVS AGENTE TCLOUD instalado com sucesso.
echo .
@echo ***************************************************************************** 
@echo * Para concluir a instalacao, e necessario que o computador seja reiniciado * 
@echo * Deseja reiniciar agora?                                                   *
@echo *****************************************************************************

@echo off
:input
set /p choice=Reiniciar a maquina? (S/N): 
if /i "%choice%"=="S" (
    shutdown /r /t 0
) else if /i "%choice%"=="N" (
    echo Voce optou por nao reiniciar a maquina.
) else (
    echo Opcao invalida. Por favor, digite S para reiniciar ou N para nao reiniciar.
    goto input
)

pause
