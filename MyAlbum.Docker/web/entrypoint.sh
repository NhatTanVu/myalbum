#!/bin/bash

set -e
run_cmd="dotnet ./MyAlbum.WebSPA.dll"

>&2 echo "Executing command"
exec $run_cmd