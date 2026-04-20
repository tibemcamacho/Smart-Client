ARG clickonce="C:\Program Files (x86)\Microsoft SDKs\ClickOnce Bootstrapper\Packages"
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /app
COPY . .
COPY config-implementation/build.bat /app/Elipgo.SmartClient
COPY config-implementation/App.config /app/Elipgo.SmartClient
COPY ["config-implementation/", "C:/Program Files (x86)/Microsoft SDKs/ClickOnce Bootstrapper/Packages/"]

RUN nuget restore Elipgo.SmartClient.sln -SolutionDirectory /app

WORKDIR /app/Elipgo.SmartClient
RUN Import-PfxCertificate -FilePath .\Sign\elipgo.pfx -Password (ConvertTo-SecureString -String '3l1pg0' -AsPlainText -Force) -CertStoreLocation Cert:\CurrentUser\My
RUN msbuild Elipgo.SmartClient.csproj /p:Configuration=Debug 
RUN powershell -noexit ".\build.bat"

FROM mcr.microsoft.com/windows/servercore:ltsc2019 AS runtime
SHELL ["powershell"]
RUN Invoke-WebRequest $('https://nginx.org/download/nginx-1.19.1.zip') -OutFile 'nginx.zip' -UseBasicParsing
RUN Expand-Archive nginx.zip -DestinationPath C:\ ;
COPY --from=build /smart-client /nginx-1.19.1/html
COPY config-implementation/nginx.conf /nginx-1.19.1/conf/nginx.conf
WORKDIR /nginx-1.19.1
EXPOSE 80
ENTRYPOINT [ "nginx.exe" ]
