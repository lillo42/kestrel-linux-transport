#!/usr/bin/env bash

usage()
{
    echo "Usage: $0 [runParameters] [verbose] [clangx.y] [cross] [staticLibLink] [cmakeargs] [makeargs]"
    echo "runParameters: buildArch, buildType, buildOS, --numProc <numproc value>"
    echo "verbose - optional argument to enable verbose build output."
    echo "clangx.y - optional argument to build using clang version x.y."
    echo "cross - optional argument to signify cross compilation,"
    echo "      - will use ROOTFS_DIR environment variable if set."
    echo "staticLibLink - Optional argument to statically link any native library."
    echo "portableLinux - Optional argument to build native libraries portable over GLIBC based Linux distros."
    echo "generateversion - Pass this in to get a version on the build output."
    echo "cmakeargs - user-settable additional arguments passed to CMake."
    exit 1
}

initHostDistroRid()
{
    if [ "$__HostOS" == "Linux" ]; then
        if [ ! -e /etc/os-release ]; then
            echo "WARNING: Can not determine runtime id for current distro."
            __HostDistroRid=""
        else
            source /etc/os-release
            __HostDistroRid="$ID.$VERSION_ID-$__HostArch"
        fi
    fi
}

initTargetDistroRid()
{
    if [ $__CrossBuild == 1 ]; then
        if [ "$__BuildOS" == "Linux" ]; then
            if [ ! -e $ROOTFS_DIR/etc/os-release ]; then
                echo "WARNING: Can not determine runtime id for current distro."
                export __DistroRid=""
            else
                source $ROOTFS_DIR/etc/os-release
                export __DistroRid="$ID.$VERSION_ID-$__BuildArch"
            fi
        fi
    else
        export __DistroRid="$__HostDistroRid"
    fi

    # Portable builds target the base RID only for Linux based platforms
    if [ $__PortableLinux == 1 ]; then
        export __DistroRid="linux-$__BuildArch"
    fi
}

setup_dirs()
{
    echo Setting up directories for build

    mkdir -p "$__BinDir"
    mkdir -p "$__IntermediatesDir"
}

# Check the system to ensure the right pre-reqs are in place
check_native_prereqs()
{
    echo "Checking pre-requisites..."

    # Check presence of CMake on the path
    hash cmake 2>/dev/null || { echo >&2 "Please install cmake before running this script"; exit 1; }

    # Check for clang
    hash clang-$__ClangMajorVersion.$__ClangMinorVersion 2>/dev/null ||  hash clang$__ClangMajorVersion$__ClangMinorVersion 2>/dev/null ||  hash clang 2>/dev/null || { echo >&2 "Please install clang before running this script"; exit 1; }
}

prepare_native_build()
{
    # Specify path to be set for CMAKE_INSTALL_PREFIX.
    # This is where all built CoreClr libraries will copied to.
    export __CMakeBinDir="$__BinDir"

    # Configure environment if we are doing a verbose build
    if [ $__VerboseBuild == 1 ]; then
        export VERBOSE=1
    fi  
}

build_native()
{
    # All set to commence the build

    echo "Commencing build of native components"
    cd "$__IntermediatesDir"

    # Regenerate the CMake solution
    echo "Invoking cmake with arguments: \"$__nativeroot\" $__CMakeArgs $__CMakeExtraArgs"
    "$__nativeroot/gen-buildsys-clang.sh" "$__nativeroot" $__ClangMajorVersion $__ClangMinorVersion $__BuildArch $__CMakeArgs "$__CMakeExtraArgs"

    # Check that the makefiles were created.

    if [ ! -f "$__IntermediatesDir/Makefile" ]; then
        echo "Failed to generate native component build project!"
        exit 1
    fi

    # Build

    echo "Executing make install -j $__NumProc $__MakeExtraArgs"

    make install -j $__NumProc $__MakeExtraArgs
    if [ $? != 0 ]; then
        echo "Failed to build native components."
        exit 1
    fi
}

__scriptpath=$(cd "$(dirname "$0")"; pwd -P)
__nativeroot=$__scriptpath
__rootRepo="$__scriptpath/.."
__rootbinpath="$__scriptpath/../bin"

# Set the various build properties here so that CMake and MSBuild can pick them up
__CMakeExtraArgs=""
__MakeExtraArgs=""
__BuildArch=x64
__BuildType=Release
__CMakeArgs=RELEASE
__BuildOS=""
__NumProc=1
__UnprocessedBuildArgs=
__CrossBuild=0
__ServerGC=0
__VerboseBuild=false
__ClangMajorVersion=3
__ClangMinorVersion=5
__StaticLibLink=0
__PortableLinux=0

CPUName=$(uname -p)
# Some Linux platforms report unknown for platform, but the arch for machine.
if [ $CPUName == "unknown" ]; then
    CPUName=$(uname -m)
fi

if [ $CPUName == "i686" ]; then
    __BuildArch=x86
fi

__BuildOS=$(uname -s)
if [ "$__BuildOS" == "Darwin" ]; then
    __BuildOS=OSX
fi


while :; do
    if [ $# -le 0 ]; then
        break
    fi

    lowerI="$(echo $1 | awk '{print tolower($0)}')"
    case $lowerI in
        -\?|-h|--help)
            usage
            exit 1
            ;;
        x86)
            __BuildArch=x86
            ;;
        x64)
            __BuildArch=x64
            ;;
        arm)
            __BuildArch=arm
            ;;
        armel)
            __BuildArch=armel
            ;;
        arm64)
            __BuildArch=arm64
            ;;
        debug)
            __BuildType=Debug
            __CMakeArgs=DEBUG 
            ;;
        release)
            __BuildType=Release
            __CMakeArgs=RELEASE 
	    ;;
        freebsd)
            __BuildOS=FreeBSD
            ;;
        linux)
            __BuildOS=Linux
            ;;
        netbsd)
            __BuildOS=NetBSD
            ;;
        osx)
            __BuildOS=OSX
            ;;
        --numproc)
            shift
            __NumProc=$1
            ;;         
        verbose)
            __VerboseBuild=1
            ;;
        staticliblink)
            __StaticLibLink=1
            ;;
        portablelinux)
            __PortableLinux=1
            ;;
        clang3.5)
            __ClangMajorVersion=3
            __ClangMinorVersion=5
            ;;
        clang3.6)
            __ClangMajorVersion=3
            __ClangMinorVersion=6
            ;;
        clang3.7)
            __ClangMajorVersion=3
            __ClangMinorVersion=7
            ;;
        clang3.8)
            __ClangMajorVersion=3
            __ClangMinorVersion=8
            ;;
        clang3.9)
            __ClangMajorVersion=3
            __ClangMinorVersion=9
            ;;
        cross)
            __CrossBuild=1
            ;;
        cmakeargs)
            if [ -n "$2" ]; then
                __CMakeExtraArgs="$__CMakeExtraArgs $2"
                shift
            else
                echo "ERROR: 'cmakeargs' requires a non-empty option argument"
                exit 1
            fi
            ;;
        makeargs)
            if [ -n "$2" ]; then
                __MakeExtraArgs="$__MakeExtraArgs $2"
                shift
            else
                echo "ERROR: 'makeargs' requires a non-empty option argument"
                exit 1
            fi
            ;;
        useservergc)
            __ServerGC=1
            ;;
        *)
            __UnprocessedBuildArgs="$__UnprocessedBuildArgs $1"
    esac

    shift
done

__CMakeExtraArgs="$__CMakeExtraArgs -DCMAKE_STATIC_LIB_LINK=$__StaticLibLink"

# Set cross build
case $CPUName in
    i686)
        if [ $__BuildArch != x86 ]; then
            __CrossBuild=1
            echo "Set CrossBuild for $__BuildArch build"
        fi
        ;;
    x86_64)
        if [ $__BuildArch != x64 ]; then
            __CrossBuild=1
            echo "Set CrossBuild for $__BuildArch build"
        fi
        ;;
esac

# Set the remaining variables based upon the determined build configuration
__IntermediatesDir="$__rootbinpath/obj/$__BuildOS.$__BuildArch.$__BuildType/Native"
__BinDir="$__rootbinpath/$__BuildOS.$__BuildArch.$__BuildType/Native"

# Make the directories necessary for build if they don't exist
setup_dirs

# Configure environment if we are doing a cross compile.
if [ "$__CrossBuild" == 1 ]; then
    export CROSSCOMPILE=1
    if ! [[ -n "$ROOTFS_DIR" ]]; then
        export ROOTFS_DIR="$__rootRepo/cross/rootfs/$__BuildArch"
    fi
fi

# init the host distro name
initHostDistroRid

# init the target distro name
initTargetDistroRid

# Check prereqs.

check_native_prereqs

# Prepare the system

prepare_native_build

# Build the corefx native components.

build_native
