#!/bin/sh

cd "$(dirname "$0")"
dotnet run --project src/sodoff.csproj
