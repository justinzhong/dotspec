@echo off

pushd ..\code\Dotspec

nuget pack Dotspec.csproj
copy *.nupkg C:\home\justin\nuget /-Y

popd