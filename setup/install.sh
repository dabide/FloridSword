#! /bin/bash
set -e

cd /opt/floridsword/setup

dpkg -l puppetlabs-release > /dev/null 2>&1
if [ $? -ne 0 ]; then
    wget -O /tmp/puppetlabs.deb https://apt.puppetlabs.com/puppetlabs-release-pc1-jessie.deb
    sudo dpkg -i /tmp/puppetlabs.deb
    rm /tmp/puppetlabs.deb

    sudo apt-get update

    apt-get install puppet-agent -y
fi

/opt/puppetlabs/bin/puppet module install saz-dnsmasq

if [ $1 != '--skip-puppet' ]; then
    /opt/puppetlabs/bin/puppet apply --debug --verbose -environmentpath /opt/floridsword/setup/environments/ --environment production
fi

# Echo resetting rc.local if changed
sed -i 's_sh /opt/floridsword/setup/install.sh_exit 0_' /etc/rc.local
