@echo off

set "solutionPath=../../src"
set "project=*.csproj"
set  "batPath=%~dp0"

setlocal enabledelayedexpansion  
echo ��ʼ�����ļ�����ȴ�������ʾ��������ɡ����˳� ...  

echo %solutionPath%

cd %solutionPath%

@echo off
rem ָ�����������ļ�
echo �������������Ժ�...

for /r %%i in (%project%) do ( 
    echo %%i 
    echo %%~ni
    echo %%~fi
    echo %%~pi
    echo %%~di
    cd %batPath%
    call publish.bat %%~di%%~pi,%%~ni,nuget.org
)

echo.  
echo �����ɹ�
pause >nul 