rem compilar o porjeto da API
rem para a compilacao total, usar o PrepedidoSpa\CompilarPrepedidoSpaApi.bat, no projeto do SPA
set aux_msbuild="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"

IF EXIST %aux_msbuild% goto msbuild_encontrado
echo %aux_msbuild% nao existe!
set /p aux_msbuild="Caminho do MSBuild [%aux_msbuild%]: "
:msbuild_encontrado


del CompilarPrepedidoApiResultado\*.* /s /q >nul 2> nul
rem waiting to avoid errors
timeout /t 1 >nul
RD CompilarPrepedidoApiResultado /s /q >nul 2> nul

%aux_msbuild% ../../../PrepedidoApi/PrepedidoApi/PrepedidoApi.csproj -t:Rebuild -p:DeployOnBuild=true -p:PublishProfile=CompilarPrepedidoApi /p:Configuration=Release
del CompilarPrepedidoApiResultado\publish\appsettings.json
del CompilarPrepedidoApiResultado\publish\appsettings.Development.json
del CompilarPrepedidoApiResultado\publish\nlog.config

IF "%~1" NEQ "sem_pausa" pause

