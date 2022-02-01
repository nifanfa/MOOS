ping -n 5 127.0.0.1 > NUL
rem probably win7 only and useful if you pushed a sanhook command
rem drvinst.exe msscsi
rem ping -n 5 127.0.0.1 > NUL
rem provide a sharename and credentials pointing to your windows install files
net use z: \\1.2.3.4\sharename /user:username password
z:
setup.exe