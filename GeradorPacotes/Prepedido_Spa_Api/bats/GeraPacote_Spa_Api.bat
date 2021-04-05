@echo off

del ..\Pacote_Spa_Api\*.* /s /q >nul 2> nul
rem waiting to avoid errors
timeout /t 1 >nul 2> nul
RD ..\Pacote_Spa_Api /s /q >nul 2> nul

rem compilar o projeto da API
echo Compilando API...
call CompilarPrepedidoApi.bat sem_pausa

IF %ERRORLEVEL% EQU 0 goto compilar_spa
echo ERRO NA COMPILACAO DA API.
IF "%~1" NEQ "sem_pausa" pause
goto final

:compilar_spa
pushd ..\..\..\PrepedidoSpa
del dist\*.* /s /q >nul 2> nul
rem waiting to avoid errors
timeout /t 1 >nul 2> nul
RD dist /s /q >nul 2> nul

rem construindo src\environments\environment.prod.versaoApi.ts
del src\environments\environment.prod.versaoApi.ts >nul 2> nul
copy src\environments\environment.prod.ts src\environments\environment.prod.versaoApi.ts
echo Compilando SPA...
del .CompilarPrepedidoGerarVersaoApi.tmp.txt >nul 2> nul
node ..\GeradorPacotes\Prepedido_Spa_Api\bats\CompilarPrepedidoGerarVersaoApi.js > .CompilarPrepedidoGerarVersaoApi.tmp.txt
node ..\GeradorPacotes\Prepedido_Spa_Api\bats\CompilarPrepedidoTrocarVersaoApi.js src\environments\environment.prod.versaoApi.ts SUBSTITUIR_VERSAO_API .CompilarPrepedidoGerarVersaoApi.tmp.txt
IF %ERRORLEVEL% EQU 0 goto compilar_ng
del src\environments\environment.prod.versaoApi.ts
echo ERRO NA COMPILACAO DO src\environments\environment.prod.ts
IF "%~1" NEQ "sem_pausa" pause
goto final

:compilar_ng
call ng build --prod
IF %ERRORLEVEL% EQU 0 goto empacotar
echo ERRO NA COMPILACAO DO SPA. VERIFIQUE SE RODOU O COMANDO "npm install" NO DIRETORIO "..\PrepedidoSpa".
IF "%~1" NEQ "sem_pausa" pause
goto final

:empacotar
echo Compilado SPA e API com sucesso
del src\environments\environment.prod.versaoApi.ts

popd
mkdir ..\Pacote_Spa_Api
pushd ..\Pacote_Spa_Api
xcopy ..\bats\CompilarPrepedidoApiResultado\publish\*.* /e
mkdir wwwroot
xcopy ..\..\..\PrepedidoSpa\dist\pre-pedido wwwroot\*.* /e
popd

rem altera a versao da API no arquivo de configuracao da API
node CompilarPrepedidoTrocarVersaoApi.js ..\Pacote_Spa_Api\versaoapi.json DEBUG ..\..\..\PrepedidoSpa\.CompilarPrepedidoGerarVersaoApi.tmp.txt
IF %ERRORLEVEL% EQU 0 goto copiar_datacompilacao
echo ERRO NA COMPILACAO DO Pacote_Spa_Api\versaoapi.json
IF "%~1" NEQ "sem_pausa" pause
goto final

:copiar_datacompilacao
copy ..\..\..\PrepedidoSpa\.CompilarPrepedidoGerarVersaoApi.tmp.txt ..\Pacote_Spa_Api\DataCompilacao.txt
copy ..\..\..\PrepedidoSpa\.CompilarPrepedidoGerarVersaoApi.tmp.txt ..\Pacote_Spa_Api\wwwroot\DataCompilacao.txt
del ..\..\..\PrepedidoSpa\.CompilarPrepedidoGerarVersaoApi.tmp.txt >nul 2> nul

git branch >> "..\Pacote_Spa_Api\VersaoGit.txt"
git show >> "..\Pacote_Spa_Api\VersaoGit.txt"

rem apagar temporarios
del CompilarPrepedidoApiResultado\*.* /s /q >nul 2> nul
rem waiting to avoid errors
timeout /t 1 >nul 2> nul
RD CompilarPrepedidoApiResultado /s /q >nul 2> nul

del wwwroot\*.* /s /q >nul 2> nul
rem waiting to avoid errors
timeout /t 1 >nul 2> nul
RD wwwroot /s /q >nul 2> nul



echo .
echo .
echo .
echo .
echo Compilacao finalizada com sucesso. A publicacao esta no diretorio Pacote_Spa_Api
cd ..
IF "%~1" NEQ "sem_pausa" pause

:final
