pushd $PWD/../src/Buildron/Buildron.ModSdk/Editor/bin/Debug

cp $PWD/../../../../Buildron.ModSdk/msbuilds/Buildron.Mod-sample.targets Buildron.Mod.targets
zip -vr $PWD/../../../../../../build/Buildron.ModSdk.zip . -x "*.DS_Store" -x "*.meta" -x "UnityEditor.dll" -x "UnityEngine.dll" >/dev/null

popd