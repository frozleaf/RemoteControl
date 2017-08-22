@echo off
ilmerge.exe /ndebug /targetplatform:v4 /target:winexe /out:CamCapture.exe /log RemoteControl.Client.CamCapture.exe AForge.Controls.dll AForge.Imaging.dll AForge.Video.DirectShow.dll AForge.Video.dll
rename CamCapture.exe CamCapture.dat