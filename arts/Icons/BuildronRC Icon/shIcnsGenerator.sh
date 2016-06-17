#!/bin/bash
PATH=/bin:/usr/bin:/sbin:/usr/sbin export PATH

mkdir icon.iconset
cp $1 icon.iconset/icon_512x512@2x.png
cd icon.iconset/

cp icon_512x512@2x.png icon_512x512.png
cp icon_512x512@2x.png icon_256x256@2x.png
cp icon_512x512@2x.png icon_256x256.png
cp icon_512x512@2x.png icon_128x128@2x.png
cp icon_512x512@2x.png icon_128x128.png
cp icon_512x512@2x.png icon_32x32@2x.png
cp icon_512x512@2x.png icon_32x32.png
cp icon_512x512@2x.png icon_16x16@2x.png
cp icon_512x512@2x.png icon_16x16.png


sips -z 1024 1024 icon_512x512@2x.png
sips -z 512 512 icon_512x512.png
sips -z 512 512 icon_256x256@2x.png
sips -z 256 256 icon_256x256.png
sips -z 256 256 icon_128x128@2x.png
sips -z 128 128 icon_128x128.png
sips -z 64 64 icon_32x32@2x.png
sips -z 32 32 icon_32x32.png
sips -z 32 32 icon_16x16@2x.png
sips -z 16 16 icon_16x16.png

cd ..
iconutil -c icns icon.iconset


