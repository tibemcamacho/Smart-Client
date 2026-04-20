@ECHO OFF

SET configuration=Release
SET environment=docker
set /p version=<%environment%.version
set /a version=version+93
echo %version% > %environment%.version

SET installUrl="https://vms.viva-telmex.com/"

msbuild /t:clean,rebuild,publish /p:Configuration=%configuration% /p:Platform=x64 /p:BuildEnvironment=%environment% /p:ApplicationRevision=%version% /p:InstallUrl=%installUrl%