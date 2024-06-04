@ECHO OFF
cd PWManager.UnitTests
dotnet restore
dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet test --logger "console;verbosity=detailed" --collect:"XPlat Code Coverage"
reportgenerator "-reports:TestResults/*/coverage*" "-targetdir:coverage" "-reporttypes:Html;TextSummary"
type coverage\Summary.txt
start coverage\index.html
pause