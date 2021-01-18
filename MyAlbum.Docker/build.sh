#!/bin/bash

Dir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
parentDir="$(dirname "$Dir")"
webProjectDir="$parentDir/MyAlbum.WebSPA"

#web
webProjectFile="$webProjectDir/MyAlbum.WebSPA.csproj"
publishDir="$Dir/web/publish"
rm -rf "$publishDir"
mkdir "$publishDir"
dotnet publish $webProjectFile -o $publishDir
#cleanup
rm -rf "$publishDir/runtimes/linux-x86"
rm -rf "$publishDir/runtimes/osx-x64"
rm -rf "$publishDir/runtimes/win"
rm -rf "$publishDir/runtimes/win-arm64"
rm -rf "$publishDir/runtimes/win-x64"
rm -rf "$publishDir/runtimes/win-x86"
rm -rf "$publishDir/cs"
rm -rf "$publishDir/de"
rm -rf "$publishDir/es"
rm -rf "$publishDir/fr"
rm -rf "$publishDir/it"
rm -rf "$publishDir/ja"
rm -rf "$publishDir/ko"
rm -rf "$publishDir/pl"
rm -rf "$publishDir/pt-BR"
rm -rf "$publishDir/ru"
rm -rf "$publishDir/tr"
rm -rf "$publishDir/zh-Hans"
rm -rf "$publishDir/zh-Hant"

#db
migrationScriptDir="$webProjectDir/sql/"
dbDir="$Dir/db"
cp -r $migrationScriptDir $dbDir

docker-compose down
docker-compose build
docker-compose up