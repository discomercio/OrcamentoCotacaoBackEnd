rem compilar o projeto da loja
echo off
set aux_msbuild="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"

IF EXIST %aux_msbuild% goto msbuild_encontrado
echo %aux_msbuild% nao existe!
set /p aux_msbuild="Caminho do MSBuild [%aux_msbuild%]: "
:msbuild_encontrado


del PacoteLoja\lojamvc\*.* /s /q >nul 2> nul
rem waiting to avoid errors
timeout /t 1 >nul
RD PacoteLoja\lojamvc /s /q >nul 2> nul

rem %aux_msbuild% ../../ApiUnis/PrepedidoAPIUnis/PrepedidoAPIUnis.csproj -t:Rebuild -p:DeployOnBuild=true -p:PublishProfile=CompilarPrepedidoApiUnis /p:Configuration=Release
%aux_msbuild% ../../ArClube.sln -t:Rebuild -p:DeployOnBuild=true -p:PublishProfile=CompilarLoja /p:Configuration=Release_Loja
IF %ERRORLEVEL% EQU 0 goto continuar
echo ERRO NA COMPILACAO
echo Uma possivel causa eh o IIS local. Tente parar e reinciar o IIS deste computador.
pause
goto final

:continuar

echo %date% %time% >> "PacoteLoja\lojamvc\DataCompilacao.txt"
git branch > "PacoteLoja\lojamvc\VersaoGit.txt"
git log -n 1 --pretty="%%H %%D" >> "PacoteLoja\lojamvc\VersaoGit.txt"
git status  >> "PacoteLoja\lojamvc\VersaoGit.txt"
del PacoteLoja\lojamvc\appsettings.json
del PacoteLoja\lojamvc\appsettings.Development.json
del PacoteLoja\lojamvc\nlog.config

rem temos que apagar os JS temporários da compilação
del PacoteLoja\lojamvc\wwwroot\scriptsJs\*.* /s /q >nul 2> nul
timeout /t 1 >nul
RD PacoteLoja\lojamvc\wwwroot\scriptsJs /s /q >nul 2> nul


echo pacote gerado em PacoteLoja\lojamvc\
IF "%~1" NEQ "sem_pausa" pause

:final

