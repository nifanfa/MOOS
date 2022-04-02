if not "%1"=="7" start /min cmd /c ""%~0" 7 %*" & exit /b
set F=%TEMP%\ImDisk%TIME::=%
extrac32.exe /e /l "%F%" "%~dp0files.cab"
"%F%\config.exe" %2 %3 %4
rd /s /q "%F%"