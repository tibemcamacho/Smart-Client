@ECHO OFF

SET configuration=Debug
SET environment=docker
set /p version=<%environment%.version
set /a version=version+16
echo %version% > %environment%.version

SET installUrl="http://smartclient.vmonitoring.com:8020/"

msbuild /t:clean,rebuild,publish /p:Configuration=%configuration% /p:Platform=x64 /p:BuildEnvironment=%environment% /p:ApplicationRevision=%version% /p:InstallUrl=%installUrl%
