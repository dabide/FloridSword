# Class: client
#
#
class client {
    package { 'ubuntu-desktop':
        ensure => latest,    
    } -> exec { 'init_5':
        command => '/sbin/init 5',      
    }
}