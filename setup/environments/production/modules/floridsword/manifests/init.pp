# Class: floridsword
#
#
class floridsword {
    package { 'ipset':
        ensure => installed,        
    }

    package { 'shorewall':
        ensure => installed,
    }

    dnsmasq::conf { 'base_config':
        ensure => present,
        source => 'puppet:///modules/floridsword/base-config',
    }

    package { 'curl':
        ensure => installed,        
    }

    package { 'libunwind8':
        ensure => installed,        
    }

    package { 'gettext':
        ensure => installed,        
    }

    file { '/opt/dotnet':
        ensure => directory,
    }
    ->
    exec { 'install_dotnet':
        command => "/usr/bin/curl -sSL https://go.microsoft.com/fwlink/?linkid=843453 | /bin/tar zx -C /opt/dotnet",
        creates => "/opt/dotnet/sdk/1.0.1",
        require => [ Package['curl'], Package['libunwind8'], Package['gettext'] ],
    }
    ->
    file { '/usr/local/bin/dotnet':
        ensure => link,
        target => '/opt/dotnet/dotnet',
    }
}