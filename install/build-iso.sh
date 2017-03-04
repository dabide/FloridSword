#! /bin/bash
set -e

NETINSTISO=$(cd "$(dirname "$1")"; pwd)/$(basename "$1")
OUTPUTISO=$(cd "$(dirname "$2")"; pwd)/$(basename "$2")
PRESEED=$(cd "$(dirname "$3")"; pwd)/$(basename "$3")
TEMPDIR=/tmp/florid_iso

echo "Creating temporary folder ${TEMPDIR}"
mkdir ${TEMPDIR}
pushd ${TEMPDIR}

echo -n "Extracting ISO file ..."
7z x -ocd -y ${NETINSTISO} > /dev/null
echo " done"

echo -n "Modifying initrd ..."
mkdir initrd
pushd initrd
zcat ../cd/install.amd/initrd.gz | cpio -iv
cat ${PRESEED} > preseed.cfg
find . -print0 | cpio -0 -H newc -ov | gzip -c > ../cd/install.amd/initrd.gz
popd
echo " done"

echo "Updating MD5 sums"
pushd cd
md5sum `find ! -name "md5sum.txt" ! -path "./isolinux/*" -follow -type f` > md5sum.txt
popd

echo -n "Creating ISO file ..."
xorriso -as mkisofs -o ${OUTPUTISO} \
 -isohybrid-mbr /usr/lib/ISOLINUX/isohdpfx.bin \
 -c isolinux/boot.cat -b isolinux/isolinux.bin \
 -no-emul-boot -boot-load-size 4 -boot-info-table ./cd
echo " done"

popd
echo -n "Deleting temporary folder ${TEMPDIR}"
rm -rf ${TEMPDIR}
echo " done"
echo "To test, run the following command:"
echo "    qemu -user-net -cdrom ${OUTPUTISO}"