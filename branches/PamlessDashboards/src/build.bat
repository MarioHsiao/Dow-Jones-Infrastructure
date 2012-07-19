SET MSBUILD=%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe

SET DefaultBuildDir=C:\Temp\Build
SET Configuration=Release
SET FrameworkVersionFile=%DefaultBuildDir%\version.txt
SET OutDir=%DefaultBuildDir%\%Configuration%
SET NuGetBasePath=%OutDir%
SET NuGetOutputPath=%OutDir%\nuget

echo %OutDir%

DEL /F /S /Q %OutDir%

%MSBUILD% /p:Configuration=%Configuration% /p:OutDir="%OutDir%" /m Framework.sln

%MSBUILD% /p:Configuration=%Configuration% /p:DefaultBuildDir="%DefaultBuildDir%" /p:NuGetBasePath="%NuGetBasePath%" /p:OutDir="%OutDir%" /p:FrameworkVersionFile="%FrameworkVersionFile%" /p:NuGetOutputPath="%NuGetOutputPath%" default.proj
