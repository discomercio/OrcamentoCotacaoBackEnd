@echo off

rem perguntar o diretorio do projeto da API (onde está o CompilarPrepedidoApi.bat)
set CaminhoApi=..\PrepedidoApi
IF EXIST %CaminhoApi%\CompilarPrepedidoApi.bat goto prepedidoapi_encontrado
echo %CaminhoApi%\CompilarPrepedidoApi.bat nao existe!
set /p CaminhoApi="Caminho da solucao da API [%CaminhoApi%]: "
:prepedidoapi_encontrado

del .CompilarPrepedidoSpaApiResultado\*.* /s /q >nul 2> nul
rem waiting to avoid errors
timeout /t 1 >nul 2> nul
RD .CompilarPrepedidoSpaApiResultado /s /q >nul 2> nul

rem compilar o projeto da API
pushd %CaminhoApi%
echo Compilando API...
call CompilarPrepedidoApi.bat sem_pausa
popd

IF %ERRORLEVEL% EQU 0 goto compilar_spa
echo ERRO NA COMPILACAO DA API.
IF "%~1" NEQ "sem_pausa" pause
goto final

:compilar_spa
del dist\*.* /s /q >nul 2> nul
rem waiting to avoid errors
timeout /t 1 >nul 2> nul
RD dist /s /q >nul 2> nul

rem construindo src\environments\environment.prod.versaoApi.ts
del src\environments\environment.prod.versaoApi.ts >nul 2> nul
copy src\environments\environment.prod.ts src\environments\environment.prod.versaoApi.ts
echo Compilando SPA...
del .CompilarPrepedidoGerarVersaoApi.tmp.txt >nul 2> nul
node CompilarPrepedidoGerarVersaoApi.js > .CompilarPrepedidoGerarVersaoApi.tmp.txt
node CompilarPrepedidoTrocarVersaoApi.js src\environments\environment.prod.versaoApi.ts SUBSTITUIR_VERSAO_API .CompilarPrepedidoGerarVersaoApi.tmp.txt
IF %ERRORLEVEL% EQU 0 goto compilar_ng
del src\environments\environment.prod.versaoApi.ts
echo ERRO NA COMPILACAO DO src\environments\environment.prod.ts
IF "%~1" NEQ "sem_pausa" pause
goto final

:compilar_ng
call ng build --prod
IF %ERRORLEVEL% EQU 0 goto empacotar
echo ERRO NA COMPILACAO DO SPA.
IF "%~1" NEQ "sem_pausa" pause
goto final

:empacotar
echo Compilado SPA e API com sucesso
del src\environments\environment.prod.versaoApi.ts

mkdir .CompilarPrepedidoSpaApiResultado
pushd .CompilarPrepedidoSpaApiResultado
xcopy ..\%CaminhoApi%\.CompilarPrepedidoApiResultado\publish\*.* /e
mkdir wwwroot
xcopy ..\dist\pre-pedido wwwroot\*.* /e
popd

rem altera a versao da API no arquivo de configuracao da API
node CompilarPrepedidoTrocarVersaoApi.js .CompilarPrepedidoSpaApiResultado\versaoapi.json DEBUG .CompilarPrepedidoGerarVersaoApi.tmp.txt
IF %ERRORLEVEL% EQU 0 goto copiar_datacompilacao
echo ERRO NA COMPILACAO DO .CompilarPrepedidoSpaApiResultado\versaoapi.json
IF "%~1" NEQ "sem_pausa" pause
goto final

:copiar_datacompilacao
copy .CompilarPrepedidoGerarVersaoApi.tmp.txt .CompilarPrepedidoSpaApiResultado\DataCompilacao.txt
copy .CompilarPrepedidoGerarVersaoApi.tmp.txt .CompilarPrepedidoSpaApiResultado\wwwroot\DataCompilacao.txt
del .CompilarPrepedidoGerarVersaoApi.tmp.txt >nul 2> nul


echo .
echo .
echo .
echo .
echo Compilacao finalizada com sucesso. A publicacao esta no diretorio .CompilarPrepedidoSpaApiResultado
IF "%~1" NEQ "sem_pausa" pause

:final
