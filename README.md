# SmartClient

Winform Main Application for Vmon5 including [Chromium C# Embeeded Browser](https://cefsharp.github.io/) and using [Click-Once Distribution](https://docs.microsoft.com/en-us/visualstudio/deployment/clickonce-security-and-deployment)

## Setup
### IDE Visual Studio 2017/2019

No other external dependencies. Just Nuget restore all proyect dependencies.

### Initial Setup

Just Nuget Restore upon Build will be triggered.

## Build & Debug
Just Build All and Smartclient Project.

## Deploy Info

1. Check Elipgo.SmartClient/Customized.targets file
```
<PropertyGroup>
	<PublishDir>C:\wamp64\www\clickonce-qa\</PublishDir>
</PropertyGroup>
```
Is deploy folder in server

2. Check Elipgo.SmartClient/build.bat file
if "%environment%"=="local" (
	SET installUrl="http://localhost.vmonitoring.com:3434/clickonce/"
) else if "%environment%"=="develop" (
	SET installUrl="http://develop.vmonitoring.com:3434/"
) else if "%environment%"=="qa" (
	SET installUrl="http://qa.vmonitoring.com:3434/"
) else if "%environment%"=="production" (
	SET installUrl="http://prod.vmonitoring.com:3434/"
)

environment must be equal to BuildEnvironment in Customized.targets

Command Line > .\build.bat Debug production