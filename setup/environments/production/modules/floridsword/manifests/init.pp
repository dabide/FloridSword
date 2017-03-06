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

    dnsmasq::conf { 'local-dns':
        ensure => present,
        source => 'puppet:///modules/floridsword/local-dns',
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
        command => "/usr/bin/curl -sSL https://go.microsoft.com/fwlink/?linkid=841689 | /bin/tar zx -C /opt/dotnet",
        creates => "/opt/dotnet/sdk/1.0.0-rc4-004771",
        require => [ Package['curl'], Package['libunwind8'], Package['gettext'] ],
    }
    ->
    file { '/usr/local/bin/dotnet':
        ensure => link,
        target => '/opt/dotnet/dotnet',
    }
}