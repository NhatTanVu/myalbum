#!/bin/bash

set -e
run_cmd="dotnet ./MyAlbum.WebSPA.dll --server.urls http://*:5001"

>&2 echo "Executing command"
exec $run_cmd