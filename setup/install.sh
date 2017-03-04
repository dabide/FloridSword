#! /bin/bash
cd /opt/floridsword/setup

dpkg -l puppetlabs-release-pc1 > /dev/null 2>&1
if [ $? -ne 0 ]; then
    wget -O /tmp/puppetlabs.deb https://apt.puppetlabs.com/puppetlabs-release-pc1-jessie.deb
    sudo dpkg -i /tmp/puppetlabs.deb
    rm /tmp/puppetlabs.deb
fi

dpkg -l puppet-agent > /dev/null 2>&1
if [ $? -ne 0 ]; then
    sudo apt-get update

    apt-get install puppet-agent -y
fi

/opt/puppetlabs/bin/puppet module install saz-dnsmasq

if [ "$1" != "--skip-puppet" ]; then
    /opt/puppetlabs/bin/puppet apply --color=no --debug --verbose \
    --environmentpath /opt/floridsword/setup/environments/ \
    --environment production \
    /opt/floridsword/setup/setup/environments/production/manifests/site.pp
fi

# Echo resetting rc.local if changed
sed -i 's_sh /opt/floridsword/setup/install\.sh.*$_exit 0_' /etc/rc.local
