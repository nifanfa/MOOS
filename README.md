[![Language switcher](https://img.shields.io/badge/Language%20%2F%20%E8%AF%AD%E8%A8%80-English%20%2F%20%E8%8B%B1%E8%AF%AD-blue)](https://github.com/nifanfa/MOOS/blob/master/README_CN.md)

<p align="center">
    <img width=300 src="MOOS-Logo.svg"/>
</p>

<p align="center">
    <a href="https://github.com/nifanfa/moos/issues"><img alt="GitHub issues" src="https://img.shields.io/github/issues/nifanfa/moos"></a>
    <a href="https://github.com/nifanfa/moos/network"><img alt="GitHub forks" src="https://img.shields.io/github/forks/nifanfa/moos"></a>
    <a href="https://github.com/nifanfa/moos/stargazers"><img alt="GitHub stars" src="https://img.shields.io/github/stars/nifanfa/moos"></a>
    <a href="https://github.com/nifanfa/moos"><img alt="GitHub license" src="https://img.shields.io/github/license/nifanfa/moos"></a>
    <img alt="GitHub license" src="https://img.shields.io/badge/Version-N%2FA-%23FF0000">
    <a href="https://discord.gg/uJstXbx8Pt"><img src="https://discordapp.com/api/guilds/987075686256762890/widget.png?style=shield" alt="Discord Shield"/></a>
</p>

# MOOS

MOOS ( **M**y **O**wn **O**perating **S**ystem ) is a C# x64 operating system compiler with the .NET 7 Native AOT technology.

## Building
For information on compiling MOOS, please read the [build wiki page](https://github.com/nifanfa/MOOS/wiki/How-do-you-build-or-compile-MOOS).

### Build requirements
- VMware Workstation Player - https://www.vmware.com/products/workstation-player.html
- Visual studio 2022 - https://visualstudio.microsoft.com/
- QEMU - https://www.qemu.org/download or VMWare ( Note, USB Does not work with VMWare )
- Windows 10-11 x64 or x86
- 8GB Ram

<br/>
<hr/>
<br/>

![image](Screenshot1.png)

## Features

| Feature | Working in VM | Working on hardware | Information |
| ------- | ------------- | ------------------- | ----------- |
| EXE Running | 🟩 | 🟩 |
| Error Throwing / Catching | 🟥 | 🟥 | 
| GC | 🟨 | ⬜ | Not safe |
| Multiprocessor | 🟩 | 🟩 |
| Multithreading | 🟩 | 🟩 |
| EHCI(USB2.0) | 🟩 | 🟩 |
| USB Keyboard | 🟨 | ⬜ |
| USB Mouse | 🟩 | ⬜ |
| USB HUB | 🟥 | 🟥 |
| PS2 Keyboard/Mouse(USB Compatible) | 🟩 | 🟩 |
| Nintendo Family Computer Emulator | 🟩 | 🟩 |
| DOOM(doomgeneric) | 🟩 | 🟩 |
| Intel® Gigabit Ethernet Network | 🟩 | 🟩 |
| Realtek RTL8139 | 🟩 | ⬜ |
| ExFAT | 🟩 | 🟩 |
| I/O APIC | 🟩 | 🟩 |
| Local APIC | 🟩 | 🟩 |
| SATA | 🟩 | ⬜ |
| IDE | 🟩 | 🟩 |
| SMBIOS | 🟩 | 🟩 |
| ACPI | 🟩 | 🟩 |
| IPv4 | 🟩 | 🟩 |
| IPv6 | 🟥 | 🟥 |
| TCP(WIP) | 🟨 | 🟥 | Network can't receive large packages  |
| UDP | 🟩 | ⬜ |
| Lan | 🟩 | 🟩 |
| Wan | 🟩 | 🟩 |

| Color | Meaning |
| ----- | ------- |
| 🟩 | Yes |
| 🟥 | No |
| 🟨 | W.I.P / Partially |
| ⬜ | Unknown |
