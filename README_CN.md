[![Language switcher](https://img.shields.io/badge/Language%20%2F%20%E8%AF%AD%E8%A8%80-Chinese%20%2F%20%E4%B8%AD%E5%9B%BD-yellow)](https://github.com/nifanfa/MOOS/blob/master/README.md)

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

MOOS ( **M**y **O**wn **O**perating **S**ystem )是一个具有.NET 7 Native AOT技术的C# x64操作系统编译器。

## 建筑物
关于构建MOOS的信息，请阅读 [建立维基页面](https://github.com/nifanfa/MOOS/wiki/How-do-you-build-or-compile-MOOS)。

### 建设要求
- VMware Workstation Player - https://www.vmware.com/products/workstation-player.html
- Visual studio 2022 - https://visualstudio.microsoft.com/
- QEMU - https://www.qemu.org/download 或 VMWare ( 注意，USB不能与VMWare一起使用。 )
- Windows 10-11 x64或x86
- 8GB Ram

<br/>
<hr/>
<br/>

![形象](Screenshot1.png)

## 特点

| Feature | Working in VM | Working on hardware | Information |
| ------- | ------------- | ------------------- | ----------- |
| EXE运行 | 🟩 | 🟩 |
| 抛出/捕获错误 | 🟥 | 🟥 | 
| GC | 🟨 | ⬜ | Not safe |
| 多个处理器 | 🟩 | 🟩 |
| 多线程 | 🟩 | 🟩 |
| EHCI (USB2.0) | 🟩 | 🟩 |
| USB键盘 | 🟨 | ⬜ |
| USB鼠标 | 🟩 | ⬜ |
| USB HUB | 🟥 | 🟥 |
| PS2 Keyboard/Mouse(USB Compatible) | 🟩 | 🟩 |
| 任天堂家庭电脑仿真器 | 🟩 | 🟩 |
| DOOM(doomgeneric) | 🟩 | 🟩 |
| Intel® 千兆位以太网网络 | 🟩 | 🟩 |
| Realtek RTL8139 | 🟩 | ⬜ |
| ExFAT | 🟩 | 🟩 |
| I/O APIC | 🟩 | 🟩 |
| 当地APIC | 🟩 | 🟩 |
| SATA | 🟩 | ⬜ |
| IDE | 🟩 | 🟩 |
| SMBIOS | 🟩 | 🟩 |
| ACPI | 🟩 | 🟩 |
| IPv4 | 🟩 | 🟩 |
| IPv6 | 🟥 | 🟥 |
| TCP(WIP) | 🟨 | 🟥 | 网络无法接收大型包裹  |
| UDP | 🟩 | ⬜ |
| Lan | 🟩 | 🟩 |
| Wan | 🟩 | 🟩 |

| 颜色 | 意义 |
| ----- | ------- |
| 🟩 | 是 |
| 🟥 | 没有 |
| 🟨 | 暂时的/部分的 |
| ⬜ | 未知 |
