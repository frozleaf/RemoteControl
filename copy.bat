@echo off
set OutputDir=E:\Projects\C#\RemoteControl\Output
set ProjDir=E:\Projects\C#\RemoteControl
rmdir /s /q %OutputDir%
md %OutputDir%
md %OutputDir%\RemoteControl.Client
md %OutputDir%\RemoteControl.Server
md %OutputDir%\RemoteControl.Plugin.Updater

xcopy /s %ProjDir%\RemoteControl.Plugin.Updater\bin\Debug %OutputDir%\RemoteControl.Plugin.Updater
xcopy /s %ProjDir%\RemoteControl.Server\bin\x86\Debug %OutputDir%\RemoteControl.Server
xcopy /s %ProjDir%\RemoteControl.Client\bin\x86\Debug %OutputDir%\RemoteControl.Client

%OutputDir%\RemoteControl.Client\combineDlls.bat
copy /y %OutputDir%\RemoteControl.Client\RemoteControl.Client.dat %OutputDir%\RemoteControl.Server\RemoteControl.Client.dat
copy /y %OutputDir%\RemoteControl.Client\RemoteControl.Client.dat %OutputDir%\RemoteControl.Plugin.Updater\RemoteControl.Client.dat
%OutputDir%\RemoteControl.Plugin.Updater\RemoteControl.Plugin.Updater.exe %OutputDir%\RemoteControl.Plugin.Updater\RemoteControl.Client.dat