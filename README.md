**Discord Server: https://discord.gg/GxwmAK7aRE**  
~~**UEFI one available! check out https://github.com/nifanfa/Solution1/tree/uefi**~~  
C# operating system demo, using Native AOT (Core RT) boot via multiboot  
| Items | Implemented | Working On Real Hardware (Tested on Supermicro X9DRI-LN4F+) | Note |
| ----- | ----------- | ----------------------------------------------------------- | ----- |
| Error Throwing / Catching | ❌ | ❌ | 
| GC | ⚠️ | ❓ | Not safe |
| Multithreading(Single Core) | ✅ | ✅ |
| PS2 Keyboard/Mouse(USB Compatible) | ✅ | ✅ |
| Nintendo Family Computer Emulator | ✅ | ✅ |
| Intel® Gigabit Ethernet Network | ✅ | ✅ |
| Realtek RTL8139 | ✅ | ❓ |
| FAT32(Read only & WIP) | ⚠️ | ✅ |
| I/O APIC | ✅ | ✅ |
| Local APIC | ✅ | ✅ |
| SATA | ⚠️ | ❌ | Can't read more than one sec at once, Can't detect sata controller on real hardware |
| IDE | ✅ | ✅ |
| SMBIOS | ✅ | ✅ |
| ACPI | ✅ | ✅ |
| IPv4 | ✅ | ✅ |
| IPv6 | ❌ | ❌ |
| TCP(WIP) | ⚠️ | ❓ | Can't receive large package |
| UDP | ✅ | ❓ |
| Lan | ✅ | ✅ |
| Wan | ✅ | ❓ |
| ![image](https://github.com/nifanfa/OS-Sharp/blob/master/VirtualBox_NativeAOT_04_04_2022_23_46_58.png) | | | **0S-Sharp On Virtual Box** |
