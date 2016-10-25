@echo off
:repeat
cls
%~dp0.\packages\Machine.Specifications.Runner.Console.0.9.2\tools\mspec-clr4.exe --include "Integrationtests" --no-teamcity-autodetect %~dp0.\binaries\CastleWindsorSamples.IntegrationTests.dll

CHOICE /C "RQ" /M "Choose (r)epeat or (q)uit"
IF ERRORLEVEL 2 GOTO quit
IF ERRORLEVEL 1 GOTO repeat

:quit
exit
