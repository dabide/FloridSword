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
    echo "Building $2"
	pushd "$1/src/$2"
	${DOTNET} restore $2.csproj
	${DOTNET} pack $2.csproj -o "${LIB}/packages" -c Release /property:PackageVersion=$3
	popd
}

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
SSVERSION=111.0.38
LIB=${DIR}/lib
SSDIR=${LIB}/src/ServiceStack
SSAWSDIR=${LIB}/src/ServiceStack.Aws
SSTEXTDIR=${LIB}/src/ServiceStack.Text
SSREDISDIR=${LIB}/src/ServiceStack.Redis
ORMLITEDIR=${LIB}/src/ServiceStack.OrmLite

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
touch "${LIB}/packages/PLACEHOLDER"

for d in ServiceStack ServiceStack.Aws ServiceStack.OrmLite ServiceStack.Redis ServiceStack.Text
do
	if [ ! -d "${LIB}/src/${d}" ]; then
		echo "Cloning ${d} repo"
		git clone --depth 1 https://github.com/ServiceStack/${d} "${LIB}/src/${d}"
	fi

	pushd "${LIB}/src/${d}"
	git clean -df
	git checkout -- .
	git fetch
	git checkout master
	popd
done

for d in ServiceStack.Text
do
	pack ${SSTEXTDIR} ${d} ${SSVERSION}
done

for d in ServiceStack.Aws
do
	pack ${SSAWSDIR} ${d} ${SSVERSION}
done

for d in ServiceStack.OrmLite ServiceStack.OrmLite.Sqlite ServiceStack.OrmLite.SqlServer
do 
	pack ${ORMLITEDIR} ${d} ${SSVERSION}
done

for d in ServiceStack.Redis
do
	pack ${SSREDISDIR} ${d} ${SSVERSION}
done

for d in ServiceStack ServiceStack.Api.Swagger ServiceStack.Client ServiceStack.Common ServiceStack.Core.SelfHost ServiceStack.Core.WebApp ServiceStack.HttpClient ServiceStack.Interfaces ServiceStack.Kestrel ServiceStack.MsgPack ServiceStack.Mvc ServiceStack.ProtoBuf ServiceStack.RabbitMq ServiceStack.Server ServiceStack.Wire
do
	pack ${SSDIR} ${d} ${SSVERSION}
done

echo "Finished succesfully"