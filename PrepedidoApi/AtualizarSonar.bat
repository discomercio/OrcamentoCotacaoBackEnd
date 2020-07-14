rem usando caminhos fixos

"C:\utils\sonar-scanner-msbuild-4.6.2.2108-net46\SonarScanner.MSBuild.exe" begin /k:"ArClube.PrepedidoApi" /d:sonar.host.url="http://sonar.its.teste:9000" ^
/d:sonar.login="fe6a41adc65a8a3eb8932b110ffb5f4a0e6fefaa" 

"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild" /t:Rebuild

"C:\utils\sonar-scanner-msbuild-4.6.2.2108-net46\SonarScanner.MSBuild.exe" end /d:sonar.login="fe6a41adc65a8a3eb8932b110ffb5f4a0e6fefaa"

pause
