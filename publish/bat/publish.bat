@echo off


cd %1
echo �ļ��� %1
echo ��Ŀ���ƣ�%2
dotnet build -c Release

echo ��Ŀ���ƣ�%2
dotnet pack -c Release --output ./bin/Release/package

cd ./bin/Release/package
set "package=*.nupkg"

for /r %%i in (%package%) do ( 
dotnet nuget push %%~ni.nupkg --source %3
)