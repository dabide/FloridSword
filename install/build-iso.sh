#! /bin/bash
if [ $# -ne 3 ]; then
    echo "Usage: build-iso.sh /path/to/netinst.iso /path/to/output.iso /path/to/preseed.cfg"
    exit 1
fi

NETINSTISO=$(cd "$(dirname "$1")"; pwd)/$(basename "$1")
OUTPUTISO=$(cd "$(dirname "$2")"; pwd)/$(basename "$2")
PRESEED=$(cd "$(dirname "$3")"; pwd)/$(basename "$3")

UUID=$(cat /proc/sys/kernel/random/uuid)
TEMPDIR=/tmp/florid_iso_${UUID}

echo "Creating temporary folder ${TEMPDIR}"
mkdir -p ${TEMPDIR}
cd ${TEMPDIR}

echo -n "Extracting ISO file ..."
7z x -ocd -y ${NETINSTISO} > /dev/null
echo " done"

echo -n "Modifying initrd ..."
mkdir initrd
cd initrd
zcat ../cd/install.amd/initrd.gz | cpio -iv > /dev/null 2>&1
cat ${PRESEED} > preseed.cfg
find . -print0 | cpio -0 -H newc -ov | gzip -c > ../cd/install.amd/initrd.gz
cd -
echo " done"

echo "Updating MD5 sums"
cd cd
md5sum `find ! -name "md5sum.txt" ! -path "./isolinux/*" -follow -type f` > md5sum.txt
cd -

echo "Creating ISO file ..."
xorriso -as mkisofs -o ${TEMPDIR}/output.iso \
 -isohybrid-mbr /usr/lib/ISOLINUX/isohdpfx.bin \
 -c isolinux/boot.cat -b isolinux/isolinux.bin \
 -no-emul-boot -boot-load-size 4 -boot-info-table ./cd
mv ${TEMPDIR}/output.iso ${OUTPUTISO}
echo "... done"

cd -
echo -n "Deleting temporary folder ${TEMPDIR}"
rm -rf ${TEMPDIR}
echo " done"
echo "To test, run the following command:"
echo "    qemu -user-net -cdrom ${OUTPUTISO}"