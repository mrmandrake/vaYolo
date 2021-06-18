dotnet restore -r osx-x64
dotnet msbuild -property:Configuration=Release -t:BundleApp -p:RuntimeIdentifier=osx-x64 -p:CFBundleDisplayName=vaYolo -p:CFBundleShortVersionString=0.0.1 -p:UseAppHost=true
mkdir ./darknet/cfg/*.cfg ./bin/Release/net6.0/osx-x64/publish/vaYolo.app/Contents/MacOS/templates
mkdir ./darknet/cfg/*.cfg ./bin/Release/net6.0/osx-x64/publish/vaYolo.app/Contents/MacOS/pretrained
cp -rvf ./darknet/cfg/*.cfg ./bin/Release/net6.0/osx-x64/publish/vaYolo.app/Contents/MacOS/templates
cp -rvf ./darknet/pretrained/*.weights ./bin/Release/net6.0/osx-x64/publish/vaYolo.app/Contents/MacOS/pretrained