![Discord Shield](https://discordapp.com/api/guilds/987075686256762890/widget.png?style=shield)  

*See screenshot below...*

**How to run?**  
Open MOOS.sln using VS2022  
Select MOOS from the VS2022 dropdown menu  
Press F5  

**Debug requirements:**  
*VMware Workstation Player https://www.vmware.com/products/workstation-player.html*  
*Intel Hardware Accelerated Execution Manager (HAXM) https://github.com/intel/haxm/releases*  
*Visual studio 2022 https://visualstudio.microsoft.com/*  
*QEMU https://www.qemu.org/download/*  
*At least 8gb of RAM, 64bit operating system*  
**Have fun!**

Welcome to ***MOOS(My own operating system)-Project***  
C# operating system demo, using Native AOT (Core RT) boot via multiboot  
| Screenshot(English is default) |
| ------ |
| ![image](Screenshot1.png) |

| Items | Implemented | Working On Real Hardware (Tested on Supermicro X9DRI-LN4F+) | Note |
| ----- | ----------- | ----------------------------------------------------------- | ----- |
| Applications(.exe) | ✅ | ✅ |
| Error Throwing / Catching | ❌ | ❌ | 
| GC | ⚠️ | ❓ | Not safe |
| Multiprocessor | ✅ | ✅ |
| Multithreading | ✅ | ✅ |
| EHCI(USB2.0) | ✅ | ✅ |
| USB Keyboard | ⚠️ | ❓ | Work in progress |
| USB Mouse | ✅ | ❓ |
| USB HUB | ❌ | ❌ |
| PS2 Keyboard/Mouse(USB Compatible) | ✅ | ✅ |
| Nintendo Family Computer Emulator | ✅ | ✅ |
| DOOM(doomgeneric) | ✅ | ✅ |
| Intel® Gigabit Ethernet Network | ✅ | ✅ |
| Realtek RTL8139 | ✅ | ❓ |
| ExFAT | ✅ | ✅ | FAT32/16/12 is no supported! |
| I/O APIC | ✅ | ✅ |
| Local APIC | ✅ | ✅ |
| SATA | ✅ | ❓ |
| IDE | ✅ | ✅ |
| SMBIOS | ✅ | ✅ |
| ACPI | ✅ | ✅ |
| IPv4 | ✅ | ✅ |
| IPv6 | ❌ | ❌ |
| TCP(WIP) | ⚠️ | ❓ | Can't receive large package |
| UDP | ✅ | ❓ |
| Lan | ✅ | ✅ |
| Wan | ✅ | ✅ 
