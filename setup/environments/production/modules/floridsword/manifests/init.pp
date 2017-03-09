# Class: floridsword
#
#
class floridsword {
    apt::source { 'dotnet_release':
        comment      => '.NET Core Tools',
        location     => 'https://apt-mo.trafficmanager.net/repos/dotnet-release/',
        repos        => 'main',
        architecture => 'amd64',
        key          => {
            'id'     => '417A0893',
            'server' => 'keyserver.ubuntu.com'  
        },
    }

    package { 'ipset':
        ensure => installed,        
    }

    package { 'shorewall':
        ensure => installed,
    }

    package { 'dotnet-dev-1.0.1':
        ensure  => installed,
        require => [ Apt::Source['dotnet_release' ] ],
    }

    dnsmasq::conf { 'base_config':
        ensure => present,
        source => 'puppet:///modules/floridsword/base-config',
    }

    class { 'unattended_upgrades':
        auto => {
            'reboot'      => true,
            'reboot_time' => '03:00',
        },
    }
}