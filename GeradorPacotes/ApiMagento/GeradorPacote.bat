rem compilar o projeto da ApiMagento
set aux_msbuild="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"

IF EXIST %aux_msbuild% goto msbuild_encontrado
echo %aux_msbuild% nao existe!
set /p aux_msbuild="Caminho do MSBuild [%aux_msbuild%]: "
:msbuild_encontrado


del PacoteApiMagento\*.* /s /q >nul 2> nul
rem waiting to avoid errors
timeout /t 1 >nul
RD PacoteApiMagento /s /q >nul 2> nul

rem %aux_msbuild% ../../ApiMagento/ApiMagento/ApiMagento/ApiMagento.csproj -t:Rebuild -p:DeployOnBuild=true -p:PublishProfile=CompilarApiMagento /p:Configuration=Release
%aux_msbuild% ../../ArClube.sln -t:Rebuild -p:DeployOnBuild=true -p:PublishProfile=CompilarApiMagento /p:Configuration=Release_ApiMagento

echo %date% %time% >> "PacoteApiMagento/publish/DataCompilacao.txt"
del PacoteApiMagento\publish\appsettings.json
del PacoteApiMagento\publish\appsettings.Development.json
del PacoteApiMagento\publish\nlog.config

echo pacote gerado em PacoteApiMagento
IF "%~1" NEQ "sem_pausa" pause

