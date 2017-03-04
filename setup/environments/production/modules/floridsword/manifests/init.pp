# Class: floridsword
#
#
class floridsword {
    package { 'shorewall':
        ensure => latest,
    }

    dnsmasq::conf { 'local-dns':
        ensure => present,
        source => 'puppet:///modules/floridsword/local-dns',
    }
}