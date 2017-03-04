# Class: client
#
#
class client {
    package { 'gnome-core':
        ensure => latest,    
    } -> exec { 'init_5':
        command => '/sbin/init 5',      
    }
}