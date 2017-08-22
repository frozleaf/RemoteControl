@echo off
ilmerge.exe /ndebug /targetplatform:v4 /target:exe /out:RemoteControl.Client_c.exe /log RemoteControl.Client.exe RemoteControl.Protocals.dll Newtonsoft.Json.Lite.dll 
copy /y RemoteControl.Client_c.exe RemoteControl.Client.dat
del RemoteControl.Client_c.exe
exit