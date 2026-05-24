 @echo off
 REM Batch-Datei für CS2SX-Befehle
 REM Wähle einen Befehl: clean, build, watch
 set /p cmd=Enter command (clean/build/watch):
 if "%cmd%"=="clean" (
     cs2sx clean fontSwitch.csproj
 ) else if "%cmd%"=="build" (
     cs2sx build fontSwitch.csproj
 ) else if "%cmd%"=="watch" (
     cs2sx watch fontSwitch.csproj
 ) else (
     echo Invalid command. Use clean, build, or watch.
 )
pause