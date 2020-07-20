rem to generate reports on scree.
rem check if the paths are ok on your computer!

mkdir ..\Testes.Automatizados.Resultados
del ..\Testes.Automatizados.Resultados\UnitTestsResults.trx  > nul
del ..\Testes.Automatizados.Resultados\Report\*.*  /q > nul


dotnet build "Testes.Automatizados.csproj" /p:DebugType=Full
"%HOMEPATH%\.nuget\packages\OpenCover\4.7.922\tools\OpenCover.Console.exe"  ^
 -target:"C:/Program Files/dotnet/dotnet.exe" -targetargs:"test \"Testes.Automatizados.csproj\" --configuration Debug --no-build" ^
 -filter:"+[Prepedido*]* +[Infra*]* -[Testes*]* " -oldStyle -register:user -output:"..\Testes.Automatizados.Resultados\UnitTestsResults.trx"


if %errorlevel% neq 0 echo echo on
if %errorlevel% neq 0 echo ERRO! Algum teste deu erro.
if %errorlevel% neq 0 pause


"%HOMEPATH%\.nuget\packages\ReportGenerator\4.2.10\tools\net47\ReportGenerator.exe" ^
-reports:"..\Testes.Automatizados.Resultados\UnitTestsResults.trx" ^
-targetdir:"..\Testes.Automatizados.Resultados\Report" 

start "report" "..\Testes.Automatizados.Resultados\Report\index.htm"
