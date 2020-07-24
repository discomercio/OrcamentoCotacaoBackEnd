rem compilar o porjeto da API
rem para a compilacao total, usar o PrepedidoSpa\CompilarPrepedidoSpaApi.bat, no projeto do SPA
set aux_msbuild="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"

IF EXIST %aux_msbuild% goto msbuild_encontrado
echo %aux_msbuild% nao existe!
set /p aux_msbuild="Caminho do MSBuild [%aux_msbuild%]: "
:msbuild_encontrado


del PacoteApiUnis\*.* /s /q >nul 2> nul
rem waiting to avoid errors
timeout /t 1 >nul
RD PacoteApiUnis /s /q >nul 2> nul

%aux_msbuild% ../../ApiUnis/PrepedidoAPIUnis/PrepedidoAPIUnis.csproj -t:Rebuild -p:DeployOnBuild=true -p:PublishProfile=CompilarPrepedidoApiUnis /p:Configuration=Release

echo %date% %time% >> "PacoteApiUnis\publish\DataCompilacao.txt"
del PacoteApiUnis\publish\appsettings.json
del PacoteApiUnis\publish\appsettings.Development.json
del PacoteApiUnis\publish\nlog.config

echo pacote gerado em PacoteApiUnis
IF "%~1" NEQ "sem_pausa" pause

