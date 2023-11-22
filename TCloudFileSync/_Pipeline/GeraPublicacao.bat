@echo Bat de gerar publicacao - GeraPublicacao.bat

@echo Verificar se pasta de publicacao e subpastas existem e se tem conteudo caso sim as deleta caso contrario realiza a criacao
if exist "publicacaoSFTP" (
	del /q "publicacaoSFTP\*"
	for /D %%p in ("publicacaoSFTP\*.*") do rmdir "%%p" /s /q
) else (
    mkdir "publicacaoSFTP"
)

if exist "publicacaoSFTP\Frontend" (
	del /q "publicacaoSFTP\Frontend\*"
	for /D %%p in ("publicacaoSFTP\Frontend\*.*") do rmdir "%%p" /s /q
) else (
    mkdir "publicacaoSFTP\Frontend"
)

if exist "publicacaoSFTP\Backend" (
	del /q "publicacaoSFTP\Backend\*"
	for /D %%p in ("publicacaoSFTP\Backend\*.*") do rmdir "%%p" /s /q
) else (
    mkdir "publicacaoSFTP\Backend"
)

if exist "publicacaoSFTP.zip" (
    del /q "publicacaoSFTP.zip"
)

@echo Buildando front end
cd ../Backend/src/TCloudFileSync.Angular
dotnet clean
dotnet restore
dotnet publish -c Release -o ../../../_Pipeline/publicacaoSFTP/Frontend -f net7.0 --self-contained -r win-x64 -p:PublishSingleFile=true
del /q "..\..\..\_Pipeline\publicacaoSFTP\Frontend\*.pdb"

@echo Buildando back end
cd ../../../../TCloudFileSync/Backend/src/TCloudFileSync.Api
dotnet clean
dotnet restore
dotnet publish -c Release -o ../../../_Pipeline/publicacaoSFTP/Backend -f net7.0 --self-contained -r win-x86 -p:PublishSingleFile=true
del /q "..\..\..\_Pipeline\publicacaoSFTP\Backend\*.pdb"

cd ../../../_Pipeline
if not exist "publicacaoSFTP\Frontend\*.*" (
    echo O diretorio de build do Angular nao contem arquivos.
    echo A publicacao sera interrompida.
    pause
)

if not exist "publicacaoSFTP\Backend\*.*" (
    echo O diretorio de build do API nao contem arquivos.
    echo A publicacao sera interrompida.
    pause
)

@echo Copia bat install para pasta de publicacao
echo D|xcopy /S /E /D "../Batchs" "publicacaoSFTP"

@echo Compactando
for /d %%X in (*) do del /q "%%X.zip" & "C:\Program Files\7-Zip\7z.exe" a "%%X.zip" "%%X\."

rmdir /s /q publicacaoSFTP

pause
