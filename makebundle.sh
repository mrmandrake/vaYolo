dotnet restore -r osx-x64
dotnet msbuild -property:Configuration=Release -t:BundleApp -p:RuntimeIdentifier=osx-x64 -p:CFBundleDisplayName=vaYolo -p:CFBundleShortVersionString=0.0.1 -p:UseAppHost=true