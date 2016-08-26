pushd $PWD/../src/Buildron/Assets/_Assets/References

cp $PWD/../../../Buildron.ModSdk/msbuilds/Buildron.Mod-sample.targets Buildron.Mod.targets
zip -vr $PWD/../../../../../build/Buildron.ModSdk.zip . -x "*.DS_Store" -x "*.meta" >/dev/null

popd