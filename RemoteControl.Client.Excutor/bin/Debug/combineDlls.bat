@echo off
ilmerge.exe /ndebug /targetplatform:v4 /target:winexe /out:Client.Excutor.exe /log RemoteControl.Client.Excutor.exe AForge.Controls.dll AForge.Imaging.dll AForge.Video.DirectShow.dll AForge.Video.dll
rename Client.Excutor.exe Client.Excutor.dat