```
ARP.Initialise();
Network.Initialise(new IPAddress(192, 168, 137, 188), new IPAddress(192, 168, 137, 1));
RTL8139.Initialise();
ARP.Require(Network.Gateway);
```
And install OpenVPN's Windows TAP rename the device name to tap
Edit Kernel.csproj
```
<Exec Command="&quot;C:\\Program Files\\qemu\\qemu-system-x86_64.exe&quot; -m 8192 -cdrom $(MSBuildProjectDirectory)\$(NativeOutputPath)$(TargetName).iso -d guest_errors -serial stdio -net nic,model=rtl8139 -net tap,ifname=tap"></Exec>
```
