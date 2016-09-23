echo ================[ Building Buildron $1 $2 $3 $4...

pushd $PWD/../../Buildron-Classic-Mods/tools >/dev/null
./buildMods-$1.sh
popd >/dev/null

echo ================[ Building player for $1
/Applications/Unity/Unity.app/Contents/MacOS/Unity -projectPath $PWD/../src/Buildron -quit -batchmode -$2 $PWD/../build/$3

echo ================[ Packing Buildron.$1.zip...
pushd $PWD/../build/ >/dev/null
cp -R Mods $4
zip -vr Buildron.$1.zip $4 -x "*.DS_Store" >/dev/null
popd >/dev/null

echo ================[ Building Buildron done.
