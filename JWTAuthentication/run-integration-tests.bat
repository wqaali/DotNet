@echo off
echo Running JWT Authentication Integration Tests...
echo.

cd JWTAuthentication.IntegrationTests

echo Restoring NuGet packages...
dotnet restore
if %ERRORLEVEL% neq 0 (
    echo Failed to restore packages
    exit /b 1
)

echo.
echo Running integration tests...
dotnet test --verbosity normal
if %ERRORLEVEL% neq 0 (
    echo Some tests failed
    exit /b 1
)

echo.
echo All tests passed successfully!
pause
