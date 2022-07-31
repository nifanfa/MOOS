```
ARP.Initialise();
Network.Initialize(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1));
RTL8139.Initialize();
ARP.Require(Network.Gateway);
```
And install OpenVPN's Windows TAP rename the device name to tap
Edit Kernel.csproj and add this command to the launch argument of qemu
```
-net nic,model=rtl8139 -net tap,ifname=tap
```
