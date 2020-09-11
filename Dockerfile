FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

RUN apt-get update && apt install -y nodejs npm
RUN npm i -g azure-functions-core-tools@3 --unsafe-perm true

COPY . .
RUN dotnet build

WORKDIR /app/src/ImageService.FunctionApp

CMD ["func", "start"]