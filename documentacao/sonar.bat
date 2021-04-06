rem sonar rodando local em http://localhost:9000/projects
echo off

cd ..

set aux_msbuild="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"

IF EXIST %aux_msbuild% goto msbuild_encontrado
echo %aux_msbuild% nao existe!
set /p aux_msbuild="Caminho do MSBuild [%aux_msbuild%]: "
:msbuild_encontrado



set aux_sonarbuild="C:\utils\sonar-scanner-msbuild-4.6.2.2108-net46\SonarScanner.MSBuild.exe"

IF EXIST %aux_sonarbuild% goto sonarbuild_encontrado
echo %aux_sonarbuild% nao existe!
set /p aux_sonarbuild="Caminho do sonarbuild [%aux_sonarbuild%]: "
:sonarbuild_encontrado

%aux_sonarbuild% begin /k:"arclube" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="a24a55b29991dbc76de1ec568dcbbabe32714203"

%aux_msbuild%  /t:Rebuild

%aux_sonarbuild%  end /d:sonar.login="a24a55b29991dbc76de1ec568dcbbabe32714203"

cd documentacao
