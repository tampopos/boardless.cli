FROM microsoft/dotnet:latest AS build-env
COPY . /src/
WORKDIR /src
RUN dotnet publish Deploys.Db -c Release -o out

FROM microsoft/dotnet:2.2-runtime
WORKDIR /app
COPY --from=build-env /src/Deploys.Db/out .
COPY .migration .migration/

ENV ConnectionStrings__Root ""
ENV Directory ./.migration
ENV Timeout 60

ENTRYPOINT [ "dotnet","Deploys.Db.dll" ]