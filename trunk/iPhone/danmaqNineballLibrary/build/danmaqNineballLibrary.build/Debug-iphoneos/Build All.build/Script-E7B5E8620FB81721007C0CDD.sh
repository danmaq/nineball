#!/bin/sh
# remove existing product lib file, just in case
rm -rf build/${BUILD_STYLE}-iphoneos/libdanmaqNineballLibrary-${BUILD_STYLE}.a

# combine lib files for various platforms into one
lipo -create "build/${BUILD_STYLE}-iphoneos/libdanmaqNineballLibrary - dev.a" \
"build/${BUILD_STYLE}-iphonesimulator/libdanmaqNineballLibrary - sim.a" \
-output "build/${BUILD_STYLE}-iphoneos/libdanmaqNineballLibrary-${BUILD_STYLE}.a"

