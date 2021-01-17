#!/bin/bash

Dir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
parentDir="$(dirname "$Dir")"
webProjectDir="$parentDir/MyAlbum.WebSPA"

#web
webProjectFile="$webProjectDir/MyAlbum.WebSPA.csproj"
binProjectDir="$webProjectDir/bin"
rm -rf $binProjectDir
objProjectDir="$webProjectDir/obj"
rm -rf $objProjectDir
publishDir="$Dir/web/publish"
rm -rf "$publishDir"
mkdir "$publishDir"
dotnet publish $webProjectFile -o $publishDir
rm -rf "$publishDir/runtimes/linux-x86"
rm -rf "$publishDir/runtimes/osx-x64"
rm -rf "$publishDir/runtimes/win"
rm -rf "$publishDir/runtimes/win-arm64"
rm -rf "$publishDir/runtimes/win-x64"
rm -rf "$publishDir/runtimes/win-x86"

#db
migrationScriptDir="$webProjectDir/sql/"
dbDir="$Dir/db"
cp -r $migrationScriptDir $dbDir

docker-compose down
docker-compose build
docker-compose up