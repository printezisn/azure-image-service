FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

RUN apt-get update && apt install -y nodejs npm
RUN npm i -g azure-functions-core-tools@3 --unsafe-perm true

COPY . .
RUN dotnet build

RUN mkdir /release
RUN cp -r ./src/ImageService.FunctionApp/* /release

WORKDIR /release

CMD ["func", "start"]