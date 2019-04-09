FROM mcr.microsoft.com/dotnet/core/sdk:2.1

COPY ./Oisys /oisys
WORKDIR /oisys

EXPOSE 5000
ENV ASPNETCORE_ENVIRONMENT=Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet OisysNew.dll