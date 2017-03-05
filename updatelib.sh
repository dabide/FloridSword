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
	${DOTNET} restore
	${DOTNET} pack -o "${LIB}/packages" -c Release
	popd
}

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
SSVERSION=111.0.37
LIB=${DIR}/lib
SSDIR=${LIB}/src/ServiceStack
SSAWSDIR=${LIB}/src/ServiceStack.Aws
SSTEXTDIR=${LIB}/src/ServiceStack.Text
SSREDISDIR=${LIB}/src/ServiceStack.Redis
ORMLITEDIR=${LIB}/src/ServiceStack.OrmLite

if [ "${OS}" == 'Windows_NT' ]; then
    DOTNET="${DIR}/bin/dotnet-lts/dotnet.exe"
    if [ ! -f ${DOTNET} ]; then
        echo -n "Downloading .NET Core 1.0.3 (LTS) ..."
        mkdir -p "${DIR}/bin/dotnet-lts"
        curl -o dotnet.zip -sSL https://go.microsoft.com/fwlink/?LinkID=836296
        unzip -q -o dotnet.zip -d "${DIR}/bin/dotnet-lts"
        rm dotnet.zip
        echo " done"
    fi
else
    DOTNET="${DIR}/bin/dotnet-lts/dotnet"
    if [ ! -f ${DOTNET} ]; then
        echo "You need to download the .NET Core 1.0.3 (LTS) .tar.gz for your platform and unpack it into ${DIR}/bin/dotnet-lts"
        exit 1
    fi
fi

# From now on, catch errors
trap errorhandler ERR

echo "Cleaning up"
rm -rf "${LIB}/packages"

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

echo "Setting version numbers to a ridiculously high value"
find "${LIB}/src" -name project.json -exec sed -i -e "s/\(^[ \t]*\"version\": \"\)1.0.0\"/\1${SSVERSION}\"/" -e "s/\(\"ServiceStack\.[^\"]*\": \"\)1.0.*\(\"\)/\1${SSVERSION}\2/" \{\} \;

for d in ServiceStack.Text
do
	pack ${SSTEXTDIR} ${d}
done

for d in ServiceStack.Aws
do
	pack ${SSAWSDIR} ${d}
done

for d in ServiceStack.OrmLite ServiceStack.OrmLite.Sqlite ServiceStack.OrmLite.SqlServer
do 
	pack ${ORMLITEDIR} ${d}
done

for d in ServiceStack.Redis
do
	pack ${SSREDISDIR} ${d}
done

for d in ServiceStack ServiceStack.Api.Swagger ServiceStack.Client ServiceStack.Common ServiceStack.Core.SelfHost ServiceStack.Core.WebApp ServiceStack.HttpClient ServiceStack.Interfaces ServiceStack.Kestrel ServiceStack.MsgPack ServiceStack.Mvc ServiceStack.ProtoBuf ServiceStack.RabbitMq ServiceStack.Server ServiceStack.Wire
do
	pack ${SSDIR} ${d}
done

echo "Finished succesfully"