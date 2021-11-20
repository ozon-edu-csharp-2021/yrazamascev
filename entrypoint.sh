#!/bin/bash

set -e
run_cmd="dotnet OzonEdu.MerchApi.dll --no-build -v d"

>&2 echo "Run MerchApi DB migrations"
dotnet OzonEdu.MerchApi.Migrator.dll --no-build -v d -- --dryrun

dotnet OzonEdu.MerchApi.Migrator.dll --no-build -v d
>&2 echo "Run MerchApi DB migrations complete"

>&2 echo "Run MerchApi: $run_cmd"
exec $run_cmd