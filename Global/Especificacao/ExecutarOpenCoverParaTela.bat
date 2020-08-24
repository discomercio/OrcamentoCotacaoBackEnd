rem to generate reports on scree.
rem check if the paths are ok on your computer!

mkdir ..\Testes.Automatizados.Resultados
del ..\Testes.Automatizados.Resultados\UnitTestsResults.trx  > nul
del ..\Testes.Automatizados.Resultados\Report\*.*  /q > nul

echo ApiUnis\Testes.Automatizados\Testes.Automatizados.csproj 
dotnet build "..\..\ApiUnis\Testes.Automatizados\Testes.Automatizados.csproj" /p:DebugType=Full
"%HOMEPATH%\.nuget\packages\OpenCover\4.7.922\tools\OpenCover.Console.exe"  ^
 -returntargetcode ^
 -target:"C:/Program Files/dotnet/dotnet.exe" -targetargs:"test \"..\..\ApiUnis\Testes.Automatizados\Testes.Automatizados.csproj\" --configuration Debug --no-build" ^
 -filter:"+[Prepedido*]* +[Infra*]* -[Testes*]* " -oldStyle -register:user -output:"..\Testes.Automatizados.Resultados\UnitTestsResults.trx"


if %errorlevel% neq 0 echo echo on
if %errorlevel% neq 0 echo ERRO! Algum teste deu erro.
if %errorlevel% neq 0 pause



echo Especificacao.csproj
dotnet build "Especificacao.csproj" /p:DebugType=Full
"%HOMEPATH%\.nuget\packages\OpenCover\4.7.922\tools\OpenCover.Console.exe"  ^
 -mergebyhash -mergeoutput -returntargetcode ^
 -target:"C:/Program Files/dotnet/dotnet.exe" -targetargs:"test \"Especificacao.csproj\" --configuration Debug --no-build" ^
 -filter:"+[*]* +[Infra*]* -[Especificacao*]* -[xunit*]* " -oldStyle -register:user -output:"..\Testes.Automatizados.Resultados\UnitTestsResults.trx"


if %errorlevel% neq 0 echo echo on
if %errorlevel% neq 0 echo ERRO! Algum teste deu erro.
if %errorlevel% neq 0 pause


"%HOMEPATH%\.nuget\packages\ReportGenerator\4.2.10\tools\net47\ReportGenerator.exe" ^
-reports:"..\Testes.Automatizados.Resultados\UnitTestsResults.trx" ^
-targetdir:"..\Testes.Automatizados.Resultados\Report" 

start "report" "..\Testes.Automatizados.Resultados\Report\index.htm"

