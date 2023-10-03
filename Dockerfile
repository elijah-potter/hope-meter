FROM node:latest AS site-build

RUN mkdir /app
WORKDIR /app

COPY . . 

WORKDIR /app/site

RUN yarn install
RUN yarn build

FROM mcr.microsoft.com/dotnet/sdk as net-build

WORKDIR /app
COPY . . 

COPY --from=site-build /app/site/build ./wwwroot

RUN dotnet publish -c release

FROM mcr.microsoft.com/dotnet/aspnet

WORKDIR /app
COPY --from=net-build /app/bin/release/net7.0/publish/ .
COPY data.db .

ENTRYPOINT ["/app/Hope"]
