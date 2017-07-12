#!/bin/bash
errorhandler() {
    # Modified verion of http://stackoverflow.com/a/4384381/352573
    errorcode=$?
    echo "Error $errorcode"
    echo "The command executing at the time of the error was"
    echo "$BASH_COMMAND"
    echo "on line ${BASH_LINENO[0]}"
    exit $errorcode
}

pack() {
    trap errorhandler ERR

    echo "Building $2 at patch version $3"
    dotnet restore $1/src/$2.Core.sln
    pushd "$1/build"
    sed -i -e "s/__releaseDate = new DateTime/__releaseDate = new  DateTime/" $1/src/$2/Env.cs
	BUILD_NUMBER=$3 "${MSBUILD}" build-core.proj /target:NuGetPack /property:Configuration=Release    
 	popd
    mv $1/NuGet.Core/*.nupkg ${LIB}/packages
}

MSBUILD="/c/Program Files (x86)/Microsoft Visual Studio/2017/Professional/MSBuild/15.0/Bin/MSBuild.exe"
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
LIB=${DIR}/lib
PATCHVERSION=$(($(printf '%(%s)T' -1) / 86400))
TAG=v4.5.12

if [ "${OS}" == 'Windows_NT' ]; then
    DOTNET="${DIR}/bin/dotnet-core/dotnet.exe"
    if [ ! -f ${DOTNET} ]; then
        echo -n "Downloading .NET Core SDK ..."
        mkdir -p "${DIR}/bin/dotnet-core"
        curl -o dotnet.zip -sSL https://download.microsoft.com/download/E/7/8/E782433E-7737-4E6C-BFBF-290A0A81C3D7/dotnet-dev-win-x64.1.0.4.zip
        unzip -q -o dotnet.zip -d "${DIR}/bin/dotnet-core"
        rm dotnet.zip
        echo " done"
    fi
else
    DOTNET="${DIR}/bin/dotnet-core/dotnet"
    if [ ! -f ${DOTNET} ]; then
        echo "You need to download the .NET Core SDK .tar.gz for your platform and unpack it into ${DIR}/bin/dotnet-core"
        exit 1
    fi
fi

# From now on, catch errors
trap errorhandler ERR

echo "Cleaning up"
rm -rf "${LIB}/packages"
mkdir -p "${LIB}/packages"
touch "${LIB}/packages/PLACEHOLDER"

for d in ServiceStack.Text
do
	if [ ! -d "${LIB}/src/${d}" ]; then
		echo "Cloning ${d} repo"
		git clone --depth 1 --branch ${TAG} https://github.com/ServiceStack/${d} "${LIB}/src/${d}"
	fi

	pushd "${LIB}/src/${d}"
    git checkout -- .
    git fetch origin
    git reset --hard ${TAG}
    popd

    pack "${LIB}/src/${d}" ServiceStack.Text ${PATCHVERSION}
done

echo "Finished succesfully"