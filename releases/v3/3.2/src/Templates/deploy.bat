@echo off

set ZIP="%CD%\7z.exe"

CALL :Clean
CALL :UpdateReferences
CALL :CompressFiles "ItemTemplates" "%CD%\Item\*"
CALL :CompressVSI "VSI" 
CALL :Deploy


:CompressFiles
for /d %%A in (%2) do (
	MKDIR "%CD%\build\%1"
	%ZIP% a "%CD%\build\%1\%%~nA.zip" "%%A\*"
)
GOTO:EOF

:CompressVSI
%ZIP% a -tzip "%CD%\build\%1\Dow Jones Templates.vsi" "%CD%\Dow Jones Templates.vscontent"
%ZIP% a -tzip "%CD%\build\%1\Dow Jones Templates.vsi" "%CD%\build\ItemTemplates\*.zip"
GOTO:EOF

:Clean
ECHO Deleting %CD%\build...
DEL /F /S /Q "%CD%\build"
RMDIR "%CD%\build"
MKDIR "%CD%\build"
GOTO:EOF

:UpdateReferences
ECHO Updating assembly references from "%CD%\..\DowJones.Web.Showcase\bin\"...
XCOPY /Y /I /S "%CD%\..\DowJones.Web.Showcase\bin\DowJones*" "%CD%\Project\MVC Application\References"
GOTO:EOF

:Deploy
ECHO Deploying templates to %USERPROFILE%\My Documents\Visual Studio 2010\Templates...

@REM XCOPY /Y /I /S "%CD%\build\*" "%USERPROFILE%\My Documents\Visual Studio 2010\Templates"
"%CD%\build\VSI\Dow Jones Templates.vsi"
GOTO:EOF